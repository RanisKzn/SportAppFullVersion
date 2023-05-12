using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Логика взаимодействия для WorkWithProductAdmin.xaml
    /// </summary>
    public partial class WorkWithProductAdmin : Window
    {
        string ModeName;
        AdminPanelWindow adminPanelWindow;
        Product product;
        private user25Entities3 context = new user25Entities3();
        public WorkWithProductAdmin(int ModeName, AdminPanelWindow adminPanelWindow, Product product)
        {
            InitializeComponent();

            ManufacturerList.ItemsSource = context.ProductManufacturer.ToList();
            SupplierList.ItemsSource = context.ProductSupplier.ToList();
            CategoryList.ItemsSource = context.ProductCategory.ToList();
            UnitTypeCB.ItemsSource = context.UnitType.ToList();

            this.adminPanelWindow = adminPanelWindow;
            if(ModeName == 1)
            {
                this.product = product;
                Enter.Content = "Редактировать";
                ArticleTB.Text = product.ProductArticleNumber;
                NameTB.Text = product.ProductName;
                CostTB.Text = product.ProductCost.ToString();
                MaxAmountTB.Text = product.ProductMaxDiscountAmount.ToString();

                ManufacturerList.SelectedItem = ((List<ProductManufacturer>)ManufacturerList.ItemsSource).Find(m => m.ProductManufacturerID == product.ProductManufacturerID);
                SupplierList.SelectedItem = ((List<ProductSupplier>)SupplierList.ItemsSource).Find(m => m.ProductSupplierID == product.ProductSupplierID);
                CategoryList.SelectedItem = ((List<ProductCategory>)CategoryList.ItemsSource).Find(m => m.ProductCategoryID == product.ProductCategoryID);
                UnitTypeCB.SelectedItem = ((List<UnitType>)UnitTypeCB.ItemsSource).Find(m => m.UnitTypeID == product.UnitTypeID);

                AmountTB.Text = product.ProductDiscountAmount.ToString();
                CountTB.Text = product.ProductQuantityInStock.ToString();
                DescriptionTB.Text = product.ProductDescription;
            }
            else
            {
                Enter.Content = "Добавить";
            }

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            adminPanelWindow.Visibility = Visibility.Visible;
            this.Close();
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            using (user25Entities3 user25Entities2 = new user25Entities3())
            {
                if (Enter.Content == "Редактировать")
                {
                    this.product.ProductArticleNumber = ArticleTB.Text;
                    this.product.ProductName = NameTB.Text;
                    this.product.ProductCost = Convert.ToDecimal(CostTB.Text);
                    this.product.UnitTypeID = ((UnitType)UnitTypeCB.SelectedItem).UnitTypeID;
                    this.product.ProductMaxDiscountAmount = Convert.ToByte(MaxAmountTB.Text);
                    this.product.ProductManufacturerID = ((ProductManufacturer)ManufacturerList.SelectedItem).ProductManufacturerID;
                    this.product.ProductSupplierID = ((ProductSupplier)SupplierList.SelectedItem).ProductSupplierID;
                    this.product.ProductCategoryID = ((ProductCategory)CategoryList.SelectedItem).ProductCategoryID;
                    this.product.ProductDiscountAmount = Convert.ToByte(AmountTB.Text);
                    this.product.ProductQuantityInStock = Convert.ToInt32(CountTB.Text);
                    this.product.ProductDescription = DescriptionTB.Text;

                    user25Entities2.Product.AddOrUpdate(this.product);
                    user25Entities2.SaveChanges();
                    MessageBox.Show("Norm");
                }
                else if(Enter.Content == "Добавить")
                {
                    this.product = new Product
                    {
                        ProductArticleNumber = ArticleTB.Text,
                        ProductName = NameTB.Text,
                        ProductCost = Convert.ToDecimal(CostTB.Text),
                        UnitTypeID = ((UnitType)UnitTypeCB.SelectedItem).UnitTypeID,
                        ProductMaxDiscountAmount = Convert.ToByte(MaxAmountTB.Text),
                        ProductManufacturerID = ((ProductManufacturer)ManufacturerList.SelectedItem).ProductManufacturerID,
                        ProductSupplierID = ((ProductSupplier)SupplierList.SelectedItem).ProductSupplierID,
                        ProductCategoryID = ((ProductCategory)CategoryList.SelectedItem).ProductCategoryID,
                        ProductDiscountAmount = Convert.ToByte(AmountTB.Text),
                        ProductQuantityInStock = Convert.ToInt32(CountTB.Text),
                        ProductDescription = DescriptionTB.Text
                    };

                    user25Entities2.Product.Add(this.product);
                    user25Entities2.SaveChanges();
                    MessageBox.Show("Norm");
                }
            }
            adminPanelWindow.Visibility = Visibility.Visible;
            adminPanelWindow.listA.ItemsSource = context.Product.OrderBy(x => x.ProductName).ToList();
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (user25Entities3 user25Entities2 = new user25Entities3())
            {
                user25Entities2.Product.Remove(user25Entities2.Product.ToList().FirstOrDefault(p => p.ProductID == this.product.ProductID));   
                user25Entities2.SaveChanges();
            }
            adminPanelWindow.Visibility = Visibility.Visible;
            adminPanelWindow.listA.ItemsSource = context.Product.OrderBy(x => x.ProductName).ToList();
            this.Close();
        }
    }
}
