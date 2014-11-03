using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Example
{
    public class Level
    {
        public int LevelId { get; set; }
        public int ScoreboardScore { get; set; }
        public ImageBrush BackgroundImage { get; set; }
        public static List<string> lstPresentImages = new List<string>();
        public static List<string> SantasMissingPresentsList = new List<string>();

        public Level(int levelId, int score, string backgroundImageSource, List<string> SantasLostPresents)
        {
            this.LevelId = levelId;
            this.ScoreboardScore = score;
            this.BackgroundImage = setImage(backgroundImageSource);
            lstPresentImages = SantasLostPresents;
            SantasMissingPresentsList = FormatPresentNames(SantasLostPresents);
        }

        private ImageBrush setImage(string backgroundImageSource)
        {
            ImageBrush i = new ImageBrush();
            i.ImageSource = new BitmapImage(new Uri(backgroundImageSource));
            i.Stretch = Stretch.UniformToFill;
            return i;
        }

        private List<string> FormatPresentNames(List<string> presents)
        {
            List<string> formattedPresents = new List<string>();
            foreach (var present in presents)
            {
                //Check if there is 2 words in the present name
                if (present.Contains("_"))
                {
                    //Create array to hold the two lowercase words
                    string[] words = present.Split('_');
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
                    formattedPresents.Add(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(present.ToLower()));
                }
            }
            return formattedPresents;
        }
    }
}
