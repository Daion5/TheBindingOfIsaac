namespace TheBindingOfIsaac
{
    partial class Form4
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form4));
            pictureBox1 = new PictureBox();
            pictureBoxitem = new PictureBox();
            buttonBuy = new Button();
            buttonLeave = new Button();
            pictureBoxChest = new PictureBox();
            btnOpen = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxitem).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxChest).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.shop1;
            pictureBox1.Location = new Point(12, 12);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(776, 426);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBoxitem
            // 
            pictureBoxitem.Location = new Point(348, 175);
            pictureBoxitem.Name = "pictureBoxitem";
            pictureBoxitem.Size = new Size(102, 86);
            pictureBoxitem.TabIndex = 1;
            pictureBoxitem.TabStop = false;
            pictureBoxitem.Click += pictureBoxitem_Click;
            // 
            // buttonBuy
            // 
            buttonBuy.Location = new Point(348, 267);
            buttonBuy.Name = "buttonBuy";
            buttonBuy.Size = new Size(102, 23);
            buttonBuy.TabIndex = 3;
            buttonBuy.Text = "BUY - 10¢";
            buttonBuy.UseVisualStyleBackColor = true;
            buttonBuy.Click += buttonBuy_Click;
            // 
            // buttonLeave
            // 
            buttonLeave.Location = new Point(348, 301);
            buttonLeave.Name = "buttonLeave";
            buttonLeave.Size = new Size(102, 23);
            buttonLeave.TabIndex = 4;
            buttonLeave.Text = "LEAVE";
            buttonLeave.UseVisualStyleBackColor = true;
            buttonLeave.Click += buttonLeave_Click;
            // 
            // pictureBoxChest
            // 
            pictureBoxChest.Image = (Image)resources.GetObject("pictureBoxChest.Image");
            pictureBoxChest.Location = new Point(612, 266);
            pictureBoxChest.Name = "pictureBoxChest";
            pictureBoxChest.Size = new Size(75, 58);
            pictureBoxChest.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxChest.TabIndex = 5;
            pictureBoxChest.TabStop = false;
            // 
            // btnOpen
            // 
            btnOpen.Location = new Point(612, 330);
            btnOpen.Name = "btnOpen";
            btnOpen.Size = new Size(75, 23);
            btnOpen.TabIndex = 6;
            btnOpen.Text = "OPEN";
            btnOpen.UseVisualStyleBackColor = true;
            btnOpen.Click += btnOpen_Click;
            // 
            // Form4
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnOpen);
            Controls.Add(pictureBoxChest);
            Controls.Add(buttonLeave);
            Controls.Add(buttonBuy);
            Controls.Add(pictureBoxitem);
            Controls.Add(pictureBox1);
            Name = "Form4";
            Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxitem).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxChest).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBoxitem;
        private Button buttonBuy;
        private Button buttonLeave;
        private PictureBox pictureBoxChest;
        private Button btnOpen;
    }
}