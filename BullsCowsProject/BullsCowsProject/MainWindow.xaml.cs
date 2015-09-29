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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            //InitializeComponent();
            Loaded += MyWindow_Loaded;
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateNumber();
        }

        void GenerateNumber()
        {
            for (int i = 0; i < number.Length; i++)
            {
                number[i] = GenerateRandomDigit();

            }
        }

        private int GenerateRandomDigit()
        {
            Random randomizer = new Random();
            int digit;
            do
            {
                digit = randomizer.Next(1, 9);

            } while (number.Contains(digit));
            return digit;
        }
        void btnTry_Click(object sender, RoutedEventArgs e)
        {
            GetValueFromTextBox();
        }

        void GetValueFromTextBox()
        {
            string playerNumber = txtInput.Text;
            int[] playerDigits = playerNumber.ToCharArray().Select(d => Convert.ToInt32(d) - 48).ToArray();
            FindBullsCows(playerDigits);
        }

        void FindBullsCows(int[] playerDigits)
        {
            
        }
    }
}
