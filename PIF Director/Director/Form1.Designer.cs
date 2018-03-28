namespace Director
{
    partial class Form1
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
            this.streamNameText = new System.Windows.Forms.TextBox();
            this.streamNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.streamTypeText = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CurrentSampleText = new System.Windows.Forms.Label();
            this.ResponceStatus = new System.Windows.Forms.Label();
            this.Choice1Button = new System.Windows.Forms.Button();
            this.Choice2Button = new System.Windows.Forms.Button();
            this.Choice3Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // streamNameText
            // 
            this.streamNameText.Location = new System.Drawing.Point(161, 10);
            this.streamNameText.Name = "streamNameText";
            this.streamNameText.Size = new System.Drawing.Size(298, 31);
            this.streamNameText.TabIndex = 0;
            this.streamNameText.Text = "Unity.Gaze.VectorName";
            this.streamNameText.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // streamNameLabel
            // 
            this.streamNameLabel.AutoSize = true;
            this.streamNameLabel.Location = new System.Drawing.Point(13, 13);
            this.streamNameLabel.Name = "streamNameLabel";
            this.streamNameLabel.Size = new System.Drawing.Size(142, 25);
            this.streamNameLabel.TabIndex = 1;
            this.streamNameLabel.Text = "Stream Name";
            this.streamNameLabel.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Stream Type";
            // 
            // streamTypeText
            // 
            this.streamTypeText.Location = new System.Drawing.Point(161, 41);
            this.streamTypeText.Name = "streamTypeText";
            this.streamTypeText.Size = new System.Drawing.Size(298, 31);
            this.streamTypeText.TabIndex = 2;
            this.streamTypeText.Text = "Unity.VectorName";
            this.streamTypeText.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(18, 89);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(137, 68);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect to Stream";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "Current Sample:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // CurrentSampleText
            // 
            this.CurrentSampleText.AutoSize = true;
            this.CurrentSampleText.Location = new System.Drawing.Point(187, 207);
            this.CurrentSampleText.Name = "CurrentSampleText";
            this.CurrentSampleText.Size = new System.Drawing.Size(0, 25);
            this.CurrentSampleText.TabIndex = 6;
            this.CurrentSampleText.Click += new System.EventHandler(this.label3_Click);
            // 
            // ResponceStatus
            // 
            this.ResponceStatus.AutoSize = true;
            this.ResponceStatus.Location = new System.Drawing.Point(18, 236);
            this.ResponceStatus.Name = "ResponceStatus";
            this.ResponceStatus.Size = new System.Drawing.Size(252, 25);
            this.ResponceStatus.TabIndex = 7;
            this.ResponceStatus.Text = "No Responce Requested";
            // 
            // Choice1Button
            // 
            this.Choice1Button.Location = new System.Drawing.Point(23, 303);
            this.Choice1Button.Name = "Choice1Button";
            this.Choice1Button.Size = new System.Drawing.Size(132, 88);
            this.Choice1Button.TabIndex = 8;
            this.Choice1Button.Text = "Choose 1";
            this.Choice1Button.UseVisualStyleBackColor = true;
            this.Choice1Button.Click += new System.EventHandler(this.button1_Click);
            // 
            // Choice2Button
            // 
            this.Choice2Button.Location = new System.Drawing.Point(161, 303);
            this.Choice2Button.Name = "Choice2Button";
            this.Choice2Button.Size = new System.Drawing.Size(132, 88);
            this.Choice2Button.TabIndex = 9;
            this.Choice2Button.Text = "Choose 2";
            this.Choice2Button.UseVisualStyleBackColor = true;
            this.Choice2Button.Click += new System.EventHandler(this.button2_Click);
            // 
            // Choice3Button
            // 
            this.Choice3Button.Location = new System.Drawing.Point(299, 303);
            this.Choice3Button.Name = "Choice3Button";
            this.Choice3Button.Size = new System.Drawing.Size(132, 88);
            this.Choice3Button.TabIndex = 10;
            this.Choice3Button.Text = "Choose 3";
            this.Choice3Button.UseVisualStyleBackColor = true;
            this.Choice3Button.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1026, 825);
            this.Controls.Add(this.Choice3Button);
            this.Controls.Add(this.Choice2Button);
            this.Controls.Add(this.Choice1Button);
            this.Controls.Add(this.ResponceStatus);
            this.Controls.Add(this.CurrentSampleText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.streamTypeText);
            this.Controls.Add(this.streamNameLabel);
            this.Controls.Add(this.streamNameText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox streamNameText;
        private System.Windows.Forms.Label streamNameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox streamTypeText;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CurrentSampleText;
        private System.Windows.Forms.Label ResponceStatus;
        private System.Windows.Forms.Button Choice1Button;
        private System.Windows.Forms.Button Choice2Button;
        private System.Windows.Forms.Button Choice3Button;
    }
}

