using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using AndroidPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: Xamarin.Forms.Dependency(typeof(Mobile.Droid.Implementation.DroidPrintService))]
namespace Mobile.Droid.Implementation
{
    public class DroidPrintService : IPrintService
    {
        public DroidPrintService()
        {
        }

        public string GetStyle()
        {
            return "file:///android_asset/primer.css";
        }

        public void Print(WebView viewToPrint, string Name)
        {
            var droidViewToPrint = AndroidPlatform.CreateRenderer(viewToPrint).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

			if (droidViewToPrint != null)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Platform.CurrentActivity.GetSystemService(Context.PrintService);

                    printMgr.Print("React App", droidViewToPrint.CreatePrintDocumentAdapter(Name), new PrintAttributes.Builder().Build());
                }
            }
        }
    }
}