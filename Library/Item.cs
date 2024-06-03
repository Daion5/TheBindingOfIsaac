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

        public static void ResetItemList()
        {
            foreach (var item in AllItems)
            {
                item.IsTaken = false;
            }
        }
        public static Item GetNextAvailableItem()
        {
            var availableItems = AllItems.Where(i => !i.IsTaken).ToList();

            if (availableItems.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                throw new NoMoreItemsException(" No more items to generate. ");
            }

            return availableItems[new Random().Next(availableItems.Count)];
        }
    }
}
