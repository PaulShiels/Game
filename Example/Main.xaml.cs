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
using System.Linq;
using System.Collections.ObjectModel;

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

        DispatcherTimer timer = new DispatcherTimer(), loadingTimer = new DispatcherTimer();
        private List<Present> lstPresentImg = new List<Present>();
        private DirectoryInfo imagesFolder = new DirectoryInfo(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString() + "\\Images\\");
        public List<Player> allPlayers = new List<Player>();
        private Player currentPlayer;
        private Button btnStartNewGame;
        private TextBox tbxPlayerName;
        private List<Level> levels = new List<Level>();
        private List<string> lstAllPresents = new List<string>();
        private RotateTransform imgLoadTransform = new RotateTransform();

		public Main()
		{
			InitializeComponent();
			Container.Children.CollectionChanged += (o, e) => Menu_RefreshWindows();
            //PopulatePesentImageList();
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
            backgroundImg.Stretch = Stretch.UniformToFill;
            Container.Background = backgroundImg;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            loadingTimer.Tick+= new EventHandler(loadingTimer_Tick);
            loadingTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void loadingTimer_Tick(object sender, EventArgs e)
        {
            imgLoadTransform.Angle += 5;
            imgLoading.RenderTransform = imgLoadTransform; 
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
            lstAllPresents.Clear();
            foreach (var img in Directory.GetFiles(imagesFolder.ToString()))
            {
                Level.lstAllPresentImages.Add(Path.GetFileNameWithoutExtension(img));
                FileInfo fi = new FileInfo(img);
                Present p = new Present(fi);
                lstPresentImg.Add(p);
		        lstAllPresents.Add(Path.GetFileNameWithoutExtension(img));
            }

            levels.Add(new Level(0, "Shortly after leaving the North Pole Santa crashed his sleigh and all the presents have gone missing. \n Can you help him find them by checking his list and returning the presents to his sleigh? \n", 0, imagesFolder.ToString() + "/Backgrounds" + "/background_L1.jpg", lstAllPresents, lstPresentImg.GetRange(0,10)));
            levels.Add(new Level(1, "", 0, imagesFolder.ToString() + "/Backgrounds" + "/background_L2.jpg", lstAllPresents, lstPresentImg.GetRange(10, 20)));
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            btnNewGame.Visibility = Visibility.Hidden;
            btnLoadGame.Visibility = Visibility.Hidden;
            btnExit.Visibility = Visibility.Hidden;
            btnNewGame.Content = null;
            btnLoadGame.Content = null;
            btnExit.Content = null;
            lbxPlayers.ItemsSource = null;
            Label lblName = new Label() {Content="Name", FontSize=30,FontFamily = new FontFamily("Kristen ITC"), Foreground=new SolidColorBrush(Colors.Red), HorizontalAlignment=HorizontalAlignment.Center };
            spStartupWindow.Children.Add(lblName);
            tbxPlayerName = new TextBox() {FontSize=27, Width=150};
            tbxPlayerName.TextChanged += tbxPlayerName_TextChanged;
            spStartupWindow.Children.Add(tbxPlayerName);
            btnStartNewGame = new Button() { Content = "Start", Margin = new Thickness(20, 50,20,0), FontSize = 30 };
            btnStartNewGame.Click +=btnStartNewGame_Click;
            spStartupWindow.Children.Add(btnStartNewGame);
            btnStartNewGame.IsEnabled = false;
            btnBack.Visibility = Visibility.Visible;
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
            PlayingWindow.Visibility = Visibility.Visible;
            ScoringWindow.Visibility = Visibility.Visible;
            StartupWindow.Visibility = Visibility.Hidden;

            //If the user has clicked New Game create a new player
            if (currentPlayer == null)
            {
                currentPlayer = new Player(allPlayers.Count, tbxPlayerName.Text, 0,levels[0]);
                allPlayers.Add(currentPlayer);

                ScoringWindow.Visibility = Visibility.Hidden;
                PlayingWindow.Visibility = Visibility.Hidden;

                //Level 1 will begin with this window which gives instructions
                MdiChild BeginningMessageWindow = new MdiChild()
                {
                    Width = StartupWindow.Width+130,
                    Height = StartupWindow.Height

                };

                //Add a stackpanel to hold the following elements
                StackPanel spMessages = new StackPanel();
                TextBlock lblMessage1 = new TextBlock() { Text = "SANTA NEEDS YOUR HELP! \n", FontSize = 29, FontFamily = new FontFamily("Kristen ITC"), TextWrapping = TextWrapping.Wrap, Foreground=new SolidColorBrush(Colors.Red) };
                TextBlock lblMessage2 = new TextBlock() { Text = currentPlayer.CurrentLevel.LevelBeginningMessage, FontSize = 20, FontFamily = new FontFamily("Kristen ITC"), TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.RosyBrown) };
                Button btnGotIt = new Button() { Content = "Got it!", FontSize = 20, FontFamily = new FontFamily("Kristen ITC"), MaxWidth=100 };
                spMessages.Children.Add(lblMessage1);
                spMessages.Children.Add(lblMessage2); spMessages.Children.Add(btnGotIt);
                BeginningMessageWindow.Content = spMessages;
                Container.Children.Add(BeginningMessageWindow);
                BeginningMessageWindow.Position = new Point((SystemParameters.PrimaryScreenWidth / 2 - StartupWindow.Width / 2), SystemParameters.PrimaryScreenHeight / 4 - StartupWindow.Height / 2);
                btnGotIt.Click += btnGotIt_Click;
            }

            allPlayers = Player.Deserialize(allPlayers);
            
            PlayingWindow_Loaded(sender, e);
            ScoringWindow_Loaded(sender, e);
            
        }

        private void btnGotIt_Click(object sender, RoutedEventArgs e)
        {
            //Keep getting the parent object up to the top level            
            Button btnGotIt = (Button)sender;
            StackPanel sp = (StackPanel)btnGotIt.Parent;
            ContentControl cc = (ContentControl)sp.Parent;
            Border b = (Border)cc.Parent;
            Grid g = (Grid)b.Parent;
            Border b1 = (Border)g.Parent;
            b1.Visibility = Visibility.Hidden;
            //Set the visibility of this object to hidden to hide the Beginning Message window
            PlayingWindow.Visibility = Visibility.Visible;
            ScoringWindow.Visibility = Visibility.Visible;
            //PlayingWindow_Loaded(sender,e);
            //ScoringWindow_Loaded(sender, e);
            timer.Start();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow.Visibility = Visibility.Visible;
            lbxPlayers.Visibility = System.Windows.Visibility.Visible;
            btnNewGame.Visibility = System.Windows.Visibility.Hidden;
            btnExit.Visibility = Visibility.Hidden;
            btnLoadGame.Visibility = System.Windows.Visibility.Hidden;
            btnNewGame.Content = null;
            btnLoadGame.Content = null;
            btnExit.Content = null;
            if (lbxPlayers.ItemsSource == null)
                lbxPlayers.ItemsSource = allPlayers;
            btnBack.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if(spStartupWindow.Children.Count==7)
            {
                spStartupWindow.Children.RemoveRange(4,4);
            }
            lbxPlayers.Visibility = System.Windows.Visibility.Hidden;
            btnNewGame.Visibility = System.Windows.Visibility.Visible;
            btnLoadGame.Visibility = System.Windows.Visibility.Visible;
            if (btnStartNewGame!=null)
                btnStartNewGame.Visibility = Visibility.Hidden;
            btnExit.Visibility = Visibility.Visible;
            btnNewGame.Content = "New Game";
            btnLoadGame.Content = "Load Game";
            btnExit.Content = "Exit";
            btnBack.Visibility = System.Windows.Visibility.Hidden;
        }

        private void PlayingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            PlayingWindow.Height = (SystemParameters.PrimaryScreenHeight - menu.Height);
            PlayingWindow.Width = SystemParameters.PrimaryScreenWidth / 2 + SystemParameters.PrimaryScreenWidth / 6;

            ImageBrush backgroundImg = new ImageBrush();
            backgroundImg.ImageSource = currentPlayer.CurrentLevel.BackgroundImage; //levels[currentPlayer.LevelsCompleted].BackgroundImage.ImageSource; //new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "background_L1.jpg"));//, UriKind.Relative));
            backgroundImg.Stretch = Stretch.UniformToFill;
            backgroundImg.AlignmentX = AlignmentX.Left;
            imgCanvas.Background = backgroundImg;

            //Set the position of the quit and load game buttons
            Canvas.SetBottom(spBtns, 5);
            Canvas.SetLeft(spBtns, 5);

            foreach (var image in currentPlayer.CurrentLevel.PresentsInThisLevel)
            {
                Level.lstPresentImages.Add(image.Type);
                //Level.SantasMissingPresentsList.Add(image.Type);
                
            }

            Level.SantasMissingPresentsList = Level.FormatPresentNames(currentPlayer.CurrentLevel.PresentsInThisLevel);
            //PopulatePesentImageList();

            //Add Presents to the list Santas List
            foreach (var present in Level.SantasMissingPresentsList)//change this to Santas list
            {
                lstPresents.Items.Add(present);
            }

            //Add the image of santa and set the position
            imgSanta.Source = new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "santa.png"));
            Canvas.SetBottom(imgSanta, 5);
            Canvas.SetRight(imgSanta, 5);

            Random rndStartLeftPosition = new Random(), rndStartTopPosition = new Random();
            int lastImgLeftPos = 0, lastImgTopPos = 0, differenceTop = 0;
            foreach (var image in currentPlayer.CurrentLevel.PresentsInThisLevel) //lstPresentImg)
            {
                image.ImageObject.Height += 100;
                image.ImageObject.Width += 100;
                cnvsPresents.Children.Add(image.ImageObject);
                differenceTop = 0;

                int newTopPos = 0;
                while (differenceTop < 100)
                {
                    newTopPos = rndStartTopPosition.Next(0, 400);
                    differenceTop = Math.Abs(newTopPos - lastImgTopPos);
                }
                Canvas.SetTop(image.ImageObject, newTopPos);

                int newLeftPos = 0;
                {
                    newLeftPos = lastImgLeftPos - 200; //rndStartLeftPosition.Next(-780, -90);
                }
                Canvas.SetLeft(image.ImageObject, newLeftPos);
                image.xAxisPosition = newLeftPos;
                //image.yAxisPosition = newTopPos;
                lastImgTopPos = newTopPos;
                lastImgLeftPos = newLeftPos;
            }    

            //timer.Start();
        }

        private void PopulatePesentImageList()
        {
            lstPresentImg.Clear();
            lstPresents.Items.Clear();
            int id = 0;    
            foreach (var file in imagesFolder.GetFiles("*.*"))
            {
                Present p = new Present(file);
                Level.lstAllPresentImages.Add(file.Name);
                lstPresentImg.Add(p);
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            Present presentToRemove = null;
            foreach (var present in currentPlayer.CurrentLevel.PresentsInThisLevel)// lstPresentImg)
            {
                present.xAxisPosition += present.MovementSpeed;
                Canvas.SetLeft(present.ImageObject, present.xAxisPosition);

                if (present.xAxisPosition >= PlayingWindow.Width-present.ImageObject.Width)// || present.xAxisPosition<=ScoringWindow.Width+10)
                {
                    presentToRemove = present;
                    cnvsPresents.Children.Remove(present.ImageObject);
                    //present.xAxisPosition = -100;
                    //Canvas.SetLeft(present.ImageObject, present.xAxisPosition);
                }
            }
            if (presentToRemove != null)
            {
                currentPlayer.CurrentLevel.PresentsInThisLevel.Remove(presentToRemove);
                lstPresentImg.Remove(presentToRemove);
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
            ImageBrush backgroundImg = new ImageBrush();
            backgroundImg.ImageSource = currentPlayer.CurrentLevel.BackgroundImage;
            // levels[currentPlayer.LevelsCompleted].BackgroundImage.ImageSource; //new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "background_L1.jpg"));//, UriKind.Relative));
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

                //If Santas list contins the present dropped remove this Present from santas list and also remove the moving image
                if (Level.lstPresentImages.Contains(presentDropped))
                {
                    //Find the present that needs to be removed                    
                    Present pToRemove =null;
                    foreach (var i in currentPlayer.CurrentLevel.PresentsInThisLevel)
                    {
                        if (i.ImageObject.Source.ToString().ToLower().Contains(presentDropped.Replace(" ", "").ToLower()))//.Replace(" ","")))
                        {
                            pToRemove = i;
                        }
                    }

                    //Is there is a present to be removed?
                    if (pToRemove != null)
                    {
                        //Increase the scores by 1
                        currentPlayer.Score++;
                        lblScore.Content = (Convert.ToInt32(lblScore.Content) + 1);
                        lblCorrect.Content = Convert.ToInt32(lblCorrect.Content) + 1;

                        //Remove the Present from all the lists including the moving image                        
                        lstPresents.Items.RemoveAt(currentPlayer.CurrentLevel.PresentsInThisLevel.IndexOf(pToRemove));
                        lstPresentImg.Remove(pToRemove);
                        currentPlayer.CurrentLevel.PresentsInThisLevel.Remove(pToRemove);

                        //If all the presents have been returned to the sleigh show a level complete message window
                        if (lstPresents.Items.Count == 0)
                        {
                            //Increase the level by 1
                            currentPlayer.CurrentLevel = levels[currentPlayer.CurrentLevel.LevelId + 1];

                            //Create the level complete window
                            MdiChild LevelCompleteWindow = new MdiChild()
                            {
                                Width = StartupWindow.Width + 130,
                                Height = StartupWindow.Height
                            };

                            //Add a stackpanel to hold the following elements
                            StackPanel spMessages = new StackPanel();
                            TextBlock lblMessage1 = new TextBlock() { Text = "LEVEL COMPLETE! \n", FontSize = 29, FontFamily = new FontFamily("Kristen ITC"), TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.Red) };
                            TextBlock lblMessage2 = new TextBlock() { Text = currentPlayer.CurrentLevel.LevelBeginningMessage, FontSize = 20, FontFamily = new FontFamily("Kristen ITC"), TextWrapping = TextWrapping.Wrap, Foreground = new SolidColorBrush(Colors.RosyBrown) };
                            Button btnGotIt = new Button() { Content = "Got it!", FontSize = 20, FontFamily = new FontFamily("Kristen ITC"), MaxWidth = 100 };
                            spMessages.Children.Add(lblMessage1);
                            spMessages.Children.Add(lblMessage2); spMessages.Children.Add(btnGotIt);
                            LevelCompleteWindow.Content = spMessages;
                            Container.Children.Add(LevelCompleteWindow);
                            LevelCompleteWindow.Position = new Point((SystemParameters.PrimaryScreenWidth / 2 - StartupWindow.Width / 2), SystemParameters.PrimaryScreenHeight / 4 - StartupWindow.Height / 2);
                            btnGotIt.Click += btnGotIt_Click;

                            //Hide the Playing window and the Scoring window to make the Level complete window visible
                            PlayingWindow.Visibility = Visibility.Hidden;
                            ScoringWindow.Visibility = Visibility.Hidden;
                        }
                    }

                    //If the prsent image has not already dissappeared, remove it now
                    if (pToRemove!=null)
                        cnvsPresents.Children.Remove(pToRemove.ImageObject);
                }
            }
            //else
            //{
            //    e.Effects = DragDropEffects.None;
            //    if (Convert.ToInt32(lblScore.Content)>0)
            //    lblScore.Content = (Convert.ToInt32(lblScore.Content) - 1);
            //    lblCorrect.Content = Convert.ToInt32(lblIncorrect.Content) + 1;
            //}               
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
            StartupWindow.Visibility = Visibility.Hidden;
            //LoadingWindow.Visibility = Visibility.Visible;
            loadingTimer.Start();
            currentPlayer = (Player)lbxPlayers.SelectedItem;

            if(currentPlayer!=null)
            {
                
                btnStartNewGame_Click(sender, e);
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure you want to quit? Any unsaved changes will be lost.", "Quit", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                currentPlayer = null;
                cnvsPresents.Children.RemoveRange(0, cnvsPresents.Children.Count);
                lstPresentImg.Clear();
                ScoringWindow.Visibility = Visibility.Hidden;
                PlayingWindow.Visibility = Visibility.Hidden;
                //StartupWindow.Visibility = Visibility.Visible;
                lbxPlayers.SelectedIndex = -1;
                btnBack_Click(sender, e);
                StartupWindow.Visibility = Visibility.Visible;
            }
            else
            {
                timer.Start();
            }
        }

        private void LoadingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingWindow.Position = new Point((SystemParameters.PrimaryScreenWidth / 2 - StartupWindow.Width / 2), SystemParameters.PrimaryScreenHeight / 4 - StartupWindow.Height / 2);
            Canvas.SetZIndex(StartupWindow, 2);
            imgLoading.Source = new BitmapImage(new Uri(imagesFolder.ToString() + "/Backgrounds/" + "wreath.png"));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}