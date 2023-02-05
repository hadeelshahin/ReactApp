using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mobile.Interfaces
{
    public interface IPrintService
    {
        void Print(WebView viewToPrint, string Name);
        string GetStyle();
    }
}
