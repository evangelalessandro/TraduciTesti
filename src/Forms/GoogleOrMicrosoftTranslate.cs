

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms
{
	public sealed partial class GoogleOrMicrosoftTranslate : Form
	{
		public string TranslatedText { get; set; }
		private bool	_googleApiNotWorking = false; // google has closed their free api service :(

		public void FillComboWithGoogleLanguages(ComboBox comboBox)
		{
			comboBox.Items.Add(new ComboBoxItem("AFRIKAANS", "af"));
			comboBox.Items.Add(new ComboBoxItem("ALBANIAN", "sq"));
			comboBox.Items.Add(new ComboBoxItem("AMHARIC", "am"));
			comboBox.Items.Add(new ComboBoxItem("ARABIC", "ar"));
			comboBox.Items.Add(new ComboBoxItem("ARMENIAN", "hy"));
			comboBox.Items.Add(new ComboBoxItem("AZERBAIJANI", "az"));
			comboBox.Items.Add(new ComboBoxItem("BASQUE", "eu"));
			comboBox.Items.Add(new ComboBoxItem("BELARUSIAN", "be"));
			comboBox.Items.Add(new ComboBoxItem("BENGALI", "bn"));
			comboBox.Items.Add(new ComboBoxItem("BOSNIAN", "bs"));
			comboBox.Items.Add(new ComboBoxItem("BULGARIAN", "bg"));
			comboBox.Items.Add(new ComboBoxItem("BURMESE", "my"));
			comboBox.Items.Add(new ComboBoxItem("CATALAN", "ca"));
			comboBox.Items.Add(new ComboBoxItem("CEBUANO", "ceb"));
			comboBox.Items.Add(new ComboBoxItem("CHICHEWA", "ny"));
			comboBox.Items.Add(new ComboBoxItem("CHINESE", "zh"));
			comboBox.Items.Add(new ComboBoxItem("CHINESE_SIMPLIFIED", "zh-CN"));
			comboBox.Items.Add(new ComboBoxItem("CHINESE_TRADITIONAL", "zh-TW"));
			comboBox.Items.Add(new ComboBoxItem("CORSICAN", "co"));
			comboBox.Items.Add(new ComboBoxItem("CROATIAN", "hr"));
			comboBox.Items.Add(new ComboBoxItem("CZECH", "cs"));
			comboBox.Items.Add(new ComboBoxItem("DANISH", "da"));
			comboBox.Items.Add(new ComboBoxItem("DUTCH", "nl"));
			comboBox.Items.Add(new ComboBoxItem("ENGLISH", "en"));
			comboBox.Items.Add(new ComboBoxItem("ESPERANTO", "eo"));
			comboBox.Items.Add(new ComboBoxItem("ESTONIAN", "et"));
			comboBox.Items.Add(new ComboBoxItem("FILIPINO", "tl"));
			comboBox.Items.Add(new ComboBoxItem("FINNISH", "fi"));
			comboBox.Items.Add(new ComboBoxItem("FRENCH", "fr"));
			comboBox.Items.Add(new ComboBoxItem("FRISIAN", "fy"));
			comboBox.Items.Add(new ComboBoxItem("GALICIAN", "gl"));
			comboBox.Items.Add(new ComboBoxItem("GEORGIAN", "ka"));
			comboBox.Items.Add(new ComboBoxItem("GERMAN", "de"));
			comboBox.Items.Add(new ComboBoxItem("GREEK", "el"));
			comboBox.Items.Add(new ComboBoxItem("GUJARATI", "gu"));
			comboBox.Items.Add(new ComboBoxItem("HAITIAN CREOLE", "ht"));
			comboBox.Items.Add(new ComboBoxItem("HAUSA", "ha"));
			comboBox.Items.Add(new ComboBoxItem("HAWAIIAN", "haw"));
			comboBox.Items.Add(new ComboBoxItem("HEBREW", "iw"));
			comboBox.Items.Add(new ComboBoxItem("HINDI", "hi"));
			comboBox.Items.Add(new ComboBoxItem("HMOUNG", "hmn"));
			comboBox.Items.Add(new ComboBoxItem("HUNGARIAN", "hu"));
			comboBox.Items.Add(new ComboBoxItem("ICELANDIC", "is"));
			comboBox.Items.Add(new ComboBoxItem("IGBO", "ig"));
			comboBox.Items.Add(new ComboBoxItem("INDONESIAN", "id"));
			comboBox.Items.Add(new ComboBoxItem("IRISH", "ga"));
			comboBox.Items.Add(new ComboBoxItem("ITALIAN", "it"));
			comboBox.Items.Add(new ComboBoxItem("JAPANESE", "ja"));
			comboBox.Items.Add(new ComboBoxItem("JAVANESE", "jw"));
			comboBox.Items.Add(new ComboBoxItem("KANNADA", "kn"));
			comboBox.Items.Add(new ComboBoxItem("KAZAKH", "kk"));
			comboBox.Items.Add(new ComboBoxItem("KHMER", "km"));
			comboBox.Items.Add(new ComboBoxItem("KOREAN", "ko"));
			comboBox.Items.Add(new ComboBoxItem("KURDISH", "ku"));
			comboBox.Items.Add(new ComboBoxItem("KYRGYZ", "ky"));
			comboBox.Items.Add(new ComboBoxItem("LAO", "lo"));
			comboBox.Items.Add(new ComboBoxItem("LATIN", "la"));
			comboBox.Items.Add(new ComboBoxItem("LATVIAN", "lv"));
			comboBox.Items.Add(new ComboBoxItem("LITHUANIAN", "lt"));
			comboBox.Items.Add(new ComboBoxItem("LUXEMBOURGISH", "lb"));
			comboBox.Items.Add(new ComboBoxItem("MACEDONIAN", "mk"));
			comboBox.Items.Add(new ComboBoxItem("MALAY", "ms"));
			comboBox.Items.Add(new ComboBoxItem("MALAGASY", "mg"));
			comboBox.Items.Add(new ComboBoxItem("MALAYALAM", "ml"));
			comboBox.Items.Add(new ComboBoxItem("MALTESE", "mt"));
			comboBox.Items.Add(new ComboBoxItem("MAORI", "mi"));
			comboBox.Items.Add(new ComboBoxItem("MARATHI", "mr"));
			comboBox.Items.Add(new ComboBoxItem("MONGOLIAN", "mn"));
			comboBox.Items.Add(new ComboBoxItem("MYANMAR", "my"));
			comboBox.Items.Add(new ComboBoxItem("NEPALI", "ne"));
			comboBox.Items.Add(new ComboBoxItem("NORWEGIAN", "no"));
			comboBox.Items.Add(new ComboBoxItem("PASHTO", "ps"));
			comboBox.Items.Add(new ComboBoxItem("PERSIAN", "fa"));
			comboBox.Items.Add(new ComboBoxItem("POLISH", "pl"));
			comboBox.Items.Add(new ComboBoxItem("PORTUGUESE", "pt"));
			comboBox.Items.Add(new ComboBoxItem("PUNJABI", "pa"));
			comboBox.Items.Add(new ComboBoxItem("ROMANIAN", "ro"));

			if (comboBox == comboBoxTo && !_googleApiNotWorking)
				comboBox.Items.Add(new ComboBoxItem("ROMANJI", "romanji"));

			comboBox.Items.Add(new ComboBoxItem("RUSSIAN", "ru"));
			comboBox.Items.Add(new ComboBoxItem("SAMOAN", "sm"));
			comboBox.Items.Add(new ComboBoxItem("SCOTS GAELIC", "gd"));
			comboBox.Items.Add(new ComboBoxItem("SERBIAN", "sr"));
			comboBox.Items.Add(new ComboBoxItem("SESOTHO", "st"));
			comboBox.Items.Add(new ComboBoxItem("SHONA", "sn"));
			comboBox.Items.Add(new ComboBoxItem("SINDHI", "sd"));
			comboBox.Items.Add(new ComboBoxItem("SINHALA", "si"));
			comboBox.Items.Add(new ComboBoxItem("SLOVAK", "sk"));
			comboBox.Items.Add(new ComboBoxItem("SLOVENIAN", "sl"));
			comboBox.Items.Add(new ComboBoxItem("SOMALI", "so"));
			comboBox.Items.Add(new ComboBoxItem("SPANISH", "es"));
			comboBox.Items.Add(new ComboBoxItem("SUNDANESE", "su"));
			comboBox.Items.Add(new ComboBoxItem("SWAHILI", "sw"));
			comboBox.Items.Add(new ComboBoxItem("SWEDISH", "sv"));
			comboBox.Items.Add(new ComboBoxItem("TAJIK", "tg"));
			comboBox.Items.Add(new ComboBoxItem("TAMIL", "ta"));
			comboBox.Items.Add(new ComboBoxItem("TELUGU", "te"));
			comboBox.Items.Add(new ComboBoxItem("THAI", "th"));
			comboBox.Items.Add(new ComboBoxItem("TURKISH", "tr"));
			comboBox.Items.Add(new ComboBoxItem("UKRAINIAN", "uk"));
			comboBox.Items.Add(new ComboBoxItem("URDU", "ur"));
			comboBox.Items.Add(new ComboBoxItem("UZBEK", "uz"));
			comboBox.Items.Add(new ComboBoxItem("VIETNAMESE", "vi"));
			comboBox.Items.Add(new ComboBoxItem("WELSH", "cy"));
			comboBox.Items.Add(new ComboBoxItem("XHOSA", "xh"));
			comboBox.Items.Add(new ComboBoxItem("YIDDISH", "yi"));
			comboBox.Items.Add(new ComboBoxItem("YORUBA", "yo"));
			comboBox.Items.Add(new ComboBoxItem("ZULU", "zu"));
		}
		public GoogleOrMicrosoftTranslate()
		{

			InitializeComponent();

			FillComboWithGoogleLanguages(comboBoxFrom);
			FillComboWithGoogleLanguages(comboBoxTo);

			comboBoxFrom.SelectedIndex = comboBoxFrom.FindString("Eng");
			comboBoxTo.SelectedIndex = comboBoxTo.FindString("Ita");

			RemovedLanguagesNotInMicrosoftTranslate(comboBoxFrom);
			RemovedLanguagesNotInMicrosoftTranslate(comboBoxTo);

			//Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.Title;
			//labelGoogleTranslate.Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.GoogleTranslate;
			//labelFrom.Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.From;
			//labelTo.Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.To;
			//labelSourceText.Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.SourceText;
			//buttonTranslate.Text = Configuration.Settings.Language.GoogleOrMicrosoftTranslate.Translate;
			txtGoogle.Text = string.Empty;
		}

		private static void RemovedLanguagesNotInMicrosoftTranslate(ComboBox comboBox)
		{
			for (int i = comboBox.Items.Count - 1; i > 0; i--)
			{
				var item = (ComboBoxItem)comboBox.Items[i];
				if (item.Value != FixMsLocale(item.Value))
					comboBox.Items.RemoveAt(i);
			}
		}

		internal void InitializeFromLanguage(string defaultFromLanguage, string defaultToLanguage)
		{
			if (defaultFromLanguage == defaultToLanguage)
				defaultToLanguage = "en";

			int i = 0;
			comboBoxFrom.SelectedIndex = 0;
			foreach (ComboBoxItem item in comboBoxFrom.Items)
			{
				if (item.Value == defaultFromLanguage)
				{
					comboBoxFrom.SelectedIndex = i;
					break;
				}
				i++;
			}

			i = 0;
			comboBoxTo.SelectedIndex = 0;
			foreach (ComboBoxItem item in comboBoxTo.Items)
			{
				if (item.Value == defaultToLanguage)
				{
					comboBoxTo.SelectedIndex = i;
					break;
				}
				i++;
			}
		}

	 

		private void GoogleOrMicrosoftTranslate_Shown(object sender, EventArgs e)
		{
			Refresh();
			//Translate();
		}

		private void Translate()
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				if (comboBoxFrom.SelectedItem == null)
					return;
				string from = (comboBoxFrom.SelectedItem as ComboBoxItem).Value;
				string to = (comboBoxTo.SelectedItem as ComboBoxItem).Value;
				string languagePair = from + "|" + to;

				txtGoogle.Text = string.Empty;

				// google translate
				bool romanji = languagePair.EndsWith("|romanji", StringComparison.InvariantCulture);
				if (romanji)
					languagePair = from + "|ja";
				var screenScrapingEncoding = GetScreenScrapingEncoding(languagePair);
				var sbOut = new StringBuilder();
				var txt = txtSourceText.Text;
				var txtChunk = "";
				var lenght = 4500;
				while (txt.Length > 0)
				{
					if (txt.Length > lenght)
					{
						var chunk = txt.IndexOf(Environment.NewLine, lenght);
						if (chunk != -1)
						{
							txtChunk = txt.Substring(0, chunk);
							txt = txt.Remove(0, chunk + Environment.NewLine.Length);
						}
						else
						{
							txtChunk = txt;
							txt = "";
						}
					}
					else
					{
						txtChunk = txt;
						txt = "";
					}
					if (sbOut.Length == 0)
					{
						sbOut.Append(
							TranslateTextViaScreenScraping(
							txtChunk, languagePair, screenScrapingEncoding, romanji));

					}
					else
					{
						sbOut.AppendLine(
							TranslateTextViaScreenScraping(
						txtChunk, languagePair, screenScrapingEncoding, romanji));
					}
				}
				txtGoogle.Text = sbOut.ToString();

				//using (var gt = new GoogleTranslate())
				//{
				//	Subtitle subtitle = new Subtitle();
				//	subtitle.Paragraphs.Add(new Paragraph(0, 0, textBoxSourceText.Text));
				//	gt.Initialize(subtitle, string.Empty, false, System.Text.Encoding.UTF8);
				//	from = FixMsLocale(from);
				//	to = FixMsLocale(to);
				//	gt.DoMicrosoftTranslate(from, to);
				//	txtMicrosoft.Text = gt.TranslatedSubtitle.Paragraphs[0].Text;
				//}
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		/// <summary>
		/// Downloads the requested resource as a <see cref="String"/> using the configured <see cref="WebProxy"/>.
		/// </summary>
		/// <param name="address">A <see cref="String"/> containing the URI to download.</param>
		/// <param name="encoding">Encoding for source text</param>
		/// <returns>A <see cref="String"/> containing the requested resource.</returns>
		public static string DownloadString(string address, Encoding encoding = null)
		{
			using (var wc = new WebClient())
			{
				wc.Proxy = GetProxy();
				if (encoding != null)
					wc.Encoding = encoding;
				return wc.DownloadString(address).Trim();
			}
		}
		/// <summary>
		/// UrlEncodes a string without the requirement for System.Web
		/// </summary>
		public static string UrlEncode(string text)
		{
			return Uri.EscapeDataString(text);
		}
		public static WebProxy GetProxy()
		{
			//if (!string.IsNullOrEmpty(Configuration.Settings.Proxy.ProxyAddress))
			//{
			//	var proxy = new WebProxy(Configuration.Settings.Proxy.ProxyAddress);

			//	if (!string.IsNullOrEmpty(Configuration.Settings.Proxy.UserName))
			//	{
			//		if (string.IsNullOrEmpty(Configuration.Settings.Proxy.Domain))
			//			proxy.Credentials = new NetworkCredential(Configuration.Settings.Proxy.UserName, Configuration.Settings.Proxy.DecodePassword());
			//		else
			//			proxy.Credentials = new NetworkCredential(Configuration.Settings.Proxy.UserName, Configuration.Settings.Proxy.DecodePassword(), Configuration.Settings.Proxy.Domain);
			//	}
			//	else
			//		proxy.UseDefaultCredentials = true;

			//	return proxy;
			//}
			return null;
		}
		public static string TranslateTextViaApi(string input, string languagePair)
		{
			//string googleApiKey = "ABQIAAAA4j5cWwa3lDH0RkZceh7PjBTDmNAghl5kWSyuukQ0wtoJG8nFBxRPlalq-gAvbeCXMCkmrysqjXV1Gw";
			string googleApiKey = SettingGoogle.GoogleApiKey;

			input = input.Replace(Environment.NewLine, NewlineString);
			input = input.Replace("'", "&apos;");
			// create the web request to the Google Translate REST interface

			//API V 1.0
			var uri = new Uri("http://ajax.googleapis.com/ajax/services/language/translate?v=1.0&q=" + UrlEncode(input) + "&langpair=" + languagePair + "&key=" + googleApiKey);

			//API V 2.0 ?
			//string[] arr = languagePair.Split('|');
			//string from = arr[0];
			//string to = arr[1];
			//string url = String.Format("https://www.googleapis.com/language/translate/v2?key={3}&q={0}&source={1}&target={2}", HttpUtility.UrlEncode(input), from, to, googleApiKey);

			var request = WebRequest.Create(uri);
			request.Proxy = GetProxy();
			var response = request.GetResponse();
			var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
			string content = reader.ReadToEnd();

			var indexOfTranslatedText = content.IndexOf("{\"translatedText\":", StringComparison.Ordinal);
			if (indexOfTranslatedText >= 0)
			{
				var start = indexOfTranslatedText + 19;
				int end = content.IndexOf("\"}", start, StringComparison.Ordinal);
				string translatedText = content.Substring(start, end - start);
				string test = translatedText.Replace("\\u003c", "<");
				test = test.Replace("\\u003e", ">");
				test = test.Replace("\\u0026#39;", "'");
				test = test.Replace("\\u0026amp;", "&");
				test = test.Replace("\\u0026quot;", "\"");
				test = test.Replace("\\u0026apos;", "'");
				test = test.Replace("\\u0026lt;", "<");
				test = test.Replace("\\u0026gt;", ">");
				test = test.Replace("\\u003d", "=");
				test = test.Replace("\\u200b", string.Empty);
				test = test.Replace("\\\"", "\"");
				test = test.Replace(" <br/>", Environment.NewLine);
				test = test.Replace("<br/>", Environment.NewLine);
				test = RemovePStyleParameters(test);
				return test;
			}
			return string.Empty;
		}

		private static string RemovePStyleParameters(string test)
		{
			var startPosition = test.IndexOf("<p style", StringComparison.Ordinal);
			if (startPosition >= 0)
			{
				var endPosition = test.IndexOf('>', startPosition + 8);
				if (endPosition > 0)
					return test.Remove(startPosition + 2, endPosition - startPosition - 2);
			}
			return test;
		}

		public static Encoding GetScreenScrapingEncoding(string languagePair)
		{
			try
			{
				string url = String.Format(GoogleTranslateUrl + "?hl=en&eotf=1&sl={0}&tl={1}&q={2}", languagePair.Substring(0, 2), languagePair.Substring(3), "123 456");
				var result = DownloadString(url).ToLower();
				int idx = result.IndexOf("charset", StringComparison.Ordinal);
				int end = result.IndexOf('"', idx + 8);
				string charset = result.Substring(idx, end - idx).Replace("charset=", string.Empty);
				return Encoding.GetEncoding(charset); // "koi8-r");
			}
			catch
			{
				return Encoding.Default;
			}
		}
		private const string SplitterString = "\n\n\n";
		private const string NewlineString = "\n";
		internal class SettingGoogle
		{
			public static string GoogleTranslateUrl { get { return "translate.google.com"; }}

			public static string GoogleApiKey {
				get {
					return "ABQIAAAA4j5cWwa3lDH0RkZceh7PjBTDmNAghl5kWSyuukQ0wtoJG8nFBxRPlalq-gAvbeCXMCkmrysqjXV1Gw";
				}
			}
			public static bool UseGooleApiPaidService {
				get {
					return false;
				}
			}
			public static string GoogleTranslateLastTargetLanguage {
				get { return "en";
				}
			}
		}
		private static string GoogleTranslateUrl {
			get {
				var url = SettingGoogle.GoogleTranslateUrl;
				if (string.IsNullOrEmpty(url) || !url.Contains(".google."))
				{
					return "https://translate.google.com/";
				}
				if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
				{
					url = "https://" + url;
				}
				return url.TrimEnd('/') + '/';
			}
		}
		/// <summary>
		/// Translate Text using Google Translate API's
		/// Google URL - https://translate.google.com/?hl=en&amp;ie=UTF8&amp;text={0}&amp;langpair={1}
		/// </summary>
		/// <param name="input">Input string</param>
		/// <param name="languagePair">2 letter Language Pair, delimited by "|".
		/// E.g. "ar|en" language pair means to translate from Arabic to English</param>
		/// <param name="encoding">Encoding to use when downloading text</param>
		/// <param name="romanji">Get Romanjii text (made during Japanese) but in a separate div tag</param>
		/// <returns>Translated to String</returns>
		public static string TranslateTextViaScreenScraping(string input, string languagePair, Encoding encoding, bool romanji)
		{
			string url = string.Format(GoogleTranslateUrl + "?hl=en&eotf=1&sl={0}&tl={1}&q={2}", languagePair.Substring(0, 2), 
				languagePair.Substring(3), UrlEncode(input));
			var result = DownloadString(url, encoding);

			var sb = new StringBuilder();
			if (romanji)
			{
				int startIndex = result.IndexOf("<div id=res-translit", StringComparison.Ordinal);
				if (startIndex > 0)
				{
					startIndex = result.IndexOf('>', startIndex);
					if (startIndex > 0)
					{
						startIndex++;
						int endIndex = result.IndexOf("</div>", startIndex, StringComparison.Ordinal);
						string translatedText = result.Substring(startIndex, endIndex - startIndex);
						string test = WebUtility.HtmlDecode(translatedText);
						test = test.Replace("= =", SplitterString).Replace("  ", " ");
						test = test.Replace("_ _", NewlineString).Replace("  ", " ");
						sb.Append(test);
					}
				}
			}
			else
			{
				int startIndex = result.IndexOf("<span id=result_box", StringComparison.Ordinal);
				if (startIndex > 0)
				{
					startIndex = result.IndexOf("<span title=", startIndex, StringComparison.Ordinal);
					while (startIndex > 0)
					{
						startIndex = result.IndexOf('>', startIndex);
						while (startIndex > 0 && result.Substring(startIndex - 3, 4) == "<br>")
							startIndex = result.IndexOf('>', startIndex + 1);
						if (startIndex > 0)
						{
							startIndex++;
							int endIndex = result.IndexOf("</span>", startIndex, StringComparison.Ordinal);
							string translatedText = result.Substring(startIndex, endIndex - startIndex);
							string test = WebUtility.HtmlDecode(translatedText);
							sb.Append(test);
							startIndex = result.IndexOf("<span title=", startIndex, StringComparison.Ordinal);
						}
					}
				}
			}
			string res = sb.ToString();
			res = res.Replace("<br><br><br>", "|");
			res = res.Replace("\r\n", "\n");
			res = res.Replace("\r", "\n");
			res = res.Replace(NewlineString, Environment.NewLine);
			res = res.Replace("<BR>", Environment.NewLine);
			res = res.Replace("<BR />", Environment.NewLine);
			res = res.Replace("<BR/>", Environment.NewLine);
			res = res.Replace("< br />", Environment.NewLine);
			res = res.Replace("< br / >", Environment.NewLine);
			res = res.Replace("<br / >", Environment.NewLine);
			res = res.Replace(" <br/>", Environment.NewLine);
			res = res.Replace(" <br/>", Environment.NewLine);
			res = res.Replace("<br/>", Environment.NewLine);
			res = res.Replace("<br />", Environment.NewLine);
			res = res.Replace("<br>", Environment.NewLine);
			res = res.Replace("</ font>", "</font>");
			res = res.Replace("</ font >", "</font>");
			res = res.Replace("<font color = \"# ", "<font color=\"#");
			res = res.Replace("<font color = ", "<font color=");
			res = res.Replace("</ b >", "</b>");
			res = res.Replace("</ b>", "</b>");
			res = res.Replace("  ", " ").Trim();
			res = res.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
			res = res.Replace(Environment.NewLine + " ", Environment.NewLine);
			res = res.Replace(Environment.NewLine + " ", Environment.NewLine);
			res = res.Replace(" " + Environment.NewLine, Environment.NewLine);
			res = res.Replace(" " + Environment.NewLine, Environment.NewLine).Trim();
			int end = res.LastIndexOf("<p>", StringComparison.Ordinal);
			if (end > 0)
				res = res.Substring(0, end);
			return res;
		}
		public static bool ContainsLetter(string s)
		{
			if (s != null)
			{
				foreach (var index in StringInfo.ParseCombiningCharacters(s))
				{
					var uc = CharUnicodeInfo.GetUnicodeCategory(s, index);
					if (uc == UnicodeCategory.LowercaseLetter || uc == UnicodeCategory.UppercaseLetter || uc == UnicodeCategory.TitlecaseLetter || uc == UnicodeCategory.ModifierLetter || uc == UnicodeCategory.OtherLetter)
						return true;
				}
			}
			return false;
		}
		public static string Elabora(string txt)
		{
			//txt = txt.Replace("\r\n\r\n", "\r\n");
			var pages = Enumerable.Range(1, 1000)
				.Select(b => b.ToString()).ToList();

			var linesOut = new List<string>();
			var lines = txt.Split(new string[] { Environment.NewLine },
						StringSplitOptions.None).ToList();
			lines.RemoveAll(a => pages.Contains(a));

			var chapter = Enumerable.Range(1, 1000)
				.Select(b => "CHAPTER" + b.ToString()).ToList();

			if (lines.Count() > 0)
			{
				//StringBuilder sb = new StringBuilder();
				foreach (var item in lines)
				{
					if (linesOut.Count == 0)
					{
						linesOut.Add(item);
					}
					else
					{
						var dat = linesOut.Last().Trim();


						if (///non deve essere un capitolo
							!chapter.Contains(item)
							&& dat.Length > 0 && (ContainsLetter(dat.Substring(dat.Length - 1, 1))
							 || dat.Substring(dat.Length - 1, 1) == ",")
							//se la prima lettera della nuova riga è Maiuscolo 
							//allora nuova riga
							&& item.Length > 0
							&& ( //item.Substring(0, 1)=="(" || 
							(//item.Substring(0, 1).ContainsLetter() &&
							item == "GOD" || item.ToString().ToUpper() != item.ToString())))
						{
							linesOut.RemoveAt(linesOut.Count() - 1);
							linesOut.Add(dat.Trim() + " " + item.Trim());
						}
						else
						{
							linesOut.Add(item);
						}
					}
				}
				//linesOut = linesOut.Where(a => !pages.Contains(a)).ToList();
				var result = String.Join(Environment.NewLine, linesOut.ToArray());
				txt = result;
			}
			lines = txt.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();

			txt = txt.Replace("\r\n ", "\r\n");

			txt = txt.Replace("image", "");
			txt = txt.Replace(" .", ".");

			txt = txt.Replace(@"E '", @"È ");
			txt = txt.Replace(@"  ", @" ");
			txt = txt.Replace(" ,", ",");
			txt = txt.Replace(" ;", ";");

			return txt;
		}

		private static string FixMsLocale(string from)
		{
			if ("ar bg zh-CHS zh-CHT cs da nl en et fi fr de el ht he hu id it ja ko lv lt no pl pt ro ru sk sl es sv th tr uk vi".Contains(from))
				return from;
			return "en";
		}

		private void buttonTranslate_Click(object sender, EventArgs e)
		{
			Translate();
		}

		private void buttonGoogle_Click(object sender, EventArgs e)
		{
			TranslatedText = txtGoogle.Text;
			DialogResult = DialogResult.OK;
		}


		private void btnCopiaGoogle_Click(object sender, EventArgs e)
		{
			Clipboard.SetData(DataFormats.Text, txtGoogle.Text);
		}


		private void txtMicrosoft_TextChanged(object sender, EventArgs e)
		{

		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{

		}

		private void btnFormatSource_Click(object sender, EventArgs e)
		{
			txtSourceText.Text = Elabora(txtSourceText.Text);
		}
	}

	public class ComboBoxItem
	{
		public string Text { get; set; }
		public string Value { get; set; }

		public ComboBoxItem(string text, string value)
		{
			if (text.Length > 1)
				text = char.ToUpper(text[0]) + text.Substring(1).ToLower();
			Text = text;

			Value = value;
		}

		public override string ToString()
		{
			return Text;
		}
	}

}
