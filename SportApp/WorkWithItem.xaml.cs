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

namespace SportApp
{
    /// <summary>
    /// Логика взаимодействия для WorkWithItem.xaml
    /// </summary>
    public partial class WorkWithItem : Window
    {
        
        private user25Entities3 context = new user25Entities3();
        List<Product> products;
        User currentUser;
        public Visibility ShowOrderVisivle {
            get 
            {
                if (products != null || products.Count != 0)
                {
                    return Visibility.Visible;
                }
                return Visibility.Hidden;
            }
            set { }
        }
        public WorkWithItem()
        {
            InitializeComponent();
            var list = context.Product.OrderBy(x => x.ProductName).ToList();
            listA.ItemsSource = list;
            filter.ItemsSource = new List<String>(){
                "all range","0-10%","10-15%","15-100%"
            };

            order.ItemsSource = new List<String>(){
                "По возрастанию","По убыванию"
            };
            AllCount.Text = list.Count.ToString();
            CountTB.Text = list.Count.ToString();

        }
        public WorkWithItem(User user) 
        {
            
            InitializeComponent();
            currentUser = user;
            var name = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
            ClientNameTB.Text = name;
            var list = context.Product.OrderBy(x => x.ProductName).ToList();
            listA.ItemsSource = list;
            filter.ItemsSource = new List<String>(){
                "all range","0-10%","10-15%","15-100%"
            };

            order.ItemsSource = new List<String>(){
                "По возрастанию","По убыванию"
            };
            AllCount.Text = list.Count.ToString();
            CountTB.Text = list.Count.ToString();
        }
        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(filter.SelectedIndex == 1)
            {
                var list = context.Product.Where(u => u.ProductDiscountAmount > 0 && u.ProductDiscountAmount < 10).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            if (filter.SelectedIndex == 2)
            {
                var list = context.Product.Where(u => u.ProductDiscountAmount > 10 && u.ProductDiscountAmount < 15).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            if(filter.SelectedIndex == 3)
            {
                var list = context.Product.Where(u => u.ProductDiscountAmount > 15 && u.ProductDiscountAmount < 100).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            if (filter.SelectedIndex == 0)
            {
                var list = context.Product.ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
        }

        private void order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter.SelectedIndex == 0)
            {
                var list = context.Product.OrderBy(x => x.ProductCost).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            if (filter.SelectedIndex == 1)
            {
                var list = context.Product.OrderByDescending(x => x.ProductCost).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
        }

        private void SerchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(SerchTB.Text))
            {
                var startList = context.Product.OrderBy(x => x.ProductName).ToList();
                listA.ItemsSource = startList;
                CountTB.Text = startList.Count.ToString();
            }
            else
            {
                var list = context.Product.Where(x => x.ProductName.Contains(SerchTB.Text)).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void ShowOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderView orderView = new OrderView(products, currentUser);
            this.Close();
            orderView.Show();
            
        }

        private void AddInOrder_Click(object sender, RoutedEventArgs e)
        {
            if(products == null)
            {
                products = new List<Product>();
            }
            if ((sender as MenuItem).DataContext is Product product)
            {
                products.Add(product);
            }
            ShowOrder.Visibility = Visibility.Visible;
        }
    }
}
