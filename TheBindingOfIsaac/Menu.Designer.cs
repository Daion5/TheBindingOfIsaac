using Library;

namespace TheBindingOfIsaac
{
    partial class Menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Menu));
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            btnIsaac = new Button();
            btnJudas = new Button();
            btnMagdalene = new Button();
            listBoxIsaac = new ListBox();
            listBoxJudas = new ListBox();
            listBoxMagdalene = new ListBox();
            win_streak = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(939, 210);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(139, 228);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(103, 139);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(487, 228);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(103, 139);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(848, 228);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(103, 139);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            // 
            // btnIsaac
            // 
            btnIsaac.Location = new Point(88, 370);
            btnIsaac.Name = "btnIsaac";
            btnIsaac.Size = new Size(75, 23);
            btnIsaac.TabIndex = 7;
            btnIsaac.Text = "CHOOSE";
            btnIsaac.UseVisualStyleBackColor = true;
            btnIsaac.Click += btnIsaac_Click;
            // 
            // btnJudas
            // 
            btnJudas.Location = new Point(439, 370);
            btnJudas.Name = "btnJudas";
            btnJudas.Size = new Size(75, 23);
            btnJudas.TabIndex = 8;
            btnJudas.Text = "CHOOSE";
            btnJudas.UseVisualStyleBackColor = true;
            btnJudas.Click += btnJudas_Click;
            // 
            // btnMagdalene
            // 
            btnMagdalene.Location = new Point(808, 370);
            btnMagdalene.Name = "btnMagdalene";
            btnMagdalene.Size = new Size(75, 23);
            btnMagdalene.TabIndex = 9;
            btnMagdalene.Text = "CHOOSE";
            btnMagdalene.UseVisualStyleBackColor = true;
            btnMagdalene.Click += btnMagdalene_Click;
            // 
            // listBoxIsaac
            // 
            listBoxIsaac.FormattingEnabled = true;
            listBoxIsaac.ItemHeight = 15;
            listBoxIsaac.Location = new Point(12, 228);
            listBoxIsaac.Name = "listBoxIsaac";
            listBoxIsaac.Size = new Size(120, 139);
            listBoxIsaac.TabIndex = 10;
            // 
            // listBoxJudas
            // 
            listBoxJudas.FormattingEnabled = true;
            listBoxJudas.ItemHeight = 15;
            listBoxJudas.Location = new Point(361, 228);
            listBoxJudas.Name = "listBoxJudas";
            listBoxJudas.Size = new Size(120, 139);
            listBoxJudas.TabIndex = 11;
            // 
            // listBoxMagdalene
            // 
            listBoxMagdalene.FormattingEnabled = true;
            listBoxMagdalene.ItemHeight = 15;
            listBoxMagdalene.Location = new Point(722, 228);
            listBoxMagdalene.Name = "listBoxMagdalene";
            listBoxMagdalene.Size = new Size(120, 139);
            listBoxMagdalene.TabIndex = 12;
            // 
            // win_streak
            // 
            win_streak.AutoSize = true;
            win_streak.Location = new Point(12, 419);
            win_streak.Name = "win_streak";
            win_streak.Size = new Size(32, 15);
            win_streak.TabIndex = 13;
            win_streak.Text = "label";
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(967, 456);
            Controls.Add(win_streak);
            Controls.Add(listBoxMagdalene);
            Controls.Add(listBoxJudas);
            Controls.Add(listBoxIsaac);
            Controls.Add(btnMagdalene);
            Controls.Add(btnJudas);
            Controls.Add(btnIsaac);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "Menu";
            Text = "Menu";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            Button characterButton1 = new Button();
            characterButton1.Text = "Isaac";
            characterButton1.Click += (s, args) => OnCharacterSelected(new Isaac());
            this.Controls.Add(characterButton1);

            Button characterButton2 = new Button();
            characterButton2.Text = "Judas";
            characterButton2.Click += (s, args) => OnCharacterSelected(new Judas());
            this.Controls.Add(characterButton2);

            Button characterButton3 = new Button();
            characterButton3.Text = "Magdalene";
            characterButton3.Click += (s, args) => OnCharacterSelected(new Magdalene());
            this.Controls.Add(characterButton3);

            this.Controls.Add(win_streak);

            UpdateWinStreakLabel();
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Button btnIsaac;
        private Button btnJudas;
        private Button btnMagdalene;
        private ListBox listBoxIsaac;
        private ListBox listBoxJudas;
        private ListBox listBoxMagdalene;
        private Label win_streak;
    }
}