using Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Item
    {
        public string Name { get; set; }
        public string StatBoost { get; set; }
        public bool IsTaken { get; set; } = false;

        public static List<Item> AllItems = new List<Item>
        {
            new Item("Mom's_Knife", "Strength"),
            new Item("Blood_Bag", "MaxHealth"),
            new Item("Lucky_Foot", "Luck"),
            new Item("The_Belt", "MovementSpeed"),
            new Item("The_Sad_Onion", "AttackSpeed"),
            new Item("Cursed_Penny", "CursedPenny"),
            new Item("Mr._Boom", "MrBoom"),
            new Item("Polyphemus", "Polyphemus"),
        };

        public static List<Item> AllShopItems = new List<Item>
        {
            new Item("BOGO_Bombs", "BOGOBombs"),
            new Item("Champion_Belt", "Strength"),
            new Item("Magic_Mushroom", "MagicMushroom"),
            new Item("Supper", "MaxHealth"),
        };

        public Item(string name, string statBoost)
        {
            Name = name;
            StatBoost = statBoost;
        }

        public override string ToString()
        {
            return $"{Name} (+{StatBoost})";
        }

        public static void RemoveItemFromList(Item item)
        {
            AllItems.Remove(item);
        }

        public static void RemoveShopItemFromList(Item item)
        {
            AllShopItems.Remove(item);
        }

        public static void ResetItemList()
        {
            foreach (var item in AllItems)
            {
                item.IsTaken = false;
            }
        }

        public static void ResetShopItemList()
        {
            foreach (var item in AllShopItems)
            {
                item.IsTaken = false;
            }
        }

        public static Item GetNextAvailableItem()
        {
            var availableItems = AllItems.Where(i => !i.IsTaken).ToList();
            if (availableItems.Count == 0)
            {
                throw new NoMoreItemsException(" No more items to generate. ");
            }
            return availableItems[new Random().Next(availableItems.Count)];
        }

        public static Item GetNextAvailableShopItem()
        {
            var availableItems = AllShopItems.Where(i => !i.IsTaken).ToList();
            if (availableItems.Count == 0)
            {
                throw new Exception("No more items available in the shop.");
            }
            return availableItems[new Random().Next(availableItems.Count)];
        }
    }
}