using System;
using System.Drawing;
using System.Windows.Forms;

namespace TheBindingOfIsaac
{
    public class CustomMessageBox : Form
    {
        private Label messageLabel;
        private PictureBox pictureBox;
        private Button okButton;

        public CustomMessageBox(string message, string imagePath)
        {
            InitializeComponents(message, imagePath);
        }

        private void InitializeComponents(string message, string imagePath)
        {
            this.ClientSize = new Size(250, 120);
            this.Text = "You Found Something!";
            this.messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.messageLabel.Location = new Point(0, 0);

            this.pictureBox = new PictureBox
            {
                Image = Image.FromFile(imagePath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(40, 40)
            };

            this.okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(75, 23)
            };
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.okButton);

            this.messageLabel.Location = new Point((this.ClientSize.Width - this.messageLabel.Width) / 2, 10);

            this.pictureBox.Location = new Point((this.ClientSize.Width - 40) / 2, this.messageLabel.Bottom + 10);
            this.okButton.Location = new Point((this.ClientSize.Width - 75) / 2, this.pictureBox.Bottom + 10);

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;

            this.AcceptButton = this.okButton;
        }

        public static DialogResult Show(string message, string imagePath)
        {
            using (var box = new CustomMessageBox(message, imagePath))
            {
                return box.ShowDialog();
            }
        }
    }
}
