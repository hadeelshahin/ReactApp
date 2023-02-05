using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Interfaces
{
    public interface ISaveInGallery
    {
        void SaveImage(byte[] image, string filename);
    }
}
