
using facereconigze.FaceServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace facereconigze.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int count = 1;
        public MainPage()
        {

            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;


        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            btn_loadImage.Click += Btn_loadImage_Click; ;
        }

        private async void Btn_loadImage_Click(object sender, RoutedEventArgs e)
        {
            MSCognitiveServices cognitiveServices = new MSCognitiveServices();
            Dictionary<string, string> faceProperties = new Dictionary<string, string>();
            string path = string.Empty;
            string album = string.Empty;
            Stream sourceImage = await cognitiveServices.LoadPhoto(path, album);
            myProgressRing.Visibility = Visibility.Visible;
            btn_loadImage.Visibility = Visibility.Collapsed;

            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream faceMemory = new MemoryStream();
            sourceImage.CopyTo(faceMemory);
            faceMemory.Position = 0;
            //sourceImage.Position = 0; // to copy again
            //sourceImage.Seek(0, SeekOrigin.Begin);
            faceMemory.Seek(0, SeekOrigin.Begin);
            MemoryStream emotionMemory = new MemoryStream();
            faceMemory.CopyTo(emotionMemory);
            emotionMemory.Position = 0;
            faceMemory.Seek(0, SeekOrigin.Begin);
            faceProperties = await cognitiveServices.GetFaceProperties(faceMemory);
            Dictionary<string, float> emotionsProperties = await cognitiveServices.GetEmotionProperties(emotionMemory);
            IRandomAccessStream a1 = await ConvertToRandomAccessStream(faceMemory);
            await bitmapImage.SetSourceAsync(a1);
            image_load.Source = bitmapImage;
            txt_Data.Text = string.Format("Age: {0}, Gender: {1}", faceProperties["Age"], faceProperties["Gender"]);
            txt_Emotion_section1.Text = string.Format("Anger: {0}% Contempt: {1}% ", emotionsProperties["Anger"], emotionsProperties["Contempt"]);
            txt_Emotion_section2.Text = string.Format("Disgust: {0}% Fear: {1}%", emotionsProperties["Disgust"], emotionsProperties["Fear"]);
            txt_Emotion_section3.Text = string.Format("Happiness: {0}% Neutral: {1}% ", emotionsProperties["Happiness"],
                emotionsProperties["Neutral"]);
            txt_Emotion_section4.Text = string.Format("Sadness: {0}% Surprise: {1}%", emotionsProperties["Sadness"], emotionsProperties["Surprise"]);
            myProgressRing.Visibility = Visibility.Collapsed;
            btn_loadImage.Visibility = Visibility.Visible;
        }

        public static async Task<IRandomAccessStream> ConvertToRandomAccessStream(MemoryStream memoryStream)
        {
            var randomAccessStream = new InMemoryRandomAccessStream();
            var outputStream = randomAccessStream.GetOutputStreamAt(0);
            var dw = new DataWriter(outputStream);
            var task = Task.Factory.StartNew(() => dw.WriteBytes(memoryStream.ToArray()));
            await task;
            await dw.StoreAsync();
            await outputStream.FlushAsync();
            return randomAccessStream;
        }
    }
}
