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
        private static EventWaitHandle choiceEventHandler;
        private bool responceRequested;
        Thread VectorStreamInlet;
        Thread ChoiceInlet;
        Thread ChoiceOutlet;

        public Form1()
        {
            InitializeComponent();
            choiceEventHandler = new EventWaitHandle(false, EventResetMode.AutoReset);
            responceRequested = false;
        }


        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (attemptingConnection) return;
            attemptingConnection = true;
            ConnectButton.Text = "Attempting to connect!";

            VectorStreamInlet = new Thread(this.ProcessVectorStream);
            VectorStreamInlet.Start();

            ChoiceInlet = new Thread(this.ProcessChoiceMarker);
            ChoiceInlet.Start();

            ChoiceOutlet = new Thread(this.SendChoiceOutlet);
            ChoiceOutlet.Start();
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
            liblsl.StreamInfo info = new liblsl.StreamInfo("Unity.Ink.Choice", "Ink.Choice", 1, 0, liblsl.channel_format_t.cf_int32, "sddsfsdf");
            liblsl.StreamOutlet outlet = new liblsl.StreamOutlet(info);
            int[] data = new int[1];
            while (true)
            {
                choiceEventHandler.WaitOne();
                if (currentChoice != -1)
                {
                    data[0] = currentChoice;
                    currentChoice = -1;
                    outlet.push_sample(data);
                }
               
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

        public void ProcessChoiceMarker()
        {
            liblsl.StreamInfo[] results = liblsl.resolve_stream("name", "Unity.Ink.Choice.Request");
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
                        responceRequested = true;
                    };
                    this.Invoke(inv);
                } else if (sample[0] == "recieved")
                {
                    MethodInvoker inv = delegate
                    {
                        ResponceStatus.Text = "No Responce Requested";
                        responceRequested = false;
                    };
                    this.Invoke(inv);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ChoiceButton1_Click(object sender, EventArgs e)
        {
            if (!responceRequested) return;
            currentChoice = 0;
            choiceEventHandler.Set();          
        }
        private void ChoiceButton2_Click(object sender, EventArgs e)
        {
            if (!responceRequested) return;
            currentChoice = 1;
            choiceEventHandler.Set();
        }
        private void ChoiceButton3_Click(object sender, EventArgs e)
        {
            if (!responceRequested) return;
            currentChoice = 2;
            choiceEventHandler.Set();
        }

        private void ExitButton_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            if (VectorStreamInlet != null) VectorStreamInlet.Abort();
            if (ChoiceInlet != null)       ChoiceInlet.Abort();
            if (ChoiceOutlet != null)      ChoiceOutlet.Abort();
        }
    }
}
