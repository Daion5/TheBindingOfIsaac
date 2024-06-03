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
        public Form2(Character character, Monster monster, string characterImageFile, string monsterImageFile, Menu menuForm)
        {
            InitializeComponent();
            this.character = character;
            this.monster = monster;
            this.characterImageFile = characterImageFile;
            this.monsterImageFile = monsterImageFile;
            this.menuForm = menuForm;

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
                this.Close();

                var dropResult = character.DropRate();
                switch (dropResult)
                {
                    case DropResult.DoubleHeart:
                        character.Heal(20);
                        UpdateHealthBars();
                        MessageBox.Show("YOU FOUND A DOUBLE HEART!");
                        break;
                    case DropResult.SingleHeart:
                        character.Heal(10);
                        UpdateHealthBars();
                        MessageBox.Show("YOU FOUND A HEART!");
                        break;
                    case DropResult.NoHeart:
                        break;
                }

            }
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
                    menuForm.OnGameEnded(false);
                    menuForm.Show();
                    menuForm.BringToFront();
                }));
            }
            else
            {
                menuForm.OnGameEnded(false);
                menuForm.Show();
                menuForm.BringToFront();
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
