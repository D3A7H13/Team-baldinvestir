using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
                inputTextBox.Clear();
            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Help OP = new Help();
            OP.Show();
        }
    }
}
