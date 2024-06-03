using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public abstract class Character
    {
        private static readonly Random random = new Random();
        public string Name { get; set; }
        public int Strength { get; set; }
        public double AttackSpeed { get; set; }
        public int MovementSpeed { get; set; }
        public int Luck { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Coins { get; set; }
        public int Bombs { get; set; }
        public int Keys { get; set; }

        protected Character(string name, int strength, double attackSpeed, int movementSpeed, int luck, int maxHealth, int currentHealth)
        {
            Name = name;
            Strength = strength;
            AttackSpeed = attackSpeed;
            MovementSpeed = movementSpeed;
            Luck = luck;
            MaxHealth = maxHealth;
            CurrentHealth = currentHealth;
            Coins = 0;
            Bombs = 0;
            Keys = 0;
        }
        public virtual int Attack()
        {
            return (int)(Strength * (AttackSpeed * 0.5));
        }

        public void TakeDamage(int damage)
        {
            if (IsDodged())
            {
                Console.WriteLine($"{Name} dodged the attack!");
            }
            else
            {
                CurrentHealth -= damage;
                if (CurrentHealth < 0)
                    CurrentHealth = 0;
            }
        }
        public bool IsDodged()
        {
            int dodgeChance = MovementSpeed * 1;
            int roll = random.Next(0, 11);
            return roll <= dodgeChance;
        }
        public enum DropResult
        {
            NoHeart,
            SingleHeart,
            DoubleHeart
        }
        public DropResult DropRate()
        {
            int dropChance = Luck * 1;
            int roll = random.Next(0, 11);
            if (roll <= dropChance)
            {
                dropChance = Luck / 2;
                roll = random.Next(0, 11);
                if (roll <= dropChance)
                {
                    return DropResult.DoubleHeart;
                }
                return DropResult.SingleHeart;
            }
            return DropResult.NoHeart;
        }

        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
        public bool IsAlive()
        {
            return CurrentHealth > 0;
        }

        public override string ToString()
        {
            return $"{Name} ({CurrentHealth}/{MaxHealth} HP)";
        }
    }

}
