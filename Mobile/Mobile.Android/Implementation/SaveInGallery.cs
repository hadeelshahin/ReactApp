using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobile.Droid.Implementation;
using Mobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(SaveInGallery))]
namespace Mobile.Droid.Implementation
{
    public class SaveInGallery : ISaveInGallery
    {
        Context currentContext => Xamarin.Essentials.Platform.CurrentActivity;
        public void SaveImage(byte[] image, string filename)
        {
            var pathDCIM = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
            Java.IO.File file = new Java.IO.File(pathDCIM, "ReactApp");
            if(!file.Exists())
            {
                file.Mkdir();
            }
            string path = System.IO.Path.Combine(file.ToString(), filename + ".png");
            System.IO.File.WriteAllBytes(path, image);
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
            currentContext.SendBroadcast(mediaScanIntent);
        }
    }
}