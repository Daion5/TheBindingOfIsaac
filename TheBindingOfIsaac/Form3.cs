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
    public partial class Form3 : Form
    {
        private Item item;
        private Character character;
        private string itemsFolderPath;
        private bool itemTaken = false;
        public event EventHandler FormClosedEvent;
        public Form3(Item item, Character character, string itemsFolderPath)
        {
            InitializeComponent();
            this.item = item;
            this.character = character;
            this.itemsFolderPath = itemsFolderPath;

            pictureItem.Image = Image.FromFile(Path.Combine(itemsFolderPath, item.Name.ToLower() + ".png"));
            pictureItem.BackColor = Color.Transparent;
            pictureItem.SizeMode = PictureBoxSizeMode.Zoom;

            pictureStone.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\stone.png");
            pictureStone.BackColor = Color.Transparent;
            pictureStone.SizeMode = PictureBoxSizeMode.Zoom;

            string displayName = item.Name.Replace('_', ' ');
            btnYes.Text = "Take: " + displayName.Replace("_", " ");
            btnNo.Text = "Leave: " + displayName.Replace("_", " ");

            btnYes.BackColor = Color.Transparent;
            btnYes.ForeColor = Color.Purple;
            btnYes.Font = new Font(btnYes.Font, FontStyle.Bold);

            btnNo.BackColor = Color.Transparent;
            btnNo.ForeColor = Color.Purple;
            btnNo.Font = new Font(btnNo.Font, FontStyle.Bold);

        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            FormClosedEvent?.Invoke(this, new ItemTakenEventArgs(itemTaken));
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            itemTaken = true;
            UpdateCharacterStats();
            this.Close();
        }
        private void UpdateCharacterStats()
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
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            itemTaken = false;
            this.Close();
        }

        public class ItemTakenEventArgs : EventArgs
        {
            public bool ItemTaken { get; }

            public ItemTakenEventArgs(bool itemTaken)
            {
                ItemTaken = itemTaken;
            }
        }
    }
}
