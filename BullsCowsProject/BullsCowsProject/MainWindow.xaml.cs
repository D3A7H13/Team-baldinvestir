using System;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BullsCowsProject
{
    public partial class MainWindow : Window
    {
        public static int moves = 0;
        static int[] number = new int[4];

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            moves = 0;
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
            IsUnique(e);
        }

        private void IsUnique(TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(checkButton);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
            else
            {
                int input;
                if (int.TryParse(e.Text, out input))
                {
                    int[] previousInput = inputTextBox.Text.ToCharArray().Select(d => Convert.ToInt32(d) - 48).ToArray();
                    for (int i = 0; i < previousInput.Length; i++)
                    {
                        if (input == previousInput[i])
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
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
            if (inputTextBox.Text.Length != 4)
            {
                MessageBox.Show("The input must be 4 digits");
            }
            else
            {
                Image[] imgArray = new Image[] { bull, bull2, bull3, bull4, cow, cow2, cow3, cow4 };
                for (int i = 0; i < imgArray.Length; i++)
                {
                    imgArray[i].Source = null;
                }

                GetValueFromTextBox();
            }
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
            moves++;

            DrawCows(cows);
            DrawBulls(bulls);
            AddHistory(string.Join("", playerDigits), bulls, cows);

            if (bulls == 4)
            {
                VictoryScreen victory = new VictoryScreen(moves);
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
                history.Content = ("The number was " + playerNumber + ". You won in " + moves + " moves!");
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

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Help OP = new Help();
            OP.setCreatingForm = this;
            OP.Show();
        }

        //Animations Code
        public void TimerStart()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += TimerBeforeAnimations;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            dispatcherTimer.Start();
        }

        private void TimerBeforeAnimations(object sender, EventArgs e)
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
            AnimateFirstBull();
            AnimateSecondBull();
            AnimateThirdBull();
            AnimateForthBull();
            AnimateTitle();
            AnimateHelp();
        }

        private void secondAnimationHelpButtonStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = HelpButton.Margin.Left;
            double oldRight = HelpButton.Margin.Right;
            double oldTop = HelpButton.Margin.Top;
            double oldBottom = HelpButton.Margin.Bottom;

            HelpButton.Margin = new Thickness(oldLeft, oldTop + 120, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -5;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.250));

            RotateTransform rt = new RotateTransform();
            HelpButton.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateHelp()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 120;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.750));

            TranslateTransform tt = new TranslateTransform();
            HelpButton.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationHelpButton = new DispatcherTimer();
            secondAnimationHelpButton.Tick += secondAnimationHelpButtonStart;
            secondAnimationHelpButton.Interval = new TimeSpan(0, 0, 0, 1, 750);

            secondAnimationHelpButton.Start();
        }

        private void AnimateTitle()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 115;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.500));

            TranslateTransform tt = new TranslateTransform();
            TitleLabel.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void ThirdAnimationForthBullStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = bull4.Margin.Left;
            double oldRight = bull4.Margin.Right;
            double oldTop = bull4.Margin.Top;
            double oldBottom = bull4.Margin.Bottom;

            bull4.RenderTransformOrigin = new Point(0, 0.5);

            bull4.Margin = new Thickness(oldLeft - 13, oldTop + 48, oldRight + 13, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = -45;
            rotate.To = -30;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.250));

            RotateTransform rt = new RotateTransform();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 7;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.250));

            TranslateTransform tt = new TranslateTransform();

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            bull4.RenderTransform = tg;

            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void secondAnimationForthBullStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = bull4.Margin.Left;
            double oldRight = bull4.Margin.Right;
            double oldTop = bull4.Margin.Top;
            double oldBottom = bull4.Margin.Bottom;

            bull4.Margin = new Thickness(oldLeft, oldTop + 85, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -45;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 20;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);

            bull4.RenderTransform = tg;

            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer ThirdAnimationForthBull = new DispatcherTimer();
            ThirdAnimationForthBull.Tick += ThirdAnimationForthBullStart;
            ThirdAnimationForthBull.Interval = new TimeSpan(0, 0, 0, 0, 500);

            ThirdAnimationForthBull.Start();
        }

        private void AnimateForthBull()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 85;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.200));

            TranslateTransform tt = new TranslateTransform();
            bull4.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationForthBull = new DispatcherTimer();
            secondAnimationForthBull.Tick += secondAnimationForthBullStart;
            secondAnimationForthBull.Interval = new TimeSpan(0, 0, 0, 1, 200);

            secondAnimationForthBull.Start();
        }

        private void secondAnimationThirdBullStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = bull3.Margin.Left;
            double oldRight = bull3.Margin.Right;
            double oldTop = bull3.Margin.Top;
            double oldBottom = bull3.Margin.Bottom;

            bull3.Margin = new Thickness(oldLeft, oldTop + 100, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = 55;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.750));

            RotateTransform rt = new RotateTransform();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 18;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.750));

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = 30;
            da2.AutoReverse = false;
            da2.Duration = new Duration(TimeSpan.FromSeconds(0.750));

            TranslateTransform tt = new TranslateTransform();

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);


            bull3.RenderTransform = tg;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
            tt.BeginAnimation(TranslateTransform.XProperty, da2);
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void AnimateThirdBull()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 100;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.100));

            TranslateTransform tt = new TranslateTransform();
            bull3.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationThirdBull = new DispatcherTimer();
            secondAnimationThirdBull.Tick += secondAnimationThirdBullStart;
            secondAnimationThirdBull.Interval = new TimeSpan(0, 0, 0, 1, 100);

            secondAnimationThirdBull.Start();
        }

        private void secondAnimationSecondBullStart(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double oldLeft = bull2.Margin.Left;
            double oldRight = bull2.Margin.Right;
            double oldTop = bull2.Margin.Top;
            double oldBottom = bull2.Margin.Bottom;

            bull2.Margin = new Thickness(oldLeft, oldTop + 60, oldRight, oldBottom);

            DoubleAnimation rotate = new DoubleAnimation();
            rotate.From = 0;
            rotate.To = -45;
            rotate.AutoReverse = false;
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            RotateTransform rt = new RotateTransform();

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 20;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = -8;
            da2.AutoReverse = false;
            da2.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();

            TransformGroup tg = new TransformGroup();
            tg.Children.Add(rt);
            tg.Children.Add(tt);


            bull2.RenderTransform = tg;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
            tt.BeginAnimation(TranslateTransform.XProperty, da2);
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

        private void AnimateSecondBull()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 60;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.900));

            TranslateTransform tt = new TranslateTransform();
            bull2.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationSecondBull = new DispatcherTimer();
            secondAnimationSecondBull.Tick += secondAnimationSecondBullStart;
            secondAnimationSecondBull.Interval = new TimeSpan(0, 0, 0, 0, 900);

            secondAnimationSecondBull.Start();
        }

        private void AnimateFirstBull()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 110;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.300));

            TranslateTransform tt = new TranslateTransform();
            bull.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();
            cow4.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationForthCow = new DispatcherTimer();
            secondAnimationForthCow.Tick += secondAnimationForthCowStart;
            secondAnimationForthCow.Interval = new TimeSpan(0, 0, 0, 0, 500);

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();
            cow3.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationThirdCow = new DispatcherTimer();
            secondAnimationThirdCow.Tick += secondAnimationThirdCowStart;
            secondAnimationThirdCow.Interval = new TimeSpan(0, 0, 0, 0, 500);

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();
            cow2.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationSecondCow = new DispatcherTimer();
            secondAnimationSecondCow.Tick += secondAnimationSecondCowStart;
            secondAnimationSecondCow.Interval = new TimeSpan(0, 0, 0, 0, 500);

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();
            cow.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);

            DispatcherTimer secondAnimationFirstCow = new DispatcherTimer();
            secondAnimationFirstCow.Tick += secondAnimationFirstCowStart;
            secondAnimationFirstCow.Interval = new TimeSpan(0, 0, 0, 0, 500);

            secondAnimationFirstCow.Start();
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
            rotate.Duration = new Duration(TimeSpan.FromSeconds(0.200));

            RotateTransform rt = new RotateTransform();
            inputTextBox.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, rotate);
        }

        private void AnimateInputBox()
        {
            DoubleAnimation drop = new DoubleAnimation();
            drop.From = 0;
            drop.To = 76;
            drop.AutoReverse = false;
            drop.Duration = new Duration(TimeSpan.FromSeconds(0.750));

            TranslateTransform tt = new TranslateTransform();
            inputTextBox.RenderTransform = tt;

            tt.BeginAnimation(TranslateTransform.YProperty, drop);

            DispatcherTimer secondAnimationInputBox = new DispatcherTimer();
            secondAnimationInputBox.Tick += secondAnimationInputBoxStart;
            secondAnimationInputBox.Interval = new TimeSpan(0, 0, 0, 0, 750);

            secondAnimationInputBox.Start();
        }

        private void AnimateRandomNumber()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 46;
            da.AutoReverse = false;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.750));

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.750));

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

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
            da.Duration = new Duration(TimeSpan.FromSeconds(0.500));

            TranslateTransform tt = new TranslateTransform();
            historyListBox.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.YProperty, da);
        }

    }
}