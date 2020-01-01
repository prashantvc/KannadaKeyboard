// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace KeyboardExtension
{
    [Register ("KeyboardViewController")]
    partial class KeyboardViewController
    {
        [Outlet]
        UIKit.UIView Row1 { get; set; }


        [Outlet]
        UIKit.UIView Row2 { get; set; }


        [Outlet]
        UIKit.UIView Row3 { get; set; }


        [Outlet]
        UIKit.UIView Row4 { get; set; }


        [Outlet]
        UIKit.UIView Row5 { get; set; }


        [Outlet]
        UIKit.UIButton Shift { get; set; }


        [Outlet]
        UIKit.UIButton ToggleLanguage { get; set; }


        [Action ("BackspacePressed:")]
        partial void BackspacePressed (Foundation.NSObject sender);


        [Action ("ChangeKeyboardPressed:")]
        partial void ChangeKeyboardPressed (Foundation.NSObject sender);


        [Action ("KeyPress:")]
        partial void KeyPress (Foundation.NSObject sender);


        [Action ("PhonaticKeyPress:")]
        partial void PhonaticKeyPress (Foundation.NSObject sender);


        [Action ("ReturnPressed:")]
        partial void ReturnPressed (Foundation.NSObject sender);


        [Action ("SpacePressed:")]
        partial void SpacePressed (Foundation.NSObject sender);


        [Action ("ToggleIndic:")]
        partial void ToggleIndic (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
        }
    }
}