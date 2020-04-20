using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
        private Thread MarkerInlet;

        private Dictionary<string,Thread> ActiveLSLStream = new Dictionary<string,Thread>();
        private ConcurrentDictionary<string, bool> IsThreadSuspended = new ConcurrentDictionary<string, bool>();

            //Test for var outlet
        liblsl.StreamInfo varOutletInfo;
        liblsl.StreamOutlet varOutlet;


        //Context Dictionary
        private ConcurrentDictionary<string, Dictionary<string, List<double>>> contextDictionary;
      

        private static bool StopAllStreams = false;

        public Form1()
        {
            InitializeComponent();
            contextDictionary = new ConcurrentDictionary<string, Dictionary<string, List<double>>>();
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

            //ChoiceInlet = new Thread(this.ProcessChoiceMarker);
            //ChoiceInlet.Start();

            ChoiceOutlet = new Thread(this.SendChoiceOutlet);
            ChoiceOutlet.Start();

            MarkerInlet = new Thread(ProcessMarkerStreamUpdate);
            MarkerInlet.Start(); 

            varOutletInfo = new liblsl.StreamInfo("Unity.Ink.Var", "Ink.Var", 1, 0, liblsl.channel_format_t.cf_string, "sddssssfsdf");
            varOutlet = new liblsl.StreamOutlet(varOutletInfo);
        }


        private void UpdateUI(string component, string val)
        {
            switch (component)
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
                if (currentChoice != -2)
                {
                    data[0] = currentChoice;
                    currentChoice = -2;
                    outlet.push_sample(data);
                }

            }

        }

        public void ProcessVectorStream()
        {
            liblsl.StreamInfo[] results = liblsl.resolve_stream("type", "Unity.VectorName");
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);

            MethodInvoker inv = delegate
            {
                ConnectButton.Text = "Connected!";
            };
            this.Invoke(inv);

            string[] sample = new string[4];

            while (true)
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

            while (true)
            {
                inlet.pull_sample(sample);

                if (sample[0] == "request")
                {
                    MethodInvoker inv = delegate
                    {
                        ResponceStatus.Text = "Responce Requested!";
                        responceRequested = true;
                    };
                    this.Invoke(inv);
                }
                else if (sample[0] == "recieved")
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


        //Stream config format:
        // 0: Stream Name, 1: Stream Type, 2+: Var names
        //Currently assumed to be doubles

        public void ProcessLSLStream(object info)
        {

            string[] streamConfig = (string[]) info;

            //Build LSL Stream info for pulling samples
            liblsl.StreamInfo[] results = liblsl.resolve_stream(streamConfig[0], streamConfig[1]);
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);

            double[] sample = new double[streamConfig.Length-2];

            //Build our dictionary of VarNames to DataList
            var streamVars = new Dictionary<string, List<double>>();

            //Config and add to dictionary
            for (int i = 2; i < streamConfig.Length; i++)
            {
                var streamV = new List<double>();
                streamVars.Add(streamConfig[i], streamV);
            }

            //And add to the context
            contextDictionary.TryAdd(streamConfig[0], streamVars);

            while (true)
            {
                try
                {
                    if (StopAllStreams || IsThreadSuspended[streamConfig[0]])
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);
                }



                inlet.pull_sample(sample);
                
                //by passing via reference, streamVars should be the correct dict, and any changes to it will be reflected
                //in the context dictionary
                contextDictionary.TryGetValue(streamConfig[0], out streamVars);

                for (int i = 0; i < streamConfig.Length; i++)
                {
                    string sampleVar = streamConfig[i + 2];
                    streamVars?[sampleVar].Add(sample[i]);
                    if (streamVars == null) continue;
                    double avg = streamVars[sampleVar].Average();
                    string varUpdateParam = streamConfig[0] +"_" + sampleVar + ":double:" + avg;
                    VariableUpdate(varUpdateParam); //pushes the variable back to ink
                }

                SendVarUpdate(null, null);
            }
        }


        public void ProcessMarkerStreamUpdate()
        {
            liblsl.StreamInfo[] results = liblsl.resolve_stream("name", "Unity.Ink.Markers");
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);

            string[] sample = new string[1];

            void VarInit()
            {
                lastVarTextbox.Text = "Active!";
            }

            Invoke((MethodInvoker) VarInit);


            while (true)
            {
                inlet.pull_sample(sample);
                if (sample[0].Contains("VARIABLEUPDATE:")) //if we get the update command
                {
                    VariableUpdate(sample[0]);
                }

                if (sample[0].StartsWith("##"))
                {
                    ProcessContextCommand(sample[0]);
                }

                if (sample[0].StartsWith("Tag:") && sample[0].EndsWith("_STOP"))
                {
                    string param = sample[0].Replace("Tag:", "").Replace("_STOP", "");

                    ProcessContextStop(param);
                }
                if (sample[0] == "request")
                {
                    MethodInvoker inv = delegate
                    {
                        ResponceStatus.Text = "Responce Requested!";
                        responceRequested = true;
                    };
                    this.Invoke(inv);
                }
                if (sample[0] == "recieved")
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

        private void ProcessContextStop(string param)
        {
            IsThreadSuspended[param] = true;
        }

        private void VariableUpdate(string sample)
        {
            string[] split = sample.Split(':'); //split the sample into its components
            string name = split[1]; //name of variable
            string type = split[2].Replace("System.", ""); //type, as string
            string stringVal = split[3]; //value, as string

            Enum.TryParse(type, out TypeCode code); //convert the type into a basic type code
            var value = (code == TypeCode.Empty) ? stringVal : Convert.ChangeType(stringVal, code); //and cast to the correct format

            MethodInvoker inv = delegate //And invoke a simple method to update or add
            {
                bool newVar = true;
                foreach (InkVar v in inkVarBindingSource)
                {
                    //if we have this entry but it needs updating
                    if (!v.VarName.Equals(name)) continue;
                    newVar = false;
                    v.SetValue(value);
                }

                if (newVar)
                {
                    inkVarBindingSource.Add(new InkVar(name, value));
                }

                lastVarTextbox.Text = sample;
                varGridView.Refresh();
            };
            try
            {
                Invoke(inv); //And make it so!
#pragma warning disable CS0168 // Variable is declared but never used
            } catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
            {

            }
        }

        private void ProcessContextCommand(string command)
        {
            command = command.TrimStart('#');
            var split = command.Split('_');

            if (split[split.Length - 1].Equals("START"))
            {
                string[] parameters = new string[split.Length - 1];
                //Copy N-1 elements into the parameter array
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = split[i]; 
                }
                SetupLSLStream(parameters);
            }


        }

        private void SetupLSLStream(object param)
        {
            string name = ((string[])param)[0];
            IsThreadSuspended.TryAdd(name, false);

            var stream = new Thread(ProcessLSLStream);
            stream.Start(param);

            //add thread to dictionary
            ActiveLSLStream.Add(name,stream);
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
            Environment.Exit(0);
        }

        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            if (VectorStreamInlet != null) VectorStreamInlet.Abort();
            if (ChoiceInlet != null) ChoiceInlet.Abort();
            if (ChoiceOutlet != null) ChoiceOutlet.Abort();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        //Send a var update message to unity
        private void SendVarUpdate(object sender, EventArgs e)
        {
            string varState = ";";
            foreach (InkVar entry in inkVarBindingSource)
            {
                if (entry.NewValue != null && !entry.NewValue.Equals(entry.CurrentValue))
                    varState += entry.VarName + ":" + entry.NewValue + ":" + entry.NewValue.GetType() + ";";
            }

            if (varState.Equals(";")) return;
            string[] data = { varState };

            varOutlet.push_sample(data);
        }

        private void streamNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void Advance_Click(object sender, EventArgs e)
        {
            currentChoice = -1;
            choiceEventHandler.Set();
        }
    }
}
