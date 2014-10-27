﻿using System;
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
    public class Present
    {
        public int PresentId;
        public string Type { get; set; }
        public bool BeingDragged { get; set; }
        public double MovementSpeed { get; set; }
        public Image ImageObject { get; set; }
        public double xAxisPosition { get; set; }
               

        public Present(FileInfo imageFile)
        {
            ImageObject = CreateImageObject(imageFile);
            MovementSpeed = GetRandomSpeed();
        }

        private Image CreateImageObject(FileInfo imageFile)
        {
            //Create the image object and set the properties
            Image i = new Image();
            i.Name = imageFile.Name.ToLower().Remove(imageFile.Name.IndexOf('.'), 4);
            i.Source = new BitmapImage(new Uri(imageFile.FullName));            
            i.MouseDown += new MouseButtonEventHandler(imgPresent_MouseDown);

            i.Height = 60;
            i.Width = 90;
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

        private double GetRandomSpeed()
        {
            //Random delay = new Random();
            Thread.Sleep(250);
            Random rnd = new Random();
            return rnd.Next(4,7);
        }

        public string GetPresentType(ImageSource imageSource)
        {
            //Check if the image path contains the name of any Presents in the list of Presents.
            foreach (var present in Level.lstPresentImages)
            {
                if (imageSource.ToString().Contains(Regex.Replace(present.ToLower(), @"\s+", "")))
                {
                    return present;
                }
            }
            return "Unknown";
        }
    }
}
