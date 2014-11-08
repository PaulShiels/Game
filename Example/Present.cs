using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Example
{
    [Serializable]
    public class Present
    {
        public int PresentId;
        public string Type { get; set; }
        public bool BeingDragged { get; set; }
        public double MovementSpeed { get; set; }
        public double xAxisPosition { get; set; }
        [NonSerialized]
        public Image ImageObject;// { get; set; } Set to NonSerialized because the image object can't be serialized and doesn't need to be serialized in this case.

        public Present(FileInfo imageFile)
        {
            this.ImageObject = CreateImageObject(imageFile);
            //this.MovementSpeed = GetRandomSpeed();
            this.Type = GetPresentType(ImageObject.Source);
        }

        public Image CreateImageObject(FileInfo imageFile)
        {
            //Create the image object and set the properties
            Image i = new Image();
            string imageFileName;
            //Remove the file extension
            imageFileName = imageFile.Name.ToLower().Remove(imageFile.Name.Length - imageFile.Extension.Length, imageFile.Extension.Length);//.Remove(imageFile.Name.IndexOf('.'), 4); 
            //Remove any white space
            //if (imageFileName.Contains(" "))
            //imageFileName = imageFileName.Remove(imageFileName.IndexOf(" "),1);
            i.Name = imageFileName;
            i.Source = new BitmapImage(new Uri(imageFile.FullName));            
            i.MouseDown += new MouseButtonEventHandler(imgPresent_MouseDown);

            i.Height = 160;
            i.Width = 160;
            return i;
        }

        private void imgPresent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //When the image is clicked copy the image path and set the being dragged property to true.
            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(String), GetPresentType(image.Source));
            DragDrop.DoDragDrop(image, data, DragDropEffects.Copy);
            BeingDragged = true;
        }

        public double GetRandomSpeed(int[] speedRange)
        {
            Random rnd = new Random();
            return rnd.Next(speedRange[0], speedRange[1]);
        }

        public string GetPresentType(ImageSource imageSource)
        {
            //Check if the image path contains the name of any Presents in the list of Presents.
            foreach (var present in Level.lstAllPresentImages)
            {
                if (imageSource.ToString().Contains(present.Replace(" ", "_")))//Regex.Replace(present.ToLower(), @"\s+", "")))
                {
                    return present;
                }
            }
            return "Unknown";
        }
    }
}
