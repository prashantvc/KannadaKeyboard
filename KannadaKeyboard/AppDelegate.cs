
using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Xamarin;

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

		MyRadioElement inscript;
		MyRadioElement phonetic;
		NSUserDefaults defaults;

		DialogViewController dv;
		WebElement webElement;

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			defaults = new NSUserDefaults ("group.prashantvc.KannadaKeyboard", NSUserDefaultsType.SuiteName);
			var dictionary = NSDictionary.FromObjectsAndKeys (new object[]{ false }, new object[]{ "use_phonetic" });
			defaults.RegisterDefaults (dictionary);
			defaults.Synchronize ();

			Insights.Initialize ("abc0939e38294cf24ff181959a2a33441215dc7b");

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

			inscript = new MyRadioElement ("Inscript", "type", defaults);
			phonetic = new MyRadioElement ("Phonetic (ಫೊನೆಟಿಕ್)", "type", defaults);

			var obj = (NSNumber)defaults.ValueForKey (new NSString ("use_phonetic"));
			bool isPhoneticEnabled = (bool)obj;

			section.Add (new RootElement ("Type", new RadioGroup ("type", isPhoneticEnabled ? 1 : 0)) {
				new Section {
					inscript,
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

	public class MyRadioElement:RadioElement
	{
		readonly NSUserDefaults defaults;

		public MyRadioElement (string caption, string group, NSUserDefaults defaults) : base (caption, group)
		{
			this.defaults = defaults;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath indexPath)
		{
			base.Selected (dvc, tableView, indexPath);
			bool phonetic = indexPath.Row == 1;
			defaults.SetBool (phonetic, "use_phonetic");

			Insights.Track ("use_phonetic", "enabled", phonetic.ToString());
		}
	}

}

