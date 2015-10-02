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
            inputTextBox.Clear();
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
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            
            dispatcherTimer.Start();

    }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            AnimateHistoryTab();
            AnimateHitoryLabel();
            AnimateCheckButton();
            AnimateRandomNumber();
            AnimateInputBox();
            AnimateFirstCow();
            AnimateSecondCow();
            AnimateThirdCow();
            AnimateForthCow();

        }

        private void secondAnimationForthCowStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = cow4.Margin.Left;
            double oldRight = cow4.Margin.Right;
            double oldTop = cow4.Margin.Top;
            double oldBottom = cow4.Margin.Bottom;

            cow4.Margin = new Thickness(oldLeft, oldTop + 45, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = 45;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = -2;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            cow4.RenderTransform = tg;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
            tt.BeginAnimation(TranslateTransform.YProperty, da);
           
        }

        private void AnimateForthCow()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 50;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            cow4.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationForthCow = new DispatcherTimer();
            secondAnimationForthCow.Tick += secondAnimationForthCowStart;
            secondAnimationForthCow.Interval = new TimeSpan(0, 0, 0, 1, 0);

            secondAnimationForthCow.Start();
        }

        private void secondAnimationThirdCowStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = cow3.Margin.Left;
            double oldRight = cow3.Margin.Right;
            double oldTop = cow3.Margin.Top;
            double oldBottom = cow3.Margin.Bottom;

            cow3.Margin = new Thickness(oldLeft, oldTop + 50, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -36;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();
            cow3.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateThirdCow()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 50;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            cow3.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationThirdCow = new DispatcherTimer();
            secondAnimationThirdCow.Tick += secondAnimationThirdCowStart;
            secondAnimationThirdCow.Interval = new TimeSpan(0, 0, 0, 1, 0);

            secondAnimationThirdCow.Start();
        }

        private void secondAnimationSecondCowStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = cow2.Margin.Left;
            double oldRight = cow2.Margin.Right;
            double oldTop = cow2.Margin.Top;
            double oldBottom = cow2.Margin.Bottom;

            cow2.Margin = new Thickness(oldLeft, oldTop + 37, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = 24;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();
            cow2.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateSecondCow()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 37;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            cow2.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationSecondCow = new DispatcherTimer();
            secondAnimationSecondCow.Tick += secondAnimationSecondCowStart;
            secondAnimationSecondCow.Interval = new TimeSpan(0, 0, 0, 1, 0);

            secondAnimationSecondCow.Start();
        }

        private void secondAnimationFirstCowStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = cow.Margin.Left;
            double oldRight = cow.Margin.Right;
            double oldTop = cow.Margin.Top;
            double oldBottom = cow.Margin.Bottom;

            cow.Margin = new Thickness(oldLeft, oldTop + 35, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -80;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();
            cow.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateFirstCow()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 35;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            cow.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationFirstCow = new DispatcherTimer();
            secondAnimationFirstCow.Tick += secondAnimationFirstCowStart;
            secondAnimationFirstCow.Interval = new TimeSpan(0, 0, 0, 1, 0);

            secondAnimationFirstCow.Start();
        }

        private void AnimateInputBox()
        {
            DoubleAnimation drop = new DoubleAnimation();
            drop.From = 0;
            drop.To = 76;
            drop.AutoReverse = false;
            drop.Duration = new Duration(TimeSpan.FromSeconds(1.000));
            drop.FillBehavior = FillBehavior.HoldEnd;
            TranslateTransform tt = new TranslateTransform();
            inputTextBox.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, drop);
            

            DispatcherTimer secondAnimationInputBox = new DispatcherTimer();
            secondAnimationInputBox.Tick += secondAnimationInputBoxStart;
            secondAnimationInputBox.Interval = new TimeSpan(0, 0, 0, 1, 0);

            secondAnimationInputBox.Start();
        }

        private void secondAnimationInputBoxStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = inputTextBox.Margin.Left;
            double oldRight = inputTextBox.Margin.Right;
            double oldTop = inputTextBox.Margin.Top;
            double oldBottom = inputTextBox.Margin.Bottom;

            inputTextBox.Margin = new Thickness(oldLeft, oldTop + 76, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -5;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();
            inputTextBox.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateRandomNumber()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 46;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            randomNumLabel.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void AnimateCheckButton()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 68;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            checkButton.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void AnimateHitoryLabel()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 20;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            HistoryLabel.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void AnimateHistoryTab()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 10;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.000));

            TranslateTransform tt = new TranslateTransform();
            historyListBox.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }
			
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //var helpform = new Help();
            //helpform.Activate();

            Help OP = new Help();
            OP.Show();
        }
    }
}
