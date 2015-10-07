using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BullsCowsProject
{
    public partial class Help : Window
    {
        internal MainWindow creatingForm;

        public MainWindow setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        public Help()
        {
            InitializeComponent();
        }

        private void DevBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "\r")
            {
                if (DevBox.Text == "devmode.enabled")
                {
                    creatingForm.TimerStart();
                    creatingForm.inputTextBox.Focus();
                    this.Close();
                }
            }
        }

        private void RulesLabel_Activate(object sender, MouseButtonEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/Resources/mark.png");
            MarkImage.Source = new BitmapImage(uri);
            DevBox.Focus();
        }
    }
}
