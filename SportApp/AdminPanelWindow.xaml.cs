﻿using System;
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
    /// Логика взаимодействия для AdminPanelWindow.xaml
    /// </summary>
    public partial class AdminPanelWindow : Window
    {

        private user25Entities3 context = new user25Entities3();
        public AdminPanelWindow(User user)
        {
            var name = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
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

        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (filter.SelectedIndex == 1)
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
            if (filter.SelectedIndex == 3)
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
            if (order.SelectedIndex == 0)
            {
                var list = context.Product.OrderBy(x => x.ProductCost).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
            if (order.SelectedIndex == 1)
            {
                var list = context.Product.OrderByDescending(x => x.ProductCost).ToList();
                listA.ItemsSource = list;
                CountTB.Text = list.Count.ToString();
            }
        }

        private void SerchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(SerchTB.Text))
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

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            if((sender as Grid).DataContext is Product product)
            {
                WorkWithProductAdmin workWithProductAdmin = new WorkWithProductAdmin(1, this, product);
                workWithProductAdmin.Show();
            }
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            WorkWithProductAdmin workWithProductAdmin = new WorkWithProductAdmin(2, this, new Product());
            workWithProductAdmin.Show();

        }
    }
}
