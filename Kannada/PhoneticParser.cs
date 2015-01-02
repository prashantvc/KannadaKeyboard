using System;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using Foundation;
using System.IO;
using System.Linq;
using System.Globalization;

namespace Kannada
{
	public class PhoneticParser
	{
		public bool PhoneticFlag { get; set; }

		public int PreviousConsonantFlaglog { get; set; }

		public int PreviousConsonantFlag { get; set; }

		public int CurrentConsonantFlag;

		public string Halant {
			get { return "್"; } // ್
		}

		public string PreviousChar;
		public StringBuilder tmp;

		public bool AFlag { get; set; }

		private readonly IList<Pattern> _patterns = new List<Pattern> ();

		public PhoneticParser ()
		{
			PreviousConsonantFlaglog = 0;
			var path = Path.Combine (NSBundle.MainBundle.BundlePath, "kan_phonetic.xml");
			Console.WriteLine (path);

			var rootElement = XElement.Load (path);
			Console.WriteLine ("Root emlement is null: {0}", rootElement == null);

			var patterns = rootElement.Descendants ("pattern");
			foreach (var p in patterns) {
				var item = new Pattern {
					Char = p.Element ("char").Value,
					Unicode = GetNativeChar (p.Element ("unicode").Value),
					Consonant = int.Parse (p.Element ("consonant").Value),
					Unicode2 = GetNativeChar (p.Element ("uni2") != null ? p.Element ("uni2").Value : string.Empty)
				};
				_patterns.Add (item);
			}
		}

		public void ResetConsonantFlag ()
		{
			PreviousConsonantFlag = PreviousConsonantFlaglog;
		}

		public KeyboardEvent GetPattern (string pattern)
		{
			var patternNode = _patterns.FirstOrDefault (p => p.Char == pattern);
			if (patternNode == null) {
				return null;
			}

			var kEvent = new KeyboardEvent ();

			CurrentConsonantFlag = patternNode.Consonant;

			if (PreviousConsonantFlag == 1
			    && CurrentConsonantFlag == 0) {

				kEvent.SetData (patternNode.Unicode2, 1);
				AFlag = pattern == "a";
			} else {
				kEvent.SetData (patternNode.Unicode, 0);
				tmp = new StringBuilder (kEvent.Char);
				if (CurrentConsonantFlag != 0) {
					if (PreviousChar == "t" && pattern == "h" && PreviousConsonantFlag == 1) {
						var value = GetUnicode ("th");
						kEvent.SetData (value, 2);
					}

					if (PreviousChar == "T" && pattern == "h" && PreviousConsonantFlag == 1) {
						//TODO:Double delete
						var value = GetUnicode ("Th");
						kEvent.SetData (value, 2);
					}

					if (PreviousChar == "s" && pattern == "h" && PreviousConsonantFlag == 1) {
						//TODO:Double delete
						var value = GetUnicode ("sh");
						kEvent.SetData (value, 2);
					}

					if (PreviousChar == "S" && pattern == "h" && PreviousConsonantFlag == 1) {
						//TODO:Double delete
						var value = GetUnicode ("Sh");
						kEvent.SetData (value, 2);
					}

					if (PreviousChar == "d" && pattern == "h" && PreviousConsonantFlag == 1) {
						//TODO:Double delete
						var value = GetUnicode ("dh");
						kEvent.SetData (value, 2);
					}

					if (PreviousChar == "D" && pattern == "h" && PreviousConsonantFlag == 1) {
						//TODO:Double delete
						var value = GetUnicode ("Dh");
						kEvent.SetData (value, 2);
					}

					tmp = new StringBuilder (kEvent.Char);
					if (pattern != "M") {
						tmp.Append (Halant);
					}
				}
				kEvent.Char = tmp.ToString ();
			}

			PreviousConsonantFlaglog = PreviousConsonantFlag;
			PreviousConsonantFlag = CurrentConsonantFlag;
			PreviousChar = pattern;

			return kEvent;
		}



		public string GetUnicode (string temp)
		{
			var node = _patterns.FirstOrDefault (p => p.Char == temp);
			return node == null ? string.Empty : node.Unicode;
		}

		string GetNativeChar (string unicode)
		{
			int code;
			bool success = int.TryParse (unicode, NumberStyles.HexNumber, null, out code);
			if (!success) {
				return string.Empty;
			}

			string unicodeString = char.ConvertFromUtf32 (code);
			return unicodeString;
		}
	}

	public class Pattern
	{
		public string Char { get; set; }

		public string Unicode { get; set; }

		public int Consonant { get; set; }

		public string Unicode2 { get; set; }
	}

	public class KeyboardEvent
	{
		public string Char {
			get;
			set;
		}

		public int DeletePosition {
			get;
			private set;
		}

		public void SetData (string character, int deletePosition)
		{
			Char = character;
			DeletePosition = deletePosition;
		}

		public KeyboardEvent (string character, int deletePosition)
		{
			Char = character;
			DeletePosition = deletePosition;
		}

		public KeyboardEvent ()
		{
			
		}

		public override string ToString ()
		{
			return string.Format ("[KeyboardEvent: Char={0}, DeletePosition={1}]", Char, DeletePosition);
		}
	}
}

