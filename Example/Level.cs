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
        public static ObservableCollection<string> lstPresentImages = new ObservableCollection<string>();
        


        public Level(int levelId, int score,  string backgroundImageSource)
        {
            this.LevelId = levelId;
            this.ScoreboardScore = score;
            this.BackgroundImage = setImage(backgroundImageSource);
        }

        private ImageBrush setImage(string backgroundImageSource)
        {
            ImageBrush i = new ImageBrush();
            i.ImageSource = new BitmapImage(new Uri(backgroundImageSource));
            i.Stretch = Stretch.UniformToFill;
            return i;
        }
    }
}
