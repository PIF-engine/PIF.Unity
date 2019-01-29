﻿namespace Director
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
            this.components = new System.ComponentModel.Container();
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
            this.exitButton = new System.Windows.Forms.Button();
            this.varGridView = new System.Windows.Forms.DataGridView();
            this.varNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newValueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inkVarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lastVarTextbox = new System.Windows.Forms.TextBox();
            this.varUpdateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.varGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inkVarBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // streamNameText
            // 
            this.streamNameText.Location = new System.Drawing.Point(161, 10);
            this.streamNameText.Name = "streamNameText";
            this.streamNameText.Size = new System.Drawing.Size(298, 31);
            this.streamNameText.TabIndex = 0;
            this.streamNameText.Text = "Unity.Gaze.VectorName";
            // 
            // streamNameLabel
            // 
            this.streamNameLabel.AutoSize = true;
            this.streamNameLabel.Location = new System.Drawing.Point(13, 13);
            this.streamNameLabel.Name = "streamNameLabel";
            this.streamNameLabel.Size = new System.Drawing.Size(142, 25);
            this.streamNameLabel.TabIndex = 1;
            this.streamNameLabel.Text = "Stream Name";
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
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(18, 89);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(275, 115);
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
            // 
            // CurrentSampleText
            // 
            this.CurrentSampleText.AutoSize = true;
            this.CurrentSampleText.Location = new System.Drawing.Point(187, 207);
            this.CurrentSampleText.Name = "CurrentSampleText";
            this.CurrentSampleText.Size = new System.Drawing.Size(0, 25);
            this.CurrentSampleText.TabIndex = 6;
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
            this.Choice1Button.Click += new System.EventHandler(this.ChoiceButton1_Click);
            // 
            // Choice2Button
            // 
            this.Choice2Button.Location = new System.Drawing.Point(161, 303);
            this.Choice2Button.Name = "Choice2Button";
            this.Choice2Button.Size = new System.Drawing.Size(132, 88);
            this.Choice2Button.TabIndex = 9;
            this.Choice2Button.Text = "Choose 2";
            this.Choice2Button.UseVisualStyleBackColor = true;
            this.Choice2Button.Click += new System.EventHandler(this.ChoiceButton2_Click);
            // 
            // Choice3Button
            // 
            this.Choice3Button.Location = new System.Drawing.Point(299, 303);
            this.Choice3Button.Name = "Choice3Button";
            this.Choice3Button.Size = new System.Drawing.Size(132, 88);
            this.Choice3Button.TabIndex = 10;
            this.Choice3Button.Text = "Choose 3";
            this.Choice3Button.UseVisualStyleBackColor = true;
            this.Choice3Button.Click += new System.EventHandler(this.ChoiceButton3_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(13, 688);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(280, 125);
            this.exitButton.TabIndex = 11;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click_1);
            // 
            // varGridView
            // 
            this.varGridView.AutoGenerateColumns = false;
            this.varGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.varGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.varNameDataGridViewTextBoxColumn,
            this.currentValueDataGridViewTextBoxColumn,
            this.newValueDataGridViewTextBoxColumn});
            this.varGridView.DataSource = this.inkVarBindingSource;
            this.varGridView.Location = new System.Drawing.Point(564, 13);
            this.varGridView.Name = "varGridView";
            this.varGridView.RowTemplate.Height = 33;
            this.varGridView.Size = new System.Drawing.Size(948, 590);
            this.varGridView.TabIndex = 12;
            // 
            // varNameDataGridViewTextBoxColumn
            // 
            this.varNameDataGridViewTextBoxColumn.DataPropertyName = "VarName";
            this.varNameDataGridViewTextBoxColumn.HeaderText = "VarName";
            this.varNameDataGridViewTextBoxColumn.Name = "varNameDataGridViewTextBoxColumn";
            this.varNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // currentValueDataGridViewTextBoxColumn
            // 
            this.currentValueDataGridViewTextBoxColumn.DataPropertyName = "CurrentValue";
            this.currentValueDataGridViewTextBoxColumn.HeaderText = "CurrentValue";
            this.currentValueDataGridViewTextBoxColumn.Name = "currentValueDataGridViewTextBoxColumn";
            this.currentValueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // newValueDataGridViewTextBoxColumn
            // 
            this.newValueDataGridViewTextBoxColumn.DataPropertyName = "NewValue";
            this.newValueDataGridViewTextBoxColumn.HeaderText = "NewValue";
            this.newValueDataGridViewTextBoxColumn.Name = "newValueDataGridViewTextBoxColumn";
            // 
            // inkVarBindingSource
            // 
            this.inkVarBindingSource.DataSource = typeof(Director.InkVar);
            // 
            // lastVarTextbox
            // 
            this.lastVarTextbox.Location = new System.Drawing.Point(564, 609);
            this.lastVarTextbox.Name = "lastVarTextbox";
            this.lastVarTextbox.Size = new System.Drawing.Size(100, 31);
            this.lastVarTextbox.TabIndex = 13;
            this.lastVarTextbox.Text = "None";
            // 
            // varUpdateButton
            // 
            this.varUpdateButton.Location = new System.Drawing.Point(442, 567);
            this.varUpdateButton.Name = "varUpdateButton";
            this.varUpdateButton.Size = new System.Drawing.Size(116, 73);
            this.varUpdateButton.TabIndex = 14;
            this.varUpdateButton.Text = "Send Var Update";
            this.varUpdateButton.UseVisualStyleBackColor = true;
            this.varUpdateButton.Click += new System.EventHandler(this.SendVarUpdate);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2058, 1133);
            this.Controls.Add(this.varUpdateButton);
            this.Controls.Add(this.lastVarTextbox);
            this.Controls.Add(this.varGridView);
            this.Controls.Add(this.exitButton);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClose);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.varGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inkVarBindingSource)).EndInit();
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
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.BindingSource inkVarBindingSource;
        private System.Windows.Forms.DataGridView varGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn varNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn newValueDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox lastVarTextbox;
        private System.Windows.Forms.Button varUpdateButton;
    }
}

