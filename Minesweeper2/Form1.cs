namespace Minesweeper2
{
    public partial class Form1 : Form
    {
        bool isForm2Opened = false;
        bool isForm3Opened = false;
        Form3 form3;
        public Form1()
        {
            InitializeComponent();
            this.MinimumSize = new Size(350, 500);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Username Text Control
            if (textBox1.Text.Length < 3 || textBox1.Text.Length > 16)
            {
                warningText("Kullanýcý adý en az 3, en fazla 16 haneli olabilir");
                button1EnabledControl(false);
            }
            else
            {
                warningText("");
                button1EnabledControl(true);
            }
        }
        private void warningText(string text)
        {
            // Writing warning text in label3 (under the start button)
            label3.Text = text;
        }
        private void button1EnabledControl(bool isEnable)
        {
            // start button enable changing
            button1.Enabled = isEnable;
            if (this.isForm2Opened && isEnable)
            {
                button1.Enabled = false;
                this.form2OpenWarning();
            }
        }

        private void form2OpenWarning()
        {
            warningText("Oyun zaten açýk!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(textBox1.Text, this);
            form2.Show();
            this.isForm2Opened = true;
            this.button1EnabledControl(true);
        }
        public void setIsForm2Opened(bool status, object sender, EventArgs e)
        {
            this.isForm2Opened = status;
            textBox1_TextChanged(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.isForm3Opened)
            {
                this.form3.Dispose();
                this.isForm3Opened = false;
            }
            this.form3 = new Form3();
            this.form3.Show();
            this.isForm3Opened = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://www.github.com/29apo29/minesweeper",
                UseShellExecute = true
            });
        }
    }
}
 