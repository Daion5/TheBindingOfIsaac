namespace TheBindingOfIsaac
{
    partial class Form2
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
            pictureBoxCharacter = new PictureBox();
            pictureBoxMonster = new PictureBox();
            btnFight = new Button();
            characterName = new Label();
            monsterName = new Label();
            progressBarCharacter = new ProgressBar();
            progressBarMonster = new ProgressBar();
            btnBomb = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharacter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMonster).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxCharacter
            // 
            pictureBoxCharacter.Location = new Point(9, 62);
            pictureBoxCharacter.Name = "pictureBoxCharacter";
            pictureBoxCharacter.Size = new Size(135, 128);
            pictureBoxCharacter.TabIndex = 0;
            pictureBoxCharacter.TabStop = false;
            // 
            // pictureBoxMonster
            // 
            pictureBoxMonster.Location = new Point(231, 62);
            pictureBoxMonster.Name = "pictureBoxMonster";
            pictureBoxMonster.Size = new Size(135, 128);
            pictureBoxMonster.TabIndex = 1;
            pictureBoxMonster.TabStop = false;
            // 
            // btnFight
            // 
            btnFight.Location = new Point(150, 98);
            btnFight.Name = "btnFight";
            btnFight.Size = new Size(75, 53);
            btnFight.TabIndex = 2;
            btnFight.Text = "FIGHT";
            btnFight.UseVisualStyleBackColor = true;
            btnFight.Click += button1_Click;
            // 
            // characterName
            // 
            characterName.AutoSize = true;
            characterName.Location = new Point(9, 15);
            characterName.Name = "characterName";
            characterName.Size = new Size(38, 15);
            characterName.TabIndex = 3;
            characterName.Text = "label1";
            // 
            // monsterName
            // 
            monsterName.AutoSize = true;
            monsterName.Location = new Point(249, 15);
            monsterName.Name = "monsterName";
            monsterName.Size = new Size(38, 15);
            monsterName.TabIndex = 4;
            monsterName.Text = "label2";
            // 
            // progressBarCharacter
            // 
            progressBarCharacter.Location = new Point(9, 33);
            progressBarCharacter.Name = "progressBarCharacter";
            progressBarCharacter.Size = new Size(100, 23);
            progressBarCharacter.TabIndex = 5;
            // 
            // progressBarMonster
            // 
            progressBarMonster.Location = new Point(249, 33);
            progressBarMonster.Name = "progressBarMonster";
            progressBarMonster.Size = new Size(100, 23);
            progressBarMonster.TabIndex = 6;
            // 
            // btnBomb
            // 
            btnBomb.Location = new Point(150, 157);
            btnBomb.Name = "btnBomb";
            btnBomb.Size = new Size(75, 23);
            btnBomb.TabIndex = 7;
            btnBomb.Text = "button1";
            btnBomb.UseVisualStyleBackColor = true;
            btnBomb.Click += btnBomb_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(389, 227);
            Controls.Add(btnBomb);
            Controls.Add(progressBarMonster);
            Controls.Add(progressBarCharacter);
            Controls.Add(monsterName);
            Controls.Add(characterName);
            Controls.Add(btnFight);
            Controls.Add(pictureBoxMonster);
            Controls.Add(pictureBoxCharacter);
            Name = "Form2";
            Text = "Form1";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharacter).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMonster).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxCharacter;
        private PictureBox pictureBoxMonster;
        private Button btnFight;
        private Label characterName;
        private Label monsterName;
        private ProgressBar progressBarCharacter;
        private ProgressBar progressBarMonster;
        private Button btnBomb;
    }
}