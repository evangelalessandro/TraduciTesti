
using Nikse.SubtitleEdit.Core;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class GoogleTranslate : PositionAndSizeForm
    {
        private Subtitle _subtitle;
        private Subtitle _translatedSubtitle;
        private bool _breakTranslation;
        private bool _googleTranslate = true;
        private MicrosoftTranslationService.SoapService _microsoftTranslationService;
        private string _bingAccessToken;
        private bool _googleApiNotWorking;
        private const string SplitterString = "\n\n\n";
        private const string NewlineString = "\n";

        private enum FormattingType
        {
            None,
            Italic,
            ItalicTwoLines
        }

        

        private FormattingType[] _formattingTypes;
        private bool[] _autoSplit;

        private Encoding _screenScrapingEncoding;

        private string _targetTwoLetterIsoLanguageName;

       
        public Subtitle TranslatedSubtitle => _translatedSubtitle;

        public GoogleTranslate()
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);

            Text = Configuration.Settings.Language.GoogleTranslate.Title;
            labelFrom.Text = Configuration.Settings.Language.GoogleTranslate.From;
            labelTo.Text = Configuration.Settings.Language.GoogleTranslate.To;
            buttonTranslate.Text = Configuration.Settings.Language.GoogleTranslate.Translate;
            labelPleaseWait.Text = Configuration.Settings.Language.GoogleTranslate.PleaseWait;
            linkLabelPoweredByGoogleTranslate.Text = Configuration.Settings.Language.GoogleTranslate.PoweredByGoogleTranslate;
            buttonOK.Text = Configuration.Settings.Language.General.Ok;
            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;

            subtitleListViewFrom.InitializeLanguage(Configuration.Settings.Language.General, Configuration.Settings);
            subtitleListViewTo.InitializeLanguage(Configuration.Settings.Language.General, Configuration.Settings);
            subtitleListViewFrom.HideColumn(SubtitleListView.SubtitleColumn.CharactersPerSeconds);
            subtitleListViewFrom.HideColumn(SubtitleListView.SubtitleColumn.WordsPerMinute);
            subtitleListViewTo.HideColumn(SubtitleListView.SubtitleColumn.CharactersPerSeconds);
            subtitleListViewTo.HideColumn(SubtitleListView.SubtitleColumn.WordsPerMinute);
            UiUtil.InitializeSubtitleFont(subtitleListViewFrom);
            UiUtil.InitializeSubtitleFont(subtitleListViewTo);
            subtitleListViewFrom.AutoSizeColumns();
            subtitleListViewFrom.AutoSizeColumns();
            UiUtil.FixLargeFonts(this, buttonOK);
        }

        internal void Initialize(Subtitle subtitle, string title, bool googleTranslate, Encoding encoding)
        {
            if (title != null)
                Text = title;

            _googleTranslate = googleTranslate;
            if (!_googleTranslate)
            {
                linkLabelPoweredByGoogleTranslate.Text = Configuration.Settings.Language.GoogleTranslate.PoweredByMicrosoftTranslate;
            }

            labelPleaseWait.Visible = false;
            progressBar1.Visible = false;
            _subtitle = subtitle;
            _translatedSubtitle = new Subtitle(subtitle);

            string defaultFromLanguage = LanguageAutoDetect.AutoDetectGoogleLanguage(encoding); // Guess language via encoding
            if (string.IsNullOrEmpty(defaultFromLanguage))
                defaultFromLanguage = LanguageAutoDetect.AutoDetectGoogleLanguage(subtitle); // Guess language based on subtitle contents
            if (defaultFromLanguage == "he")
                defaultFromLanguage = "iw";

            FillComboWithLanguages(comboBoxFrom);
            int i = 0;
            foreach (ComboBoxItem item in comboBoxFrom.Items)
            {
                if (item.Value == defaultFromLanguage)
                {
                    comboBoxFrom.SelectedIndex = i;
                    break;
                }
                i++;
            }

            FillComboWithLanguages(comboBoxTo);
            i = 0;
            string uiCultureTargetLanguage = Configuration.Settings.Tools.GoogleTranslateLastTargetLanguage;
            if (uiCultureTargetLanguage == defaultFromLanguage)
            {
                foreach (string s in Utilities.GetDictionaryLanguages())
                {
                    string temp = s.Replace("[", string.Empty).Replace("]", string.Empty);
                    if (temp.Length > 4)
                    {
                        temp = temp.Substring(temp.Length - 5, 2).ToLower();

                        if (temp != uiCultureTargetLanguage)
                        {
                            uiCultureTargetLanguage = temp;
                            break;
                        }
                    }
                }
            }
            comboBoxTo.SelectedIndex = 0;
            foreach (ComboBoxItem item in comboBoxTo.Items)
            {
                if (item.Value == uiCultureTargetLanguage)
                {
                    comboBoxTo.SelectedIndex = i;
                    break;
                }
                i++;
            }

            subtitleListViewFrom.Fill(subtitle);
            GoogleTranslate_Resize(null, null);

            _googleApiNotWorking = !Configuration.Settings.Tools.UseGooleApiPaidService; // google has closed their free api service :(

            _formattingTypes = new FormattingType[_subtitle.Paragraphs.Count];
            _autoSplit = new bool[_subtitle.Paragraphs.Count];
        }

        private void buttonTranslate_Click(object sender, EventArgs e)
        {
            if (buttonTranslate.Text == Configuration.Settings.Language.General.Cancel)
            {
                buttonTranslate.Enabled = false;
                _breakTranslation = true;
                buttonOK.Enabled = true;
                buttonCancel.Enabled = true;
                return;
            }

            // empty all texts
            foreach (Paragraph p in _translatedSubtitle.Paragraphs)
                p.Text = string.Empty;

            _targetTwoLetterIsoLanguageName = (comboBoxTo.SelectedItem as ComboBoxItem).Value;
            if (!_googleTranslate)
            {
                string from = (comboBoxFrom.SelectedItem as ComboBoxItem).Value;
                string to = _targetTwoLetterIsoLanguageName;
                if (!string.IsNullOrEmpty(Configuration.Settings.Tools.MicrosoftTranslatorApiKey))
                {
                    try
                    {
                        _bingAccessToken = GetBingAccesstoken(Configuration.Settings.Tools.MicrosoftTranslatorApiKey);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("Make sure 'Client ID' and 'Client secret' are correct!" + Environment.NewLine + Environment.NewLine + exception.Message + Environment.NewLine + exception.StackTrace);
                        return;
                    }
                    if (!string.IsNullOrEmpty(_bingAccessToken))
                    {
                        DoMicrosoftTranslateNew(from, to); // uses new api with access token
                        return;
                    }
                }
                DoMicrosoftTranslate(from, to); // uses old api key
                return;
            }

            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;
            _breakTranslation = false;
            buttonTranslate.Text = Configuration.Settings.Language.General.Cancel;
            const int textMaxSize = 1000;
            Cursor.Current = Cursors.WaitCursor;
            progressBar1.Maximum = _subtitle.Paragraphs.Count;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            labelPleaseWait.Visible = true;
            int start = 0;
            try
            {
                var sb = new StringBuilder();
                int index = 0;
                for (int i = 0; i < _subtitle.Paragraphs.Count; i++)
                {
                    Paragraph p = _subtitle.Paragraphs[i];
                    string text = SetFormattingTypeAndSplitting(i, p.Text, (comboBoxFrom.SelectedItem as ComboBoxItem).Value.StartsWith("zh"));
                    text = string.Format("{1} {0}", text, SplitterString);
                    if (Utilities.UrlEncode(sb + text).Length >= textMaxSize)
                    {
                        FillTranslatedText(DoTranslate(sb.ToString()), start, index - 1);
                        sb.Clear();
                        progressBar1.Refresh();
                        Application.DoEvents();
                        start = index;
                    }
                    sb.Append(text);
                    index++;
                    progressBar1.Value = index;
                    if (_breakTranslation)
                        break;
                }
                if (sb.Length > 0)
                    FillTranslatedText(DoTranslate(sb.ToString()), start, index - 1);
            }
            catch (WebException webException)
            {
                MessageBox.Show(webException.Source + ": " + webException.Message);
            }
            finally
            {
                labelPleaseWait.Visible = false;
                progressBar1.Visible = false;
                Cursor.Current = Cursors.Default;
                buttonTranslate.Text = Configuration.Settings.Language.GoogleTranslate.Translate;
                buttonTranslate.Enabled = true;
                buttonOK.Enabled = true;
                buttonCancel.Enabled = true;

                Configuration.Settings.Tools.GoogleTranslateLastTargetLanguage = _targetTwoLetterIsoLanguageName;
            }
        }

        private string SetFormattingTypeAndSplitting(int i, string text, bool skipSplit)
        {
            text = text.Trim();
            if (text.StartsWith("<i>", StringComparison.Ordinal) && text.EndsWith("</i>", StringComparison.Ordinal) && text.Contains("</i>" + Environment.NewLine + "<i>") && Utilities.GetNumberOfLines(text) == 2 && Utilities.CountTagInText(text, "<i>") == 1)
            {
                _formattingTypes[i] = FormattingType.ItalicTwoLines;
                text = HtmlUtil.RemoveOpenCloseTags(text, HtmlUtil.TagItalic);
            }
            else if (text.StartsWith("<i>", StringComparison.Ordinal) && text.EndsWith("</i>", StringComparison.Ordinal) && Utilities.CountTagInText(text, "<i>") == 1)
            {
                _formattingTypes[i] = FormattingType.Italic;
                text = text.Substring(3, text.Length - 7);
            }
            else
            {
                _formattingTypes[i] = FormattingType.None;
            }

            if (skipSplit)
            {
                return text;
            }

            var lines = text.SplitToLines();
            if (Configuration.Settings.Tools.TranslateAutoSplit && lines.Count == 2 && !string.IsNullOrEmpty(lines[0]) && (Utilities.AllLettersAndNumbers + ",").Contains(lines[0].Substring(lines[0].Length - 1)))
            {
                _autoSplit[i] = true;
                text = Utilities.RemoveLineBreaks(text);
            }

            return text;
        }

        private void FillTranslatedText(string translatedText, int start, int end)
        {
            int index = start;
            foreach (string s in SplitToLines(translatedText))
            {
                if (index < _translatedSubtitle.Paragraphs.Count)
                {
                    string cleanText = s.Replace("</p>", string.Empty).Trim();
                    int indexOfP = cleanText.IndexOf(SplitterString.Trim(), StringComparison.Ordinal);
                    if (indexOfP >= 0 && indexOfP < 4)
                        cleanText = cleanText.Remove(0, indexOfP);
                    cleanText = cleanText.Replace(SplitterString, string.Empty).Trim();
                    if (cleanText.Contains('\n') && !cleanText.Contains('\r'))
                        cleanText = cleanText.Replace("\n", Environment.NewLine);
                    cleanText = cleanText.Replace(" ...", "...");
                    cleanText = cleanText.Replace("<br/>", Environment.NewLine);
                    cleanText = cleanText.Replace("<br />", Environment.NewLine);
                    cleanText = cleanText.Replace("<br / >", Environment.NewLine);
                    cleanText = cleanText.Replace("< br />", Environment.NewLine);
                    cleanText = cleanText.Replace("< br / >", Environment.NewLine);
                    cleanText = cleanText.Replace("< br/ >", Environment.NewLine);
                    cleanText = cleanText.Replace(Environment.NewLine + " ", Environment.NewLine);
                    cleanText = cleanText.Replace(" " + Environment.NewLine, Environment.NewLine);
                    cleanText = cleanText.Replace("<I>", "<i>");
                    cleanText = cleanText.Replace("< I>", "<i>");
                    cleanText = cleanText.Replace("</ i>", "</i>");
                    cleanText = cleanText.Replace("</ I>", "</i>");
                    cleanText = cleanText.Replace("</I>", "</i>");
                    cleanText = cleanText.Replace("< i >", "<i>");
                    if (cleanText.StartsWith("<i> ", StringComparison.Ordinal))
                        cleanText = cleanText.Remove(3, 1);
                    if (cleanText.EndsWith(" </i>", StringComparison.Ordinal))
                        cleanText = cleanText.Remove(cleanText.Length - 5, 1);
                    cleanText = cleanText.Replace(Environment.NewLine + "<i> ", Environment.NewLine + "<i>");
                    cleanText = cleanText.Replace(" </i>" + Environment.NewLine, "</i>" + Environment.NewLine);

                    if (_autoSplit[index])
                    {
                        cleanText = Utilities.AutoBreakLine(cleanText);
                    }
                    if (Utilities.GetNumberOfLines(cleanText) == 1 && Utilities.GetNumberOfLines(_subtitle.Paragraphs[index].Text) == 2)
                    {
                        if (!_autoSplit[index])
                        {
                            cleanText = Utilities.AutoBreakLine(cleanText);
                        }
                    }

                    if (_formattingTypes[index] == FormattingType.ItalicTwoLines || _formattingTypes[index] == FormattingType.Italic)
                    {
                        _translatedSubtitle.Paragraphs[index].Text = "<i>" + cleanText + "</i>";
                    }
                    else
                    {
                        _translatedSubtitle.Paragraphs[index].Text = cleanText;
                    }
                }
                index++;
            }
            subtitleListViewTo.Fill(_translatedSubtitle);
            subtitleListViewTo.SelectIndexAndEnsureVisible(end);
        }

        private List<string> SplitToLines(string translatedText)
        {
            if (!_googleTranslate)
            {
                translatedText = translatedText.Replace("+- +", "+-+");
                translatedText = translatedText.Replace("+ -+", "+-+");
                translatedText = translatedText.Replace("+ - +", "+-+");
                translatedText = translatedText.Replace("+ +", "+-+");
                translatedText = translatedText.Replace("+-+", "|");
            }
            return translatedText.Split('|').ToList();
        }

        private string DoTranslate(string input)
        {
            string languagePair = (comboBoxFrom.SelectedItem as ComboBoxItem).Value + "|" + (comboBoxTo.SelectedItem as ComboBoxItem).Value;
            bool romanji = languagePair.EndsWith("|romanji", StringComparison.InvariantCulture);
            if (romanji)
                languagePair = (comboBoxFrom.SelectedItem as ComboBoxItem).Value + "|ja";

            input = PreTranslate(input.TrimEnd('|').Trim());

            string result = null;
            if (!_googleApiNotWorking)
            {
                try
                {
                    result = TranslateTextViaApi(input, languagePair);
                }
                catch
                {
                    _googleApiNotWorking = true;
                    result = string.Empty;
                }
            }

            // fallback to screen scraping
            if (string.IsNullOrEmpty(result))
            {
                if (_screenScrapingEncoding == null)
                    _screenScrapingEncoding = GetScreenScrapingEncoding(languagePair);
                result = TranslateTextViaScreenScraping(input, languagePair, _screenScrapingEncoding, romanji);
                _googleApiNotWorking = true;
            }

            return PostTranslate(result);
        }

       

        public void FillComboWithLanguages(ComboBox comboBox)
        {
            if (!_googleTranslate)
            {
                foreach (var bingLanguageCode in GetBingLanguageCodes())
                {
                    comboBox.Items.Add(new ComboBoxItem(bingLanguageCode.Value, bingLanguageCode.Key));
                }
                return;
            }

            FillComboWithGoogleLanguages(comboBox);
        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/hh456380.aspx - current list is from October 2015
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetBingLanguageCodes()
        {
            return new Dictionary<string, string>
            {
                {"af", "Afrikaans"},
                {"ar", "Arabic"},
                {"bn", "Bangla"},
                { "bs-Latn", "Bosnian (Latin)"},
                { "bg", "Bulgarian"},
                { "yue", "Cantonese (Traditional)"},
                { "ca", "Catalan"},
                { "zh-CHS", "Chinese Simplified"},
                { "zh-CHT", "Chinese Traditional"},
                { "hr", "Croatian"},
                { "cs", "Czech"},
                { "da", "Danish"},
                { "nl", "Dutch"},
                { "en", "English"},
                { "et", "Estonian"},
                { "fj", "Fijian"},
                { "fil", "Filipino"},
                { "fi", "Finnish"},
                { "fr", "French"},
                { "de", "German"},
                { "el", "Greek"},
                { "ht", "Haitian Creole"},
                { "he", "Hebrew"},
                { "hi", "Hindi"},
                { "mww", "Hmong Daw"},
                { "hu", "Hungarian"},
                { "id", "Indonesian"},
                { "it", "Italian"},
                { "ja", "Japanese"},
                { "sw", "Kiswahili"},
                { "tlh", "Klingon"},
                { "tlh-Qaak", "Klingon (pIqaD)"},
                { "ko", "Korean"},
                { "lv", "Latvian"},
                { "lt", "Lithuanian"},
                { "ms", "Malay"},
                { "mt", "Maltese"},
                { "no", "Norwegian"},
                { "fa", "Persian"},
                { "pl", "Polish"},
                { "pt", "Portuguese"},
                { "otq", "Querétaro Otomi"},
                { "ro", "Romanian"},
                { "ru", "Russian"},
                { "sm", "Samoan"},
                { "sr-Cyrl", "Serbian (Cyrillic)"},
                { "sr-Latn", "Serbian (Latin)"},
                { "sk", "Slovak"},
                { "sl", "Slovenian"},
                { "es", "Spanish"},
                { "sv", "Swedish"},
                { "ty", "Tahitian"},
                { "th", "Thai"},
                { "to", "Tongan"},
                { "tr", "Turkish"},
                { "uk", "Ukrainian"},
                { "ur", "Urdu"},
                { "vi", "Vietnamese"},
                { "cy", "Welsh"},
                { "yua", "Yucatec Maya"},
            };
        }

        

        private void LinkLabel1LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_googleTranslate ? GoogleTranslateUrl : "http://www.bing.com/translator");
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            if (subtitleListViewTo.Items.Count > 0)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private string PreTranslate(string s)
        {
            if ((comboBoxFrom.SelectedItem as ComboBoxItem).Value == "en")
            {
                s = Regex.Replace(s, @"\bI'm ", "I am ");
                s = Regex.Replace(s, @"\bI've ", "I have ");
                s = Regex.Replace(s, @"\bI'll ", "I will ");
                s = Regex.Replace(s, @"\bI'd ", "I would ");  // had or would???
                s = Regex.Replace(s, @"\b(I|i)t's ", "$1t is ");
                s = Regex.Replace(s, @"\b(Y|y)ou're ", "$1ou are ");
                s = Regex.Replace(s, @"\b(Y|y)ou've ", "$1ou have ");
                s = Regex.Replace(s, @"\b(Y|y)ou'll ", "$1ou will ");
                s = Regex.Replace(s, @"\b(Y|y)ou'd ", "$1ou would "); // had or would???
                s = Regex.Replace(s, @"\b(H|h)e's ", "$1e is ");
                s = Regex.Replace(s, @"\b(S|s)he's ", "$1he is ");
                s = Regex.Replace(s, @"\b(W|w)e're ", "$1e are ");
                s = Regex.Replace(s, @"\bwon't ", "will not ");
                s = Regex.Replace(s, @"\b(W|w)e're ", "$1e are ");
                s = Regex.Replace(s, @"\bwon't ", "will not ");
                s = Regex.Replace(s, @"\b(T|t)hey're ", "$1hey are ");
                s = Regex.Replace(s, @"\b(W|w)ho's ", "$1ho is ");
                s = Regex.Replace(s, @"\b(T|t)hat's ", "$1hat is ");
                s = Regex.Replace(s, @"\b(W|w)hat's ", "$1hat is ");
                s = Regex.Replace(s, @"\b(W|w)here's ", "$1here is ");
                s = Regex.Replace(s, @"\b(W|w)ho's ", "$1ho is ");
                s = Regex.Replace(s, @"\B'(C|c)ause ", "$1ecause "); // \b (word boundry) does not workig with '
            }
            return s;
        }

        private string PostTranslate(string s)
        {
            if ((comboBoxTo.SelectedItem as ComboBoxItem).Value == "da")
            {
                s = s.Replace("Jeg ved.", "Jeg ved det.");
                s = s.Replace(", jeg ved.", ", jeg ved det.");

                s = s.Replace("Jeg er ked af.", "Jeg er ked af det.");
                s = s.Replace(", jeg er ked af.", ", jeg er ked af det.");

                s = s.Replace("Come on.", "Kom nu.");
                s = s.Replace(", come on.", ", kom nu.");
                s = s.Replace("Come on,", "Kom nu,");

                s = s.Replace("Hey ", "Hej ");
                s = s.Replace("Hey,", "Hej,");

                s = s.Replace(" gonna ", " ville ");
                s = s.Replace("Gonna ", "Vil ");

                s = s.Replace("Ked af.", "Undskyld.");
            }
            return s;
        }

        private void FormGoogleTranslate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && labelPleaseWait.Visible == false)
                DialogResult = DialogResult.Cancel;
            else if (e.KeyCode == Keys.Escape && labelPleaseWait.Visible)
            {
                _breakTranslation = true;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == UiUtil.HelpKeys)
                Utilities.ShowHelp("#translation");
            else if (e.Control && e.Shift && e.Alt && e.KeyCode == Keys.L)
            {
                Cursor.Current = Cursors.WaitCursor;
                Configuration.Settings.Language.Save(Path.Combine(Configuration.BaseDirectory, "LanguageMaster.xml"));
                TranslateViaGoogle((comboBoxFrom.SelectedItem as ComboBoxItem).Value + "|" +
                                            (comboBoxTo.SelectedItem as ComboBoxItem).Value);
                Cursor.Current = Cursors.Default;
            }
        }

        public static void TranslateViaGoogle(string languagePair)
        {
            var doc = new XmlDocument();
            doc.Load(Configuration.BaseDirectory + "Language.xml");
            if (doc.DocumentElement != null)
                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    TranslateNode(node, languagePair);

            doc.Save(Configuration.BaseDirectory + "Language.xml");
        }

        private static void TranslateNode(XmlNode node, string languagePair)
        {
            if (node.ChildNodes.Count == 0)
            {
                string oldText = node.InnerText;
                string newText = TranslateTextViaApi(node.InnerText, languagePair);
                if (!string.IsNullOrEmpty(oldText) && !string.IsNullOrEmpty(newText))
                {
                    if (oldText.Contains("{0:"))
                    {
                        newText = oldText;
                    }
                    else
                    {
                        if (!oldText.Contains(" / "))
                            newText = newText.Replace(" / ", "/");

                        if (!oldText.Contains(" ..."))
                            newText = newText.Replace(" ...", "...");

                        if (!oldText.Contains("& "))
                            newText = newText.Replace("& ", "&");

                        if (!oldText.Contains("# "))
                            newText = newText.Replace("# ", "#");

                        if (!oldText.Contains("@ "))
                            newText = newText.Replace("@ ", "@");

                        if (oldText.Contains("{0}"))
                        {
                            newText = newText.Replace("(0)", "{0}");
                            newText = newText.Replace("(1)", "{1}");
                            newText = newText.Replace("(2)", "{2}");
                            newText = newText.Replace("(3)", "{3}");
                            newText = newText.Replace("(4)", "{4}");
                            newText = newText.Replace("(5)", "{5}");
                            newText = newText.Replace("(6)", "{6}");
                            newText = newText.Replace("(7)", "{7}");
                        }
                    }
                }
                node.InnerText = newText;
            }
            else
            {
                foreach (XmlNode childNode in node.ChildNodes)
                    TranslateNode(childNode, languagePair);
            }
        }

        private void GoogleTranslate_Resize(object sender, EventArgs e)
        {
            int width = (Width / 2) - (subtitleListViewFrom.Left * 3) + 19;
            subtitleListViewFrom.Width = width;
            subtitleListViewTo.Width = width;

            int height = Height - (subtitleListViewFrom.Top + buttonTranslate.Height + 60);
            subtitleListViewFrom.Height = height;
            subtitleListViewTo.Height = height;

            comboBoxFrom.Left = subtitleListViewFrom.Left + (subtitleListViewFrom.Width - comboBoxFrom.Width);
            labelFrom.Left = comboBoxFrom.Left - 5 - labelFrom.Width;

            subtitleListViewTo.Left = width + (subtitleListViewFrom.Left * 2);
            labelTo.Left = subtitleListViewTo.Left;
            comboBoxTo.Left = labelTo.Left + labelTo.Width + 5;
            buttonTranslate.Left = comboBoxTo.Left + comboBoxTo.Width + 9;
            labelPleaseWait.Left = buttonTranslate.Left + buttonTranslate.Width + 9;
            progressBar1.Left = labelPleaseWait.Left;
            progressBar1.Width = subtitleListViewTo.Width - (progressBar1.Left - subtitleListViewTo.Left);
        }

        private MicrosoftTranslationService.SoapService MsTranslationServiceClient
        {
            get
            {
                if (_microsoftTranslationService == null)
                {
                    _microsoftTranslationService = new MicrosoftTranslationService.SoapService { Proxy = Utilities.GetProxy() };
                }
                return _microsoftTranslationService;
            }
        }

        private string BingTranslateViaAccessToken(string accessToken, string text, string fromLanguage, string toLanguage)
        {
            //max 10000 chars!
            string url = string.Format("https://api.microsofttranslator.com/V2/Http.svc/Translate?appid=&text={0}&from={1}&to={2}", Utilities.UrlEncode(text), fromLanguage, toLanguage);
            var req = WebRequest.Create(url);
            req.Method = "GET";
            req.Headers["Authorization"] = "Bearer " + accessToken;
            var response = req.GetResponse() as HttpWebResponse;
            if (response == null)
            {
                return null;
            }
            var responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return null;
            }

            using (var reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                string xml = reader.ReadToEnd();
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return doc.InnerText;
            }
        }

        private string GetBingAccesstoken(string clientSecret)
        {
            string datamarketAccessUri = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
            var webRequest = WebRequest.Create(datamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.Headers.Add("Ocp-Apim-Subscription-Key", clientSecret);
            webRequest.ContentLength = 0;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                var responseStream = webResponse.GetResponseStream();
                if (responseStream == null)
                {
                    return null;
                }
                using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                {
                    return reader.ReadToEnd(); // raw access token
                }
            }
        }

        public void DoMicrosoftTranslate(string from, string to)
        {
            MicrosoftTranslationService.SoapService client = MsTranslationServiceClient;
            _breakTranslation = false;
            buttonTranslate.Text = Configuration.Settings.Language.General.Cancel;
            const int textMaxSize = 10000;
            Cursor.Current = Cursors.WaitCursor;
            progressBar1.Maximum = _subtitle.Paragraphs.Count;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            labelPleaseWait.Visible = true;
            int start = 0;
            bool overQuota = false;

            try
            {
                var sb = new StringBuilder();
                int index = 0;
                foreach (Paragraph p in _subtitle.Paragraphs)
                {
                    string text = string.Format("{1}{0}|", SetFormattingTypeAndSplitting(index, p.Text, from.StartsWith("zh")), SplitterString);
                    if (!overQuota)
                    {
                        if (Utilities.UrlEncode(sb + text).Length >= textMaxSize)
                        {
                            try
                            {
                                FillTranslatedText(client.Translate(Configuration.Settings.Tools.MicrosoftBingApiId, sb.ToString().Replace(Environment.NewLine, "<br />"), from, to, "text/plain", "general"), start, index - 1);
                            }
                            catch (System.Web.Services.Protocols.SoapHeaderException exception)
                            {
                                MessageBox.Show("Sorry, Microsoft is closing their free api: " + exception.Message);
                                overQuota = true;
                            }
                            sb.Clear();
                            progressBar1.Refresh();
                            Application.DoEvents();
                            start = index;
                        }
                        sb.Append(text);
                    }
                    index++;
                    progressBar1.Value = index;
                    if (_breakTranslation)
                        break;
                }
                if (sb.Length > 0 && !overQuota)
                {
                    try
                    {
                        FillTranslatedText(client.Translate(Configuration.Settings.Tools.MicrosoftBingApiId, sb.ToString().Replace(Environment.NewLine, "<br />"), from, to, "text/plain", "general"), start, index - 1);
                    }
                    catch (System.Web.Services.Protocols.SoapHeaderException exception)
                    {
                        MessageBox.Show("Sorry, Microsoft is closing their free api: " + exception.Message);
                    }
                }
            }
            finally
            {
                labelPleaseWait.Visible = false;
                progressBar1.Visible = false;
                Cursor.Current = Cursors.Default;
                buttonTranslate.Text = Configuration.Settings.Language.GoogleTranslate.Translate;
                buttonTranslate.Enabled = true;
            }
        }

        public void DoMicrosoftTranslateNew(string from, string to)
        {
            _breakTranslation = false;
            buttonTranslate.Text = Configuration.Settings.Language.General.Cancel;
            const int textMaxSize = 10000;
            Cursor.Current = Cursors.WaitCursor;
            progressBar1.Maximum = _subtitle.Paragraphs.Count;
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            labelPleaseWait.Visible = true;
            int start = 0;
            bool overQuota = false;

            try
            {
                var sb = new StringBuilder();
                int index = 0;
                foreach (Paragraph p in _subtitle.Paragraphs)
                {
                    string text = $"{SetFormattingTypeAndSplitting(index, p.Text, @from.StartsWith("zh"))} +-+ ";
                    if (!overQuota)
                    {
                        if (Utilities.UrlEncode(sb + text).Length >= textMaxSize)
                        {
                            try
                            {
                                FillTranslatedText(BingTranslateViaAccessToken(_bingAccessToken, sb.ToString(), from, to), start, index - 1);
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                                overQuota = true;
                            }
                            sb.Clear();
                            progressBar1.Refresh();
                            Application.DoEvents();
                            start = index;
                        }
                        sb.Append(text);
                    }
                    index++;
                    progressBar1.Value = index;
                    if (_breakTranslation)
                        break;
                }
                if (sb.Length > 0 && !overQuota)
                {
                    try
                    {
                        FillTranslatedText(BingTranslateViaAccessToken(_bingAccessToken, sb.ToString(), from, to), start, index - 1);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
            finally
            {
                labelPleaseWait.Visible = false;
                progressBar1.Visible = false;
                Cursor.Current = Cursors.Default;
                buttonTranslate.Text = Configuration.Settings.Language.GoogleTranslate.Translate;
                buttonTranslate.Enabled = true;
            }
        }

        private void SyncListViews(ListView listViewSelected, SubtitleListView listViewOther)
        {
            if (listViewSelected.SelectedItems.Count > 0)
            {
                var first = listViewSelected.TopItem.Index;
                int index = listViewSelected.SelectedItems[0].Index;
                if (index < listViewOther.Items.Count)
                {
                    listViewOther.SelectIndexAndEnsureVisible(index, false);
                    if (first >= 0)
                        listViewOther.TopItem = listViewOther.Items[first];
                }
            }
        }

        private void subtitleListViewFrom_DoubleClick(object sender, EventArgs e)
        {
            SyncListViews(subtitleListViewFrom, subtitleListViewTo);
        }

        private void subtitleListViewTo_DoubleClick(object sender, EventArgs e)
        {
            SyncListViews(subtitleListViewTo, subtitleListViewFrom);
        }

        public string GetFileNameWithTargetLanguage(string oldFileName, string videoFileName, Subtitle oldSubtitle, SubtitleFormat subtitleFormat)
        {
            if (!string.IsNullOrEmpty(_targetTwoLetterIsoLanguageName))
            {
                if (!string.IsNullOrEmpty(videoFileName))
                {
                    return Path.GetFileNameWithoutExtension(videoFileName) + "." + _targetTwoLetterIsoLanguageName.ToLower() + subtitleFormat.Extension;
                }
                else if (!string.IsNullOrEmpty(oldFileName))
                {
                    var s = Path.GetFileNameWithoutExtension(oldFileName);
                    if (oldSubtitle != null)
                    {
                        var lang = "." + LanguageAutoDetect.AutoDetectGoogleLanguage(oldSubtitle);
                        if (lang.Length == 3 && s.EndsWith(lang, StringComparison.OrdinalIgnoreCase))
                            s = s.Remove(s.Length - 3);
                    }
                    return s + "." + _targetTwoLetterIsoLanguageName.ToLower() + subtitleFormat.Extension;
                }
            }
            return null;
        }

        private void comboBoxTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _screenScrapingEncoding = null;
        }

    }
}
