namespace TheBindingOfIsaac
{
    partial class Form3
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
            pictureStone = new PictureBox();
            pictureItem = new PictureBox();
            btnYes = new Button();
            btnNo = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureStone).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureItem).BeginInit();
            SuspendLayout();
            // 
            // pictureStone
            // 
            pictureStone.Location = new Point(47, 154);
            pictureStone.Name = "pictureStone";
            pictureStone.Size = new Size(146, 130);
            pictureStone.TabIndex = 0;
            pictureStone.TabStop = false;
            // 
            // pictureItem
            // 
            pictureItem.Location = new Point(47, 18);
            pictureItem.Name = "pictureItem";
            pictureItem.Size = new Size(146, 130);
            pictureItem.TabIndex = 2;
            pictureItem.TabStop = false;
            // 
            // btnYes
            // 
            btnYes.Location = new Point(47, 290);
            btnYes.Name = "btnYes";
            btnYes.Size = new Size(146, 23);
            btnYes.TabIndex = 3;
            btnYes.Text = "TAKE";
            btnYes.UseVisualStyleBackColor = true;
            btnYes.Click += btnYes_Click;
            // 
            // btnNo
            // 
            btnNo.Location = new Point(47, 319);
            btnNo.Name = "btnNo";
            btnNo.Size = new Size(146, 23);
            btnNo.TabIndex = 4;
            btnNo.Text = "LEAVE";
            btnNo.UseVisualStyleBackColor = true;
            btnNo.Click += btnNo_Click;
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(242, 344);
            Controls.Add(btnNo);
            Controls.Add(btnYes);
            Controls.Add(pictureItem);
            Controls.Add(pictureStone);
            Name = "Form3";
            Text = "Form3";
            ((System.ComponentModel.ISupportInitialize)pictureStone).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureItem).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureStone;
        private PictureBox pictureItem;
        private Button btnYes;
        private Button btnNo;
    }
}