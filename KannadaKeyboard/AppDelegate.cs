﻿
using Foundation;
using UIKit;
using MonoTouch.Dialog;



namespace KannadaKeyboard
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		
		UINavigationController navigation;
		UIWindow window;

		RadioElement phonetic;

		DialogViewController dv;
		WebElement webElement;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			var section = new Section ("Keyboard Type");

			webElement = new WebElement ();

			var root = new RootElement ("ಕಿಲೀಮಣಿ ") { 
				new Section ("To Enable Keyboard") {
					new StyledStringElement ("Instructions", () => {
						webElement.SetPageTitle ("Instructions");
						webElement.HtmlFile = "instructions";
						dv.NavigationController.PushViewController (webElement, true);
					}) {
                        Accessory = UITableViewCellAccessory.DisclosureIndicator,
                        TextColor = UIColor.LabelColor
                    }
				},
				section,
				new Section {
					new StyledStringElement ("Privacy Policy", () => {
						webElement.SetPageTitle ("Privacy Policy");
						webElement.HtmlFile = "PrivatePolicy";
						dv.NavigationController.PushViewController (webElement, true);
					}) { 
						Accessory = UITableViewCellAccessory.DisclosureIndicator ,
                        TextColor = UIColor.LabelColor
                    }
				}
			};

			//inscript = new RadioElement ("Inscript", "type", defaults);
			phonetic = new RadioElement ("Phonetic (ಫೊನೆಟಿಕ್)", "type");


			section.Add (new RootElement ("Type", new RadioGroup ("type", 0)) {
				new Section ("More to come...") {
					phonetic
				}
			});

			dv = new DialogViewController (root) {
				Autorotate = true
			};
			navigation = new UINavigationController ();
			navigation.PushViewController (dv, true);				

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = navigation;
            window.MakeKeyAndVisible ();
			return true;
		}
	}

}

