using Library;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace TheBindingOfIsaac
{
    public partial class Menu : Form
    {
        public class UserData
        {
            public int WinStreak { get; set; }
        }
        public static int WinStreak { get; private set; } = 0;
        private string userDataFolderPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\data";
        private void SaveUserData()
        {
            Directory.CreateDirectory(userDataFolderPath);

            UserData userData = new UserData
            {
                WinStreak = WinStreak
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
                }
            }
        }
        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserData();
        }
        public Menu()
        {
            InitializeComponent();
            InitializeListBoxes();
            this.FormClosing += Menu_FormClosing;
            LoadUserData();
            UpdateWinStreakLabel();
        }
        private void OnCharacterSelected(Character selectedCharacter)
        {
            this.Hide();
            Thread gameThread = new Thread(() => StartGame(selectedCharacter));
            gameThread.Start();
        }
        private void StartGame(Character selectedCharacter)
        {
            Application.Run(new Form1(selectedCharacter, this));
            this.Invoke(new Action(() => this.Show()));
        }
        public void OnGameEnded(bool won)
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
                MessageBox.Show("You lost!");
            }
            Item.ResetItemList();
            SaveUserData();
            UpdateWinStreakLabel();
            this.Show();
        }

        private void UpdateWinStreakLabel()
        {
            win_streak.Text = "WIN STREAK: " + WinStreak;
        }
        private void InitializeListBoxes()
        {
            Isaac isaac = new Isaac();
            Judas judas = new Judas();
            Magdalene magdalene = new Magdalene();

            listBoxIsaac.Items.Add("Name: " + isaac.Name);
            listBoxIsaac.Items.Add("Strength: " + isaac.Strength);
            listBoxIsaac.Items.Add("Attack Speed: " + isaac.AttackSpeed);
            listBoxIsaac.Items.Add("Movement Speed: " + isaac.MovementSpeed);
            listBoxIsaac.Items.Add("Luck: " + isaac.Luck);
            listBoxIsaac.Items.Add("Max Health: " + isaac.MaxHealth);
            listBoxIsaac.Items.Add("Coins: " + isaac.Coins);
            listBoxIsaac.Items.Add("Bombs: " + isaac.Bombs);
            listBoxIsaac.Items.Add("Keys: " + isaac.Keys);

            listBoxJudas.Items.Add("Name: " + judas.Name);
            listBoxJudas.Items.Add("Strength: " + judas.Strength);
            listBoxJudas.Items.Add("Attack Speed: " + judas.AttackSpeed);
            listBoxJudas.Items.Add("Movement Speed: " + judas.MovementSpeed);
            listBoxJudas.Items.Add("Luck: " + judas.Luck);
            listBoxJudas.Items.Add("Max Health: " + judas.MaxHealth);
            listBoxJudas.Items.Add("Coins: " + judas.Coins);
            listBoxJudas.Items.Add("Bombs: " + judas.Bombs);
            listBoxJudas.Items.Add("Keys: " + judas.Keys);

            listBoxMagdalene.Items.Add("Name: " + magdalene.Name);
            listBoxMagdalene.Items.Add("Strength: " + magdalene.Strength);
            listBoxMagdalene.Items.Add("Attack Speed: " + magdalene.AttackSpeed);
            listBoxMagdalene.Items.Add("Movement Speed: " + magdalene.MovementSpeed);
            listBoxMagdalene.Items.Add("Luck: " + magdalene.Luck);
            listBoxMagdalene.Items.Add("Max Health: " + magdalene.MaxHealth);
            listBoxMagdalene.Items.Add("Coins: " + magdalene.Coins);
            listBoxMagdalene.Items.Add("Bombs: " + magdalene.Bombs);
            listBoxMagdalene.Items.Add("Keys: " + magdalene.Keys);
        }
        private void btnIsaac_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = new Isaac();
            StartGameInNewThread(selectedCharacter);
        }

        private void btnJudas_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = new Judas();
            StartGameInNewThread(selectedCharacter);
        }

        private void btnMagdalene_Click(object sender, EventArgs e)
        {
            Character selectedCharacter = new Magdalene();
            StartGameInNewThread(selectedCharacter);
        }
        private void StartGameInNewThread(Character selectedCharacter)
        {
            this.Hide();
            Thread gameThread = new Thread(() =>
            {
                Application.Run(new Form1(selectedCharacter, this));
            });
            gameThread.SetApartmentState(ApartmentState.STA);
            gameThread.Start();
        }
    }
}
