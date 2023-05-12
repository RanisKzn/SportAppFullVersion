using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace SportApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private user25Entities3 context =  new user25Entities3();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(context.User.Any(u => u.UserLogin == userLogin.Text && u.UserPassword == userPassword.Text))
            {
                var user = context.User.Where(u => u.UserLogin == userLogin.Text && u.UserPassword == userPassword.Text).FirstOrDefault();
                
                if (user.RoleID == 1)
                {
                    WorkWithItem workwithItem = new WorkWithItem(user);
                    workwithItem.Show();
                }
                else if(user.RoleID == 2)
                {
                    AdminPanelWindow adminPanelWindow = new AdminPanelWindow(user);
                    adminPanelWindow.Show();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Ввден неправильный логин или пароль");
                CaptchaForUser captcha = new CaptchaForUser();
                captcha.Show();
                this.Close();
               
            }
        }

        public void Sleep()
        {
            int milliseconds = 5000;
            Thread.Sleep(milliseconds);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WorkWithItem workwithItem = new WorkWithItem();
            this.Close();
            workwithItem.Show();
        }
    }
}
