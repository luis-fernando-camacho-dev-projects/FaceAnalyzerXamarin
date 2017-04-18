using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace facereconigze.WinPhone
{
    public sealed partial class Dialog : UserControl
    {
        public Dialog()
        {
            this.InitializeComponent();
            //// Change the background code using the same that the phone's theme (light or dark).
            //this.panelSplashScreen.Background =
            //  new SolidColorBrush((Color)new facereconigze.WinPhone.App().Resources["PhoneBackgroundColor"]);

            //// Adjust the code to the width of the actual screen
            //this.progressBar1.Width = this.panelSplashScreen.Width =
            //  Application.Current.Host.Content.ActualWidth;
            //Application.Current.Resources[""]
        }
    }
}
