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
using EasyCaptcha.Wpf;

namespace SportApp
{
    /// <summary>
    /// Логика взаимодействия для Captcha.xaml
    /// </summary>
    public partial class CaptchaForUser : Window
    {
        public CaptchaForUser()
        {
            InitializeComponent();
            MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            var answer = MyCaptcha.CaptchaText;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MyCaptcha.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            answerTB.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(MyCaptcha.CaptchaText == answerTB.Text)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                mainWindow.Sleep();

            }
            else
            {
                MessageBox.Show("Неправильно");
            }
        }
    }
}
