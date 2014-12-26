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


		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var nib = UINib.FromName ("KanndaPhoneticView", null); //UINib("KeyboardView", bundle: nil)
			var objects = nib.Instantiate (this, null);
			View = objects [0] as UIView;

			parser = new PhoneticParser ();
			Console.WriteLine (parser);

			if (Shift != null) {
				Shift.TouchUpInside += ( sender, e) => {
					UpdateShiftText (!isShiftPressed);
					UpdateKeyboardLayout ();
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
			//Shift.SetTitle (ispressed ? "S" : "s", UIControlState.Normal);
			Shift.BackgroundColor = ispressed ? UIColor.GroupTableViewBackgroundColor : UIColor.White;
		}

		partial void ChangeKeyboardPressed (NSObject sender)
		{
			AdvanceToNextInputMode ();
		}

		partial void BackspacePressed (NSObject sender)
		{
			TextDocumentProxy.DeleteBackward ();
			if (parser != null) {
				parser.ResetConsonantFlag();
			}
		}

		partial void ReturnPressed (NSObject sender)
		{
			TextDocumentProxy.InsertText ("\n");
			if (parser != null) {
				parser.PreviousConsonantFlag = 0;
			}
		}

		partial void SpacePressed (NSObject sender)
		{
			TextDocumentProxy.InsertText (" ");
			if (parser != null) {
				parser.PreviousConsonantFlag = 0;
			}
		}

		partial void PhonaticKeyPress (NSObject sender)
		{
			var button = sender as UIButton;
			var text = button.Title (UIControlState.Normal);
			var unicode = parser.GetPattern (text);
			Console.WriteLine (unicode);
			for (int i = 0; i < unicode.DeletePosition; i++) {
				TextDocumentProxy.DeleteBackward ();
			}
			if (parser.AFlag) {
				parser.AFlag = false;
				return;
			}
			TextDocumentProxy.InsertText (unicode.Char);
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
			 

//			UIView.animateWithDuration(0.2, animations: {
//				button.transform = CGAffineTransformScale(CGAffineTransformIdentity, 2.0, 2.0)
//			}, completion: {(_) -&gt; Void in
//				button.transform =
//					CGAffineTransformScale(CGAffineTransformIdentity, 1, 1)
//				})


			UIView.Animate (0.2, () => {
				button.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), 2f, 2f);
			}, () => {
				button.Transform = CGAffineTransform.Scale (CGAffineTransform.MakeIdentity (), (nfloat)1f, (nfloat)1f);
			});
		}

		public override void TextWillChange (NSObject textInput)
		{
			// The app is about to change the document's contents. Perform any preparation here.
		}

		public override void TextDidChange (NSObject textInput)
		{
			// 
		}

		PhoneticParser parser;
		bool isShiftPressed;

		string[] row1titles = { "#", "್ರ", "ರ್", "ಜ್ಞ", "ತ್ರ", "ಕ್ಷ", "ಶ್ರ", "(", ")", "ಃ", "ಋ" };
		string[] row2titles = { "ಔ", "ಐ", "ಆ", "ಈ", "ಊ", "ಭ", "ಙ", "ಘ", "ಧ", "ಝ", "ಢ" };
		string[] row3titles = { "ಓ", "ಏ", "ಅ", "ಇ", "ಉ", "ಫ", "ಱ", "ಖ", "ಥ", "ಛ", "ಠ" };
		string[] row4titles = { "", "ಎ", "ಣ", "ಞ", "ೢ", "ಳ", "ಶ", "ಷ", "ಒ", "ೣ", "" };

		string[] normalRow1 = { "೧", "೨", "೩", "೪", "೫", "೬", "೭", "೮", "೯", "೦", "-" };
		string[] normalRow2 =	{ "ೌ", "ೈ", "ಾ", "ೀ", "ೂ", "ಬ", "ಹ", "ಗ", "ದ", "ಜ", "ಡ" };
		string[] normalRow3 = { "ೋ", "ೇ", "್", "ಿ", "ು", "ಪ", "ರ", "ಕ", "ತ", "ಚ", "ಟ" };
		string[] normalRow4 = { "s", "ೆ", "ಂ", "ಮ", "ನ", "ವ", "ಲ", "ಸ", "ಯ", "ೃ", "b" };

	}
}

