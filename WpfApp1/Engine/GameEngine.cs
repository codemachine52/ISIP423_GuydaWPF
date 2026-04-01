using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoguelikeWPF.Models;

namespace RoguelikeWPF.Engine
{
    //класс отвечает за генерацию событий, математику боя и ведение журнала (логи игры).
    public class GameEngine
    {
        public Player Hero { get; private set; }
        public int CurrentFloor { get; private set; }
        public List<Enemy> CurrentEnemies { get; private set; }
        public List<string> EventLog { get; private set; }
        public bool IsGameOver { get; private set; }

        private Random _rnd = new Random();

        public GameEngine()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            Hero = new Player();
            CurrentFloor = 0;
            EventLog = new List<string>();
            CurrentEnemies = new List<Enemy>();
            IsGameOver = false;
            Log("Игра началась! Добро пожаловать в подземелье.");
        }

        public void Log(string message)
        {
            EventLog.Add($"[Этаж {CurrentFloor}] {message}");
        }

        // Переход на следующий ход/комнату
        public void NextRoom()
        {
            CurrentFloor++;
            CurrentEnemies.Clear();

            if (CurrentFloor % 4 == 0) //босс только на каждом 4 этаже
            {
                SpawnBoss();
            }
            else
            {
                // 50/50 Сундук или Враг
                if (_rnd.NextDouble() < 0.5)
                {
                    Log("Вы нашли сундук!");
                    // Логика сундука обрабатывается на уровне UI (ожидание выбора игрока)
                }
                else
                {
                    SpawnEnemies();
                }
            }
        }

        private void SpawnEnemies() //появление врагов
        {
            int count = _rnd.Next(1, 4); // От 1 до 3 врагов
            for (int i = 0; i < count; i++)
            {
                CurrentEnemies.Add(GenerateRegularEnemy());
            }
            Log($"Вы встретили врагов: {count} шт.");
        }

        private Enemy GenerateRegularEnemy()
        {
            int typeRoll = _rnd.Next(3);
            Enemy e = new Enemy();
            switch (typeRoll)
            {
                case 0: // Гоблин
                    e.Name = "Гоблин"; e.Type = EnemyType.Goblin;
                    e.MaxHP = 30; e.Attack = 12; e.Defense = 3; e.CritChance = 0.20;
                    break;
                case 1: // Скелет
                    e.Name = "Скелет"; e.Type = EnemyType.Skeleton;
                    e.MaxHP = 40; e.Attack = 10; e.Defense = 5; e.IgnoresArmor = true;
                    break;
                case 2: // Маг
                    e.Name = "Маг"; e.Type = EnemyType.Mage;
                    e.MaxHP = 25; e.Attack = 15; e.Defense = 2; e.FreezeChance = 0.15;
                    break;
            }
            e.CurrentHP = e.MaxHP;
            return e;
        }

        private void SpawnBoss() //появление босса
        {
            Enemy boss = new Enemy() { IsBoss = true };
            int bossRoll = _rnd.Next(4);
            // Базовые статы для расчетов
            double baseHp = 0, baseAtk = 0, baseDef = 0;

            switch (bossRoll)
            {
                case 0: // ВВГ (Гоблин)
                    boss.Name = "Босс ВВГ"; boss.Type = EnemyType.Goblin;
                    baseHp = 30; baseAtk = 12; baseDef = 3;
                    boss.MaxHP = (int)(baseHp * 2.0); boss.Attack = (int)(baseAtk * 1.5); boss.Defense = (int)(baseDef * 1.2);
                    boss.CritChance = 0.20 + 0.10;
                    break;
                case 1: // Ковальский (Скелет)
                    boss.Name = "Босс Ковальский"; boss.Type = EnemyType.Skeleton;
                    baseHp = 40; baseAtk = 10; baseDef = 5;
                    boss.MaxHP = (int)(baseHp * 2.5); boss.Attack = (int)(baseAtk * 1.3); boss.Defense = (int)(baseDef * 1.4);
                    boss.IgnoresArmor = true;
                    break;
                case 2: // Архимаг C++ (Маг)
                    boss.Name = "Архимаг C++"; boss.Type = EnemyType.Mage;
                    baseHp = 25; baseAtk = 15; baseDef = 2;
                    boss.MaxHP = (int)(baseHp * 1.8); boss.Attack = (int)(baseAtk * 1.6); boss.Defense = (int)(baseDef * 1.1);
                    boss.FreezeChance = 0.15 + 0.10;
                    break;
                case 3: // Пестов С-- (Скелет)
                    boss.Name = "Пестов С--"; boss.Type = EnemyType.Skeleton;
                    baseHp = 40; baseAtk = 10; baseDef = 5;
                    boss.MaxHP = (int)(baseHp * 1.3); boss.Attack = (int)(baseAtk * 1.8); boss.Defense = (int)(baseDef * 0.6);
                    boss.IgnoresArmor = true; boss.FreezeChance = 0.15; // По таблице
                    break;
            }
            boss.CurrentHP = boss.MaxHP;
            CurrentEnemies.Add(boss);
            Log($"ОСТОРОЖНО! Появился босс: {boss.Name}!");
        }

