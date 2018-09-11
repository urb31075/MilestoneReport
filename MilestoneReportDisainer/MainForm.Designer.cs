namespace MilestoneReportDisainer
{
    partial class MainForm
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
            this.ExportButton = new System.Windows.Forms.Button();
            this.ReportButton = new System.Windows.Forms.Button();
            this.DesignModeCheckBox = new System.Windows.Forms.CheckBox();
            this.ReportTemplatePathTextBox = new System.Windows.Forms.TextBox();
            this.ReportFromDbButto = new System.Windows.Forms.Button();
            this.ReadFromBlobButton = new System.Windows.Forms.Button();
            this.SaveToBlobButton = new System.Windows.Forms.Button();
            this.InfoListBox = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.WorkProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(171, 46);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(59, 23);
            this.ExportButton.TabIndex = 8;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButtonClick);
            // 
            // ReportButton
            // 
            this.ReportButton.Location = new System.Drawing.Point(105, 46);
            this.ReportButton.Name = "ReportButton";
            this.ReportButton.Size = new System.Drawing.Size(60, 23);
            this.ReportButton.TabIndex = 7;
            this.ReportButton.Text = "Report";
            this.ReportButton.UseVisualStyleBackColor = true;
            this.ReportButton.Click += new System.EventHandler(this.ReportButtonClick);
            // 
            // DesignModeCheckBox
            // 
            this.DesignModeCheckBox.AutoSize = true;
            this.DesignModeCheckBox.Checked = true;
            this.DesignModeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DesignModeCheckBox.Location = new System.Drawing.Point(12, 48);
            this.DesignModeCheckBox.Name = "DesignModeCheckBox";
            this.DesignModeCheckBox.Size = new System.Drawing.Size(86, 17);
            this.DesignModeCheckBox.TabIndex = 6;
            this.DesignModeCheckBox.Text = "DesignMode";
            this.DesignModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // ReportTemplatePathTextBox
            // 
            this.ReportTemplatePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReportTemplatePathTextBox.Location = new System.Drawing.Point(12, 12);
            this.ReportTemplatePathTextBox.Name = "ReportTemplatePathTextBox";
            this.ReportTemplatePathTextBox.Size = new System.Drawing.Size(1063, 20);
            this.ReportTemplatePathTextBox.TabIndex = 9;
            this.ReportTemplatePathTextBox.Text = "C:\\PROJECTS\\MilestoneReport\\MilestoneReportDisainer\\ReportTemplate\\MilestoneRepor" +
    "t.frx";
            // 
            // ReportFromDbButto
            // 
            this.ReportFromDbButto.Location = new System.Drawing.Point(447, 48);
            this.ReportFromDbButto.Name = "ReportFromDbButto";
            this.ReportFromDbButto.Size = new System.Drawing.Size(118, 23);
            this.ReportFromDbButto.TabIndex = 12;
            this.ReportFromDbButto.Text = "Report from DB";
            this.ReportFromDbButto.UseVisualStyleBackColor = true;
            this.ReportFromDbButto.Click += new System.EventHandler(this.ReportFromDbButtoClick);
            // 
            // ReadFromBlobButton
            // 
            this.ReadFromBlobButton.Location = new System.Drawing.Point(337, 48);
            this.ReadFromBlobButton.Name = "ReadFromBlobButton";
            this.ReadFromBlobButton.Size = new System.Drawing.Size(104, 23);
            this.ReadFromBlobButton.TabIndex = 11;
            this.ReadFromBlobButton.Text = "Read from Blob";
            this.ReadFromBlobButton.UseVisualStyleBackColor = true;
            this.ReadFromBlobButton.Click += new System.EventHandler(this.ReadFromBlobButtonClick);
            // 
            // SaveToBlobButton
            // 
            this.SaveToBlobButton.Location = new System.Drawing.Point(236, 48);
            this.SaveToBlobButton.Name = "SaveToBlobButton";
            this.SaveToBlobButton.Size = new System.Drawing.Size(94, 23);
            this.SaveToBlobButton.TabIndex = 10;
            this.SaveToBlobButton.Text = "Save to Blob";
            this.SaveToBlobButton.UseVisualStyleBackColor = true;
            this.SaveToBlobButton.Click += new System.EventHandler(this.SaveToBlobButtonClick);
            // 
            // InfoListBox
            // 
            this.InfoListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoListBox.FormattingEnabled = true;
            this.InfoListBox.Location = new System.Drawing.Point(12, 86);
            this.InfoListBox.Name = "InfoListBox";
            this.InfoListBox.Size = new System.Drawing.Size(1063, 394);
            this.InfoListBox.TabIndex = 13;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.WorkProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 507);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1087, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // WorkProgressBar
            // 
            this.WorkProgressBar.Name = "WorkProgressBar";
            this.WorkProgressBar.Size = new System.Drawing.Size(200, 16);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1087, 529);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.InfoListBox);
            this.Controls.Add(this.ReportFromDbButto);
            this.Controls.Add(this.ReadFromBlobButton);
            this.Controls.Add(this.SaveToBlobButton);
            this.Controls.Add(this.ReportTemplatePathTextBox);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.ReportButton);
            this.Controls.Add(this.DesignModeCheckBox);
            this.Name = "MainForm";
            this.Text = "MilestoneReportDisainer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ReportButton;
        private System.Windows.Forms.CheckBox DesignModeCheckBox;
        private System.Windows.Forms.TextBox ReportTemplatePathTextBox;
        private System.Windows.Forms.Button ReportFromDbButto;
        private System.Windows.Forms.Button ReadFromBlobButton;
        private System.Windows.Forms.Button SaveToBlobButton;
        private System.Windows.Forms.ListBox InfoListBox;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar WorkProgressBar;
        private System.Windows.Forms.Button button1;
    }
}

