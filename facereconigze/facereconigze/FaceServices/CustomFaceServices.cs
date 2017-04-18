using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace facereconigze.FaceServices
{
    public class MSCognitiveServices
    {
        /// <summary>
        /// Gets Face Properties
        /// </summary>
        /// <param name="data">source image stream</param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetFaceProperties(Stream data)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            try
            {
                FaceServiceClient _faceServiceClient = new FaceServiceClient("4ecdc6a2fa0c4e0c9757ccedb44fa66b");
                List<FaceAttributeType> faceAttributes = new List<FaceAttributeType>();
                faceAttributes.Add(FaceAttributeType.Age);
                faceAttributes.Add(FaceAttributeType.Smile);
                faceAttributes.Add(FaceAttributeType.Gender);
                faceAttributes.Add(FaceAttributeType.Glasses);
                data.Position = 0;
                Face[] faces = await _faceServiceClient.DetectAsync(data, true, false, faceAttributes);
                values.Add("Age", faces[0].FaceAttributes.Age.ToString());
                values.Add("Gender", faces[0].FaceAttributes.Gender.ToString());

            }
            catch (FaceAPIException ex)
            {
                //TODO log
                throw;
            }
            return values;
        }

        /// <summary>
        /// Get Emotion Properties
        /// </summary>
        /// <param name="data">source image stream</param>
        /// <returns></returns>
        public async Task<Dictionary<string, float>> GetEmotionProperties(Stream data)
        {
            Dictionary<string, float> emotions = new Dictionary<string, float>();
            try
            {

                EmotionServiceClient _emotionService = new EmotionServiceClient("93a06ed52a9f424881f96c9fd84c17ac");
                Emotion[] emotionsResponse = await _emotionService.RecognizeAsync(data);
                emotions.Add("Anger", emotionsResponse[0].Scores.Anger * 100);
                emotions.Add("Contempt", emotionsResponse[0].Scores.Contempt * 100);
                emotions.Add("Disgust", emotionsResponse[0].Scores.Disgust * 100);
                emotions.Add("Fear", emotionsResponse[0].Scores.Fear * 100);
                emotions.Add("Happiness", emotionsResponse[0].Scores.Happiness * 100);
                emotions.Add("Neutral", emotionsResponse[0].Scores.Neutral * 100);
                emotions.Add("Sadness", emotionsResponse[0].Scores.Sadness * 100);
                emotions.Add("Surprise", emotionsResponse[0].Scores.Surprise * 100);

            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }

            return emotions;
        }

        /// <summary>
        /// Gets Stream photo from pictures cellphone location CROSS Platform
        /// </summary>
        /// <param name="path">source of path location</param>
        /// <param name="albumPath">source albumPath</param>
        /// <returns></returns>
        public async Task<Stream> LoadPhoto(string path, string albumPath)
        {
            PickMediaOptions option = new PickMediaOptions();
            MediaFile media = await CrossMedia.Current.PickPhotoAsync(option);
            path = media.Path;
            return media.GetStream();
        }

    }

}

