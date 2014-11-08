using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.ComponentModel;

namespace Example
{
    [Serializable]
    public class Level
    {
        public int LevelId { get; set; }
        public int ScoreboardScore { get; set; }
        public string LevelBeginningMessage { get; set; }
        public static List<string> lstAllPresentImages = new List<string>();
        public static List<string> lstPresentImages = new List<string>();
        public static List<string> SantasMissingPresentsList = new List<string>();
        public List<Present> PresentsInThisLevel { get; set; }
        public Bitmap BackgroundImage { get; set; }
        public int[] PresentSpeedRange { get; set; }

        public Level(int levelId, string beginMessage, int score, string backgroundImageSource, List<string> SantasLostPresents, List<Present> presentsForThisLevel, int[] presentSpeedRange)
        {
            this.LevelId = levelId;
            this.LevelBeginningMessage = beginMessage;
            this.ScoreboardScore = score;
            this.BackgroundImage = setImage(backgroundImageSource);
            //lstPresentImages = SantasLostPresents;
            //SantasMissingPresentsList = FormatPresentNames(SantasLostPresents);
            this.PresentsInThisLevel = presentsForThisLevel;
            this.PresentSpeedRange = presentSpeedRange;
        }

        private Bitmap setImage(string backgroundImageSource)
        {
            //ImageBrush i = new ImageBrush();
            //i.ImageSource = new BitmapImage(new Uri(backgroundImageSource));
            //i.Stretch = Stretch.UniformToFill;

            Bitmap btImg = new Bitmap(backgroundImageSource);// = new Bitmap();
            //image.Source = new BitmapImage(new Uri(backgroundImageSource));
            return btImg;
        }

        public static List<string> FormatPresentNames(List<Present> presents)
        {
            List<string> formattedPresents = new List<string>();
            foreach (var present in presents)
            {
                //Check if there is 2 words in the present name
                if (present.Type.Contains("_"))
                {
                    //Create array to hold the two lowercase words
                    string[] words = present.Type.Split('_');
                    StringBuilder sb = new StringBuilder();

                    //For each word in the present name, make the first letter uppercase
                    foreach (var w in words)
                    {
                        sb.Append(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(w.ToLower()));
                        sb.Append(" ");
                    }

                    //Add the reformatted present name to the list to be returned
                    formattedPresents.Add(sb.ToString());
                }
                else
                {
                    //Make the first letter of the present name uppercase
                    formattedPresents.Add(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(present.Type.ToLower()));
                }
            }
            return formattedPresents;
        }

    }
}
