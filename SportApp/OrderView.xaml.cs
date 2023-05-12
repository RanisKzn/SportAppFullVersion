using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace SportApp
{
    /// <summary>
    /// Логика взаимодействия для OrderView.xaml
    /// </summary>
    public partial class OrderView : Window
    {
        private user25Entities3 context = new user25Entities3();
        Order order;
        decimal allProductPrice;
        decimal allProductAmount;
        User currentUser;
        List<OrderProduct> orderProductList;
        public OrderView(List<Product> products, User user)
        {
            InitializeComponent();
            var name = $"{user.UserSurname} {user.UserName} {user.UserPatronymic}";
            currentUser = user;
            order = new Order();
            order.OrderID = context.Order.Max(o => o.OrderID) + 1;
            orderProductList = new List<OrderProduct>();
            PickUpPointCB.ItemsSource = context.PickupPoint.ToList();
            foreach (var product in products)
            {
                var orderProduct = new OrderProduct() { 
                    OrderID = order.OrderID,
                    Product = product,
                    ProductID = product.ProductID,
                    Count = 1 
                };
                orderProductList.Add(orderProduct);
            }
            
            listA.ItemsSource = orderProductList;
            CalculShowPriceAmount(orderProductList);
        } 

        private void CalculShowPriceAmount(List<OrderProduct> productList)
        {
            foreach (var product in productList)
            {
                allProductPrice += product.Product.ProductCost * product.Count;
                if (product.Product.ProductDiscountAmount != null)
                {
                    allProductPrice += Convert.ToDecimal(product.Product.ProductCost - ((product.Product.ProductCost / 100) * product.Product.ProductDiscountAmount));
                    allProductAmount += Convert.ToDecimal(product.Product.ProductDiscountAmount);
                }
            }
            ProductAmountTB.Text = $"{allProductAmount.ToString()}%";
            ProductPriceTB.Text = $"{allProductPrice.ToString()}";
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is OrderProduct orderProduct)
            {
                orderProductList.Where(x => x.ProductID == orderProduct.ProductID).FirstOrDefault().Count++;
            }
            CalculShowPriceAmount(orderProductList);
            listA.ItemsSource = orderProductList;
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is OrderProduct orderProduct)
            {
                orderProductList.Where(x => x.ProductID == orderProduct.ProductID).FirstOrDefault().Count--;
                if(orderProductList.Where(x => x.ProductID == orderProduct.ProductID).FirstOrDefault().Count == 0)
                {
                    orderProductList.Remove(orderProductList.Where(x => x.ProductID == orderProduct.ProductID).FirstOrDefault());
                }
            }
            CalculShowPriceAmount(orderProductList);
            listA.ItemsSource = orderProductList;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
                WorkWithItem workWithItem = new WorkWithItem(currentUser);
                workWithItem.Show();
            
        }

        private DateTime DeliveryDate(List<OrderProduct> oProductList)
        {
            if(oProductList.Count >= 6)
            {
                return DateTime.Now.AddDays(6);
            }
            else
            {
                foreach (var item in oProductList)
                {
                    if(item.Product.ProductQuantityInStock < item.Count) 
                    {
                        return DateTime.Now.AddDays(6);
                    }
                }
            }
            return DateTime.Now.AddDays(3);
            
        }
        private void CreateTalon(Order order, List<OrderProduct> orderProducts)
        {
            var app = new Excel.Application()
            {
                SheetsInNewWorkbook = 1
            };
            app.Workbooks.Add();
            app.Worksheets.Add();
            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            worksheet.Name = "Талон";
            worksheet.Cells[1][1] = "Дата Заказа";
            worksheet.Cells[1][2] = "Номер заказа";
            worksheet.Cells[1][3] = "Список пролуктов";
            worksheet.Cells[1][4] = "Цена";
            worksheet.Cells[1][5] = "Скидка";
            worksheet.Cells[1][6] = "Пункт выдачи";
            worksheet.Cells[1][7] = "Код получения";

            worksheet.Cells[2][1] = order.OrderCreateDate.ToString("d");
            worksheet.Cells[2][2] = order.OrderID;
            string  productList = "";

            foreach (var item in order.OrderProduct)
            {
                productList +=$" {context.Product.Where(x => x.ProductID == item.ProductID).FirstOrDefault().ProductName} \n";
            };
            worksheet.Cells[2][3] = productList;
            worksheet.Cells[2][4] = allProductPrice;
            worksheet.Cells[2][5] = allProductAmount;
            worksheet.Cells[2][6] = context.PickupPoint.Where(x => x.PickupPointID == order.PickupPointID).FirstOrDefault().Address;
            worksheet.Cells[2][7] = order.OrderGetCode;



            worksheet.Columns.AutoFit();

            app.Visible = true;

            app.Application.ActiveWorkbook.SaveAs("C:/Users/deadr/Downloads/SportApp (1)/SportApp/SportApp/SportApp.sln/test.xlsx");

            var excelDocument = app.Workbooks.Open("C:/Users/deadr/Downloads/SportApp (1)/SportApp/SportApp/SportApp.sln/test.xlsx");
            excelDocument.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, @"C:/Users/deadr/Downloads/SportApp (1)/SportApp/SportApp/SportApp.sln/test.xlsx");
            app.Quit();
        }
        private void FormOrderBT_Click(object sender, RoutedEventArgs e)
        {
            if(PickUpPointCB.SelectedItem != null)
            {
                using (user25Entities3 user25Entities2 = new user25Entities3())
                {
                    var maxOrderCode = context.Order.Max(o => o.OrderGetCode);
                    var newOrder = new Order()
                    {
                        OrderCreateDate = DateTime.Now,
                        OrderDeliveryDate = DeliveryDate(orderProductList),
                        PickupPointID = ((PickupPoint)PickUpPointCB.SelectedItem).PickupPointID,
                        UserID = currentUser.UserID,
                        OrderStatusID = 1,
                        OrderGetCode = maxOrderCode++,
                    };
                    user25Entities2.Order.Add(newOrder);
                    foreach (var item in orderProductList)
                    {
                        var newOrderProduct = new OrderProduct() {
                            OrderID = newOrder.OrderID,
                            ProductID = item.ProductID,
                            Count = item.Count
                        };
                        user25Entities2.OrderProduct.Add(newOrderProduct);
                    }
                    user25Entities2.SaveChanges(); 
                    CreateTalon(newOrder, orderProductList);
                }
            }
            MessageBox.Show("norm");
            this.Close();
        }
    }
}
