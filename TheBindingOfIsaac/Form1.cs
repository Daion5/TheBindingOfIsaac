using Library;
using Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
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
        public Form1(Character selectedCharacter, Menu menu)
        {
            InitializeComponent();
            this.KeyPreview = true;
            random = new Random();
            character = selectedCharacter;
            menuForm = menu;
            InitializeGame();
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
        }
        private void Form1_Load(object sender, EventArgs e) { }
        private void InitializeAvailableItems()
        {
            availableItems = new List<Item>(Item.AllItems);
        }
        private void SetAllowedRoomCount()
        {
            allowedRoomCount = currentFloorLevel == 1 ? 10 : currentFloorLevel == 2 ? 15 : 20;
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
            this.Controls.Add(startPanel);

            totalRoomsGenerated = 1;
            paintedRoomIndex = random.Next(1, allowedRoomCount - 1);
            GenerateAdjacentRooms(startPanel);
        }

        private void GenerateAdjacentRooms(Panel panel)
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
                GenerateAdjacentRooms(roomPanel);
            }
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
            if (e.KeyCode != Keys.Up && e.KeyCode != Keys.Down && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right && e.KeyCode != Keys.Tab && e.KeyCode != Keys.Enter)
                return;


            int step = 100;
            Point currentPosition = pictureBox1.Location;
            Point newPosition = currentPosition;

            switch (e.KeyCode)
            {
                case Keys.Right:
                    newPosition.X += step;
                    break;
                case Keys.Left:
                    newPosition.X -= step;
                    break;
                case Keys.Down:
                    newPosition.Y += step;
                    break;
                case Keys.Up:
                    newPosition.Y -= step;
                    break;
                case Keys.Tab:
                    ShowCharacterStats();
                    return;
                case Keys.Enter:
                    Panel currentPanel = GetCurrentPanel(currentPosition);
                    if (currentPanel != null)
                    {
                        Console.WriteLine($"Current panel color: {currentPanel.BackColor}");
                        if (currentPanel.BackColor == ColorTranslator.FromHtml("#fffffd"))
                        {
                            if (currentFloorLevel == 3)
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
                                Form floorNavigatorForm = new Form();
                                PictureBox floorNavigatorImage = new PictureBox();

                                floorNavigatorImage.Image = Image.FromFile($@"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\currentFloorLevel\floor_navigator_{currentFloorLevel + 1}.png");
                                floorNavigatorImage.SizeMode = PictureBoxSizeMode.AutoSize;
                                floorNavigatorForm.Size = new Size(floorNavigatorImage.Image.Width, floorNavigatorImage.Image.Height + 38);
                                floorNavigatorImage.Location = new Point(0, 0);
                                floorNavigatorForm.Controls.Add(floorNavigatorImage);
                                floorNavigatorForm.StartPosition = FormStartPosition.CenterScreen;
                                floorNavigatorForm.Show();

                                Timer timer = new Timer();
                                timer.Interval = 3000;
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
                    form3.FormClosedEvent += (s, args) =>
                    {
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
            }
        }
        private void ShowEndingAndRestart()
        {
            if (gameEnded) { return; }

            gameEnded = true;

            this.Invoke((MethodInvoker)delegate
            {
                this.Hide();
                menuForm.OnGameEnded(true);
                menuForm.Show();
                this.Close();
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
                        }
                    }

                    Form2 form2 = new Form2(character, monster, characterImagePath, monsterImagePath, menuForm);
                    form2.ShowDialog();

                    if (roomPanel.BackColor == Color.Red)
                    {
                        PictureBox endingChestPicture = new PictureBox
                        {
                            Image = Image.FromFile(currentFloorLevel == 3
                                ? @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\ending_chest.png"
                                : @"C:\Users\Daion\Desktop\PZ\TBOI\TheBindingOfIsaac\images\assets\trapdoor.png"),
                            SizeMode = PictureBoxSizeMode.Zoom,
                            BackColor = Color.Transparent,
                        };
                        endingChestPicture.Location = new Point((roomPanel.Width - endingChestPicture.Width) / 2, (roomPanel.Height - endingChestPicture.Height) / 2);
                        roomPanel.Controls.Add(endingChestPicture);
                        roomPanel.BackColor = ColorTranslator.FromHtml("#fffffd");
                    }
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
        private void ShowCharacterStats()
        {
            if (characterStatsForm != null && !characterStatsForm.IsDisposed)
                return;

            characterStatsForm = new Form();
            characterStatsForm.Text = "Character Stats";

            int yOffset = 10;
            AddStatLabel("Name: " + character.Name, 10, yOffset, characterStatsForm);
            AddStatLabel("Strength: " + character.Strength, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Attack Speed: " + character.AttackSpeed, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Movement: " + character.MovementSpeed, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Luck: " + character.Luck, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("HP: " + character.CurrentHealth + "/" + character.MaxHealth, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Coins: " + character.Coins, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Bombs: " + character.Bombs, 10, yOffset += 25, characterStatsForm);
            AddStatLabel("Keys: " + character.Keys, 10, yOffset += 25, characterStatsForm);

            characterStatsForm.StartPosition = FormStartPosition.CenterScreen;
            characterStatsForm.FormClosed += (sender, e) => characterStatsForm = null;
            characterStatsForm.Show();
        }


        private void AddStatLabel(string text, int x, int y, Form form)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            form.Controls.Add(label);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                ShowCharacterStats();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}