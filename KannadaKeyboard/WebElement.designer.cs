// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace KannadaKeyboard
{
	[Register ("WebElement")]
	partial class WebElement
	{
		[Outlet]
		UIKit.UIWebView InstructionWebView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (InstructionWebView != null) {
				InstructionWebView.Dispose ();
				InstructionWebView = null;
			}
		}
	}
}