        // БОЙ
        // Возвращает true, если комната зачищена
        public bool ProcessTurn(bool playerDefends)
        {
            if (Hero.IsFrozen)
            {
                Log("Вы заморожены и пропускаете ход!");
                Hero.IsFrozen = false;
            }
            else
            {
                if (!playerDefends)
                {
                    // Атака игрока (бьем первого живого)
                    var target = CurrentEnemies.FirstOrDefault(e => !e.IsDead);
                    if (target != null)
                    {
                        int dmg = Math.Max(1, Hero.GetTotalAttack() - target.Defense);
                        target.TakeDamage(dmg);
                        Log($"Вы ударили {target.Name} на {dmg} урона.");
                    }
                }
                else
                {
                    Log("Вы приготовились к защите.");
                }
            }

            // Ход врагов
            foreach (var enemy in CurrentEnemies.Where(e => !e.IsDead))
            {
                ProcessEnemyAttack(enemy, playerDefends);
                if (Hero.IsDead)
                {
                    IsGameOver = true;
                    Log("Вы погибли...");
                    return false;
                }
            }

            // Очистка мертвых
            CurrentEnemies.RemoveAll(e => e.IsDead);

            if (CurrentEnemies.Count == 0 && !IsGameOver)
            {
                Log("Комната зачищена!");
                return true;
            }
            return false;
        }

        private void ProcessEnemyAttack(Enemy enemy, bool playerDefends) //атака врагами игрока
        {
            int incDmg = enemy.Attack;

            // Проверка на крит
            if (enemy.CritChance > 0 && _rnd.NextDouble() < enemy.CritChance)
            {
                incDmg *= 2;
                Log($"{enemy.Name} наносит КРИТИЧЕСКИЙ УДАР!");
            }

            // Защита игрока
            if (playerDefends)
            {
                if (_rnd.NextDouble() < 0.40) // 40% шанс уклонения
                {
                    Log($"Вы уклонились от атаки {enemy.Name}!");
                    return;
                }
                else
                {
                    // Блок 70-100% от брони
                    double blockPercent = _rnd.NextDouble() * 0.3 + 0.7; // от 0.7 до 1.0
                    int blockAmount = (int)(Hero.GetTotalDefense() * blockPercent);
                    if (enemy.IgnoresArmor)
                    {
                        blockAmount = 0;
                        Log($"{enemy.Name} игнорирует вашу броню!");
                    }
                    incDmg -= blockAmount;
                    Log($"Вы заблокировали {blockAmount} урона.");
                }
            }
            else
            {
                // Обычный расчет брони
                int armor = enemy.IgnoresArmor ? 0 : Hero.GetTotalDefense();
                incDmg -= armor;
            }

            incDmg = Math.Max(1, incDmg);
            Hero.TakeDamage(incDmg);
            Log($"{enemy.Name} наносит вам {incDmg} урона.");

            // Заморозка
            if (enemy.FreezeChance > 0 && _rnd.NextDouble() < enemy.FreezeChance)
            {
                Hero.IsFrozen = true;
                Log($"{enemy.Name} ЗАМОРОЗИЛ вас!");
            }
        }

        // Генерация лута
        public Item GenerateLoot()
        {
            int roll = _rnd.Next(3);
            if (roll == 0) return new ItemPotion() { Name = "Зелье здоровья", HealAmount = 999 };
            if (roll == 1) return new ItemWeapon() { Name = $"Меч (Ур.{CurrentFloor / 2 + 1})", Damage = _rnd.Next(5, 15 + CurrentFloor) };
            return new ItemArmor() { Name = $"Доспех (Ур.{CurrentFloor / 2 + 1})", DefenseBonus = _rnd.Next(2, 10 + CurrentFloor / 2) };
        }
    }
}