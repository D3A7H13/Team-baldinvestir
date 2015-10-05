using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BullsCowsProject
{
    /// <summary>
    /// Interaction logic for VictoryScreen.xaml
    /// </summary>
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
            if (moves > 1 && moves <= 5)
            {
                var uri = new Uri("pack://application:,,,/Resources/gold.png");
                MedalImage.Source = new BitmapImage(uri);
            }
            else if (moves > 5 && moves <= 10)
            {
                var uri = new Uri("pack://application:,,,/Resources/silver.png");
                MedalImage.Source = new BitmapImage(uri);
            }
            else 
            {
                var uri = new Uri("pack://application:,,,/Resources/bronze.png");
                MedalImage.Source = new BitmapImage(uri);
            }
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
