

using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace facereconigze
{
    public class MyClass
    {
        public MyClass()
        {
            

        }

            public async Task<Stream> LoadPhoto( string path,   string albumPath)
            {
                PickMediaOptions option = new PickMediaOptions();
                MediaFile media= await CrossMedia.Current.PickPhotoAsync(option);
                path = media.Path;
                return media.GetStream();
            }

        
    }
}

