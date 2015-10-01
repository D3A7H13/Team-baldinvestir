using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BullsCowsProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int[] number = new int[4];

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            inputTextBox.MaxLength = 4;
            GenerateNumber();
        }

        void GenerateNumber()
        {
            for (int i = 0; i < number.Length; i++)
            {
                number[i] = GenerateRandomDigit();
            }
            randomNumLabel.Content = string.Join("", number);
        }

        private void inputTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            CheckIsNumeric(e);
        }

        private void inputTextBox_TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void CheckIsNumeric(TextCompositionEventArgs e)
        {
            int result;

            if (!(int.TryParse(e.Text, out result)))
            {
                e.Handled = true;
            }
        }

        private int GenerateRandomDigit()
        {
            Random randomizer = new Random();
            int digit;
            do
            {
                digit = randomizer.Next(1, 9);

            }
            while (number.Contains(digit));

            return digit;
        }

        void checkButton_Click(object sender, RoutedEventArgs e)
        {

            Image[] imgArray = new Image[] { bull, bull2, bull3, bull4, cow, cow2, cow3, cow4 };
            for (int i = 0; i < imgArray.Length; i++)
            {
                imgArray[i].Source = null;
            }

            GetValueFromTextBox();
        }

        void GetValueFromTextBox()
        {
            string playerNumber = inputTextBox.Text;
            int[] playerDigits = playerNumber.ToCharArray().Select(d => Convert.ToInt32(d) - 48).ToArray();

            FindBullsCows(playerDigits);
        }

        void FindBullsCows(int[] playerDigits)
        {
            int bulls = 0;
            int cows = 0;
            for (int i = 0; i < playerDigits.Length; i++)
            {
                if (number.Contains(playerDigits[i]))
                {
                    if (number[i] == playerDigits[i])
                    {
                        bulls++;
                    }
                    else
                    {
                        cows++;
                    }
                }
            }


            DrawCows(cows);
            DrawBulls(bulls);
            AddHistory(string.Join("", playerDigits), bulls, cows);

            if (bulls == 4)
            {
                VictoryScreen victory = new VictoryScreen();
                victory.setCreatingForm = this;
                this.IsEnabled = false;
                victory.Show();
            }

        }

        private void AddHistory(string playerNumber, int bulls, int cows)
        {
            ListBoxItem history = new ListBoxItem();
            if (bulls == 4)
            {
                history.Content = "The number was " + playerNumber + ". You won!";
            }
            else
            {
                history.Content = "In " + playerNumber + " there are " + bulls + " bulls and " + cows + " cows";
            }

            historyListBox.Items.Insert(0, history);
        }

        void DrawBulls(int bulls)
        {
            Image[] imgArray = new Image[] { bull, bull2, bull3, bull4 };
            for (int i = 0; i < bulls; i++)
            {
                var uri = new Uri("pack://application:,,,/Resources/bull.png");
                imgArray[i].Source = new BitmapImage(uri);
            }
        }

        void DrawCows(int cows)
        {
            Image[] imgArray = new Image[] { cow, cow2, cow3, cow4 };
            for (int i = 0; i < cows; i++)
            {
                var uri = new Uri("pack://application:,,,/Resources/cow.png");
                imgArray[i].Source = new BitmapImage(uri);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            /*DoubleAnimation ne = new DoubleAnimation();
            ne.From = 0;
            ne.To = 40;
            ne.Duration = new Duration(TimeSpan.FromSeconds(0.500));
            ne.RepeatBehavior = new RepeatBehavior(1);
            ne.AutoReverse = true;
            TranslateTransform tt = new TranslateTransform();
            checkButton.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, ne);
            */
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            
            dispatcherTimer.Start();

    }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(5.000));
            da.RepeatBehavior = RepeatBehavior.Forever;
            DoubleAnimation ne = new DoubleAnimation();
            ne.From = 0;
            ne.To = 100;
            ne.Duration = new Duration(TimeSpan.FromSeconds(0.500));
            ne.RepeatBehavior = RepeatBehavior.Forever;
            ne.AutoReverse = true;
            TranslateTransform tt = new TranslateTransform();
            
            
            RotateTransform rt = new RotateTransform();
            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);
            
            lblHello.RenderTransform = tg;

            rt.BeginAnimation(RotateTransform.AngleProperty, da);
            tt.BeginAnimation(TranslateTransform.YProperty, ne);
            //tg.BeginAnimation(RotateTransform.AngleProperty, da);
            //tg.BeginAnimation(TranslateTransform.YProperty, ne);
        }
    }
}
