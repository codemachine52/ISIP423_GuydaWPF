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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public class ReportViewModel
    {
        public report Source { get; }
        public string ReportTypeLabel { get; }
        public Brush ReportTypeBadgeColor { get; }
        public string TargetDescription { get; }
        public string ReporterLogin { get; }
        public string ReportDateFormatted { get; }

        public ReportViewModel(report r)
        {
            Source = r;

            if (r.BookID != null && r.AuthorID != null)
            {
                ReportTypeLabel = "Автор";
                ReportTypeBadgeColor = Brush("#7A6048");
                var author = Core.Context.user_.FirstOrDefault(u => u.ID == r.AuthorID);
                TargetDescription = author != null ? $"Автор: {author.Login}" : $"Автор (ID {r.AuthorID})";
            }
            else if (r.AuthorID == null && r.BookID != null)
            {
                ReportTypeLabel = "Книга";
                ReportTypeBadgeColor = Brush("#5C3D2E");
                TargetDescription = r.book != null ? $"Книга: «{r.book.Name}»" : $"Книга (ID {r.BookID})";
            }
            else if (r.reviewID != null)
            {
                ReportTypeLabel = "Отзыв";
                ReportTypeBadgeColor = Brush("#A08060");
                if (r.review != null)
                {
                    var text = r.review.Description ?? "";
                    if (text.Length > 80) text = text.Substring(0, 80) + "…";
                    TargetDescription = $"Отзыв: «{text}»";
                }
                else TargetDescription = $"Отзыв (ID {r.reviewID})";
            }
            else
            {
                ReportTypeLabel = "Неизвестно";
                ReportTypeBadgeColor = Brushes.Gray;
                TargetDescription = "Цель не определена";
            }
            ReporterLogin = r.user_?.Login ?? "—";
            ReportDateFormatted = r.reportDate.ToString("dd.MM.yyyy");
        }

        private static SolidColorBrush Brush(string hex) =>
            new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
    }
    public class BookAdminViewModel
    {
        public book Source { get; }
        public string Name { get; }
        public string AuthorLogin { get; }
        public string RatingDisplay { get; }
        public bool IsFreeze { get; private set; }
        public string FreezeLabel => IsFreeze ? "Заморожена" : "Активна";
        public Brush FreezeBadgeColor => IsFreeze ? Brush("#C0392B") : Brush("#7A9E7E");
        public string FreezeButtonLabel => IsFreeze ? "Разморозить" : "Заморозить";
        public Brush FreezeButtonColor => IsFreeze ? Brush("#7A9E7E") : Brush("#C0392B");

        public BookAdminViewModel(book b)
        {
            Source = b;
            Name = b.Name ?? "—";
            AuthorLogin = b.user_?.Login ?? "—";
            RatingDisplay = b.Rating.HasValue ? $"⭐ {b.Rating.Value:F1}" : "—";
            IsFreeze = b.IsFreeze;
        }

        public void UpdateFreeze(bool frozen) => IsFreeze = frozen;

        private static SolidColorBrush Brush(string hex) =>
            new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
    }
    public partial class AdminPage : Page
    {
        private user_ _currentAdmin;
        public List<role> AllRoles { get; set; }
        private List<ReportViewModel> _allReports = new List<ReportViewModel>();
        private List<BookAdminViewModel> _allBooks = new List<BookAdminViewModel>();
        public AdminPage(user_ currentUser)
        {
            InitializeComponent();
            _currentAdmin = currentUser;
            AllRoles = Core.Context.role.ToList();
            this.DataContext = this;
            RefreshData();
        }
        private void RefreshData()
        {
            Core.Context.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            DGridUsers.ItemsSource = Core.Context.user_.ToList();
            LBoxUnfreezeRequests.ItemsSource = Core.Context.requestUnFreeze.Include("user_").Include("book").ToList();
            LBoxRoleRequests.ItemsSource = Core.Context.requestRole.Include("user_").ToList();
            _allBooks = Core.Context.book.Include("user_").ToList().Select(b => new BookAdminViewModel(b)).ToList();
            ApplyBookFilter();
            _allReports = Core.Context.report.Include("user_").Include("user_1").Include("book").Include("review").ToList().Select(r => new ReportViewModel(r)).ToList();
            ApplyReportFilter();
        }
        private void ApplyBookFilter()
        {
            if (DGridBooks == null) return;
            IEnumerable<BookAdminViewModel> filtered = _allBooks;
            if (RbBooksActive != null && RbBooksActive.IsChecked == true)
                filtered = _allBooks.Where(b => !b.IsFreeze);
            else if (RbBooksFrozen != null && RbBooksFrozen.IsChecked == true)
                filtered = _allBooks.Where(b => b.IsFreeze);
            DGridBooks.ItemsSource = filtered.ToList();
        }
        private void BookFilter_Changed(object sender, RoutedEventArgs e)
            => ApplyBookFilter();
        private void BtnToggleBookFreeze_Click(object sender, RoutedEventArgs e)
        {
            var vm = (sender as Button)?.Tag as BookAdminViewModel;
            if (vm == null) return;
            vm.Source.IsFreeze = !vm.Source.IsFreeze;
            Core.Context.SaveChanges();
            var action = vm.Source.IsFreeze ? "заморожена" : "разморожена";
            MessageBox.Show($"Книга «{vm.Source.Name}» {action}.");
            RefreshData();
        }
        private void ApplyReportFilter()
        {
            if (LBoxReports == null) return;
            IEnumerable<ReportViewModel> filtered = _allReports;
            if (RbRepBooks != null && RbRepBooks.IsChecked == true)
                filtered = _allReports.Where(r => r.Source.BookID != null);
            else if (RbAuthors != null && RbAuthors.IsChecked == true)
                filtered = _allReports.Where(r => r.Source.AuthorID != null);
            else if (RbReviews != null && RbReviews.IsChecked == true)
                filtered = _allReports.Where(r => r.Source.reviewID != null);
            LBoxReports.ItemsSource = filtered.ToList();
        }
        private void ReportFilter_Changed(object sender, RoutedEventArgs e)
            => ApplyReportFilter();
        private void BtnAcceptReport_Click(object sender, RoutedEventArgs e)
        {
            var vm = (sender as Button)?.Tag as ReportViewModel;
            if (vm == null) return;
            var rep = vm.Source;

            if (rep.BookID != null && rep.AuthorID != null)
            {
                var author = Core.Context.user_.Find(rep.AuthorID);
                if (author != null)
                {
                    author.IsFreeze = true;
                    MessageBox.Show($"Аккаунт автора «{author.Login}» заморожен.");
                }
            }
            else if (rep.BookID != null && rep.book != null)
            {
                rep.book.IsFreeze = true;
                MessageBox.Show($"Книга «{rep.book.Name}» заморожена.");
            }
            else if (rep.reviewID != null && rep.review != null)
            {
                var author = Core.Context.user_.Find(rep.review.UserID);
                if (author != null)
                {
                    author.IsFreeze = true;
                    MessageBox.Show($"Аккаунт «{author.Login}» заморожен за отзыв.");
                }
            }

            Core.Context.report.Remove(rep);
            Core.Context.SaveChanges();
            RefreshData();
        }
        private void BtnDeclineReport_Click(object sender, RoutedEventArgs e)
        {
            var vm = (sender as Button)?.Tag as ReportViewModel;
            if (vm == null) return;
            Core.Context.report.Remove(vm.Source);
            Core.Context.SaveChanges();
            RefreshData();
            MessageBox.Show("Жалоба отклонена.");
        }
        private void FreezeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            var u = cb.DataContext as user_;
            if (u.ID == _currentAdmin.ID)
            {
                MessageBox.Show("Вы не можете заморозить самого себя!");
                u.IsFreeze = false;
                cb.IsChecked = false;
                return;
            }
            Core.Context.SaveChanges();
        }
        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            if (cb == null || !cb.IsLoaded) return;
            var u = cb.DataContext as user_;
            if (u == null) return;
            if (u.ID == _currentAdmin.ID && u.RoleID != 3)
            {
                MessageBox.Show("Вы не можете сменить роль самому себе!");
                u.RoleID = 3;
                cb.SelectedValue = 3;
                return;
            }
            try
            {
                Core.Context.Entry(u).State = System.Data.Entity.EntityState.Modified;
                Core.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении роли: " + ex.Message);
            }
        }
        private void BtnChangePass_Click(object sender, RoutedEventArgs e)
        {
            var u = (sender as Button).Tag as user_;
            u.Password = "password1";
            Core.Context.SaveChanges();
            MessageBox.Show($"Пароль для {u.Login} сброшен на 'password1'");
        }
        private void BtnAcceptUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestUnFreeze;
            if (req.bookID != null) req.book.IsFreeze = false;
            else req.user_.IsFreeze = false;
            Core.Context.requestUnFreeze.Remove(req);
            Core.Context.SaveChanges();
            RefreshData();
        }
        private void BtnDeclineUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestUnFreeze;
            if (req == null) return;
            Core.Context.requestUnFreeze.Remove(req);
            Core.Context.SaveChanges();
            RefreshData();
            MessageBox.Show("Заявка на разморозку отклонена");
        }
        private void BtnAcceptAuthor_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestRole;
            req.user_.RoleID = 2;
            Core.Context.requestRole.Remove(req);
            Core.Context.SaveChanges();
            RefreshData();
        }
        private void BtnDeclineAuthor_Click(object sender, RoutedEventArgs e)
        {
            var req = (sender as Button).Tag as requestRole;
            if (req == null) return;
            Core.Context.requestRole.Remove(req);
            Core.Context.SaveChanges();
            RefreshData();
            MessageBox.Show("Заявка на роль автора отклонена");
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) => RefreshData();
    }
}
