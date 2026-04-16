using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Anastasia423WPF.Model;

namespace Anastasia423WPF.Pages
{
    public partial class CartPage : Page
    {
        public User user { get; set; }
        public CartPage(User user1)
        {
            user = user1;
            InitializeComponent();
            RefreshCart();
        }

        private void RefreshCart()
        {
            CartList.ItemsSource = null;
            CartList.ItemsSource = ShoppingCart.Items;

            // Считаем сумму (используем нашу логику из partial или просто Sum)
            double total = ShoppingCart.Items.Sum(p => (double)p.Price * (1 - p.Discount / 100));
            TotalSumText.Text = $"{total:N0} ₽";
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            if (ShoppingCart.Items.Count == 0)
            {
                MessageBox.Show("Невозможно оформить заказ! Корзина пуста", "Ошибка заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var answ = MessageBox.Show("Желаете оплатить картой онлайн? При выборе варианта 'нет' оплата будет при получении.", "Оплата заказа", MessageBoxButton.YesNo, MessageBoxImage.Question);
            using (var db = new MG_CosmeticEntities()) //наша бд
            {
                // 1. Создаем запись в таблице Order
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    ReceiveDate = DateTime.Now.AddDays(3), // доставка через 3 дня для примера
                    ClientID = user.ID,             // ID авторизованного юзера
                    Status = "Новый"
                };
                if (answ == MessageBoxResult.Yes)
                {
                    newOrder.PaymentWay = "Картой онлайн";
                }
                else
                {
                    newOrder.PaymentWay = "При получении";
                }

                db.Order.Add(newOrder);
                db.SaveChanges(); // Сохраняем, чтобы получить ID заказа

                // 2. Сохраняем состав заказа в Product_Order
                foreach (var item in ShoppingCart.Items)
                {
                    var orderDetail = new Product_Order
                    {
                        OrderID = newOrder.ID,
                        ProductID = item.ID,
                        CountProd = 1 // Для начала по одному, потом можно добавить счетчик
                    };
                    db.Product_Order.Add(orderDetail);
                }

                db.SaveChanges();
                MessageBox.Show($"Заказ №{newOrder.ID} оформлен!");
                ShoppingCart.Items.Clear();
                NavigationService.GoBack();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}