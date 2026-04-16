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

namespace Anastasia423WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для OrderDetailsWindow.xaml
    /// </summary>
    public partial class OrderDetailsWindow : Window
    {
        public OrderDetailsWindow(Order selectedOrder)
        {
            InitializeComponent();

            OrderNumberTxt.Text = $"Заказ №{selectedOrder.ID}";
            OrderDateTxt.Text = $"Дата оформления: {selectedOrder.OrderDate:dd.MM.yyyy}";

            // Загружаем список товаров в этом заказе из таблицы Product_Order
            // Обязательно подтягиваем данные о самом продукте через .Product
            OrderItemsList.ItemsSource = Core.Context.Product_Order
                .Where(po => po.OrderID == selectedOrder.ID)
                .ToList();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
