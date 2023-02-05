using Foundation;
using Mobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(Mobile.iOS.Implementation.iOSPrintService))]
namespace Mobile.iOS.Implementation
{
    public class iOSPrintService : IPrintService
    {
        public iOSPrintService()
        {
        }

        public string GetStyle()
        {
            return NSBundle.MainBundle.BundlePath + "primer.css";
        }

        public void Print(WebView viewToPrint, string Name)
        {
            var appleViewToPrint = Platform.CreateRenderer(viewToPrint).NativeView;

            var printInfo = UIPrintInfo.PrintInfo;

            printInfo.OutputType = UIPrintInfoOutputType.General;
            printInfo.JobName = "React App";
            printInfo.Orientation = UIPrintInfoOrientation.Portrait;
            printInfo.Duplex = UIPrintInfoDuplex.None;

            var printController = UIPrintInteractionController.SharedPrintController;

            printController.PrintInfo = printInfo;
            printController.ShowsPageRange = true;
            printController.PrintFormatter = appleViewToPrint.ViewPrintFormatter;

            printController.Present(true, (printInteractionController, completed, error) => { });
        }
    }
}