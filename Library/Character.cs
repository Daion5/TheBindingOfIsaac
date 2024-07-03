using System;
using System.Collections.Generic;

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
        public int ActiveItemCharges { get; set; }
        public int MaxActiveItemCharges { get; set; }
        public string ActiveItemName { get; set; }
        public int BombDamage { get; set; } = 30;

        protected Character(string name, int strength, double attackSpeed, int movementSpeed, int luck, int maxHealth, int currentHealth, string activeItemName, int maxActiveItemCharges)
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
            ActiveItemCharges = 0;
            MaxActiveItemCharges = maxActiveItemCharges;
            ActiveItemName = activeItemName;
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

        public void TakeDamageBomb(int bombDamage)
        {
            CurrentHealth -= bombDamage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }

        public bool IsDodged()
        {
            int dodgeChance = MovementSpeed * 1;
            int roll = random.Next(0, 11);
            return roll <= dodgeChance;
        }

        public enum DropHeartResult
        {
            NoHeart,
            HalfHeart,
            SingleHeart,
            DoubleHeart
        }

        public enum DropResult
        {
            Nothing,
            Coin,
            Bomb,
            Key
        }

        public DropHeartResult HeartDropRate()
        {
            int halfHeartChance = (Luck + 1) * 1;
            int roll = random.Next(0, 11);
            if (roll <= halfHeartChance)
            {
                return DropHeartResult.HalfHeart;
            }

            int heartChance = Luck * 1;
            roll = random.Next(0, 11);
            if (roll <= heartChance)
            {
                int doubleHeartChance = Luck / 2;
                roll = random.Next(0, 11);
                if (roll <= doubleHeartChance)
                {
                    return DropHeartResult.DoubleHeart;
                }
                return DropHeartResult.SingleHeart;
            }

            return DropHeartResult.NoHeart;
        }

        public DropResult DropRate()
        {
            int coinChance = Luck / 2;
            int bombChance = Luck / 3;
            int keyChance = Luck / 4;

            int roll = random.Next(0, 11);
            if (roll <= coinChance)
            {
                return DropResult.Coin;
            }

            roll = random.Next(0, 11);
            if (roll <= bombChance)
            {
                return DropResult.Bomb;
            }

            roll = random.Next(0, 11);
            if (roll <= keyChance)
            {
                return DropResult.Key;
            }

            return DropResult.Nothing;
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

        public void UseActiveItem()
        {
            if (ActiveItemCharges == MaxActiveItemCharges)
            {
                ActiveItemCharges = 0;
                switch (ActiveItemName)
                {
                    case "Yum Heart":
                        YumHeartEffect();
                        break;
                    case "The Book Of Belial":
                        BookOfBelialEffect();
                        break;
                    case "Wooden Nickel":
                        WoodenNickelEffect();
                        break;
                }
            }
        }

        public void YumHeartEffect()
        {
            Console.WriteLine("Yum Heart used! Healing 20 HP.");
            Heal(20);
        }

        public void BookOfBelialEffect()
        {
            Console.WriteLine("Book of Belial used! Increasing strength by 1.");
            Strength++;
        }

        public void WoodenNickelEffect()
        {
            Console.WriteLine("Wooden Nickel used! Gaining 1 coin.");
            Coins++;
        }

        public void IncrementActiveItemCharges()
        {
            if (ActiveItemCharges < MaxActiveItemCharges)
            {
                ActiveItemCharges++;
            }
        }
    }
}
