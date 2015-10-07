using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BullsCowsProject
{

    public partial class VictoryScreen : Window
    {
        internal MainWindow creatingForm;
        public int moves;

        public VictoryScreen()
        {
            InitializeComponent();
        }

        public VictoryScreen(int moves)
        {
            this.moves = moves;
            InitializeComponent();
            SetMedal();
        }

        private void SetMedal()
        {
            var uri = new Uri("pack://application:,,,/Resources/bronze.png");
            if (moves == 1)
            {
                uri = new Uri("pack://application:,,,/Resources/lucky.png");
            }
            else if (moves > 1 && moves <= 5)
            {
                uri = new Uri("pack://application:,,,/Resources/gold.png");
            }
            else if (moves > 5 && moves <= 10)
            {
                uri = new Uri("pack://application:,,,/Resources/silver.png");
            }
            MedalImage.Source = new BitmapImage(uri);
        }



        //setting the parent window that we can control from this window
        public MainWindow setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        //Starting new game ->
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newGame = new MainWindow();
            newGame.Show();
            creatingForm.Close();
            this.Close();
        }

        //closing the game
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
