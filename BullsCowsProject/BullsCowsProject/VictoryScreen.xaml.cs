using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace BullsCowsProject
{

    public partial class VictoryScreen : Window
    {
        internal MainWindow creatingForm { get; set; }
        public int moves;

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public VictoryScreen()
        {
            InitializeComponent();
        }

        public VictoryScreen(int moves)
        {
            this.moves = moves;
            InitializeComponent();
            Loaded += VictoryScreen_Loaded;
        }

        private void VictoryScreen_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

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

        // Starting new game ->
        private void newGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newGame = new MainWindow();
            newGame.Show();
            creatingForm.Close();
            this.Close();
        }

        // Closing the game
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
