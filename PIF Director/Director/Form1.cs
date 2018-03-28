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
        private int currentChoice = -1;

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

            Thread inputVectorStream = new Thread(this.ProcessVectorStream);
            inputVectorStream.Start();

            Thread inputChoiceStream = new Thread(this.ProcessChoiceStream);
            inputChoiceStream.Start();

            Thread choiceOutlet = new Thread(this.SendChoiceOutlet);
            choiceOutlet.Start();
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

        public void SendChoiceOutlet()
        {
            liblsl.StreamInfo info = new liblsl.StreamInfo("Unity.Ink.Choice", "Ink.Choice", 1, 100, liblsl.channel_format_t.cf_int32, "sddsfsdf");
            liblsl.StreamOutlet outlet = new liblsl.StreamOutlet(info);
            int[] data = new int[1];
            while (true)
            {
                if (currentChoice != -1)
                {
                    data[0] = currentChoice;
                    currentChoice = -1;
                    outlet.push_sample(data);
                }
                System.Threading.Thread.Sleep(10);
            }

        }

        public void ProcessVectorStream()
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

        public void ProcessChoiceStream()
        {
            liblsl.StreamInfo[] results = liblsl.resolve_stream("type", "Choice.Request");
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);

            string[] sample = new string[1];

            while(true)
            {
                inlet.pull_sample(sample);

                if(sample[0] == "request")
                {
                    MethodInvoker inv = delegate
                    {
                        ResponceStatus.Text = "Responce Requested!";
                    };
                    this.Invoke(inv);
                } else if (sample[0] == "recieved")
                {
                    MethodInvoker inv = delegate
                    {
                        ResponceStatus.Text = "No Responce Requested";
                    };
                    this.Invoke(inv);
                }
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentChoice = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            currentChoice = 1;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            currentChoice = 2;
        }

    }
}
