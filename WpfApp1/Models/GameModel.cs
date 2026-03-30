using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeWPF.Models
{
    //абстрактный класс сущность для представления общих понятий, которые конкретизируются в производных классах
    public abstract class Entity
    {
        public string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public bool IsDead => CurrentHP <= 0;

        public void TakeDamage(int damage)
        {
            CurrentHP -= damage;
            if (CurrentHP < 0) CurrentHP = 0;
        }
    }

    public class Player : Entity
    {
        public ItemWeapon CurrentWeapon { get; set; }
        public ItemArmor CurrentArmor { get; set; }
        public bool IsFrozen { get; set; }

        public Player()
        {
            Name = "Герой";
            MaxHP = 100;
            CurrentHP = 100;
            Attack = 10;
            Defense = 5;
        }

        public int GetTotalAttack() => Attack + (CurrentWeapon?.Damage ?? 0);
        public int GetTotalDefense() => Defense + (CurrentArmor?.DefenseBonus ?? 0);
    }

    public enum EnemyType { Goblin, Skeleton, Mage }

    public class Enemy : Entity
    {
        public EnemyType Type { get; set; }
        public double CritChance { get; set; } = 0;
        public double FreezeChance { get; set; } = 0;
        public bool IgnoresArmor { get; set; } = false;
        public bool IsBoss { get; set; } = false;
    }

    // --- Предметы ---
    public abstract class Item
    {
        public string Name { get; set; }
    }

    public class ItemWeapon : Item
    {
        public int Damage { get; set; }
    }

    public class ItemArmor : Item
    {
        public int DefenseBonus { get; set; }
    }

    public class ItemPotion : Item
    {
        public int HealAmount { get; set; } // Если 0 или огромное число - лечит фулл
    }
}
