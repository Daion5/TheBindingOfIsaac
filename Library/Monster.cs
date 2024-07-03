using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Library
{
    public class Monster : Character
    {
        public int Depth { get; set; }

        public static List<Monster> AllMonsters = new List<Monster>
        {
            new Monster("Clotty", 1, 2, 1, 0, 15, 15, 1),
            new Monster("Fatty", 1, 2, 0, 0, 20, 20, 1),
            new Monster("Fly", 1, 2, 5, 0, 3, 3, 1),
            new Monster("Gaper", 1, 2, 1, 0, 10, 10, 1),
            new Monster("Horf", 1, 2, 0, 0, 10, 10, 1),
            new Monster("Pooter", 1, 2, 1, 0, 8, 8, 1),
            new Monster("Spider", 1, 2, 3, 0, 6, 6, 1),

            new Monster("Bony", 2, 2, 1, 0, 8, 8, 2),
            new Monster("Boom_Fly", 2, 2, 2, 0, 8, 8, 2),
            new Monster("Hopper", 2, 2, 4, 0, 10, 10, 2),
            new Monster("Host", 2, 2, 2, 0, 14, 14, 2),
            new Monster("Maw", 2, 2, 0, 0, 20, 20, 2),
            new Monster("Trite", 2, 2, 2, 0, 10, 10, 2),
            new Monster("Charger", 2, 2, 4, 0, 10, 10, 2),

            new Monster("Baby", 3, 2, 3, 0, 15, 15, 3),
            new Monster("Brain", 3, 2, 1, 0, 10, 10, 3),
            new Monster("Keeper", 3, 2, 2, 0, 25, 25, 3),
            new Monster("Night_Crawler", 3, 2, 3, 0, 25, 25, 3),
            new Monster("Nulls", 3, 2, 1, 0, 20, 20, 3),
            new Monster("Swarmer", 3, 2, 1, 0, 25, 25, 3),
            new Monster("Wizoob", 3, 2, 2, 0, 10, 10, 3),

            new Monster("Dople", 4, 2, 0, 0, 20, 20, 4),
            new Monster("Eye", 4, 2, 0, 0, 15, 15, 4),
            new Monster("Fred", 4, 2, 2, 0, 25, 25, 4),
            new Monster("Globin", 4, 2, 1, 0, 25, 25, 4),
            new Monster("Gurgling", 4, 2, 2, 0, 25, 25, 4),
            new Monster("Red_Host", 4, 2, 0, 0, 20, 20, 4),
            new Monster("Vis", 4, 2, 1, 0, 25, 25, 4),
        };

        public static List<Monster> AllBosses = new List<Monster>
{
            new Monster("Larry_Jr", 4, 3, 4, 0, 22, 22, 1),
            new Monster("Monstro", 1, 2, 1, 0, 80, 80, 1),
            new Monster("The_Duke_of_Flies", 2, 2, 1, 0, 60, 60, 1),

            new Monster("Dark_One", 3, 2, 3, 0, 100, 100, 2),
            new Monster("Mega_Maw", 3, 2, 0, 0, 120, 120, 2),
            new Monster("The_Forsaken", 3, 2, 1, 0, 80, 80, 2),

            new Monster("Gish", 4, 2, 1, 0, 130, 130, 3),
            new Monster("Monstro_II", 4, 3, 1, 0, 110, 110, 3),
            new Monster("The_Gate", 4, 2, 0, 0, 130, 130, 3),

            new Monster("Mom's_Heart", 5, 2, 0, 0, 150, 150, 4),
        };
        public Monster(string name, int strength, double attackSpeed, int movementSpeed, int luck, int maxHealth, int currentHealth, int depth)
            : base(name, strength, attackSpeed, movementSpeed, luck, maxHealth, currentHealth, "", 0)
        {
            this.Depth = depth;
        }
        public Monster(Monster monster)
            : this(monster.Name, monster.Strength, monster.AttackSpeed, monster.MovementSpeed, monster.Luck, monster.MaxHealth, monster.CurrentHealth, monster.Depth)
        {
        }
    }
}