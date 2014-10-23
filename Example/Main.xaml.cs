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
using System.Windows.Data;
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
        public List<Player> allPlayers = new List<Player>();
        private Player currentPlayer;
        private Button btnStartNewGame;
        private TextBox tbxPlayerName;
        private List<Level> levels = new List<Level>();

		public Main()
		{
			InitializeComponent();
			Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();

            //Container.Children.Add(new MdiChild
            //{
            //    Name = "startScreen"
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
            ImageBrush backgroundImg = new ImageBrush();
            backgroundImg.ImageSource = new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "background_Container.jpg"));
            backgroundImg.Stretch = Stretch.Uniform;
            Container.Background = backgroundImg;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void StartupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartupWindow.Position = new Point((SystemParameters.PrimaryScreenWidth / 2 - StartupWindow.Width / 2), SystemParameters.PrimaryScreenHeight / 4 - StartupWindow.Height/2);
            Canvas.SetZIndex(StartupWindow, 2);
            StartupWindow.Width = 350;
            StartupWindow.Height = 350;
            TextBox tbxPlayerName = new TextBox() { FontSize = 27, Width = 150 };
            PopulateListLevels();
            allPlayers.Clear();
            allPlayers = Player.Deserialize(allPlayers);
        }

        private void PopulateListLevels()
        {
            levels.Add(new Level(1, 0, imagesFolder.ToString() + "/Backgrounds/" + "background_L1.jpg"));
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            btnNewGame.Visibility = Visibility.Hidden;
            btnLoadGame.Visibility = Visibility.Hidden;
            spStartupWindow.Children.Remove(btnNewGame);
            spStartupWindow.Children.Remove(btnLoadGame);
            Label lblName = new Label() {Content="Name", FontSize=30, HorizontalAlignment=HorizontalAlignment.Center };
            spStartupWindow.Children.Add(lblName);
            tbxPlayerName = new TextBox() {FontSize=27, Width=150};
            tbxPlayerName.TextChanged += tbxPlayerName_TextChanged;
            spStartupWindow.Children.Add(tbxPlayerName);
            btnStartNewGame = new Button() { Content = "Start", Margin = new Thickness(20, 50,20,0), FontSize = 30 };
            btnStartNewGame.Click +=btnStartNewGame_Click;
            spStartupWindow.Children.Add(btnStartNewGame);
            btnStartNewGame.IsEnabled = false;            
        }        

        void tbxPlayerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbxPlayerName.Text))
                btnStartNewGame.IsEnabled = false;
            else
                btnStartNewGame.IsEnabled = true;
        }

        private void btnStartNewGame_Click(object sender, RoutedEventArgs e)
        {
            if (currentPlayer == null)
            {
                currentPlayer = new Player(allPlayers.Count, tbxPlayerName.Text, 0, 0);
                allPlayers.Add(currentPlayer);
            }
            allPlayers = Player.Deserialize(allPlayers);
            PopulatePesentImageList();
            PlayingWindow_Loaded(sender, e);
            ScoringWindow_Loaded(sender, e);
            PlayingWindow.Visibility = Visibility.Visible;
            ScoringWindow.Visibility = Visibility.Visible;
            StartupWindow.Close();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {            
            spStartupWindow.Children.Remove(btnNewGame);
            spStartupWindow.Children.Remove(btnLoadGame);
            lbxPlayers.ItemsSource = allPlayers;
        }

        private void PlayingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayingWindow.Height = (SystemParameters.PrimaryScreenHeight - menu.Height);
            PlayingWindow.Width = SystemParameters.PrimaryScreenWidth / 2 + SystemParameters.PrimaryScreenWidth / 6;

            ImageBrush backgroundImg = new ImageBrush();
            backgroundImg.ImageSource = levels[currentPlayer.LevelsCompleted].BackgroundImage.ImageSource; //new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "background_L1.jpg"));//, UriKind.Relative));
            backgroundImg.Stretch = Stretch.UniformToFill;
            backgroundImg.AlignmentX = AlignmentX.Left;
            imgCanvas.Background = backgroundImg;

            Canvas.SetBottom(spBtns, 5);
            Canvas.SetLeft(spBtns, 5);
            imgSanta.Source = new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "santa.png"));
            Canvas.SetBottom(imgSanta, 5);
            Canvas.SetRight(imgSanta, 5);

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

        private void ScoringWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ScoringWindow.Height = (SystemParameters.PrimaryScreenHeight - menu.Height);
            ScoringWindow.Width = SystemParameters.PrimaryScreenWidth / 2 - SystemParameters.PrimaryScreenWidth / 6;
            ScoringWindow.Position = new Point((SystemParameters.PrimaryScreenWidth - ScoringWindow.Width), 0);
            Canvas.SetZIndex(ScoringWindow, 1);

            ImageBrush backgroundImg = new ImageBrush();
            backgroundImg.ImageSource = levels[currentPlayer.LevelsCompleted].BackgroundImage.ImageSource; //new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "background_L1.jpg"));//, UriKind.Relative));
            backgroundImg.Stretch = Stretch.UniformToFill;
            backgroundImg.AlignmentX = AlignmentX.Right;
            ScoringWindow.Background = backgroundImg;
            imgSleigh.Source = new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "sleigh.png"));
            lblScore.Content = currentPlayer.Score.ToString();

        }

        private void imgSleigh_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(String)))
            {
                e.Effects = DragDropEffects.Copy;
                string presentDropped = e.Data.GetData(typeof(String)).ToString();

                //If Santas list contins the present dropped remove this Presnt from santas list
                if (Present.lstPresentImages.Contains(presentDropped))
                {
                    currentPlayer.Score++;
                    lblScore.Content = (Convert.ToInt32(lblScore.Content) + 1);
                    lblCorrect.Content = Convert.ToInt32(lblCorrect.Content) + 1;

                    Present.lstPresentImages.RemoveAt(lstPresents.Items.IndexOf(presentDropped));

                    //Find the present that needs to be removed
                    //Remove it from the list of presents
                    //Remove it from the xaml
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
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
                if (Convert.ToInt32(lblScore.Content)>0)
                lblScore.Content = (Convert.ToInt32(lblScore.Content) - 1);

                lblCorrect.Content = Convert.ToInt32(lblIncorrect.Content) + 1;
            }               
        }

        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (allPlayers.Count > currentPlayer.PlayerId)
                allPlayers.RemoveAt(currentPlayer.PlayerId);
            allPlayers.Add(currentPlayer);
            Player.Serialize(allPlayers);
        }

        private void lbxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPlayer = (Player)lbxPlayers.SelectedItem;
            btnStartNewGame_Click(sender, e);
        }

        

        

        

        
	}
}