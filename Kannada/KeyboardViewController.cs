using System;

using Foundation;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace Kannada
{
	public partial class KeyboardViewController : UIInputViewController
	{
		public KeyboardViewController (IntPtr handle) : base (handle)
		{
		}


		public override void UpdateViewConstraints ()
		{
			base.UpdateViewConstraints ();

			// Add custom view sizing constraints here
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var nib = UINib.FromName ("KeyboardView", null); //UINib("KeyboardView", bundle: nil)
			var objects = nib.Instantiate (this, null);
			View = objects [0] as UIView;
		}

		partial void ChangeKeyboardPressed (NSObject sender)
		{
			AdvanceToNextInputMode();
		}

		partial void BackspacePressed (NSObject sender)
		{
			TextDocumentProxy.DeleteBackward();
		}

		partial void ReturnPressed (NSObject sender)
		{
			TextDocumentProxy.InsertText("\n");
		}

		partial void SpacePressed (NSObject sender)
		{
			TextDocumentProxy.InsertText(" ");
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

		string[][] keyArray = {
			{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-" },
			{ "ೌ", "ೈ", "ಾ", "ೀ", "ೂ", "ಬ", "ಹ", "ಗ", "ದ", "ಜ", "ಡ" },
			{ "ೋ", "ೇ", "್", "ಿ", "ು", "ಪ", "ರ", "ಕ", "ತ", "ಚ", "ಟ" },
			{ "s", "ೆ", "ಂ", "ಮ", "ನ", "ವ", "ಲ", "ಸ", "ಯ", "ೃ", "b" }
		};
	}
}

