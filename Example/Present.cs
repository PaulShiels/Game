using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Example
{
    class Present
    {
        public string Type { get; set; }
        public bool BeingDragged { get; set; }
        public int MovementSpeed { get; set; }
        public Image ImageObject { get; set; }
        

        public Present(FileInfo imageFile)
        {
            ImageObject = CreateImageObject(imageFile);
            MovementSpeed = GetRandomSpeed();
        }

        private Image CreateImageObject(FileInfo imageFile)
        {
            //Create the image object and set the properties
            Image i = new Image();
            i.Source = new BitmapImage(new Uri(imageFile.FullName));
            i.MouseDown += new MouseButtonEventHandler(imgPresent_MouseDown);

            i.Height = 60;
            i.Width = 90;
            return i;
        }

        private void imgPresent_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Image img = (Image)sender;
            //var pic = e.Source as Image;
            //DragDrop.DoDragDrop(img, e, DragDropEffects.Move);

            Image image = e.Source as Image;
            DataObject data = new DataObject(typeof(ImageSource), image.Source);
            DragDrop.DoDragDrop(image, data, DragDropEffects.Copy);
            BeingDragged = true;
        }

        private int GetRandomSpeed()
        {
            Random rnd = new Random();
            return rnd.Next(5, 25);
        }
    }
}
