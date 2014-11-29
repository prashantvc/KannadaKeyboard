using System;

using ObjCRuntime;
using Foundation;
using UIKit;
using CoreGraphics;
using System.Collections.Generic;

namespace Kannada
{
	public partial class KeyboardViewController : UIInputViewController
	{
		UIButton nextKeyboardButton;

		public KeyboardViewController (IntPtr handle) : base (handle)
		{
		}


		public override void UpdateViewConstraints ()
		{
			base.UpdateViewConstraints ();

			// Add custom view sizing constraints here
		}

		UIButton[] buttons;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

//			var buttonTitles = new[] { "ಪ", "್", "ರ", "ಶ", "ಾ", "ಂ", "ತ" };
//			buttons = CreateButtons (buttonTitles);
//			var topRow = new UIView (CGRect.FromLTRB (0, 0, 320, 40));
//
//			topRow.AddSubviews (buttons);
//			View.AddSubview (topRow);
//
		//	AddConstraints (buttons, topRow);



			var nib = UINib.FromName ("KeyboardView", null); //UINib("KeyboardView", bundle: nil)
			var objects = nib.Instantiate (this, null);
			View = objects [0] as UIView;

			// Perform custom UI setup here
			nextKeyboardButton = new UIButton (UIButtonType.System);

			nextKeyboardButton.SetTitle ("Next Keyboard", UIControlState.Normal);
			nextKeyboardButton.SizeToFit ();
			nextKeyboardButton.TranslatesAutoresizingMaskIntoConstraints = false;

			nextKeyboardButton.AddTarget (this, new Selector ("advanceToNextInputMode"), UIControlEvent.TouchUpInside);

			View.AddSubview (nextKeyboardButton);

			var nextKeyboardButtonLeftSideConstraint = NSLayoutConstraint.Create (nextKeyboardButton, NSLayoutAttribute.Left, NSLayoutRelation.Equal, View, NSLayoutAttribute.Left, 1.0f, 0.0f);
			var nextKeyboardButtonBottomConstraint = NSLayoutConstraint.Create (nextKeyboardButton, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, View, NSLayoutAttribute.Bottom, 1.0f, 0.0f);
			View.AddConstraints (new [] {
				nextKeyboardButtonLeftSideConstraint,
				nextKeyboardButtonBottomConstraint
			});
		}

		UIButton[] CreateButtons (string[] buttonTitles)
		{

			var buttonList = new List<UIButton> ();
			foreach (var title in buttonTitles) {
				var button = UIButton.FromType (UIButtonType.System);
				button.SetTitle (title, UIControlState.Normal);
				button.TranslatesAutoresizingMaskIntoConstraints = false;
				button.BackgroundColor = UIColor.White;
				button.SetTitleColor (UIColor.DarkGray, UIControlState.Normal);
				button.AddTarget (ButtonPressed, UIControlEvent.TouchUpInside);
				buttonList.Add (button);
			}

			return buttonList.ToArray ();
		}

		void ButtonPressed (object sender, EventArgs e)
		{
			var button = sender as UIButton;
			var text = button.Title (UIControlState.Normal);
			Console.WriteLine (text);

			if (!string.IsNullOrEmpty (text)) {
				TextDocumentProxy.InsertText (text);
			}
		}

		void AddConstraints (UIButton[] keys, UIView containingView)
		{
			for (int i = 0; i < keys.Length; i++) {

				var key = keys [i];
				var topConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Top, NSLayoutRelation.Equal, containingView, NSLayoutAttribute.Top, 1, 1);
				var bottomConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, containingView, NSLayoutAttribute.Bottom, 1, -1);

				NSLayoutConstraint leftConstraint;
				if (i == 0) {
					leftConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Left, NSLayoutRelation.Equal, containingView, NSLayoutAttribute.Left, 1, 1);
				} else {
					leftConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Left, NSLayoutRelation.Equal, keys [i - 1], NSLayoutAttribute.Right, 1, 1);
					var widthConstraint = NSLayoutConstraint.Create (keys [0], NSLayoutAttribute.Width, NSLayoutRelation.Equal, key, NSLayoutAttribute.Width, 1, 0);
					containingView.AddConstraint (widthConstraint);
				}

				NSLayoutConstraint rightConstraint;

				if (i == keys.Length - 1) {
					rightConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Right, NSLayoutRelation.Equal, containingView, NSLayoutAttribute.Right, 1, -1);
				} else {
					rightConstraint = NSLayoutConstraint.Create (key, NSLayoutAttribute.Right, NSLayoutRelation.Equal, keys [i + 1], NSLayoutAttribute.Left, 1, -1);
				}
				
				containingView.AddConstraints (new[]{ topConstraint, bottomConstraint, rightConstraint, leftConstraint });
			}
		}

		public override void TextWillChange (NSObject textInput)
		{
			// The app is about to change the document's contents. Perform any preparation here.
		}

		public override void TextDidChange (NSObject textInput)
		{
			// The app has just changed the document's contents, the document context has been updated.
			UIColor textColor = null;

			if (TextDocumentProxy.KeyboardAppearance == UIKeyboardAppearance.Dark) {
				textColor = UIColor.White;
			} else {
				textColor = UIColor.Black;
			}

			nextKeyboardButton.SetTitleColor (textColor, UIControlState.Normal);
		}
	}
}

