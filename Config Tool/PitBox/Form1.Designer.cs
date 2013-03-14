namespace PitBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.ribbonTab1 = new C1.Win.C1Ribbon.RibbonTab();
            this.ribbonGroup1 = new C1.Win.C1Ribbon.RibbonGroup();
            this.c1Ribbon1 = new C1.Win.C1Ribbon.C1Ribbon();
            this.ribbonApplicationMenu1 = new C1.Win.C1Ribbon.RibbonApplicationMenu();
            this.ribbonConfigToolBar1 = new C1.Win.C1Ribbon.RibbonConfigToolBar();
            this.ribbonQat1 = new C1.Win.C1Ribbon.RibbonQat();
            this.ribbonTab2 = new C1.Win.C1Ribbon.RibbonTab();
            this.ribbonGroup2 = new C1.Win.C1Ribbon.RibbonGroup();
            this.ribbonButton2 = new C1.Win.C1Ribbon.RibbonButton();
            this.ribbonButton1 = new C1.Win.C1Ribbon.RibbonButton();
            this.serialPortCombo = new C1.Win.C1Ribbon.RibbonComboBox();
            this.ribbonGroup3 = new C1.Win.C1Ribbon.RibbonGroup();
            this.ribbonButton3 = new C1.Win.C1Ribbon.RibbonButton();
            this.c1CommandHolder1 = new C1.Win.C1Command.C1CommandHolder();
            this.c1StatusBar1 = new C1.Win.C1Ribbon.C1StatusBar();
            this.lblCommStatus = new C1.Win.C1Ribbon.RibbonLabel();
            this.ribbonSeparator1 = new C1.Win.C1Ribbon.RibbonSeparator();
            this.lblHwVersion = new C1.Win.C1Ribbon.RibbonLabel();
            this.lblVersion = new C1.Win.C1Ribbon.RibbonLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRed = new System.Windows.Forms.TextBox();
            this.txtDelay = new System.Windows.Forms.TextBox();
            this.txtGreen = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblConnectionStatus = new C1.Win.C1Ribbon.RibbonLabel();
            this.ribbonSeparator2 = new C1.Win.C1Ribbon.RibbonSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.c1Ribbon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1CommandHolder1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1StatusBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(245, 140);
            this.textBox1.TabIndex = 2;
            // 
            // ribbonTab1
            // 
            this.ribbonTab1.Name = "ribbonTab1";
            this.ribbonTab1.Text = "Tab";
            // 
            // ribbonGroup1
            // 
            this.ribbonGroup1.Name = "ribbonGroup1";
            this.ribbonGroup1.Text = "Group";
            // 
            // c1Ribbon1
            // 
            this.c1Ribbon1.ApplicationMenuHolder = this.ribbonApplicationMenu1;
            this.c1Ribbon1.ConfigToolBarHolder = this.ribbonConfigToolBar1;
            this.c1Ribbon1.Location = new System.Drawing.Point(0, 0);
            this.c1Ribbon1.Name = "c1Ribbon1";
            this.c1Ribbon1.QatHolder = this.ribbonQat1;
            this.c1Ribbon1.Size = new System.Drawing.Size(413, 150);
            this.c1Ribbon1.Tabs.Add(this.ribbonTab2);
            // 
            // ribbonApplicationMenu1
            // 
            this.ribbonApplicationMenu1.Enabled = false;
            this.ribbonApplicationMenu1.Name = "ribbonApplicationMenu1";
            // 
            // ribbonConfigToolBar1
            // 
            this.ribbonConfigToolBar1.Name = "ribbonConfigToolBar1";
            // 
            // ribbonQat1
            // 
            this.ribbonQat1.Name = "ribbonQat1";
            // 
            // ribbonTab2
            // 
            this.ribbonTab2.Groups.Add(this.ribbonGroup2);
            this.ribbonTab2.Groups.Add(this.ribbonGroup3);
            this.ribbonTab2.Name = "ribbonTab2";
            this.ribbonTab2.Text = "Main";
            // 
            // ribbonGroup2
            // 
            this.ribbonGroup2.Items.Add(this.ribbonButton2);
            this.ribbonGroup2.Items.Add(this.ribbonButton1);
            this.ribbonGroup2.Items.Add(this.serialPortCombo);
            this.ribbonGroup2.Name = "ribbonGroup2";
            this.ribbonGroup2.Text = "Configuration";
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.Description = "Read configuration from EEPROM";
            this.ribbonButton2.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.LargeImage")));
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton2.SmallImage")));
            this.ribbonButton2.Text = "Read";
            this.ribbonButton2.ToolTip = "Read the confituration from the EEPROM";
            this.ribbonButton2.Click += new System.EventHandler(this.ribbonButton2_Click);
            // 
            // ribbonButton1
            // 
            this.ribbonButton1.Description = "Save settings to EEPROM";
            this.ribbonButton1.LargeImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.LargeImage")));
            this.ribbonButton1.Name = "ribbonButton1";
            this.ribbonButton1.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton1.SmallImage")));
            this.ribbonButton1.Text = "Upload";
            this.ribbonButton1.ToolTip = "Save current configuration to EEPROM";
            this.ribbonButton1.Click += new System.EventHandler(this.ribbonButton1_Click);
            // 
            // serialPortCombo
            // 
            this.serialPortCombo.Label = "Serial Port";
            this.serialPortCombo.Name = "serialPortCombo";
            this.serialPortCombo.SelectedIndexChanged += new System.EventHandler(this.serialPortCombo_SelectedIndexChanged);
            // 
            // ribbonGroup3
            // 
            this.ribbonGroup3.Items.Add(this.ribbonButton3);
            this.ribbonGroup3.Name = "ribbonGroup3";
            this.ribbonGroup3.Text = "Firmware";
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.LargeImage = global::PitBox.Properties.Resources.chip;
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.SmallImage = ((System.Drawing.Image)(resources.GetObject("ribbonButton3.SmallImage")));
            this.ribbonButton3.Text = "Flash";
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            // 
            // c1CommandHolder1
            // 
            this.c1CommandHolder1.Owner = this;
            // 
            // c1StatusBar1
            // 
            this.c1StatusBar1.LeftPaneItems.Add(this.lblCommStatus);
            this.c1StatusBar1.LeftPaneItems.Add(this.ribbonSeparator1);
            this.c1StatusBar1.LeftPaneItems.Add(this.lblConnectionStatus);
            this.c1StatusBar1.LeftPaneItems.Add(this.ribbonSeparator2);
            this.c1StatusBar1.LeftPaneItems.Add(this.lblHwVersion);
            this.c1StatusBar1.Location = new System.Drawing.Point(0, 349);
            this.c1StatusBar1.Name = "c1StatusBar1";
            this.c1StatusBar1.RightPaneItems.Add(this.lblVersion);
            this.c1StatusBar1.Size = new System.Drawing.Size(413, 23);
            // 
            // lblCommStatus
            // 
            this.lblCommStatus.Description = "The status of the COM port";
            this.lblCommStatus.Name = "lblCommStatus";
            this.lblCommStatus.Text = "Closed";
            // 
            // ribbonSeparator1
            // 
            this.ribbonSeparator1.Name = "ribbonSeparator1";
            // 
            // lblHwVersion
            // 
            this.lblHwVersion.Name = "lblHwVersion";
            this.lblHwVersion.Text = "Firmware Version:";
            // 
            // lblVersion
            // 
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Text = "Version:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRed);
            this.groupBox1.Location = new System.Drawing.Point(14, 173);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(128, 51);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Red On Time (ms)";
            // 
            // txtRed
            // 
            this.txtRed.Location = new System.Drawing.Point(7, 19);
            this.txtRed.Name = "txtRed";
            this.txtRed.Size = new System.Drawing.Size(52, 20);
            this.txtRed.TabIndex = 0;
            this.txtRed.Text = "1000";
            this.txtRed.Validating += new System.ComponentModel.CancelEventHandler(this.time_Validating);
            // 
            // txtDelay
            // 
            this.txtDelay.Location = new System.Drawing.Point(6, 19);
            this.txtDelay.Name = "txtDelay";
            this.txtDelay.Size = new System.Drawing.Size(54, 20);
            this.txtDelay.TabIndex = 2;
            this.txtDelay.Text = "1000";
            this.txtDelay.Validating += new System.ComponentModel.CancelEventHandler(this.time_Validating);
            // 
            // txtGreen
            // 
            this.txtGreen.Location = new System.Drawing.Point(7, 19);
            this.txtGreen.Name = "txtGreen";
            this.txtGreen.Size = new System.Drawing.Size(52, 20);
            this.txtGreen.TabIndex = 1;
            this.txtGreen.Text = "1000";
            this.txtGreen.Validating += new System.ComponentModel.CancelEventHandler(this.time_Validating);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtGreen);
            this.groupBox2.Location = new System.Drawing.Point(13, 230);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(128, 51);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Green On Time (ms)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtDelay);
            this.groupBox3.Location = new System.Drawing.Point(12, 287);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(128, 51);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Red On Time (ms)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox1);
            this.groupBox4.Location = new System.Drawing.Point(148, 173);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(257, 165);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Debug Info";
            // 
            // lblConnectionStatus
            // 
            this.lblConnectionStatus.Name = "lblConnectionStatus";
            this.lblConnectionStatus.Text = "Not Connected";
            // 
            // ribbonSeparator2
            // 
            this.ribbonSeparator2.Name = "ribbonSeparator2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 372);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.c1StatusBar1);
            this.Controls.Add(this.c1Ribbon1);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "Pit Box Configuration";
            this.VisualStyleHolder = C1.Win.C1Ribbon.VisualStyle.Windows7;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.c1Ribbon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1CommandHolder1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.c1StatusBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private C1.Win.C1Ribbon.RibbonTab ribbonTab1;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup1;
        private C1.Win.C1Ribbon.C1Ribbon c1Ribbon1;
        private C1.Win.C1Ribbon.RibbonApplicationMenu ribbonApplicationMenu1;
        private C1.Win.C1Ribbon.RibbonConfigToolBar ribbonConfigToolBar1;
        private C1.Win.C1Ribbon.RibbonQat ribbonQat1;
        private C1.Win.C1Ribbon.RibbonTab ribbonTab2;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup2;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton1;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton2;
        private C1.Win.C1Command.C1CommandHolder c1CommandHolder1;
        private C1.Win.C1Ribbon.C1StatusBar c1StatusBar1;
        private C1.Win.C1Ribbon.RibbonComboBox serialPortCombo;
        private System.Windows.Forms.GroupBox groupBox1;
        private C1.Win.C1Ribbon.RibbonLabel lblCommStatus;
        private C1.Win.C1Ribbon.RibbonLabel lblVersion;
        private System.Windows.Forms.TextBox txtDelay;
        private System.Windows.Forms.TextBox txtGreen;
        private System.Windows.Forms.TextBox txtRed;
        private C1.Win.C1Ribbon.RibbonSeparator ribbonSeparator1;
        private C1.Win.C1Ribbon.RibbonLabel lblHwVersion;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private C1.Win.C1Ribbon.RibbonGroup ribbonGroup3;
        private C1.Win.C1Ribbon.RibbonButton ribbonButton3;
        private C1.Win.C1Ribbon.RibbonLabel lblConnectionStatus;
        private C1.Win.C1Ribbon.RibbonSeparator ribbonSeparator2;
    }
}

