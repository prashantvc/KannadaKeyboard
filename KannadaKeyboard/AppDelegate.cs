
using Foundation;
using UIKit;
using MonoTouch.Dialog;

using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

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

			MobileCenter.Start ("3e5e305a-d1b1-405a-9233-9b917f658437",
					typeof (Analytics), typeof (Crashes));


			var section = new Section ("Keyboard Type");

			webElement = new WebElement ();

			var root = new RootElement ("ಕಿಲೀಮಣಿ ") { 
				new Section ("To Enable Keyboard") {
					new StyledStringElement ("Instructions", () => {
						webElement.SetPageTitle ("Instructions");
						webElement.HtmlFile = "instructions";
						dv.NavigationController.PushViewController (webElement, true);
					}) { Accessory = UITableViewCellAccessory.DisclosureIndicator }
				},
				section,
				new Section {
					new StyledStringElement ("Privacy Policy", () => {
						webElement.SetPageTitle ("Privacy Policy");
						webElement.HtmlFile = "PrivatePolicy";
						dv.NavigationController.PushViewController (webElement, true);
					}) { 
						Accessory = UITableViewCellAccessory.DisclosureIndicator 
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
			window.MakeKeyAndVisible ();
			window.AddSubview (navigation.View);

			return true;
		}
	}

}

