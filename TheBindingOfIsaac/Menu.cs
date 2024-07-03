using Library;
using NAudio.Wave;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TheBindingOfIsaac
{
    public partial class Menu : Form
    {
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        public class UserData
        {
            public int WinStreak { get; set; }
            public int BestStreak { get; set; }
            public int WorstStreak { get; set; }
            public bool IsIsaacTaintedUnlocked { get; set; } = false;
            public bool IsJudasTaintedUnlocked { get; set; } = false;
            public bool IsMagdaleneTaintedUnlocked { get; set; } = false;
        }
        public static int WinStreak { get; private set; } = 0;
        public static int BestStreak { get; private set; } = 0;
        public static int WorstStreak { get; private set; } = 0;
        private string userDataFolderPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\data";
        private bool isIsaacTainted = false;
        private bool isJudasTainted = false;
        private bool isMagdaleneTainted = false;
        private Character currentIsaac;
        private Character currentJudas;
        private Character currentMagdalene;
        private bool isIsaacTaintedUnlocked = false;
        private bool isJudasTaintedUnlocked = false;
        private bool isMagdaleneTaintedUnlocked = false;
        private void SaveUserData()
        {
            Directory.CreateDirectory(userDataFolderPath);

            UserData userData = new UserData
            {
                WinStreak = WinStreak,
                BestStreak = BestStreak,
                WorstStreak = WorstStreak,
                IsIsaacTaintedUnlocked = isIsaacTaintedUnlocked,
                IsJudasTaintedUnlocked = isJudasTaintedUnlocked,
                IsMagdaleneTaintedUnlocked = isMagdaleneTaintedUnlocked
            };

            string userDataFilePath = Path.Combine(userDataFolderPath, "userData.xml");

            XmlSerializer serializer = new XmlSerializer(typeof(UserData));
            using (FileStream fileStream = new FileStream(userDataFilePath, FileMode.Create))
            {
                serializer.Serialize(fileStream, userData);
            }
        }
        private void LoadUserData()
        {
            string userDataFilePath = Path.Combine(userDataFolderPath, "userData.xml");
            if (File.Exists(userDataFilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(UserData));
                using (FileStream fileStream = new FileStream(userDataFilePath, FileMode.Open))
                {
                    UserData userData = (UserData)serializer.Deserialize(fileStream);
                    WinStreak = userData.WinStreak;
                    BestStreak = userData.BestStreak;
                    WorstStreak = userData.WorstStreak;
                    isIsaacTaintedUnlocked = userData.IsIsaacTaintedUnlocked;
                    isJudasTaintedUnlocked = userData.IsJudasTaintedUnlocked;
                    isMagdaleneTaintedUnlocked = userData.IsMagdaleneTaintedUnlocked;

                    UpdateIsaacDisplay(currentIsaac);
                    UpdateJudasDisplay(currentJudas);
                    UpdateMagdaleneDisplay(currentMagdalene);
                }
            }
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserData();
            StopMusic();
        }
        public Menu()
        {
            InitializeComponent();
            InitializeListBoxes();
            this.FormClosing += Menu_FormClosing;
            this.Text = "Character Menu";
            LoadUserData();
            UpdateWinStreakLabel();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;

            waveOutDevice = new WaveOut();
            audioFileReader = new AudioFileReader(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\menu.mp3");
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
            audioFileReader.Volume = 0.5f;

            UpdateIsaacDisplay(currentIsaac);
            UpdateJudasDisplay(currentJudas);
            UpdateMagdaleneDisplay(currentMagdalene);
        }

        private void PlayMusic()
        {
            audioFileReader = new AudioFileReader(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\menu.mp3");
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

        private void StopMusic()
        {
            waveOutDevice.Stop();
            waveOutDevice.Dispose();
            waveOutDevice = new WaveOut();
        }

        private void OnCharacterSelected(Character selectedCharacter)
        {
            StopMusic();
            this.Hide();
            Thread gameThread = new Thread(() => StartGame(selectedCharacter));
            gameThread.Start();
        }

        private void StartGame(Character selectedCharacter)
        {
            Application.Run(new Form1(selectedCharacter, this));
            this.Invoke(new Action(() => this.Show()));
        }

        public void OnGameEnded(bool won, Character selectedCharacter)
        {
            if (won)
            {
                if (WinStreak < 0)
                {
                    WinStreak = 1;
                }
                else
                {
                    WinStreak++;
                }
                if (WinStreak > BestStreak)
                {
                    BestStreak = WinStreak;
                }

                if (selectedCharacter is Isaac)
                {
                    isIsaacTaintedUnlocked = true;
                }
                else if (selectedCharacter is Judas)
                {
                    isJudasTaintedUnlocked = true;
                }
                else if (selectedCharacter is Magdalene)
                {
                    isMagdaleneTaintedUnlocked = true;
                }

                SaveUserData();
                MessageBox.Show("You won!");
            }
            else
            {
                if (WinStreak > 0)
                {
                    WinStreak = -1;
                }
                else
                {
                    WinStreak--;
                }
                if (WinStreak < WorstStreak)
                {
                    WorstStreak = WinStreak;
                }
                MessageBox.Show("You lost!");
            }
            ResetCharacters();
            Item.ResetItemList();
            Item.ResetShopItemList();
            SaveUserData();
            UpdateWinStreakLabel();
            this.Show();
            PlayMusic();
        }

        private void UpdateWinStreakLabel()
        {
            win_streak.Text = "WIN STREAK: " + WinStreak;
            win_streak.MouseHover += new EventHandler(WinStreakLabel_MouseHover);
        }

        private void WinStreakLabel_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.OwnerDraw = true;
            toolTip.Draw += new DrawToolTipEventHandler(toolTip_Draw);

            toolTip.SetToolTip(win_streak, $"BEST STREAK: {BestStreak}\nWORST STREAK: {WorstStreak}");
        }
        private void toolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.DrawBackground();
            e.DrawBorder();

            string text = e.ToolTipText;
            string[] lines = text.Split('\n');
            Font font = e.Font;

            for (int i = 0; i < lines.Length; i++)
            {
                Color color = i == 0 ? Color.DarkGreen : Color.Red;
                e.Graphics.DrawString(lines[i], font, new SolidBrush(color), new PointF(2, 2 + (i * font.Height)));
            }
        }
        private void InitializeListBoxes()
        {
            currentIsaac = new Isaac();
            UpdateIsaacDisplay(currentIsaac);

            currentJudas = new Judas();
            UpdateJudasDisplay(currentJudas);

            currentMagdalene = new Magdalene();
            UpdateMagdaleneDisplay(currentMagdalene);
        }
        private void UpdateIsaacDisplay(Character isaac)
        {
            listBoxIsaac.Items.Clear();
            listBoxIsaac.Items.Add("Name: " + isaac.Name);
            listBoxIsaac.Items.Add("Strength: " + isaac.Strength);
            listBoxIsaac.Items.Add("Attack Speed: " + isaac.AttackSpeed);
            listBoxIsaac.Items.Add("Movement Speed: " + isaac.MovementSpeed);
            listBoxIsaac.Items.Add("Luck: " + isaac.Luck);
            listBoxIsaac.Items.Add("Max Health: " + isaac.MaxHealth);
            listBoxIsaac.Items.Add("Coins: " + isaac.Coins);
            listBoxIsaac.Items.Add("Bombs: " + isaac.Bombs);
            listBoxIsaac.Items.Add("Keys: " + isaac.Keys);

            if (isaac.Name == "Isaac")
            {
                pictureBox2.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Isaac.png");
            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Tainted_Isaac.png");
            }

            btnSwapIsaac.Visible = isIsaacTaintedUnlocked;
        }

        private void UpdateJudasDisplay(Character judas)
        {
            listBoxJudas.Items.Clear();
            listBoxJudas.Items.Add("Name: " + judas.Name);
            listBoxJudas.Items.Add("Strength: " + judas.Strength);
            listBoxJudas.Items.Add("Attack Speed: " + judas.AttackSpeed);
            listBoxJudas.Items.Add("Movement Speed: " + judas.MovementSpeed);
            listBoxJudas.Items.Add("Luck: " + judas.Luck);
            listBoxJudas.Items.Add("Max Health: " + judas.MaxHealth);
            listBoxJudas.Items.Add("Coins: " + judas.Coins);
            listBoxJudas.Items.Add("Bombs: " + judas.Bombs);
            listBoxJudas.Items.Add("Keys: " + judas.Keys);

            if (judas.Name == "Judas")
            {
                pictureBox3.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Judas.png");
            }
            else
            {
                pictureBox3.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Tainted_Judas.png");
            }

            btnSwapJudas.Visible = isJudasTaintedUnlocked;
        }
        private void UpdateMagdaleneDisplay(Character magdalene)
        {
            listBoxMagdalene.Items.Clear();
            listBoxMagdalene.Items.Add("Name: " + magdalene.Name);
            listBoxMagdalene.Items.Add("Strength: " + magdalene.Strength);
            listBoxMagdalene.Items.Add("Attack Speed: " + magdalene.AttackSpeed);
            listBoxMagdalene.Items.Add("Movement Speed: " + magdalene.MovementSpeed);
            listBoxMagdalene.Items.Add("Luck: " + magdalene.Luck);
            listBoxMagdalene.Items.Add("Max Health: " + magdalene.MaxHealth);
            listBoxMagdalene.Items.Add("Coins: " + magdalene.Coins);
            listBoxMagdalene.Items.Add("Bombs: " + magdalene.Bombs);
            listBoxMagdalene.Items.Add("Keys: " + magdalene.Keys);

            if (magdalene.Name == "Magdalene")
            {
                pictureBox4.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Magdalene.png");
            }
            else
            {
                pictureBox4.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\Tainted_Magdalene.png");
            }

            btnSwapMagdalene.Visible = isMagdaleneTaintedUnlocked;
        }
        private void btnIsaac_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = currentIsaac;
            StartGameInNewThread(selectedCharacter);
        }


        private void btnJudas_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = currentJudas;
            StartGameInNewThread(selectedCharacter);
        }

        private void btnMagdalene_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = currentMagdalene;
            StartGameInNewThread(selectedCharacter);
        }
        private void StartGameInNewThread(Character selectedCharacter)
        {
            StopMusic();
            this.Hide();
            Thread gameThread = new Thread(() =>
            {
                Application.Run(new Form1(selectedCharacter, this));
            });
            gameThread.SetApartmentState(ApartmentState.STA);
            gameThread.Start();
        }

        private void btnSwapIsaac_Click(object sender, EventArgs e)
        {
            if (isIsaacTainted)
            {
                currentIsaac = new Isaac();
                isIsaacTainted = false;
            }
            else
            {
                currentIsaac = new Tainted_Isaac();
                isIsaacTainted = true;
            }
            UpdateIsaacDisplay(currentIsaac);
        }

        private void btnSwapJudas_Click(object sender, EventArgs e)
        {
            if (isJudasTainted)
            {
                currentJudas = new Judas();
                isJudasTainted = false;
            }
            else
            {
                currentJudas = new Tainted_Judas();
                isJudasTainted = true;
            }
            UpdateJudasDisplay(currentJudas);
        }

        private void btnSwapMagdalene_Click(object sender, EventArgs e)
        {
            if (isMagdaleneTainted)
            {
                currentMagdalene = new Magdalene();
                isMagdaleneTainted = false;
            }
            else
            {
                currentMagdalene = new Tainted_Magdalene();
                isMagdaleneTainted = true;
            }
            UpdateMagdaleneDisplay(currentMagdalene);
        }

        private void ResetCharacters()
        {
            currentIsaac = new Tainted_Isaac();
            currentIsaac = new Isaac();

            currentJudas = new Tainted_Judas();
            currentJudas = new Judas();

            currentMagdalene = new Tainted_Magdalene();
            currentMagdalene = new Magdalene();

            UpdateIsaacDisplay(currentIsaac);
            UpdateJudasDisplay(currentJudas);
            UpdateMagdaleneDisplay(currentMagdalene);
        }
    }
}
