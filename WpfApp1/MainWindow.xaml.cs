using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using RoguelikeWPF.Engine;
using RoguelikeWPF.Models;

namespace RoguelikeWPF
{
    public partial class MainWindow : Window
    {
        private GameEngine _engine;
        private Item _currentLoot;
        private const string ImagesFolder = "Images";

        public MainWindow()
        {
            InitializeComponent();
            _engine = new GameEngine();

            // ЗАГРУЗКА ФОНА КОМНАТЫ
            try
            {
                string bgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder, "room_bg.jpg");
                if (File.Exists(bgPath))
                {
                    BitmapImage bgImage = new BitmapImage();
                    bgImage.BeginInit();
                    bgImage.CacheOption = BitmapCacheOption.OnLoad;
                    bgImage.UriSource = new Uri(bgPath);
                    bgImage.EndInit();

                    BackgroundImage.Source = bgImage;
                }
            }
            catch (Exception ex)
            {
                // Если что-то пойдет не так, фон просто останется белым
                _engine.Log("Ошибка загрузки фона: " + ex.Message);
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            StartMenu.Visibility = Visibility.Collapsed;
            GameOverScreen.Visibility = Visibility.Collapsed;
            GameScreen.Visibility = Visibility.Visible;

            _engine.ResetGame();
            EnterNewRoom();
        }

        private void EnterNewRoom()
        {
            _engine.NextRoom();
            UpdateUI();

            if (_engine.CurrentEnemies.Any())
            {
                Enemy firstEnemy = _engine.CurrentEnemies.First();

                if (firstEnemy.IsBoss)
                    TxtRoomContent.Text = "БОСС: " + firstEnemy.Name + "!";
                else
                    TxtRoomContent.Text = "Враги (" + _engine.CurrentEnemies.Count + " шт.): " + firstEnemy.Name;

                CombatPanel.Visibility = Visibility.Visible;
                LootPanel.Visibility = Visibility.Collapsed;

                // Прячем картинку лута, так как сейчас бой
                LootImage.Visibility = Visibility.Collapsed;

                LoadEnemyImage(firstEnemy);
            }
            else
            {
                TxtRoomContent.Text = "СУНДУК";
                _currentLoot = _engine.GenerateLoot();

                // Показываем картинку сундука слева
                SetRoomImage("chest.png");

                // Показываем элемент лута справа
                LootImage.Visibility = Visibility.Visible;

                // Выбираем картинку для выпавшего предмета
                if (_currentLoot is ItemPotion)
                {
                    SetLootImage("zelie.png");
                }
                else if (_currentLoot is ItemArmor)
                {
                    SetLootImage("armor.png");
                }
                else
                {
                    SetLootImage("mech.png");
                }

                TxtLootInfo.Text = "В сундуке: " + _currentLoot.Name + "\nВзять?";

                CombatPanel.Visibility = Visibility.Collapsed;
                LootPanel.Visibility = Visibility.Visible;
            }
        }

        private void LoadEnemyImage(Enemy enemy)
        {
            string imageName = "goblin.png";

            if (enemy.IsBoss)
            {
                switch (enemy.Type)
                {
                    case EnemyType.Goblin:
                        imageName = "vvg_boss.jpg";
                        break;
                    case EnemyType.Skeleton:
                        imageName = "kovalski_boss.png";
                        break;
                    case EnemyType.Mage:
                        imageName = "cplusplus_boss.jpg";
                        break;
                    default:
                        imageName = "goblin.png";
                        break;
                }
            }
            else
            {
                switch (enemy.Type)
                {
                    case EnemyType.Goblin:
                        imageName = "goblin.png";
                        break;
                    case EnemyType.Skeleton:
                        imageName = "skeleton.png";
                        break;
                    case EnemyType.Mage:
                        imageName = "mage.png";
                        break;
                    default:
                        imageName = "skeleton.jpg";
                        break;
                }
            }

            SetRoomImage(imageName);
        }

        private void SetRoomImage(string fileName)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder, fileName);

                if (File.Exists(path))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(path);
                    image.EndInit();
                    RoomImage.Source = image;
                }
                else
                {
                    _engine.Log("Картинка не найдена: " + fileName);
                    RoomImage.Source = null;
                }
            }
            catch (Exception ex)
            {
                _engine.Log("Ошибка загрузки фото: " + ex.Message);
            }
        }

        private void SetLootImage(string fileName)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ImagesFolder, fileName);

                if (File.Exists(path))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(path);
                    image.EndInit();
                    LootImage.Source = image;
                }
                else
                {
                    LootImage.Source = null;
                }
            }
            catch (Exception ex)
            {
                _engine.Log("Ошибка загрузки фото лута: " + ex.Message);
            }
        }

        private void UpdateUI()
        {
            TxtFloor.Text = "Этаж: " + _engine.CurrentFloor;
            TxtHP.Text = _engine.Hero.CurrentHP + "/" + _engine.Hero.MaxHP;

            TxtStatus.Text = _engine.Hero.IsFrozen ? "❄ ЗАМОРОЖЕН ❄" : "";

            if (_engine.Hero.CurrentWeapon != null)
                TxtWeapon.Text = "Оружие: " + _engine.Hero.CurrentWeapon.Name + " (+" + _engine.Hero.CurrentWeapon.Damage + ")";
            else
                TxtWeapon.Text = "Оружие: Кулаки (+0)";

            if (_engine.Hero.CurrentArmor != null)
                TxtArmor.Text = "Доспех: " + _engine.Hero.CurrentArmor.Name + " (+" + _engine.Hero.CurrentArmor.DefenseBonus + ")";
            else
                TxtArmor.Text = "Доспех: Нет (+0)";

            ListLog.ItemsSource = null;
            ListLog.ItemsSource = _engine.EventLog;
            if (ListLog.Items.Count > 0)
                ListLog.ScrollIntoView(ListLog.Items[ListLog.Items.Count - 1]);

            if (_engine.IsGameOver)
            {
                GameScreen.Visibility = Visibility.Collapsed;
                GameOverScreen.Visibility = Visibility.Visible;
            }
        }

        private void ExecuteTurn(bool isDefending)
        {
            bool roomCleared = _engine.ProcessTurn(isDefending);
            UpdateUI();

            if (roomCleared && !_engine.IsGameOver)
            {
                CombatPanel.Visibility = Visibility.Collapsed;
                EnterNewRoom();
            }
        }

        private void BtnAttack_Click(object sender, RoutedEventArgs e)
        {
            ExecuteTurn(false);
        }
        private void BtnDefend_Click(object sender, RoutedEventArgs e)
        { 
            ExecuteTurn(true);
        }

        private void BtnTakeLoot_Click(object sender, RoutedEventArgs e)
        {
            if (_currentLoot is ItemPotion)
            {
                _engine.Hero.CurrentHP = _engine.Hero.MaxHP;
                _engine.Log("Вы выпили зелье. Здоровье восстановлено!");
            }
            else if (_currentLoot is ItemWeapon)
            {
                _engine.Hero.CurrentWeapon = (ItemWeapon)_currentLoot;
                _engine.Log("Вы экипировали: " + _currentLoot.Name);
            }
            else if (_currentLoot is ItemArmor)
            {
                _engine.Hero.CurrentArmor = (ItemArmor)_currentLoot;
                _engine.Log("Вы надели: " + _currentLoot.Name);
            }

            EnterNewRoom();
        }

        private void BtnDropLoot_Click(object sender, RoutedEventArgs e)
        {
            _engine.Log("Вы выбросили: " + _currentLoot.Name);
            EnterNewRoom();
        }
    }
}