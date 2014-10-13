using System;
using System.Windows;
using System.Windows.Media.Imaging;
using Example.Controls;
using WPF.MDI;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Threading;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
namespace Example
{
	/// <summary>
	/// Interaction logic for Main.xaml
	/// </summary>
	public partial class Main : Window
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Main"/> class.
		/// </summary>

        DispatcherTimer timer = new DispatcherTimer();
        private List<Present> lstPresentImg = new List<Present>();
        private DirectoryInfo imagesFolder = new DirectoryInfo(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\Images\\");
        private int position;

		public Main()
		{
			InitializeComponent();
			Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();

            //Container.Children.Add(new MdiChild
            //{
            //    Title = "Empty Window Using Code",
            //    Icon = new BitmapImage(new Uri("OriginalLogo.png", UriKind.Relative))
            //});

            //Container.Children.Add(new MdiChild
            //{
            //    Title = "Window Using Code",
            //    Content = new ExampleControl(),
            //    Width = 714,
            //    Height = 734,
            //    Position = new Point(200, 30)
            //});
		}

        private void ContainerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0 ,100);
        }

        private void PlayingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayingWindow.Height = (SystemParameters.PrimaryScreenHeight - menu.Height);
            PlayingWindow.Width = SystemParameters.PrimaryScreenWidth / 2;
            //PlayingWindow.Position = new Point((SystemParameters.PrimaryScreenWidth - ScoringWindow.Width), 0);
            PopulatePesentImageList();
            int startLeftPosition = -50, startTopPosition = 20;
            foreach (var image in lstPresentImg)
            {
                imgCanvas.Children.Add(image.ImageObject);
                Canvas.SetTop(image.ImageObject, startTopPosition);
                Canvas.SetLeft(image.ImageObject, startLeftPosition);
                startTopPosition += 80;
                startLeftPosition -= 30;
            }           

            //MessageBox.Show(imgBall)
            timer.Start();
        }

        private void PopulatePesentImageList()
        {
            int id = 0;    
            foreach (var file in imagesFolder.GetFiles("*.*"))
            {
                Present p = new Present(file);
                lstPresentImg.Add(p);
            }
        }       


        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (var present in lstPresentImg)
            {
                Canvas.SetLeft(present.ImageObject, present.MovementSpeed);
                present.MovementSpeed++;
            }
            
        }

		#region Theme Menu Events

		/// <summary>
		/// Handles the Click event of the Generic control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Generic_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = true;
			Luna.IsChecked = false;
			Aero.IsChecked = false;

			Container.Theme = ThemeType.Generic;
		}

		/// <summary>
		/// Handles the Click event of the Luna control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Luna_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = false;
			Luna.IsChecked = true;
			Aero.IsChecked = false;

			Container.Theme = ThemeType.Luna;
		}

		/// <summary>
		/// Handles the Click event of the Aero control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void Aero_Click(object sender, RoutedEventArgs e)
		{
			Generic.IsChecked = false;
			Luna.IsChecked = false;
			Aero.IsChecked = true;

			Container.Theme = ThemeType.Aero;
		}

		#endregion

		#region Menu Events

		int ooo = 1;

		/// <summary>
		/// Handles the Click event of the 'Normal window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddWindow_Click(object sender, RoutedEventArgs e)
		{
			Container.Children.Add(new MdiChild { Content = new Label { Content = "Normal window" }, Title = "Window " + ooo++ });
		}

		/// <summary>
		/// Handles the Click event of the 'Fixed window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddFixedWindow_Click(object sender, RoutedEventArgs e)
		{
			Container.Children.Add(new MdiChild { Content = new Label { Content = "Fixed width window" }, Title = "Window " + ooo++, Resizable = false });
		}

		/// <summary>
		/// Handles the Click event of the 'Scroll window' menu item.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		private void AddScrollWindow_Click(object sender, RoutedEventArgs e)
		{
			StackPanel sp = new StackPanel { Orientation = Orientation.Vertical };
			sp.Children.Add(new TextBlock { Text = "Window with scroll", Margin = new Thickness(5) });
			sp.Children.Add(new ComboBox { Margin = new Thickness(20), Width = 300 });
			ScrollViewer sv = new ScrollViewer { Content = sp, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto };

			Container.Children.Add(new MdiChild { Content = sv, Title = "Window " + ooo++ });
		}

		/// <summary>
		/// Refresh windows list
		/// </summary>
		void Menu_RefreshWindows()
		{
			WindowsMenu.Items.Clear();
			MenuItem mi;
			for (int i = 0; i < Container.Children.Count; i++)
			{
				MdiChild child = Container.Children[i];
				mi = new MenuItem { Header = child.Title };
				mi.Click += (o, e) => child.Focus();
				WindowsMenu.Items.Add(mi);
			}
			WindowsMenu.Items.Add(new Separator());
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Cascade" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.Cascade;
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Horizontally" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileHorizontal;
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Vertically" });
			mi.Click += (o, e) => Container.MdiLayout = MdiLayout.TileVertical;

			WindowsMenu.Items.Add(new Separator());
			WindowsMenu.Items.Add(mi = new MenuItem { Header = "Close all" });
			mi.Click += (o, e) => Container.Children.Clear();
		}

		#endregion

        #region Scoring Window

        private void ScoringWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ScoringWindow.Height = (SystemParameters.PrimaryScreenHeight - menu.Height);
            ScoringWindow.Width = SystemParameters.PrimaryScreenWidth / 2;
            ScoringWindow.Position = new Point((SystemParameters.PrimaryScreenWidth - ScoringWindow.Width), 0);
            //lstPresents.ItemsSource = Present.lstPresentImages;
            imgSleigh.Source = new BitmapImage(new Uri(imagesFolder.ToString() + "sleigh.png"));            
        }

        #endregion        

        private void imgSleigh_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(String)))
            {
                e.Effects = DragDropEffects.Copy;
                string presentDropped = e.Data.GetData(typeof(String)).ToString();

                //If Santas list contins the present dropped remove this Presnt from santas list
                if (Present.lstPresentImages.Contains(presentDropped))
                {
                    Present.lstPresentImages.RemoveAt(lstPresents.Items.IndexOf(presentDropped));
                    Present pToRemove =null;
                    foreach (var i in lstPresentImg)
                    {
                        if (i.ImageObject.Source.ToString().Contains(presentDropped.ToLower().Replace(" ","")))
                        {
                            pToRemove = i;
                        }
                    }

                    if(pToRemove != null)
                    lstPresentImg.Remove(pToRemove);
                    imgCanvas.Children.Remove(pToRemove.ImageObject);
                    
                    //lstPresents.ItemsSource = Present.lstPresentImages;
                        //Present.lstPresentImages.IndexOf(presentDropped)
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }               
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        
	}
}