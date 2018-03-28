using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSL;

namespace Director
{
    public partial class Form1 : Form
    {

        private bool attemptingConnection = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (attemptingConnection) return;
            attemptingConnection = true;
            ConnectButton.Text = "Attempting to connect!";
            Thread inputStream = new Thread(this.ProcessStream);
            inputStream.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void UpdateUI(string component, string val)
        {
            switch(component)
            {
                case "ConnectButton":
                    ConnectButton.Text = val;
                    break;

                case "CurrenSampleText":
                    CurrentSampleText.Text = val;
                    break;

                default: break;
            }
        }


        public void ProcessStream()
        {
            liblsl.StreamInfo[] results = liblsl.resolve_stream("type" , "Unity.VectorName");
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);

            MethodInvoker inv = delegate
            {
                ConnectButton.Text = "Connected!";
            };
            this.Invoke(inv);

            string[] sample = new string[4];

            while(true)
            {
                inlet.pull_sample(sample);
                inv = delegate
                {
                    CurrentSampleText.Text = sample[0] + ", at: (" + sample[1] + "," + sample[2] + "," + sample[3] + ")";
                };
                this.Invoke(inv);
            }


        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
