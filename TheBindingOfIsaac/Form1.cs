using Library;
using Library.Exceptions;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static TheBindingOfIsaac.Form3;
using Timer = System.Windows.Forms.Timer;

namespace TheBindingOfIsaac
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox1;
        private Random random;
        private int totalRoomsGenerated;
        private int allowedRoomCount;
        private int paintedRoomIndex;
        private int currentFloorLevel;
        private Menu menuForm;
        private Panel startPanel;
        private Panel furthestPanelFromStart;
        private List<Item> availableItems;
        private Character character;
        private Monster monster;
        private ProgressBar progressBarMonster;
        private Form characterStatsForm = null;
        private bool gameEnded = false;
        private Label statsLabel;
        private string heartsFolderPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\hearts";
        private string pickupsFolderPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\pickups";
        private IWavePlayer waveOutDevice;
        private AudioFileReader audioFileReader;
        private bool shopRoomGenerated = false;
        private Item currentShopItem;
        private bool shopRoomGeneratedOnCurrentFloor;
        private bool chestOpened = false;
        private bool itemTaken = false;


        public Form1(Character selectedCharacter, Menu menu)
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Text = "The Binding Of Isaac";
            random = new Random();
            character = selectedCharacter;
            menuForm = menu;
            InitializeGame();
            InitializeStatsLabel();
            UpdateStatsLabel();
        }

        private void InitializeGame()
        {
            totalRoomsGenerated = 0;
            currentFloorLevel = 1;
            SetAllowedRoomCount();
            paintedRoomIndex = random.Next(1, allowedRoomCount - 1);
            InitializePictureBox(character);
            InitializeAvailableItems();
            GenerateRooms();
            ShowNavigator();
            PlayMusicBasedOnFloorLevel(currentFloorLevel);
        }

        private void ShowNavigator() {
            Form floorNavigatorForm = new Form();
            PictureBox floorNavigatorImage = new PictureBox();

            floorNavigatorImage.Image = Image.FromFile($@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\currentFloorLevel\floor_navigator_1.png");
            floorNavigatorImage.SizeMode = PictureBoxSizeMode.AutoSize;
            floorNavigatorForm.Size = new Size(floorNavigatorImage.Image.Width, floorNavigatorImage.Image.Height + 38);
            floorNavigatorImage.Location = new Point(0, 0);
            floorNavigatorForm.Controls.Add(floorNavigatorImage);
            floorNavigatorForm.StartPosition = FormStartPosition.CenterScreen;
            floorNavigatorForm.TopMost = true;
            floorNavigatorForm.Show();

            Timer timer = new Timer();
            timer.Interval = 2000;
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                floorNavigatorForm.Close();
            };
            timer.Start();
        }
        private void InitializeStatsLabel()
        {
            statsLabel = new Label();
            statsLabel.Location = new Point(10, 50);
            statsLabel.AutoSize = true;
            this.Controls.Add(statsLabel);
        }
        private void UpdateStatsLabel()
        {
            foreach (Control control in this.Controls.OfType<PictureBox>().ToList())
            {
                if (control.Tag?.ToString() == "heart" || control.Tag?.ToString() == "pickup" || control.Tag?.ToString() == "item")
                {
                    this.Controls.Remove(control);
                    control.Dispose();
                }
            }

            int maxHealth = character.MaxHealth;
            int numFullHearts = character.CurrentHealth / 20;
            int remainderHealth = character.CurrentHealth % 20;

            int startX = 10;
            int startY = 10;
            int heartWidth = 40;
            int heartHeight = 40;

            for (int i = 0; i < maxHealth / 20; i++)
            {
                PictureBox heart = new PictureBox();
                heart.SizeMode = PictureBoxSizeMode.Zoom;
                heart.Size = new Size(heartWidth, heartHeight);
                heart.Location = new Point(startX + i * (heartWidth + 5), startY);
                heart.Tag = "heart";

                if (i < numFullHearts)
                {
                    heart.Image = Image.FromFile(Path.Combine(heartsFolderPath, "heart_full.png"));
                }
                else if (i == numFullHearts && remainderHealth >= 10)
                {
                    heart.Image = Image.FromFile(Path.Combine(heartsFolderPath, "heart_half.png"));
                }
                else
                {
                    heart.Image = Image.FromFile(Path.Combine(heartsFolderPath, "heart_empty.png"));
                }

                this.Controls.Add(heart);
            }

            PictureBox itemPictureBox = new PictureBox();
            itemPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            itemPictureBox.Size = new Size(50, 50);
            itemPictureBox.Location = new Point(10, 200);
            itemPictureBox.Tag = "item";

            switch (character.ActiveItemName)
            {
                case "Yum Heart":
                    itemPictureBox.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items\spacebar\Yum_Heart.png");
                    break;
                case "The Book Of Belial":
                    itemPictureBox.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items\spacebar\The_Book_Of_Belial.png");
                    break;
                case "Wooden Nickel":
                    itemPictureBox.Image = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items\spacebar\Wooden_Nickel.png");
                    break;
            }

            this.Controls.Add(itemPictureBox);

            statsLabel.Text = $"Strength: {character.Strength}\n" +
                             $"Attack Speed: {character.AttackSpeed}\n" +
                             $"Movement: {character.MovementSpeed}\n" +
                             $"Luck: {character.Luck}\n" +
                             $"HP: {character.CurrentHealth}/{character.MaxHealth}\n" +
                             $"Coins: {character.Coins}\n" +
                             $"Bombs: {character.Bombs}\n" +
                             $"Keys: {character.Keys}\n" +
                             $"Active Item Charges: {character.ActiveItemCharges}/{character.MaxActiveItemCharges}";
        }

        private void Form1_Load(object sender, EventArgs e) { }
        private void InitializeAvailableItems()
        {
            availableItems = new List<Item>(Item.AllItems);
        }

        private void SetAllowedRoomCount()
        {
            allowedRoomCount = currentFloorLevel switch
            {
                1 => 10,
                2 => 15,
                3 => 20,
                4 => 25,
            };
        }

        private void InitializePictureBox(Character character)
        {
            if (pictureBox1 != null)
            {
                this.Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
            }

            pictureBox1 = new PictureBox();
            pictureBox1.Image = Image.FromFile($@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\{character.Name}.png");
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.Location = new Point((this.ClientSize.Width - pictureBox1.Width) / 2, (this.ClientSize.Height - pictureBox1.Height) / 2);
            pictureBox1.BackColor = Color.Transparent;
            this.Controls.Add(pictureBox1);
        }


        private void GenerateRooms()
        {
            this.Controls.OfType<Panel>().ToList().ForEach(panel => this.Controls.Remove(panel));

            startPanel = new Panel();
            startPanel.Tag = startPanel;
            startPanel.BackColor = Color.White;
            startPanel.Size = new Size(100, 100);
            startPanel.Location = new Point((this.ClientSize.Width - startPanel.Width) / 2, (this.ClientSize.Height - startPanel.Height) / 2);
            startPanel.BorderStyle = BorderStyle.FixedSingle;
            startPanel.BackColor = Color.LightGreen;
            this.Controls.Add(startPanel);

            totalRoomsGenerated = 1;
            shopRoomGeneratedOnCurrentFloor = false;
            bool yellowRoomGenerated = false;
            GenerateAdjacentRooms(startPanel, ref yellowRoomGenerated);

            if (!yellowRoomGenerated)
            {
                paintedRoomIndex = totalRoomsGenerated;
                GenerateAdjacentRooms(startPanel, ref yellowRoomGenerated);
            }
        }


        private void GenerateAdjacentRooms(Panel panel, ref bool yellowRoomGenerated)
        {
            List<string> edges = new List<string>() { "Right", "Bottom", "Left", "Top" };
            Shuffle(edges);

            foreach (var edge in edges)
            {
                if (totalRoomsGenerated >= allowedRoomCount)
                    break;

                if (HasNeighbor(panel, edge))
                {
                    continue;
                }

                if (!IsInsideWindow(panel.Location, panel.Size))
                {
                    continue;
                }

                totalRoomsGenerated++;
                Point location = panel.Location;
                Panel nextPanel = panel;

                switch (edge)
                {
                    case "Right":
                        location.X += panel.Width;
                        break;
                    case "Bottom":
                        location.Y += panel.Height;
                        break;
                    case "Left":
                        location.X -= panel.Width;
                        break;
                    case "Top":
                        location.Y -= panel.Height;
                        break;
                }

                Panel roomPanel = new Panel();
                roomPanel.BackColor = ColorTranslator.FromHtml("#fffffe");
                roomPanel.Size = new Size(100, 100);
                roomPanel.Location = location;
                roomPanel.BorderStyle = BorderStyle.FixedSingle;
                this.Controls.Add(roomPanel);

                if (totalRoomsGenerated == paintedRoomIndex && totalRoomsGenerated != 1 && totalRoomsGenerated != allowedRoomCount)
                {
                    roomPanel.BackColor = Color.Yellow;
                    GenerateElement(roomPanel, "Item");
                    yellowRoomGenerated = true;
                }
                else if (currentFloorLevel > 1 && !shopRoomGeneratedOnCurrentFloor && totalRoomsGenerated != 1 && totalRoomsGenerated != allowedRoomCount && roomPanel.BackColor != Color.Yellow)
                {
                    if (CanGenerateShopRoom(roomPanel))
                    {
                        roomPanel.BackColor = ColorTranslator.FromHtml("#D2B48C");
                        GenerateElement(roomPanel, "Shop");
                        shopRoomGeneratedOnCurrentFloor = true;
                    }
                    else
                    {
                        GenerateElement(roomPanel, "Monster");
                    }
                }
                else if (totalRoomsGenerated == allowedRoomCount)
                {
                    roomPanel.BackColor = Color.Red;
                    GenerateElement(roomPanel, "Boss");
                }
                else
                {
                    GenerateElement(roomPanel, "Monster");
                }

                furthestPanelFromStart = roomPanel;
                GenerateAdjacentRooms(roomPanel, ref yellowRoomGenerated);
            }
        }
        private bool CanGenerateShopRoom(Panel roomPanel)
        {
            return roomPanel.BackColor != Color.Yellow && totalRoomsGenerated != 1 && totalRoomsGenerated != allowedRoomCount;
        }
        private void GenerateElement(Panel roomPanel, string elementType)
        {
            if (elementType == "Monster")
            {
                GenerateMonster(roomPanel);
            }
            else if (elementType == "Item")
            {
                try
                {
                    GenerateItem(roomPanel);
                }
                catch (NoMoreItemsException ex)
                {
                    Console.WriteLine(ex.ToString());
                    Console.ResetColor();
                }
            }
            else if (elementType == "Boss")
            {
                GenerateBoss(roomPanel);
            }
            else if (elementType == "Shop" && currentFloorLevel > 1)
            {
                GenerateShopItem();
                roomPanel.BackColor = ColorTranslator.FromHtml("#D2B48C");
            }
        }

        private void SetElementImage(Panel roomPanel, string imageName, string folderPath, Size imageSize)
        {
            string imagePath = $@"{folderPath}\{imageName.ToLower()}.png";
            PictureBox elementPicture = new PictureBox
            {
                Image = Image.FromFile(imagePath),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            if (roomPanel.BackColor == ColorTranslator.FromHtml("#fffffe"))
            {
                elementPicture.Size = new Size(40, 40);
            }
            else if (roomPanel.BackColor == Color.Red)
            {
                elementPicture.Size = imageSize;
            }

            elementPicture.Location = new Point((roomPanel.Width - elementPicture.Width) / 2, (roomPanel.Height - elementPicture.Height) / 2);
            roomPanel.Controls.Add(elementPicture);
        }

        private void GenerateBoss(Panel roomPanel)
        {
            List<Monster> validBosses = Monster.AllBosses.Where(b => b.Depth == currentFloorLevel).ToList();
            int bossIndex = random.Next(validBosses.Count);
            Monster boss = new Monster(validBosses[bossIndex]);
            roomPanel.Tag = boss;
            if (currentFloorLevel == 1)
            {
                SetElementImage(roomPanel, boss.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\first_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 2)
            {
                SetElementImage(roomPanel, boss.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\second_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 3)
            {
                SetElementImage(roomPanel, boss.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\third_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 4)
            {
                SetElementImage(roomPanel, boss.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\fourth_floor", new Size(100, 100));
            }
        }

        private void GenerateItem(Panel roomPanel)
        {
            try
            {
                Item item = Item.GetNextAvailableItem();
                roomPanel.Tag = item;
                SetElementImage(roomPanel, item.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items", new Size(60, 60));
                item.IsTaken = true;
            }
            catch (NoMoreItemsException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }

        private void GenerateShopItem()
        {
            if (currentFloorLevel > 1)
            {
                currentShopItem = Item.AllShopItems[new Random().Next(Item.AllShopItems.Count)];
            }
            else
            {
                currentShopItem = null;
            }
        }


        private void GenerateMonster(Panel roomPanel)
        {
            List<Monster> validMonsters = Monster.AllMonsters.Where(m => m.Depth == currentFloorLevel).ToList();
            int monsterIndex = random.Next(validMonsters.Count);
            Monster monster = new Monster(validMonsters[monsterIndex]);
            roomPanel.Tag = monster;
            if (currentFloorLevel == 1)
            {
                SetElementImage(roomPanel, monster.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\first_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 2)
            {
                SetElementImage(roomPanel, monster.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\second_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 3)
            {
                SetElementImage(roomPanel, monster.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\third_floor", new Size(100, 100));
            }
            else if (currentFloorLevel == 4)
            {
                SetElementImage(roomPanel, monster.Name, @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\fourth_floor", new Size(100, 100));
            }
        }

        private bool HasNeighbor(Panel panel, string edge)
        {
            Point location = panel.Location;

            switch (edge)
            {
                case "Right":
                    location.X += panel.Width;
                    break;
                case "Bottom":
                    location.Y += panel.Height;
                    break;
                case "Left":
                    location.X -= panel.Width;
                    break;
                case "Top":
                    location.Y -= panel.Height;
                    break;
            }

            return this.Controls.OfType<Panel>().Any(p => p.Location == location);
        }
        private bool IsInsideWindow(Point location, Size size)
        {
            return location.X >= 0 && location.X + size.Width <= this.ClientSize.Width && location.Y >= 0 && location.Y + size.Height <= this.ClientSize.Height;
        }

        private void Shuffle(List<string> edges)
        {
            int n = edges.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int j = random.Next(i, n);
                (edges[i], edges[j]) = (edges[j], edges[i]);
            }
        }
        private void MoveToStartPanel()
        {
            if (startPanel != null)
            {
                pictureBox1.Location = new Point(startPanel.Location.X + (startPanel.Width - pictureBox1.Width) / 2,
                                                 startPanel.Location.Y + (startPanel.Height - pictureBox1.Height) / 2);
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && e.KeyCode != Keys.Enter && e.KeyCode != Keys.Space)
                return;

            int step = 100;
            Point currentPosition = pictureBox1.Location;
            Point newPosition = currentPosition;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    newPosition.X += step;
                    UpdateStatsLabel();
                    startPanel.BackColor = Color.White;
                    break;
                case Keys.Left:
                    newPosition.X -= step;
                    UpdateStatsLabel();
                    startPanel.BackColor = Color.White;
                    break;
                case Keys.Down:
                    newPosition.Y += step;
                    UpdateStatsLabel();
                    startPanel.BackColor = Color.White;
                    break;
                case Keys.Up:
                    newPosition.Y -= step;
                    UpdateStatsLabel();
                    startPanel.BackColor = Color.White;
                    break;
                case Keys.Space:
                    character.UseActiveItem();
                    UpdateStatsLabel();
                    return;
                case Keys.Enter:
                    UpdateStatsLabel();
                    Panel currentPanel = GetCurrentPanel(currentPosition);
                    if (currentPanel != null)
                    {
                        Console.WriteLine($"Current panel color: {currentPanel.BackColor}");
                        if (currentPanel.BackColor == ColorTranslator.FromHtml("#fffffd"))
                        {
                            if (currentFloorLevel == 4)
                            {
                                Form winForm = new Form();
                                Image img = Image.FromFile(@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\win.png");
                                PictureBox pictureBox = new PictureBox
                                {
                                    Image = img,
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Dock = DockStyle.Fill
                                };
                                winForm.Controls.Add(pictureBox);
                                winForm.StartPosition = FormStartPosition.CenterScreen;
                                winForm.Size = new Size(400, 400);
                                winForm.Show();

                                Timer timer = new Timer();
                                timer.Interval = 3000;
                                timer.Tick += (s, args) =>
                                {
                                    timer.Stop();
                                    winForm.Close();
                                    ShowEndingAndRestart();
                                };
                                timer.Start();
                            }

                            else
                            {
                                if (waveOutDevice != null)
                                {
                                    waveOutDevice.Stop();
                                    waveOutDevice.Dispose();
                                    waveOutDevice = null;
                                }
                                PlayMusicBasedOnFloorLevel(currentFloorLevel + 1);
                                Form floorNavigatorForm = new Form();
                                PictureBox floorNavigatorImage = new PictureBox();
                                ResetShopState();
                                floorNavigatorImage.Image = Image.FromFile($@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\currentFloorLevel\floor_navigator_{currentFloorLevel + 1}.png");
                                floorNavigatorImage.SizeMode = PictureBoxSizeMode.AutoSize;
                                floorNavigatorForm.Size = new Size(floorNavigatorImage.Image.Width, floorNavigatorImage.Image.Height + 38);
                                floorNavigatorImage.Location = new Point(0, 0);
                                floorNavigatorForm.Controls.Add(floorNavigatorImage);
                                floorNavigatorForm.StartPosition = FormStartPosition.CenterScreen;
                                floorNavigatorForm.Show();

                                Timer timer = new Timer();
                                timer.Interval = 2000;
                                timer.Tick += (s, args) =>
                                {
                                    timer.Stop();
                                    floorNavigatorForm.Close();

                                    currentFloorLevel++;
                                    SetAllowedRoomCount();
                                    InitializeAvailableItems();
                                    GenerateRooms();
                                    MoveToStartPanel();

                                    foreach (Control control in Controls)
                                    {
                                        if (control.Tag != null && control.Tag.ToString() == "startPanel")
                                        {
                                            control.Controls.Add(pictureBox1);
                                            break;
                                        }
                                    }
                                };
                                timer.Start();
                            }
                            if (waveOutDevice == null)
                            {
                                waveOutDevice = new WaveOut();
                            }
                        }
                    }
                    return;
            }

            Panel initialPanel = GetCurrentPanel(currentPosition);
            Panel nextPanel = GetCurrentPanel(newPosition);

            if (initialPanel != null && nextPanel != null)
            {
                if (HasDoorAccess(initialPanel, nextPanel, e.KeyCode))
                {
                    pictureBox1.Location = newPosition;
                }
                else
                {
                    if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
                    {
                        pictureBox1.Location = ClampToPanelBoundsLeftUp(currentPosition, newPosition, initialPanel);
                    }
                    else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
                    {
                        pictureBox1.Location = ClampToPanelBoundsRightDown(currentPosition, newPosition, initialPanel);
                    }
                }
                if (nextPanel.BackColor == ColorTranslator.FromHtml("#fffffe"))
                {
                    StartCombat(nextPanel);
                    nextPanel.BackColor = Color.White;
                }
                if (nextPanel.BackColor == Color.Red)
                {
                    StartCombat(nextPanel);
                    nextPanel.BackColor = ColorTranslator.FromHtml("#fffffd");
                }
                Item item = nextPanel.Tag as Item;
                if (nextPanel.BackColor == Color.Yellow && item != null)
                {
                    string itemsFolderPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\items";
                    Form3 form3 = new Form3(item, character, itemsFolderPath);
                    form3.StartPosition = FormStartPosition.CenterScreen;
                    form3.FormClosedEvent += (s, args) =>
                    {
                        UpdateStatsLabel();
                        var eventArgs = args as ItemTakenEventArgs;
                        if (eventArgs != null && eventArgs.ItemTaken)
                        {
                            var itemPicture = nextPanel.Controls.OfType<PictureBox>().FirstOrDefault();
                            if (itemPicture != null)
                            {
                                nextPanel.Controls.Remove(itemPicture);
                                itemPicture.Dispose();
                            }
                            nextPanel.BackColor = Color.White;
                        }
                    };
                    form3.Show();
                }
                CheckShopRoom();
            }
        }
        private void ShowEndingAndRestart()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel && panel.BackColor == ColorTranslator.FromHtml("#D2B48C"))
                {
                    foreach (Control panelControl in panel.Controls)
                    {
                        if (panelControl is PictureBox)
                        {
                            panelControl.Visible = true;
                            panelControl.Enabled = true;
                        }
                    }
                }
            }

            if (gameEnded) { return; }

            gameEnded = true;

            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
                menuForm.OnGameEnded(true, character);
                menuForm.Show();
                this.Close();
                this.Dispose();
                Application.Restart();
            });
        }



        private void StartCombat(Panel roomPanel)
        {
            try
            {
                Monster monster = roomPanel.Tag as Monster;

                if (monster != null)
                {
                    string characterImagePath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\characters\" + character.Name + ".png";
                    string monsterImagePath = GetMonsterImagePath(monster);

                    foreach (Control control in roomPanel.Controls)
                    {
                        if (control is PictureBox)
                        {
                            roomPanel.Controls.Remove(control);
                            character.IncrementActiveItemCharges();
                        }
                    }

                    Form2 form2 = new Form2(character, monster, characterImagePath, monsterImagePath, menuForm);
                    form2.StartPosition = FormStartPosition.CenterScreen;
                    form2.ShowDialog();

                    if (roomPanel.BackColor == Color.Red)
                    {
                        PictureBox endingChestPicture = new PictureBox
                        {
                            Image = Image.FromFile(currentFloorLevel == 4
                                ? @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\ending_chest.png"
                                : @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\trapdoor.png"),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.Transparent,
                        };
                        endingChestPicture.Location = new Point((roomPanel.Width - endingChestPicture.Width) / 2, (roomPanel.Height - endingChestPicture.Height) / 2);
                        roomPanel.Controls.Add(endingChestPicture);
                        roomPanel.BackColor = ColorTranslator.FromHtml("#fffffd");
                    }
                    UpdateStatsLabel();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    throw new MonsterNotFoundException(" Monster not found in the room. ");
                }
            }
            catch (MonsterNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ResetColor();
            }
        }

        private string GetMonsterImagePath(Monster monster)
        {
            Console.WriteLine($"Nazwa potwora: {monster.Name}");
            foreach (var boss in Monster.AllBosses)
            {
                Console.WriteLine($"Boss na liœcie: {boss.Name}");
            }

            bool isBoss = Monster.AllBosses.Any(b => b.Name == monster.Name);
            Console.WriteLine($"Czy {monster.Name} jest bossem: {isBoss}");

            if (isBoss)
            {
                if (currentFloorLevel == 1)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\first_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 2)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\second_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 3)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\third_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 4)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\bosses\fourth_floor\" + monster.Name + ".png";
                }
            }
            else
            {
                if (currentFloorLevel == 1)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\first_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 2)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\second_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 3)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\third_floor\" + monster.Name + ".png";
                }
                if (currentFloorLevel == 4)
                {
                    return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\monsters\fourth_floor\" + monster.Name + ".png";
                }
            }
            return @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\default.png";
        }


        private Color GetPixelColor(Point position)
        {
            return this.BackColor;
        }

        private bool HasDoorAccess(Panel currentPanel, Panel nextPanel, Keys direction)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel door && door.BackColor == Color.Blue)
                {
                    if (IsDoorBetweenPanels(door, currentPanel, nextPanel, direction))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsDoorBetweenPanels(Panel door, Panel panel1, Panel panel2, Keys direction)
        {
            switch (direction)
            {
                case Keys.Right:
                    return door.Left == panel1.Right && door.Top <= pictureBox1.Top && door.Bottom >= pictureBox1.Bottom && door.Left <= panel2.Left;

                case Keys.Left:
                    return door.Right == panel1.Left && door.Top <= pictureBox1.Top && door.Bottom >= pictureBox1.Bottom && door.Right >= panel2.Right;

                case Keys.Down:
                    return door.Top == panel1.Bottom && door.Left <= pictureBox1.Left && door.Right >= pictureBox1.Right && door.Top <= panel2.Top;

                case Keys.Up:
                    return door.Bottom == panel1.Top && door.Left <= pictureBox1.Left && door.Right >= pictureBox1.Right && door.Bottom >= panel2.Bottom;
            }
            return false;
        }

        private Point ClampToPanelBoundsLeftUp(Point currentPosition, Point newPosition, Panel currentPanel)
        {
            if (newPosition.X < 0 || GetPixelColor(newPosition) == Color.Black)
            {
                newPosition.X = Math.Max(0, currentPosition.X);
            }
            if (newPosition.Y < 0 || GetPixelColor(newPosition) == Color.Black)
            {
                newPosition.Y = Math.Max(0, currentPosition.Y);
            }
            return newPosition;
        }

        private Point ClampToPanelBoundsRightDown(Point currentPosition, Point newPosition, Panel currentPanel)
        {
            if (newPosition.X < 0 || GetPixelColor(newPosition) == Color.Black)
            {
                newPosition.X = Math.Max(0, currentPosition.X);
            }
            if (newPosition.Y < 0 || GetPixelColor(newPosition) == Color.Black)
            {
                newPosition.Y = Math.Max(0, currentPosition.Y);
            }
            return newPosition;
        }

        private Panel GetCurrentPanel(Point position)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel panel && panel.Bounds.Contains(position))
                {
                    return panel;
                }
            }
            return null;
        }
        private void CheckShopRoom()
        {
            Panel currentPanel = GetCurrentPanel(pictureBox1.Location);
            if (currentPanel != null && currentPanel.BackColor == ColorTranslator.FromHtml("#D2B48C"))
            {
                ShowForm4();
            }
        }

        private void ShowForm4()
        {
            Form4 form4 = new Form4(currentShopItem, character, UpdateStatsLabel, chestOpened, itemTaken);
            this.Text = "Shop";
            form4.StartPosition = FormStartPosition.CenterScreen;
            form4.ShowDialog();

            chestOpened = form4.ChestOpened;
            itemTaken = form4.ItemTaken;
        }
        private void ResetShopState()
        {
            chestOpened = false;
            itemTaken = false;
            currentShopItem = Item.AllShopItems[new Random().Next(Item.AllShopItems.Count)];
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdateStatsLabel();
        }

        private void PlayMusicBasedOnFloorLevel(int floorLevel)
        {
            string musicPath = "";
            switch (floorLevel)
            {
                case 1:
                    musicPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\floor_1.mp3";
                    break;
                case 2:
                    musicPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\floor_2.mp3";
                    break;
                case 3:
                    musicPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\floor_3.mp3";
                    break;
                case 4:
                    musicPath = @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\soundtrack\floor_4.mp3";
                    break;
            }

            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
            }
            else
            {
                waveOutDevice = new WaveOut();
            }

            audioFileReader = new AudioFileReader(musicPath);
            audioFileReader.Volume = 0.5f;
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }

    }
}