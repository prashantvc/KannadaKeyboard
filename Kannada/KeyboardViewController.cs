using System;

using Foundation;
using UIKit;
using CoreGraphics;

namespace Kannada
{
	public partial class KeyboardViewController : UIInputViewController
	{
		public KeyboardViewController (IntPtr handle) : base (handle)
		{
		}

	
		void UpdateKeyboardLayout ()
		{
			if (isShiftPressed) {
				SetKeyTitle (Row1, row1titles);
				SetKeyTitle (Row2, row2titles);
				SetKeyTitle (Row3, row3titles);
			} else {
				SetKeyTitle (Row1, normalRow1);
				SetKeyTitle (Row2, normalRow2);
				SetKeyTitle (Row3, normalRow3);
			}
			SetRemainging ();
		}

		bool IsPhoneticEnabled{ get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var defaults = new NSUserDefaults ("group.prashantvc.KannadaKeyboard", NSUserDefaultsType.SuiteName);
			var obj = defaults.ValueForKey (new NSString ("use_phonetic"));
			Console.WriteLine ("Use Phonetic {0}", obj);

			IsPhoneticEnabled = (bool)((NSNumber)obj);

			Console.WriteLine ("Is phonetic: {0}", IsPhoneticEnabled);

			var nibfile = IsPhoneticEnabled ? "KanndaPhoneticView" : "KeyboardView";
			var nib = UINib.FromName (nibfile, null); 
			var objects = nib.Instantiate (this, null);
			View = objects [0] as UIView;

			if (IsPhoneticEnabled) {
				parser = new PhoneticParser ();
			}

			if (Shift != null) {
				Shift.TouchUpInside += ( sender, e) => {
					UpdateShiftText (!isShiftPressed);
					if (!IsPhoneticEnabled) {
						UpdateKeyboardLayout ();
					}
				};
			}
		}

		void SetRemainging ()
		{
			var title = isShiftPressed ? row4titles : normalRow4;
			for (int i = 1; i <= 9; i++) {
				var button = Row4.Subviews [i] as UIButton;
				button.SetTitle (title [i], UIControlState.Normal);
			}
		}

		void SetKeyTitle (UIView row, string[] titles)
		{
			int i = 0;
			foreach (UIButton item in row) {
				item.SetTitle (titles [i++], UIControlState.Normal);
				Console.WriteLine ("{0} {1}", i, item.Title (UIControlState.Normal));
			}
		}

		void UpdateShiftText (bool ispressed)
		{
			isShiftPressed = ispressed;

			Shift.BackgroundColor = ispressed ? buttonTextColor : UIColor.White;
			Shift.SetTitleColor (ispressed ? UIColor.White : buttonTextColor, UIControlState.Normal);
		}

		partial void ChangeKeyboardPressed (NSObject sender)
		{
			AdvanceToNextInputMode ();
		}

		partial void BackspacePressed (NSObject sender)
		{
			TextDocumentProxy.DeleteBackward ();
			if (IsPhoneticEnabled) {
				parser.ResetConsonantFlag ();
			}
		}

		partial void ReturnPressed (NSObject sender)
		{
			TextDocumentProxy.InsertText ("\n");
			if (IsPhoneticEnabled) {
				parser.PreviousConsonantFlag = 0;
			}
		}

		partial void SpacePressed (NSObject sender)
		{
			TextDocumentProxy.InsertText (" ");
			if (IsPhoneticEnabled) {
				parser.PreviousConsonantFlag = 0;
			}
		}

		partial void PhonaticKeyPress (NSObject sender)
		{
			var button = sender as UIButton;
			var text = button.Title (UIControlState.Normal);

			text = isShiftPressed ? text : text.ToLowerInvariant ();

			var unicode = isEnglishEnabled ? new KeyboardEvent (text, 0) : parser.GetPattern (text);

			for (int i = 0; i < unicode.DeletePosition; i++) {
				TextDocumentProxy.DeleteBackward ();
			}

			if (parser.AFlag) {
				parser.AFlag = false;
				unicode.Char = string.Empty;
			}

			TextDocumentProxy.InsertText (unicode.Char);
			UpdateShiftText (false);
			AnimateButton (button);
		}

		void EnterSymbol(UIButton button){

		}

		static void AnimateButton (UIButton button)
		{
			UIView.Animate (0.1, () => {
				button.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), 2f, 2f);
			}, () => {
				button.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), (nfloat)1f, (nfloat)1f);
			});
		}

		partial void KeyPress (NSObject sender)
		{
			var button = sender as UIButton;
			var text = button.Title (UIControlState.Normal);
			Console.WriteLine (text);

			if (!string.IsNullOrEmpty (text)) {
				TextDocumentProxy.InsertText (text);

				if (isShiftPressed) {
					UpdateShiftText (false);
					UpdateKeyboardLayout ();
				}
			}

			AnimateButton (button);
		}

		partial void ToggleIndic (NSObject sender)
		{
			isEnglishEnabled = !isEnglishEnabled;
			ToggleLanguage.SetTitle (isEnglishEnabled ? "ಅ" : "EN", UIControlState.Normal);
		}

		PhoneticParser parser;
		bool isShiftPressed;
		bool isEnglishEnabled;

		UIColor buttonTextColor = UIColor.FromRGBA (0.196f, 0.3098f, 0.52f, 1f);

		string[] row1titles = { "#", "್ರ", "ರ್", "ಜ್ಞ", "ತ್ರ", "ಕ್ಷ", "ಶ್ರ", "(", ")", "ಃ", "ಋ" };
		string[] row2titles = { "ಔ", "ಐ", "ಆ", "ಈ", "ಊ", "ಭ", "ಙ", "ಘ", "ಧ", "ಝ", "ಢ" };
		string[] row3titles = { "ಓ", "ಏ", "ಅ", "ಇ", "ಉ", "ಫ", "ಱ", "ಖ", "ಥ", "ಛ", "ಠ" };
		string[] row4titles = { "", "ಎ", "ಣ", "ಞ", "ೢ", "ಳ", "ಶ", "ಷ", "ಒ", "ೣ", "" };

		string[] normalRow1 = { "೧", "೨", "೩", "೪", "೫", "೬", "೭", "೮", "೯", "೦", "-" };
		string[] normalRow2 =	{ "ೌ", "ೈ", "ಾ", "ೀ", "ೂ", "ಬ", "ಹ", "ಗ", "ದ", "ಜ", "ಡ" };
		string[] normalRow3 = { "ೋ", "ೇ", "್", "ಿ", "ು", "ಪ", "ರ", "ಕ", "ತ", "ಚ", "ಟ" };
		string[] normalRow4 = { "s", "ೆ", "ಂ", "ಮ", "ನ", "ವ", "ಲ", "ಸ", "ಯ", "ೃ", "b" };

		string[] phoneticNumber = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"};
		string[] phoneticSymbol = { "!", "@", "#", "₹", "&", "*", "(", ")", "-", "."};

	}
}

