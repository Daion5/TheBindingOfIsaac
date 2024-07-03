using Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TheBindingOfIsaac
{
    public partial class Form4 : Form
    {
        private Item shopItem;
        private Character character;
        private Action updateStatsCallback;
        private bool chestOpened;
        private bool itemTaken;

        public bool ChestOpened => chestOpened;
        public bool ItemTaken => itemTaken;


        public Form4(Item item, Character character, Action updateStatsCallback, bool chestOpened, bool itemTaken)
        {
            InitializeComponent();
            this.Text = "Shop";
            shopItem = item;
            this.character = character;
            this.updateStatsCallback = updateStatsCallback;
            this.itemTaken = itemTaken;
            this.chestOpened = chestOpened;
            LoadShopItem();
            LoadChestState();
            UpdateChestButtonState();
        }

        private void LoadShopItem()
        {
            if (shopItem != null && !itemTaken)
            {
                string imagePath = Path.Combine(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items\shop", shopItem.Name + ".png");
                if (File.Exists(imagePath))
                {
                    pictureBoxitem.Image = Image.FromFile(imagePath);
                    pictureBoxitem.BackColor = Color.Transparent;
                    pictureBoxitem.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    MessageBox.Show("Image not found: " + imagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                pictureBoxitem.Visible = false;
                buttonBuy.Visible = false;
            }
            UpdateButtonState();
        }

        private void LoadChestState()
        {
            if (chestOpened)
            {
                btnOpen.Visible = false;
                pictureBoxChest.Visible = false;
            }
        }

        private void UpdateButtonState()
        {
            if (shopItem == null || shopItem.IsTaken || character.Coins < 10)
            {
                buttonBuy.Enabled = false;
                buttonBuy.Visible = true;
            }
            else
            {
                buttonBuy.Enabled = true;
                buttonBuy.Visible = true;
            }
        }
        private void buttonBuy_Click(object sender, EventArgs e)
        {
            if (shopItem != null && !shopItem.IsTaken && character.Coins >= 10)
            {
                character.Coins -= 10;
                ApplyItemEffect(shopItem);
                shopItem.IsTaken = true;
                itemTaken = true;
                pictureBoxitem.Image = null;
                pictureBoxitem.Visible = false;
                buttonBuy.Visible = false;
                UpdateButtonState();
                updateStatsCallback?.Invoke();
            }
        }

        private void ApplyItemEffect(Item item)
        {
            switch (item.StatBoost)
            {
                case "Strength":
                    character.Strength += 1;
                    break;
                case "MaxHealth":
                    character.MaxHealth += 20;
                    character.Heal(30);
                    break;
                case "Luck":
                    character.Luck += 1;
                    break;
                case "MovementSpeed":
                    character.MovementSpeed += 1;
                    break;
                case "AttackSpeed":
                    character.AttackSpeed += 1;
                    break;
                case "MagicMushroom":
                    character.MaxHealth += 20;
                    character.Heal(30);
                    character.Strength += 1;
                    break;
                case "BOGOBombs":
                    character.Bombs += 5;
                    break;
                case "CursedPenny":
                    character.Coins += 30;
                    character.Luck -= 2;
                    break;
                case "MrBoom":
                    character.Bombs += 1;
                    character.BombDamage += 20;
                    break;
                case "Polyphemus":
                    character.MovementSpeed = 0;
                    character.Strength *= 2;
                    break;
            }
        }

        private void buttonLeave_Click(object sender, EventArgs e)
        {
            updateStatsCallback?.Invoke();
            this.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (character.Keys > 0 && !chestOpened)
            {
                character.Keys--;
                OpenChest();
                chestOpened = true;
                btnOpen.Visible = false;
                pictureBoxChest.Visible = false;
                updateStatsCallback?.Invoke();
            }
        }
        private void OpenChest()
        {
            Random random = new Random();
            int result = random.Next(1, 9);

            switch (result)
            {
                case 1:
                    int bombs = random.Next(2, 5);
                    character.Bombs += bombs;
                    MessageBox.Show($"You found {bombs} bombs!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    int coins = random.Next(6, 15);
                    character.Coins += coins;
                    MessageBox.Show($"You found {coins} coins!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 3:
                    MessageBox.Show("Nothing happened.", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 4:
                    int damage = random.Next(1, 11);
                    character.TakeDamage(damage);
                    MessageBox.Show($"You took {damage} HP damage!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case 5:
                    int healAmount = random.Next(20, 41);
                    character.Heal(healAmount);
                    MessageBox.Show($"You healed {healAmount} HP!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 6:
                    character.MaxHealth += 20;
                    MessageBox.Show("You received a health container!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 7:
                    character.Luck += 1;
                    MessageBox.Show("Your luck increased by 1!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 8:
                    character.AttackSpeed += 1;
                    MessageBox.Show("Your attack speed increased by 1!", "Chest Opened", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }

        private void UpdateChestButtonState()
        {
            btnOpen.Enabled = !chestOpened && character.Keys > 0;
            btnOpen.Visible = !chestOpened;
        }
        private void pictureBoxitem_Click(object sender, EventArgs e)
        {
        }
    }
}
