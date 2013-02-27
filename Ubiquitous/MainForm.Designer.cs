﻿namespace Ubiquitous
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bWorkerSteamPoll = new System.ComponentModel.BackgroundWorker();
            this.bWorkerSc2TvPoll = new System.ComponentModel.BackgroundWorker();
            this.contextMenuChat = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.steamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sc2TvruToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCommercial = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelViewers = new System.Windows.Forms.Label();
            this.trackBarTransparency = new System.Windows.Forms.TrackBar();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textMessages = new SC2TV.RTFControl.ExRichTextBox();
            this.pictureCurrentChat = new System.Windows.Forms.PictureBox();
            this.buttonFullscreen = new System.Windows.Forms.Button();
            this.imageListChatSize = new System.Windows.Forms.ImageList(this.components);
            this.textCommand = new System.Windows.Forms.TextBox();
            this.buttonInvisible = new System.Windows.Forms.Button();
            this.comboSc2Channels = new Ubiquitous.ComboBoxWithId();
            this.comboGGChannels = new Ubiquitous.ComboBoxWithId();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.pictureEmpire = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureGoha = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBattlelog = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureGoodgame = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureSkype = new System.Windows.Forms.PictureBox();
            this.pictureSteamBot = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureSteamAdmin = new System.Windows.Forms.PictureBox();
            this.pictureSc2tv = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureTwitch = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pictureSc2tvStream = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureGohaStream = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.pictureStream = new System.Windows.Forms.PictureBox();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.timerEverySecond = new System.Windows.Forms.Timer(this.components);
            this.contextMenuChat.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransparency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCurrentChat)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmpire)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBattlelog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoodgame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSkype)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSteamBot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSteamAdmin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSc2tv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTwitch)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSc2tvStream)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGohaStream)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStream)).BeginInit();
            this.SuspendLayout();
            // 
            // bWorkerSteamPoll
            // 
            this.bWorkerSteamPoll.WorkerSupportsCancellation = true;
            this.bWorkerSteamPoll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSteamPoll_DoWork);
            this.bWorkerSteamPoll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerPoll_RunWorkerCompleted);
            // 
            // bWorkerSc2TvPoll
            // 
            this.bWorkerSc2TvPoll.WorkerSupportsCancellation = true;
            this.bWorkerSc2TvPoll.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bWorkerSc2TvPoll_DoWork);
            this.bWorkerSc2TvPoll.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bWorkerSc2TvPoll_RunWorkerCompleted);
            // 
            // contextMenuChat
            // 
            this.contextMenuChat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steamToolStripMenuItem,
            this.twitchToolStripMenuItem,
            this.sc2TvruToolStripMenuItem,
            this.skypeToolStripMenuItem});
            this.contextMenuChat.Name = "contextMenuChat";
            this.contextMenuChat.Size = new System.Drawing.Size(120, 92);
            // 
            // steamToolStripMenuItem
            // 
            this.steamToolStripMenuItem.Name = "steamToolStripMenuItem";
            this.steamToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.steamToolStripMenuItem.Text = "Steam";
            // 
            // twitchToolStripMenuItem
            // 
            this.twitchToolStripMenuItem.Name = "twitchToolStripMenuItem";
            this.twitchToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.twitchToolStripMenuItem.Text = "Twitch.tv";
            // 
            // sc2TvruToolStripMenuItem
            // 
            this.sc2TvruToolStripMenuItem.Name = "sc2TvruToolStripMenuItem";
            this.sc2TvruToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.sc2TvruToolStripMenuItem.Text = "Sc2tv.ru";
            // 
            // skypeToolStripMenuItem
            // 
            this.skypeToolStripMenuItem.Name = "skypeToolStripMenuItem";
            this.skypeToolStripMenuItem.Size = new System.Drawing.Size(119, 22);
            this.skypeToolStripMenuItem.Text = "Skype";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.buttonCommercial);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.labelViewers);
            this.panel1.Controls.Add(this.trackBarTransparency);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.textMessages);
            this.panel1.Controls.Add(this.pictureCurrentChat);
            this.panel1.Controls.Add(this.buttonFullscreen);
            this.panel1.Controls.Add(this.textCommand);
            this.panel1.Controls.Add(this.buttonInvisible);
            this.panel1.Controls.Add(this.comboSc2Channels);
            this.panel1.Controls.Add(this.comboGGChannels);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.buttonSettings);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.ForeColor = System.Drawing.SystemColors.Window;
            this.panel1.Location = new System.Drawing.Point(-3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(649, 485);
            this.panel1.TabIndex = 0;
            // 
            // buttonCommercial
            // 
            this.buttonCommercial.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCommercial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCommercial.Font = new System.Drawing.Font("Chiller", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCommercial.Image = global::Ubiquitous.Properties.Resources.coins;
            this.buttonCommercial.Location = new System.Drawing.Point(283, 2);
            this.buttonCommercial.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCommercial.Name = "buttonCommercial";
            this.buttonCommercial.Size = new System.Drawing.Size(17, 17);
            this.buttonCommercial.TabIndex = 4;
            this.buttonCommercial.Text = "Ads";
            this.buttonCommercial.UseVisualStyleBackColor = true;
            this.buttonCommercial.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.pictureBox1.Image = global::Ubiquitous.Properties.Resources.eye;
            this.pictureBox1.Location = new System.Drawing.Point(5, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // labelViewers
            // 
            this.labelViewers.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelViewers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelViewers.Location = new System.Drawing.Point(3, 1);
            this.labelViewers.Name = "labelViewers";
            this.labelViewers.Size = new System.Drawing.Size(45, 17);
            this.labelViewers.TabIndex = 0;
            this.labelViewers.Text = "0";
            this.labelViewers.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // trackBarTransparency
            // 
            this.trackBarTransparency.AutoSize = false;
            this.trackBarTransparency.BackColor = System.Drawing.Color.Black;
            this.trackBarTransparency.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Ubiquitous.Properties.Settings.Default, "globalTransparency", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.trackBarTransparency.Location = new System.Drawing.Point(176, 2);
            this.trackBarTransparency.Maximum = 100;
            this.trackBarTransparency.Name = "trackBarTransparency";
            this.trackBarTransparency.Size = new System.Drawing.Size(104, 17);
            this.trackBarTransparency.TabIndex = 3;
            this.trackBarTransparency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarTransparency.Value = global::Ubiquitous.Properties.Settings.Default.globalTransparency;
            this.trackBarTransparency.MouseMove += new System.Windows.Forms.MouseEventHandler(this.trackBarTransparency_MouseMove);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.Black;
            this.checkBox2.Checked = global::Ubiquitous.Properties.Settings.Default.globalHideBorder;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Ubiquitous.Properties.Settings.Default, "globalHideBorder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(118, 1);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(57, 17);
            this.checkBox2.TabIndex = 2;
            this.checkBox2.Text = "Border";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Black;
            this.checkBox1.Checked = global::Ubiquitous.Properties.Settings.Default.globalOnTop;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Ubiquitous.Properties.Settings.Default, "globalOnTop", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(50, 1);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(62, 17);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "On Top";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            this.checkBox1.MouseLeave += new System.EventHandler(this.checkBox1_MouseLeave);
            this.checkBox1.MouseHover += new System.EventHandler(this.checkBox1_MouseHover);
            // 
            // textMessages
            // 
            this.textMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textMessages.BackColor = System.Drawing.Color.Black;
            this.textMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textMessages.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textMessages.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textMessages.HiglightColor = SC2TV.RTFControl.RtfColor.Black;
            this.textMessages.Location = new System.Drawing.Point(0, 3);
            this.textMessages.Name = "textMessages";
            this.textMessages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.textMessages.Size = new System.Drawing.Size(500, 459);
            this.textMessages.TabIndex = 23;
            this.textMessages.Text = "";
            this.textMessages.TextColor = SC2TV.RTFControl.RtfColor.Aqua;
            this.textMessages.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.textMessages_LinkClicked);
            this.textMessages.Click += new System.EventHandler(this.textMessages_Click);
            this.textMessages.SizeChanged += new System.EventHandler(this.textMessages_SizeChanged);
            this.textMessages.DoubleClick += new System.EventHandler(this.textMessages_DoubleClick);
            this.textMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textMessages_MouseDown);
            this.textMessages.MouseLeave += new System.EventHandler(this.textMessages_MouseLeave);
            this.textMessages.MouseHover += new System.EventHandler(this.textMessages_MouseHover);
            this.textMessages.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textMessages_MouseMove);
            this.textMessages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textMessages_MouseUp);
            // 
            // pictureCurrentChat
            // 
            this.pictureCurrentChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureCurrentChat.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureCurrentChat.ContextMenuStrip = this.contextMenuChat;
            this.pictureCurrentChat.Image = global::Ubiquitous.Properties.Resources.twitchicon;
            this.pictureCurrentChat.Location = new System.Drawing.Point(2, 465);
            this.pictureCurrentChat.Name = "pictureCurrentChat";
            this.pictureCurrentChat.Size = new System.Drawing.Size(17, 18);
            this.pictureCurrentChat.TabIndex = 34;
            this.pictureCurrentChat.TabStop = false;
            // 
            // buttonFullscreen
            // 
            this.buttonFullscreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFullscreen.BackColor = System.Drawing.SystemColors.Window;
            this.buttonFullscreen.ImageIndex = 0;
            this.buttonFullscreen.ImageList = this.imageListChatSize;
            this.buttonFullscreen.Location = new System.Drawing.Point(624, 463);
            this.buttonFullscreen.Name = "buttonFullscreen";
            this.buttonFullscreen.Size = new System.Drawing.Size(23, 21);
            this.buttonFullscreen.TabIndex = 9;
            this.buttonFullscreen.UseVisualStyleBackColor = false;
            this.buttonFullscreen.Click += new System.EventHandler(this.buttonFullscreen_Click_1);
            // 
            // imageListChatSize
            // 
            this.imageListChatSize.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListChatSize.ImageStream")));
            this.imageListChatSize.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListChatSize.Images.SetKeyName(0, "fullscreen.png");
            this.imageListChatSize.Images.SetKeyName(1, "fullscreen_exit.png");
            // 
            // textCommand
            // 
            this.textCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textCommand.BackColor = System.Drawing.Color.Black;
            this.textCommand.ForeColor = System.Drawing.Color.LightYellow;
            this.textCommand.Location = new System.Drawing.Point(21, 465);
            this.textCommand.Name = "textCommand";
            this.textCommand.Size = new System.Drawing.Size(600, 20);
            this.textCommand.TabIndex = 33;
            this.textCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textCommand_KeyUp);
            // 
            // buttonInvisible
            // 
            this.buttonInvisible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInvisible.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonInvisible.Location = new System.Drawing.Point(583, 388);
            this.buttonInvisible.Name = "buttonInvisible";
            this.buttonInvisible.Size = new System.Drawing.Size(62, 23);
            this.buttonInvisible.TabIndex = 6;
            this.buttonInvisible.Text = "Invisible";
            this.buttonInvisible.UseVisualStyleBackColor = true;
            this.buttonInvisible.Visible = false;
            // 
            // comboSc2Channels
            // 
            this.comboSc2Channels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSc2Channels.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.comboSc2Channels.DropDownWidth = 300;
            this.comboSc2Channels.ForeColor = System.Drawing.SystemColors.Window;
            this.comboSc2Channels.FormattingEnabled = true;
            this.comboSc2Channels.Location = new System.Drawing.Point(504, 438);
            this.comboSc2Channels.Name = "comboSc2Channels";
            this.comboSc2Channels.Size = new System.Drawing.Size(86, 21);
            this.comboSc2Channels.TabIndex = 7;
            this.comboSc2Channels.DropDown += new System.EventHandler(this.comboSc2Channels_DropDown);
            this.comboSc2Channels.SelectionChangeCommitted += new System.EventHandler(this.comboSc2Channels_SelectionChangeCommitted);
            // 
            // comboGGChannels
            // 
            this.comboGGChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboGGChannels.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.comboGGChannels.DropDownWidth = 300;
            this.comboGGChannels.ForeColor = System.Drawing.SystemColors.Window;
            this.comboGGChannels.FormattingEnabled = true;
            this.comboGGChannels.Location = new System.Drawing.Point(503, 481);
            this.comboGGChannels.Name = "comboGGChannels";
            this.comboGGChannels.Size = new System.Drawing.Size(121, 21);
            this.comboGGChannels.TabIndex = 26;
            this.comboGGChannels.Visible = false;
            this.comboGGChannels.DropDown += new System.EventHandler(this.comboGGChannels_DropDown);
            this.comboGGChannels.SelectedIndexChanged += new System.EventHandler(this.comboGGChannels_SelectedIndexChanged);
            this.comboGGChannels.SelectionChangeCommitted += new System.EventHandler(this.comboGGChannels_SelectionChangeCommitted);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.pictureEmpire);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.pictureGoha);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.pictureBattlelog);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.pictureGoodgame);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.pictureSkype);
            this.groupBox1.Controls.Add(this.pictureSteamBot);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.pictureSteamAdmin);
            this.groupBox1.Controls.Add(this.pictureSc2tv);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.pictureTwitch);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox1.Location = new System.Drawing.Point(503, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 262);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chat login status";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label13.Location = new System.Drawing.Point(34, 230);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 18);
            this.label13.TabIndex = 18;
            this.label13.Text = "Empire.tv";
            // 
            // pictureEmpire
            // 
            this.pictureEmpire.Image = ((System.Drawing.Image)(resources.GetObject("pictureEmpire.Image")));
            this.pictureEmpire.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureEmpire.InitialImage")));
            this.pictureEmpire.Location = new System.Drawing.Point(6, 228);
            this.pictureEmpire.Name = "pictureEmpire";
            this.pictureEmpire.Size = new System.Drawing.Size(20, 20);
            this.pictureEmpire.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureEmpire.TabIndex = 17;
            this.pictureEmpire.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label10.Location = new System.Drawing.Point(34, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 18);
            this.label10.TabIndex = 16;
            this.label10.Text = "Goha.tv";
            // 
            // pictureGoha
            // 
            this.pictureGoha.Image = ((System.Drawing.Image)(resources.GetObject("pictureGoha.Image")));
            this.pictureGoha.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureGoha.InitialImage")));
            this.pictureGoha.Location = new System.Drawing.Point(6, 202);
            this.pictureGoha.Name = "pictureGoha";
            this.pictureGoha.Size = new System.Drawing.Size(20, 20);
            this.pictureGoha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureGoha.TabIndex = 15;
            this.pictureGoha.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(34, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 18);
            this.label9.TabIndex = 14;
            this.label9.Text = "Battlelog";
            // 
            // pictureBattlelog
            // 
            this.pictureBattlelog.Image = ((System.Drawing.Image)(resources.GetObject("pictureBattlelog.Image")));
            this.pictureBattlelog.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBattlelog.InitialImage")));
            this.pictureBattlelog.Location = new System.Drawing.Point(6, 176);
            this.pictureBattlelog.Name = "pictureBattlelog";
            this.pictureBattlelog.Size = new System.Drawing.Size(20, 20);
            this.pictureBattlelog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBattlelog.TabIndex = 13;
            this.pictureBattlelog.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(32, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 18);
            this.label8.TabIndex = 12;
            this.label8.Text = "Goodgame";
            // 
            // pictureGoodgame
            // 
            this.pictureGoodgame.Image = ((System.Drawing.Image)(resources.GetObject("pictureGoodgame.Image")));
            this.pictureGoodgame.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureGoodgame.InitialImage")));
            this.pictureGoodgame.Location = new System.Drawing.Point(6, 149);
            this.pictureGoodgame.Name = "pictureGoodgame";
            this.pictureGoodgame.Size = new System.Drawing.Size(20, 20);
            this.pictureGoodgame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureGoodgame.TabIndex = 11;
            this.pictureGoodgame.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(32, 125);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 18);
            this.label5.TabIndex = 10;
            this.label5.Text = "Skype";
            // 
            // pictureSkype
            // 
            this.pictureSkype.Image = ((System.Drawing.Image)(resources.GetObject("pictureSkype.Image")));
            this.pictureSkype.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureSkype.InitialImage")));
            this.pictureSkype.Location = new System.Drawing.Point(6, 123);
            this.pictureSkype.Name = "pictureSkype";
            this.pictureSkype.Size = new System.Drawing.Size(20, 20);
            this.pictureSkype.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSkype.TabIndex = 9;
            this.pictureSkype.TabStop = false;
            // 
            // pictureSteamBot
            // 
            this.pictureSteamBot.Image = ((System.Drawing.Image)(resources.GetObject("pictureSteamBot.Image")));
            this.pictureSteamBot.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureSteamBot.InitialImage")));
            this.pictureSteamBot.Location = new System.Drawing.Point(6, 19);
            this.pictureSteamBot.Name = "pictureSteamBot";
            this.pictureSteamBot.Size = new System.Drawing.Size(20, 20);
            this.pictureSteamBot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSteamBot.TabIndex = 2;
            this.pictureSteamBot.TabStop = false;
            this.pictureSteamBot.Click += new System.EventHandler(this.pictureSteamBot_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(32, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "Sc2tv.ru";
            // 
            // pictureSteamAdmin
            // 
            this.pictureSteamAdmin.Image = ((System.Drawing.Image)(resources.GetObject("pictureSteamAdmin.Image")));
            this.pictureSteamAdmin.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureSteamAdmin.InitialImage")));
            this.pictureSteamAdmin.Location = new System.Drawing.Point(6, 45);
            this.pictureSteamAdmin.Name = "pictureSteamAdmin";
            this.pictureSteamAdmin.Size = new System.Drawing.Size(20, 20);
            this.pictureSteamAdmin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSteamAdmin.TabIndex = 3;
            this.pictureSteamAdmin.TabStop = false;
            // 
            // pictureSc2tv
            // 
            this.pictureSc2tv.Image = ((System.Drawing.Image)(resources.GetObject("pictureSc2tv.Image")));
            this.pictureSc2tv.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureSc2tv.InitialImage")));
            this.pictureSc2tv.Location = new System.Drawing.Point(6, 97);
            this.pictureSc2tv.Name = "pictureSc2tv";
            this.pictureSc2tv.Size = new System.Drawing.Size(20, 20);
            this.pictureSc2tv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSc2tv.TabIndex = 7;
            this.pictureSc2tv.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(32, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Steam Bot";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(32, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "Twitch.tv(IRC)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(32, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 18);
            this.label2.TabIndex = 4;
            this.label2.Text = "Steam Admin";
            // 
            // pictureTwitch
            // 
            this.pictureTwitch.Image = ((System.Drawing.Image)(resources.GetObject("pictureTwitch.Image")));
            this.pictureTwitch.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureTwitch.InitialImage")));
            this.pictureTwitch.Location = new System.Drawing.Point(6, 71);
            this.pictureTwitch.Name = "pictureTwitch";
            this.pictureTwitch.Size = new System.Drawing.Size(20, 20);
            this.pictureTwitch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureTwitch.TabIndex = 5;
            this.pictureTwitch.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.pictureSc2tvStream);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.pictureGohaStream);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.pictureStream);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupBox2.Location = new System.Drawing.Point(503, 271);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(143, 111);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stream status";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label14.Location = new System.Drawing.Point(31, 82);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Sc2tv.ru";
            // 
            // pictureSc2tvStream
            // 
            this.pictureSc2tvStream.Image = ((System.Drawing.Image)(resources.GetObject("pictureSc2tvStream.Image")));
            this.pictureSc2tvStream.Location = new System.Drawing.Point(6, 76);
            this.pictureSc2tvStream.Name = "pictureSc2tvStream";
            this.pictureSc2tvStream.Size = new System.Drawing.Size(21, 24);
            this.pictureSc2tvStream.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureSc2tvStream.TabIndex = 4;
            this.pictureSc2tvStream.TabStop = false;
            this.pictureSc2tvStream.Click += new System.EventHandler(this.pictureSc2tvStream_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label12.Location = new System.Drawing.Point(31, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 3;
            this.label12.Text = "Goha.TV";
            // 
            // pictureGohaStream
            // 
            this.pictureGohaStream.Image = ((System.Drawing.Image)(resources.GetObject("pictureGohaStream.Image")));
            this.pictureGohaStream.Location = new System.Drawing.Point(6, 47);
            this.pictureGohaStream.Name = "pictureGohaStream";
            this.pictureGohaStream.Size = new System.Drawing.Size(21, 24);
            this.pictureGohaStream.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureGohaStream.TabIndex = 2;
            this.pictureGohaStream.TabStop = false;
            this.pictureGohaStream.Click += new System.EventHandler(this.pictureGohaStream_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label11.Location = new System.Drawing.Point(31, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Twitch.TV";
            // 
            // pictureStream
            // 
            this.pictureStream.Image = ((System.Drawing.Image)(resources.GetObject("pictureStream.Image")));
            this.pictureStream.Location = new System.Drawing.Point(6, 19);
            this.pictureStream.Name = "pictureStream";
            this.pictureStream.Size = new System.Drawing.Size(21, 24);
            this.pictureStream.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureStream.TabIndex = 0;
            this.pictureStream.TabStop = false;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSettings.Location = new System.Drawing.Point(503, 388);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSettings.TabIndex = 5;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = false;
            this.buttonSettings.Click += new System.EventHandler(this.buttonSettings_Click_1);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(503, 465);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Goodgame.ru";
            this.label7.Visible = false;
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(503, 422);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Sc2Tv:";
            this.label6.Visible = false;
            // 
            // timerEverySecond
            // 
            this.timerEverySecond.Enabled = true;
            this.timerEverySecond.Interval = 1000;
            this.timerEverySecond.Tick += new System.EventHandler(this.timerEverySecond_Tick);
            // 
            // MainForm
            // 
            this.AcceptButton = this.buttonInvisible;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 483);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Ubiquitous - MultiChat";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Enter += new System.EventHandler(this.MainForm_Enter);
            this.MouseEnter += new System.EventHandler(this.MainForm_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.MainForm_MouseLeave);
            this.MouseHover += new System.EventHandler(this.MainForm_MouseHover);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
            this.contextMenuChat.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTransparency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCurrentChat)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEmpire)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBattlelog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGoodgame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSkype)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSteamBot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSteamAdmin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSc2tv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTwitch)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSc2tvStream)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureGohaStream)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStream)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bWorkerSteamPoll;
        private System.ComponentModel.BackgroundWorker bWorkerSc2TvPoll;
        private System.Windows.Forms.ContextMenuStrip contextMenuChat;
        private System.Windows.Forms.ToolStripMenuItem steamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sc2TvruToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skypeToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private SC2TV.RTFControl.ExRichTextBox textMessages;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonInvisible;
        private System.Windows.Forms.Label label6;
        private ComboBoxWithId comboSc2Channels;
        private ComboBoxWithId comboGGChannels;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureGoha;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBattlelog;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureGoodgame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureSkype;
        private System.Windows.Forms.PictureBox pictureSteamBot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureSteamAdmin;
        private System.Windows.Forms.PictureBox pictureSc2tv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureTwitch;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureStream;
        private System.Windows.Forms.Button buttonSettings;
        private System.Windows.Forms.ImageList imageListChatSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureGohaStream;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.PictureBox pictureEmpire;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.PictureBox pictureSc2tvStream;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TrackBar trackBarTransparency;
        private System.Windows.Forms.Timer timerEverySecond;
        private System.Windows.Forms.Label labelViewers;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureCurrentChat;
        private System.Windows.Forms.Button buttonFullscreen;
        private System.Windows.Forms.TextBox textCommand;
        private System.Windows.Forms.Button buttonCommercial;
    }
}

