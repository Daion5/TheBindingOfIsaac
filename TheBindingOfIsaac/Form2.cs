using Library;
using System;
using System.Drawing;
using System.Windows.Forms;
using static Library.Character;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Windows.Forms.Timer;

namespace TheBindingOfIsaac
{
    public partial class Form2 : Form
    {
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                return;
            }

            base.WndProc(ref m);
        }

        private Character character;
        private Monster monster;
        private Menu menuForm;
        private string characterImageFile;
        private string monsterImageFile;
        private bool gameEnded = false;
        private bool isBombUsed = false;
        private bool isBombButtonLocked = false;

        public Form2(Character character, Monster monster, string characterImageFile, string monsterImageFile, Menu menuForm)
        {
            InitializeComponent();
            this.character = character;
            this.monster = monster;
            this.characterImageFile = characterImageFile;
            this.monsterImageFile = monsterImageFile;
            this.menuForm = menuForm;
            this.Text = "Battle Room";

            pictureBoxCharacter.Image = Image.FromFile(characterImageFile);
            pictureBoxCharacter.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxMonster.Image = Image.FromFile(monsterImageFile);
            pictureBoxMonster.SizeMode = PictureBoxSizeMode.StretchImage;
            characterName.Text = character.Name;
            string displayName = monster.Name.Replace('_', ' ');
            monsterName.Text = displayName;
            progressBarCharacter.Maximum = character.MaxHealth;
            progressBarCharacter.Value = character.CurrentHealth;
            progressBarMonster.Maximum = monster.MaxHealth;
            progressBarMonster.Value = Math.Min(monster.CurrentHealth, progressBarMonster.Maximum);

            btnFight.Click += button1_Click;
            btnBomb.Click += btnBomb_Click;

            UpdateBombButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gameEnded) { return; }

            int damageDealtByCharacter = (int)character.Attack();
            monster.TakeDamage(damageDealtByCharacter);
            UpdateHealthBars();

            if (monster.IsAlive())
            {
                int damageDealtByMonster = (int)monster.Attack();
                character.TakeDamage(damageDealtByMonster);
                UpdateHealthBars();

                if (!character.IsAlive())
                {
                    gameEnded = true;
                    ShowLoseForm();
                }
            }
            else
            {
                gameEnded = true;
                WinBattle();
            }

            isBombUsed = false;
        }

        private void btnBomb_Click(object sender, EventArgs e)
        {
            if (!isBombButtonLocked && character.Bombs > 0 && monster.IsAlive())
            {
                isBombButtonLocked = true;

                character.Bombs--;
                monster.TakeDamageBomb(character.BombDamage);
                UpdateHealthBars();
                UpdateBombButton();

                if (!monster.IsAlive())
                {
                    gameEnded = true;
                    WinBattle();
                }

                Timer timer = new Timer();
                timer.Interval = 500;
                timer.Tick += (s, args) =>
                {
                    isBombButtonLocked = false;
                    timer.Stop();
                };
                timer.Start();
            }
        }


        private void UpdateBombButton()
        {
            btnBomb.Text = $"Bomb: {character.Bombs}";
            btnBomb.Enabled = character.Bombs > 0;
        }

        private void ShowLoseForm()
        {
            Form loseForm = new Form();
            Image img = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\lose.png");
            PictureBox pictureBox = new PictureBox
            {
                Image = img,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill
            };
            loseForm.Controls.Add(pictureBox);
            loseForm.StartPosition = FormStartPosition.CenterScreen;
            loseForm.Size = new Size(400, 500);
            loseForm.Show();

            Timer timer = new Timer();
            timer.Interval = 3000;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                loseForm.Close();
                CloseFormsAndReturnToMenu();
            };
            timer.Start();
        }

        private void CloseFormsAndReturnToMenu()
        {
            Form2 form2 = Application.OpenForms.OfType<Form2>().FirstOrDefault();
            if (form2 != null)
            {
                form2.Invoke(new Action(() =>
                {
                    form2.Close();
                }));
            }

            Form1 form1 = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form1 != null)
            {
                form1.Invoke(new Action(() =>
                {
                    form1.Close();
                }));
            }

            if (menuForm.InvokeRequired)
            {
                menuForm.Invoke(new Action(() =>
                {
                    menuForm.OnGameEnded(false, character);
                    menuForm.Show();
                    menuForm.BringToFront();
                }));
            }
            else
            {
                menuForm.OnGameEnded(false, character);
                menuForm.Show();
                menuForm.BringToFront();
            }
        }

        private void WinBattle()
        {
            this.Close();

            var dropHeartResult = character.HeartDropRate();
            switch (dropHeartResult)
            {
                case DropHeartResult.DoubleHeart:
                    character.Heal(20);
                    UpdateHealthBars();
                    CustomMessageBox.Show("YOU FOUND A DOUBLE HEART!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\hearts\hearts_drops\double_heart.png");
                    break;
                case DropHeartResult.SingleHeart:
                    character.Heal(10);
                    UpdateHealthBars();
                    CustomMessageBox.Show("YOU FOUND A HEART!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\hearts\hearts_drops\heart.png");
                    break;
                case DropHeartResult.HalfHeart:
                    character.Heal(5);
                    UpdateHealthBars();
                    CustomMessageBox.Show("YOU FOUND A HALF OF HEART!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\hearts\hearts_drops\half_heart.png");
                    break;
                case DropHeartResult.NoHeart:
                    break;
            }

            var dropResult = character.DropRate();
            switch (dropResult)
            {
                case DropResult.Coin:
                    character.Coins++;
                    CustomMessageBox.Show("YOU FOUND A PENNY!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\pickups\penny.png");
                    break;
                case DropResult.Bomb:
                    character.Bombs++;
                    CustomMessageBox.Show("YOU FOUND A BOMB!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\pickups\bomb.png");
                    break;
                case DropResult.Key:
                    character.Keys++;
                    CustomMessageBox.Show("YOU FOUND A KEY!", @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\pickups\key.png");
                    break;
                case DropResult.Nothing:
                    break;
            }
        }

        private void UpdateHealthBars()
        {
            if (character.CurrentHealth >= progressBarCharacter.Minimum && character.CurrentHealth <= progressBarCharacter.Maximum)
            {
                progressBarCharacter.Value = character.CurrentHealth;
            }
            else
            {
                progressBarCharacter.Value = Math.Min(Math.Max(character.CurrentHealth, progressBarCharacter.Minimum), progressBarCharacter.Maximum);
            }

            if (monster.CurrentHealth >= progressBarMonster.Minimum && monster.CurrentHealth <= progressBarMonster.Maximum)
            {
                progressBarMonster.Value = monster.CurrentHealth;
            }
            else
            {
                progressBarMonster.Value = Math.Min(Math.Max(monster.CurrentHealth, progressBarMonster.Minimum), progressBarMonster.Maximum);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
