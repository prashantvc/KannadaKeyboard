// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using Foundation;
using UIKit;
using System.CodeDom.Compiler;

namespace KannadaKeyboard
{
	[Register ("KannadaKeyboardViewController")]
	partial class KannadaKeyboardViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView MyTextField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (MyTextField != null) {
				MyTextField.Dispose ();
				MyTextField = null;
			}
		}
	}
}
