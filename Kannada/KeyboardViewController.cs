using System;

using Foundation;
using UIKit;

namespace Kannada
{
	public partial class KeyboardViewController : UIInputViewController
	{
		public KeyboardViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var nib = UINib.FromName ("KeyboardView", null); //UINib("KeyboardView", bundle: nil)
			var objects = nib.Instantiate (this, null);
			View = objects [0] as UIView;

			Shift.TouchUpInside += ( sender, e) => {
				UpdateShiftText (!isShiftPressed);
				if (isShiftPressed) {
					SetKeyTitle (Row2, row2titles);
					SetKeyTitle (Row3, row3titles);
					//SetKeyTitle(Row4, row4titles);
				} else {
					SetKeyTitle (Row2, normalRow2);
					SetKeyTitle (Row3, normalRow3);
				}

				SetRemainging();
			};
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
			Shift.SetTitle (ispressed ? "S" : "s", UIControlState.Normal);
		}

		partial void ChangeKeyboardPressed (NSObject sender)
		{
			AdvanceToNextInputMode ();
		}

		partial void BackspacePressed (NSObject sender)
		{
			TextDocumentProxy.DeleteBackward ();
		}

		partial void ReturnPressed (NSObject sender)
		{
			TextDocumentProxy.InsertText ("\n");
		}

		partial void SpacePressed (NSObject sender)
		{
			TextDocumentProxy.InsertText (" ");
		}

		partial void KeyPress (NSObject sender)
		{
			var button = sender as UIButton;
			var text = button.Title (UIControlState.Normal);
			Console.WriteLine (text);

			if (!string.IsNullOrEmpty (text)) {
				TextDocumentProxy.InsertText (text);
			}

		}

		public override void TextWillChange (NSObject textInput)
		{
			// The app is about to change the document's contents. Perform any preparation here.
		}

		public override void TextDidChange (NSObject textInput)
		{
			// 
		}

		bool isShiftPressed;
		bool isCapsLocked;

		string[] row2titles = { "ಔ", "ಐ", "ಆ", "ಈ", "ಊ", "ಭ", "ಙ", "ಘ", "ಧ", "ಝ", "ಢ" };
		string[] row3titles = { "ಓ", "ಏ", "ಅ", "ಇ", "ಉ", "ಫ", "ಱ", "ಖ", "ಥ", "ಛ", "ಠ" };
		string[] row4titles = { "", "ಎ", "ಣ", "ಞ", "", "ಳ", "ಶ", "ಷ", "ಒ", "", "" };


		string[] normalRow1 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-" };
		string[] normalRow2 =	{ "ೌ", "ೈ", "ಾ", "ೀ", "ೂ", "ಬ", "ಹ", "ಗ", "ದ", "ಜ", "ಡ" };
		string[] normalRow3 = { "ೋ", "ೇ", "್", "ಿ", "ು", "ಪ", "ರ", "ಕ", "ತ", "ಚ", "ಟ" };
		string[] normalRow4 = { "s", "ೆ", "ಂ", "ಮ", "ನ", "ವ", "ಲ", "ಸ", "ಯ", "ೃ", "b" };

	}
}

