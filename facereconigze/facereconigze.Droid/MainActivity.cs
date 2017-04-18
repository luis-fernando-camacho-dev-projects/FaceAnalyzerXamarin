using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Graphics;
using facereconigze.FaceServices;
using System.Collections.Generic;

namespace facereconigze.Droid
{
	[Activity (Label = "facereconigze.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
        Android.App.ProgressDialog progress;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);

            //button.Click += delegate {
            //	button.Text = string.Format ("{0} clicks!", count++);
            //             MyClass c = new MyClass();
            //             await c.LoadPhoto();
            //};
            button.Click += button_Click;


		}

        public async void button_Click(object sender, EventArgs e)
        {
            MSCognitiveServices cognitiveServices = new MSCognitiveServices();
            MyClass c = new MyClass();
            string path = string.Empty;
            string albumPath = string.Empty;
            Stream data = await c.LoadPhoto(path, albumPath);
            progress = new Android.App.ProgressDialog(this);
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading... Please wait...");
            progress.SetCancelable(false);
            progress.Show();
            MemoryStream faceStream = new MemoryStream();
            data.CopyTo(faceStream);
            data.Seek(0, SeekOrigin.Begin);
            MemoryStream emotionStream = new MemoryStream();
            data.CopyTo(emotionStream);
            data.Seek(0, SeekOrigin.Begin);
            faceStream.Seek(0, SeekOrigin.Begin);
            emotionStream.Seek(0, SeekOrigin.Begin);
            Dictionary<string, string> faceProperties = await cognitiveServices.GetFaceProperties(faceStream);
            Dictionary<string, float> emotionProperties = await cognitiveServices.GetEmotionProperties(emotionStream);
            var et_faceText = FindViewById<TextView>(Resource.Id.et_faceText );
            et_faceText.Text = string.Format("Gender: {0} Age: {1}", faceProperties["Gender"], faceProperties["Age"]);
            var et_emotionText = FindViewById<TextView>(Resource.Id.et_emotionText); 
            var et_emotionText_1 = FindViewById<TextView>(Resource.Id.et_emotionText_1);
            var et_emotionText_2 = FindViewById<TextView>(Resource.Id.et_emotionText_2);
            var et_emotionText_3 = FindViewById<TextView>(Resource.Id.et_emotionText_3);
            et_emotionText.Text = string.Format("Anger: {0}% Contempt:{1}% ", emotionProperties["Anger"], emotionProperties["Contempt"]);
            et_emotionText_1.Text = string.Format("Fear: {0}% Happiness:{1}% ", emotionProperties["Fear"], emotionProperties["Happiness"]);
            et_emotionText_2.Text = string.Format("Sadness: {0}% Surprise:{1}%", emotionProperties["Sadness"], emotionProperties["Surprise"]);
            et_emotionText_3.Text = string.Format("Disgust:{0}% Neutral:{1}%", emotionProperties["Disgust"], emotionProperties["Neutral"]);
            var imageView = FindViewById<ImageView>(Resource.Id.myImageView);
            using (var stream = data)
            {
                var bitmap = BitmapFactory.DecodeStream(stream);
                imageView.SetImageBitmap(bitmap);
                // do stuff with bitmap
            }
            progress.Hide();
            faceStream.Close();
            emotionStream.Close();
        }

    }
}


