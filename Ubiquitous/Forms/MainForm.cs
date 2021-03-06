﻿
using WMPLib;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Deployment;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using dotSteam;
using dotIRC;
using dotGoodgame;
using dotSC2TV;
using dotSkype;
using dotTwitchTV;
using dotXSplit;
using System.Web;
using System.Text.RegularExpressions;
using dotGohaTV;
using dotEmpireTV;
using dotCybergame;
using System.Configuration;
using dotOBS;
using dotUtilities;
using dotHashd;
using System.Runtime.InteropServices;
using System.Net;
using dotWebServer;
using dotLastfm;
using dotYoutube;
using dotGamersTv;
using dotJetSetPro;
using System.IO;
using dotInterfaces;
using dotHitboxTv;

namespace Ubiquitous
{
    public partial class MainForm : Form
    {
        #region Delegates


        delegate void SetTransparencyCB(Color color,bool chroma);
        delegate void SetVisibilityCB(Control control, bool state);
        delegate void SetTopMostCB(Form control, bool topmost);
        delegate void SetComboValueCB(ComboBox combo, object value);
        delegate void SetTooltipCB(ToolTip tooltip, Control control, string value);
        delegate void SetCheckedToolTipCB(ToolStripMenuItem item, bool state);


        #endregion
        #region DllImport
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        #endregion
        #region Constants
        private const int NONANTIALIASED_QUALITY = 3;

        private const int WM_NCHITTEST = 0x84;
        private const int HTTRANSPARENT = -1;
        #endregion

        #region Private classes and enums
        public enum EndPoint
        {
            Sc2Tv,
            TwitchTV,
            Steam,
            SteamAdmin,
            Skype,
            Console,
            SkypeGroup,
            Bot,
            Goodgame,
            Battlelog,
            Gohatv,
            Empiretv,
            Cybergame,
            Hashd,
            Music,
            Youtube,
            GamersTV,
            JetSet,
            HitBox,
            All,
            Notice,
            Error
        }
        private class ChatUser
        {
            public ChatUser(string fullName, string nickName, EndPoint endPoint)
            {
                FullName = fullName;
                NickName = nickName;
                EndPoint = endPoint;
            }
            public String FullName
            {
                get;
                set;
            }
            public String NickName
            {
                get;
                set;
            }
            public EndPoint EndPoint
            {
                get;
                set;
            }
        }
        private class ChatAlias
        {
            public ChatAlias(string alias, EndPoint endpoint, ChatIcon icon = ChatIcon.Default, String mynick = "")
            {
                Alias = alias;
                Endpoint = endpoint;
                Icon = icon;
                MyNick = mynick;
            }
            public ChatIcon Icon
            {
                get;
                set;
            }
            public EndPoint Endpoint
            {
                get;
                set;
            }
            public string Alias
            {
                get;
                set;
            }
            public string MyNick
            {
                get;
                set;
            }

        }
        private class AdminCommand
        {
            private enum CommandType
            {
                BoolCmd,
                PartnerCmd,
                EmptyParam,
                ReplyCmd,
            }
            private string partnerHandle;
            private string _re;
            private bool _flag;
            private Func<string, Result> _action;
            private Func<bool, Result> _action2;
            private Func<Result> _action3;
            private Func<string, UbiMessage, Result> _action4;
            private CommandType _type;
            private UbiMessage _message;
            private string _switchto;

            public AdminCommand(string re, Func<Result> action)
            {
                _re = re;
                _action3 = action;
                _type = CommandType.EmptyParam;
            }
            public AdminCommand(string re, Func<string, Result> action)
            {
                _re = re;
                _action = action;
                _type = CommandType.PartnerCmd;
            }
            public AdminCommand(string re, Func<string, UbiMessage, Result> action)
            {
                _re = re;
                _action4 = action;                
                _type = CommandType.ReplyCmd;
                _message = new UbiMessage("", EndPoint.SteamAdmin);
                _switchto = "";
            }
            public AdminCommand(string re, Func<bool, Result> action, bool flag)
            {
                _re = re;
                _action2 = action;
                _flag = flag;
                _type = CommandType.BoolCmd;
            }
            public Result Execute()
            {
                Result result = Result.Failed;
                switch (_type)
                {
                    case CommandType.BoolCmd:
                        result = _action2(_flag);
                        break;
                    case CommandType.PartnerCmd:
                        if( partnerHandle != null )
                            result = _action(partnerHandle);
                        break;
                    case CommandType.ReplyCmd:
                        if (_message != null )
                        {
                            result = _action4( _switchto, _message );
                        }
                        break;
                    case CommandType.EmptyParam:
                        result = _action3();
                        break;
                }
                return result;
            }

            public bool isCommand(string command)
            {
                if (Regex.IsMatch(command, _re,RegexOptions.IgnoreCase))
                {
                    Match reCommand = Regex.Match(command, _re, RegexOptions.IgnoreCase);
                    switch (_type)
                    {
                        case CommandType.BoolCmd:
                            break;
                        case CommandType.PartnerCmd:
                            if (reCommand.Groups.Count > 0)
                                partnerHandle = reCommand.Groups[1].Value;
                            break;
                        case CommandType.ReplyCmd:
                            if (reCommand.Groups.Count >= 3)
                            {
                                _switchto = reCommand.Groups[1].Value;
                                _message = new UbiMessage(reCommand.Groups[2].Value, EndPoint.SteamAdmin);
                            }
                            break;
                        case CommandType.EmptyParam:
                            break;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public class UbiMessage
        {
            private string _text, _from, _to;
            private EndPoint _fromEndpoint, _toEndpoint;
            private bool _textonly = true;
            public bool TextOnly
            {
                get { return _textonly; }
                set { _textonly = value; }
            }
            public UbiMessage(string text)
            {
                _text = text;
                _fromEndpoint = EndPoint.All;
                _toEndpoint = EndPoint.Console;
                _from = null;
            }
            public UbiMessage(string text, EndPoint fromendpoint)
            {
                _text = text;
                _fromEndpoint = fromendpoint;
                _toEndpoint = EndPoint.Console;
                _from = null;
            }
            public UbiMessage(string text, EndPoint fromendpoint, EndPoint toendpoint)
            {
                _text = text;
                _fromEndpoint = fromendpoint;
                _toEndpoint = toendpoint;
                _from = null;
            }

            public UbiMessage(string text, string fromName, EndPoint fromEndPoint)
            {
                _text = text;
                _from = fromName;
                _fromEndpoint = fromEndPoint;
                _toEndpoint = EndPoint.Console;
            }
            public Color NickColor
            {
                get;
                set;
            }
            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }
            public string FromName
            {
                get { return _from; }
                set { _from = value; }
            }
            public EndPoint FromEndPoint
            {
                get { return _fromEndpoint; }
                set { _fromEndpoint = value; }
            }
            public String ToGroupName
            {
                get;
                set;
            }
            public String FromGroupName
            {
                get;
                set;
            }
            public string ToName
            {
                get { return _to; }
                set { _to = value; }
            }
            public EndPoint ToEndPoint
            {
                get { return _toEndpoint; }
                set { _toEndpoint = value; }
            }
            public string ImagePath
            {
                get
                {
                    switch (_fromEndpoint)
                    {
                        case EndPoint.Sc2Tv:
                            return "sc2icon.png";
                        case EndPoint.TwitchTV:
                            return "twitchicon.png";
                        case EndPoint.Skype:
                            return "skypeicon.png";
                        case EndPoint.SkypeGroup:
                            return "skypeicon.png";
                        case EndPoint.Steam:
                            return "steamicon.png";
                        case EndPoint.SteamAdmin:
                            return "steamicon.png";
                        case EndPoint.Console:
                            return "adminicon.png";
                        case EndPoint.Bot:
                            return "adminicon.png";
                        case EndPoint.Goodgame:
                            return "goodgameicon.png";
                        case EndPoint.Battlelog:
                            return "bf3icon.png";
                        case EndPoint.Empiretv:
                            return "empire.bmp";
                        case EndPoint.Gohatv:
                            return "goha.bmp";
                        case EndPoint.Cybergame:
                            return "cybergame.gif";
                        case EndPoint.Hashd:
                            return "hashd.png";
                        case EndPoint.Music:
                            return "music.png";
                        case EndPoint.Youtube:
                            return "youtube.png";
                        case EndPoint.GamersTV:
                            return "gamerstvicon.png";
                        case EndPoint.JetSet:
                            return "jetset.png";
                        case EndPoint.HitBox:
                            return "hitbox.png";
                        default:
                            return "adminicon.png";

                    }
                }
            }
            public ChatIcon Icon
            {
                get
                {
                    switch (_fromEndpoint)
                    {
                        case EndPoint.Sc2Tv:
                            return ChatIcon.Sc2Tv;
                        case EndPoint.TwitchTV:
                            return ChatIcon.TwitchTv;
                        case EndPoint.Skype:
                            return ChatIcon.Skype;
                        case EndPoint.SkypeGroup:
                            return ChatIcon.Skype;
                        case EndPoint.Steam:
                            return ChatIcon.Steam;
                        case EndPoint.SteamAdmin:
                            return ChatIcon.Admin;
                        case EndPoint.Console:
                            return ChatIcon.Admin;
                        case EndPoint.Bot:
                            return ChatIcon.Admin;
                        case EndPoint.Goodgame:
                            return ChatIcon.Goodgame;
                        case EndPoint.Battlelog:
                            return ChatIcon.Battlelog;
                        case EndPoint.Empiretv:
                            return ChatIcon.Empire;
                        case EndPoint.Gohatv:
                            return ChatIcon.Goha;
                        case EndPoint.Cybergame:
                            return ChatIcon.Cybergame;
                        case EndPoint.Hashd:
                            return ChatIcon.Hashd;
                        case EndPoint.Music:
                            return ChatIcon.Music;
                        case EndPoint.Youtube:
                            return ChatIcon.Youtube;
                        case EndPoint.GamersTV:
                            return ChatIcon.GamersTv;
                        case EndPoint.JetSet:
                            return ChatIcon.JetSet;
                        case EndPoint.HitBox:
                            return ChatIcon.HitBox;
                        default:
                            return ChatIcon.Default;
                    }
                }
            }

        }
        #endregion 

        #region Private properties
        private string formTitle;
        private Point _Offset = Point.Empty;
        private Point _OffsetViewers = Point.Empty;
        private Point _OffsetForm = Point.Empty;
        private Properties.Settings settings;
        private const string twitchIRCDomain = "jtvirc.com";
        private const string gohaIRCDomain = "i.gohanet.ru";
        private Log log;
        private ChannelList endpointList;
        private BindingSource endpointListBS;
        public BindingSource profileListBS;
        private SteamAPISession.User steamAdmin;
        private List<SteamAPISession.Update> updateList;
        private SteamAPISession steamBot;
        private SteamAPISession.LoginStatus status;
        private StatusImage checkMark;
        private StatusImage streamStatus;
        public Sc2Chat sc2tv;
        private IrcClient twitchIrc;
        private IrcClient gohaIrc;
        private SkypeChat skype;
        private List<AdminCommand> adminCommands;
        private List<ChatAlias> chatAliases;
        private UbiMessage lastMessageSent;
        private List<UbiMessage> lastMessagePerEndpoint;
        private BindingSource channelsSC2;
        private BindingSource channelsGG;
        private uint sc2ChannelId = 0;
        private bool streamIsOnline = false;
        private BGWorker gohaBW, gohaStreamBW, steamBW, sc2BW, twitchBW, skypeBW, twitchTV, goodgameBW, battlelogBW,
                        empireBW, cyberBW, obsremoteBW, hashdBW, youtubeBW, gamerstvBW, jetsetBW, hitboxBW;
        private EmpireTV empireTV;
        public GohaTV gohaTVstream;
        public Twitch twitchChannel;
        private EndPoint currentChat;
        public Goodgame ggChat;
        private OBSRemote obsRemote;
        private XSplit xsplit;
        private StatusServer statusServer;
//        private Battlelog battlelog;
        public Cybergame cybergame;
        private List<ChatUser> chatUsers;
        private bool isClosing = false;
        private ToolTip viewersTooltip;
        private FontDialog fontDialog;
        private Form debugForm;
        private Hashd hashd;
        private YouTube youtube;
        private GamersTv gamerstv;
        private JetSetPro jetset;
        private uint MaxViewers = 0;
        private bool twitchTriedOAuth = false;
        public TwitchWeb twitchWeb;
        public List<String> comboProfiles;
        IPHostEntry twitchServers = new IPHostEntry();
        IPAddress nextTwitchIP = new IPAddress(0);
        private ULastFm lastFm;
        private HitBox hitbox;
        private WebChat webChat;
        private object lockTwitchConnect = new object();
        private object lockTwitchMessage = new object();
        private object lockSendMessage = new object();
        private object lockOBS = new object();
        private System.Threading.Timer forceCloseTimer;
        private System.Threading.Timer twitchPing;
        private System.Threading.Timer twitchDisconnect;
        private System.Threading.Timer obsChatSourceSwitch;

        #endregion 

        #region Form events and methods

        
        public MainForm()
        {


            //UnprotectConfig();

            settings = Properties.Settings.Default;
            try
            {
                if (String.IsNullOrEmpty(settings.currentProfile))
                {
                    settings.currentProfile = "Default";
                }

                if (settings.chatProfiles == null)
                {
                    settings.chatProfiles = new ChatProfiles();
                    settings.chatProfiles.WriteProfile(settings.currentProfile, settings);
                }
                else if (!settings.chatProfiles.Profiles.Any(p => p.Name.Equals(settings.currentProfile)))
                {
                    settings.chatProfiles.WriteProfile(settings.currentProfile, settings);
                }

            }
            catch { }

            
            InitializeComponent();


            endpointList = new ChannelList();
            endpointListBS = new BindingSource();

            if( settings.globalDebug )
                debugForm = new DebugForm();

            //RefreshChatProperties();
            log = new Log(textMessages);


            chatUsers = new List<ChatUser>();
            
            Enum.TryParse<EndPoint>(settings.globalDefaultChat, out currentChat);

            lastMessageSent = new UbiMessage("", EndPoint.Console);
            adminCommands = new List<AdminCommand>();
            chatAliases = new List<ChatAlias>();
            lastMessagePerEndpoint = new List<UbiMessage>();
            adminCommands.Add(new AdminCommand(@"^/r\s*([^\s]*)\s*(.*)", ReplyCommand));
            adminCommands.Add(new AdminCommand(@"^/stream$", StartStopStreamsCommand));
            adminCommands.Add(new AdminCommand(@"^/gohaconfirm\s*(.*)", GohaConfirmCommand));
            adminCommands.Add(new AdminCommand(@"^/gohasetpass", GohaUpdatePassword));
            adminCommands.Add(new AdminCommand(@"^/width\s*(.*)", SetFormWidth));
            adminCommands.Add(new AdminCommand(@"^/height\s*(.*)", SetFormHeight));
            adminCommands.Add(new AdminCommand(@"^/scene\s*(.*)", SetOBSScene));

            chatAliases.Add(new ChatAlias(settings.twitchChatAlias, EndPoint.TwitchTV, ChatIcon.TwitchTv, settings.TwitchUser.ToLower()));
            chatAliases.Add(new ChatAlias(settings.sc2tvChatAlias, EndPoint.Sc2Tv, ChatIcon.Sc2Tv, settings.Sc2tvUser));
            chatAliases.Add(new ChatAlias(settings.steamChatAlias, EndPoint.Steam, ChatIcon.Steam));
            chatAliases.Add(new ChatAlias(settings.skypeChatAlias, EndPoint.Skype, ChatIcon.Skype));
//            chatAliases.Add(new ChatAlias(settings.battlelogChatAlias, EndPoint.Battlelog, ChatIcon.Battlelog));
            chatAliases.Add(new ChatAlias(settings.gohaChatAlias, EndPoint.Gohatv, ChatIcon.Goha, settings.GohaUser));
            chatAliases.Add(new ChatAlias(settings.empireAlias, EndPoint.Empiretv, ChatIcon.Empire, settings.empireUser.ToLower()));
            chatAliases.Add(new ChatAlias(settings.goodgameChatAlias, EndPoint.Goodgame, ChatIcon.Goodgame, settings.goodgameUser.ToLower()));
            chatAliases.Add(new ChatAlias(settings.cyberAlias, EndPoint.Cybergame, ChatIcon.Cybergame, settings.cyberUser.ToLower()));
            chatAliases.Add(new ChatAlias(settings.hashdAlias, EndPoint.Hashd, ChatIcon.Hashd, settings.hashdUser.ToLower()));
            chatAliases.Add(new ChatAlias(settings.youtubeAlias, EndPoint.Youtube, ChatIcon.Youtube));
            chatAliases.Add(new ChatAlias(settings.gmtvAlias, EndPoint.GamersTV, ChatIcon.GamersTv));
            chatAliases.Add(new ChatAlias(settings.jetsetAlias, EndPoint.JetSet, ChatIcon.JetSet));
            chatAliases.Add(new ChatAlias(settings.hitboxAlias, EndPoint.HitBox, ChatIcon.HitBox));
            chatAliases.Add(new ChatAlias("@all", EndPoint.All, ChatIcon.Default));

            var switchTo = chatAliases.FirstOrDefault( c => c.Endpoint == currentChat );
            if( switchTo != null )
                SwitchToChat(switchTo.Alias,false);

            uint.TryParse(settings.Sc2tvId, out sc2ChannelId);
            Debug.Print(String.Format("Sc2tv Channel ID: {0}",sc2ChannelId));

            sc2tv = new Sc2Chat(settings.sc2LastMsgId + 1);

            sc2tv.Logon += OnSc2TvLogin;
            sc2tv.ChannelList += OnSc2TvChannelList;
            sc2tv.MessageReceived += OnSc2TvMessageReceived;
            sc2tv.channelList = new Channels();

            gohaIrc = new IrcClient();
            gohaIrc.Connected += OnGohaConnect;
            gohaIrc.Registered += OnGohaRegister;
            gohaIrc.Disconnected += OnGohaDisconnect;


            checkMark = new StatusImage(Properties.Resources.checkMarkGreen, Properties.Resources.checkMarkRed);
            streamStatus = new StatusImage(Properties.Resources.streamOnline, Properties.Resources.streamOffline);


            statusServer = new StatusServer();
            //battlelog = new Battlelog();

            
            steamBW = new BGWorker(ConnectSteamBot, null);
            sc2BW = new BGWorker(ConnectSc2tv, null);
            
            twitchBW = new BGWorker(ConnectTwitchIRC, null);                
            gohaBW = new BGWorker(ConnectGohaIRC, null);
            twitchTV = new BGWorker(ConnectTwitchChannel, null);
            skypeBW = new BGWorker(ConnectSkype, null);
            cyberBW = new BGWorker(ConnectCybergame, null);
            hashdBW = new BGWorker(ConnectHashd, null);
            youtubeBW = new BGWorker(ConnectYoutube, null);
            gamerstvBW = new BGWorker(ConnectGamersTV, null );
            jetsetBW = new BGWorker(ConnectJetSet, null);
            hitboxBW = new BGWorker(ConnectHitBox, null);
 
            goodgameBW = new BGWorker(ConnectGoodgame, null);
//            battlelogBW = new BGWorker(ConnectBattlelog, null);

            if (settings.enableXSplitStats)
            {
                xsplit = new XSplit();
                xsplit.OnFrameDrops += OnXSplitFrameDrops;
                xsplit.OnStatusRefresh += OnXSplitStatusRefresh;
            }
            if (settings.enableStatusServer)
            {
                statusServer.Start();
            }

            gohaTVstream = new GohaTV();
            gohaStreamBW = new BGWorker(ConnectGohaStream, null);

            empireTV = new EmpireTV();
            empireBW = new BGWorker(ConnectEmpireTV, null);

            obsremoteBW = new BGWorker(ConnectOBSRemote, null);

            settings.PropertyChanged += new PropertyChangedEventHandler(settings_PropertyChanged);
            settings.SettingsSaving += new SettingsSavingEventHandler(settings_SettingsSaving);

            fontDialog = new FontDialog();

            forceCloseTimer = new System.Threading.Timer(new TimerCallback(ForceClose), null, Timeout.Infinite, Timeout.Infinite);

            obsChatSourceSwitch = new System.Threading.Timer(new TimerCallback(OBSChatSwitch), null, Timeout.Infinite, Timeout.Infinite);

            if (settings.webEnable)
            {
                int port;
                int.TryParse(settings.webPort, out port);
                try
                {
                    webChat = new WebChat(port);
                }
                catch (Exception e)
                {
                    SendMessage(new UbiMessage("Web server error: " + e.Message, EndPoint.Error));
                }
            }

            if (settings.lastFmEnable && !String.IsNullOrEmpty(settings.lastFmLogin) && !String.IsNullOrEmpty(settings.lastFmPassword))
            {
                try
                {
                    lastFm = new ULastFm();
                    lastFm.OnLogin += new EventHandler<EventArgs>(lastFm_OnLogin);
                    lastFm.OnTrackChange += new EventHandler<LastFmArgs>(lastFm_OnTrackChange);
                    ThreadPool.QueueUserWorkItem(t => lastFm.Authenticate(settings.lastFmLogin, settings.lastFmPassword));
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message + " " + ex.StackTrace);
                }

            }

            //@Debug.Print("Config is here:" + ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
            #region Set tooltips
            ToolTip fullScreenDblClk = new ToolTip();

            fullScreenDblClk.AutoPopDelay = 2000;
            fullScreenDblClk.InitialDelay = 100;
            fullScreenDblClk.ReshowDelay = 100;
            fullScreenDblClk.ShowAlways = false;

            // Set up the ToolTip text for the Button and Checkbox.
            fullScreenDblClk.SetToolTip(textMessages, "DblClick - switch mode, Hold RMB - move window");

            viewersTooltip = new ToolTip();

            viewersTooltip.AutoPopDelay = 2000;
            viewersTooltip.InitialDelay = 0;
            viewersTooltip.ReshowDelay = 0;
            viewersTooltip.ShowAlways = false;

            viewersTooltip.SetToolTip(labelViewers, String.Format("Twitch.tv: {0}, Cybergame.tv: {0}, Hashd.tv: {0}", 0));

            var tooltip = new ToolTip();
            tooltip.AutoPopDelay = 2000;
            tooltip.InitialDelay = 0;
            tooltip.ReshowDelay = 0;
            tooltip.ShowAlways = false;
            tooltip.SetToolTip(buttonStreamStartStop, "Click to start/stop streaming in OBS");
            
            #endregion
            

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (settings == null)
                    return cp;

                if (!AllowTransparency)
                    return cp;

                if (!settings.globalMouseTransparent)
                    return cp;

                CreateParams curStyle = cp;
                if (settings.globalMouseTransparent && ModifierKeys != Keys.Control)
                {
                    curStyle.ExStyle |= 0x80000 | 0x20;
                    SetTransparency(textMessages.BackColor, settings.globalUseChroma); 
                    return curStyle;
                }
                else 
                {
                    return curStyle;
                }
                
            }
        }
        /*protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == (int)WM_NCHITTEST && ModifierKeys != Keys.Control)
                m.Result = (IntPtr)HTTRANSPARENT;
            else
                base.WndProc(ref m);
        }
        */
        void OBSChatSwitch(object x)
        {
            if (!settings.obsRemoteEnable)
                return;

            lock (lockOBS)
            {
                var chatSource = settings.obsChatSourceName;

                if (settings.obsRemoteEnable && obsRemote.Opened && !String.IsNullOrEmpty(chatSource))
                {
                    obsRemote.SetSourceRendererPart(chatSource, false);
                }
            }
        }
        void lastFm_OnTrackChange(object sender, LastFmArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0} - {1}",e.Artist, e.Title), EndPoint.Music, EndPoint.SteamAdmin));
        }

        void lastFm_OnLogin(object sender, EventArgs e)
        {
            SendMessage(new UbiMessage("Last.Fm: logged in!", EndPoint.Music, EndPoint.Notice));
        }
        private void pictureSteamBot_Click(object sender, EventArgs e)
        {
            settings.steamEnabled = !settings.steamEnabled;
            settings.Save();
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //textMessages.Antialias = false;
            RefreshChatProperties();

            SetupComboProfiles();

            uint lines = 0;
            uint.TryParse(settings.generalHistoryLines, out lines);
            textMessages.MaxLines = lines;

            formTitle = String.Format("{0} {1}", this.Text, GetRunningVersion());
            this.Text = formTitle;
            contextMenuChat.Items.Clear();

            foreach (ChatAlias chatAlias in chatAliases)
            {
                contextMenuChat.Items.Add(String.Format("{0} ({1})",chatAlias.Endpoint.ToString(), chatAlias.Alias),log.GetChatBitmap(chatAlias.Icon));
                endpointList.channels.Add(new Channel(chatAlias.Endpoint));
            }
            //TODO:! channelList bindings

            endpointListBS.DataSource = endpointList.channels;
           
            if (settings.isFullscreenMode)
            {
                switchFullScreenMode();
                Size = settings.globalCompactSize;
            }
            else
            {
                Size = settings.globalFullSize;
            }
            SwitchBorder();
            Size = new System.Drawing.Size(settings.mainformWidth, settings.mainformHeight);


            StartPosition = settings.mainformStartPos;
            Point newLocation = settings.mainFormPosition;

            if (settings.mainFormPosition.X > SystemInformation.VirtualScreen.Width ||
                settings.mainFormPosition.X < 0)
                newLocation.X = 0;

            if (settings.mainFormPosition.Y > SystemInformation.VirtualScreen.Height ||
                settings.mainFormPosition.Y < 0)
                newLocation.Y = 0;
                
            Location = newLocation;
            settings.globalMouseTransparent = settings.globalMouseTransparent;

            UpdateChatEnable();

        }
        private Version GetRunningVersion()
        {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                }
                else
                {
                    return Assembly.GetExecutingAssembly().GetName().Version;
                }
        }
        private void button1_Click(object sender, EventArgs e)
        {

            //Advertising
            if (twitchIrc != null && settings.twitchEnabled && twitchIrc.IsRegistered)
            {
                ThreadPool.QueueUserWorkItem(f => SendMessageToTwitchIRC(new UbiMessage("/commercial", EndPoint.SteamAdmin, EndPoint.TwitchTV)));
                SendMessage(new UbiMessage("TwitchTv: advertising started!", EndPoint.TwitchTV, EndPoint.Notice));
            }           

            if (cybergame != null && settings.cyberEnabled && cybergame.isLoggedIn)
            {
                cybergame.StartAdvertising();
                SendMessage(new UbiMessage("Cybergame: advertising started!", EndPoint.Cybergame, EndPoint.Notice));
            }

        }
        void settings_SettingsSaving(object sender, CancelEventArgs e)
        {

        }
        void RefreshChatProperties()
        {
            String[] refreshProperties = {
                "globalChatFont",
                "globalToolBoxBack",
                "globalChatEnableTimestamps",
                "globalChatTextColor",
                "globalTimestampForeground",
                "globalPersonalMessageBack",
                "globalPersonalMessageFont",
                "personalMessageColor"};

            foreach (var p in refreshProperties)
                SetChatProperties(p);


          

        }
        void UpdateChatEnable()
        {
            chatStatusBattlelog.Visible = settings.battlelogEnabled;
            chatStatusCybergame.Visible = settings.cyberEnabled;
            chatStatusEmpire.Visible = settings.empireEnabled;
            chatStatusGamerTv.Visible = settings.gmtvEnabled;
            chatStatusGoha.Visible = settings.gohaEnabled;
            chatStatusGohaWeb.Visible = settings.gohaEnabled;
            chatStatusGoodgame.Visible = settings.goodgameEnabled;
            chatStatusHashd.Visible = settings.hashdEnabled;
            chatStatusSc2tv.Visible = settings.sc2tvEnabled;
            chatStatusSkype.Visible = settings.skypeEnabled;
            chatStatusSteamAdmin.Visible = settings.steamEnabled;
            chatStatusSteamBot.Visible = settings.steamEnabled;
            chatStatusTwitch.Visible = settings.twitchEnabled;
            chatStatusOBS.Visible = settings.obsRemoteEnable;
            
            chatStatusBattlelog.Dock = settings.battlelogEnabled?DockStyle.Top:DockStyle.None;
            chatStatusCybergame.Dock = settings.cyberEnabled?DockStyle.Top:DockStyle.None;
            chatStatusEmpire.Dock = settings.empireEnabled?DockStyle.Top:DockStyle.None;
            chatStatusGamerTv.Dock = settings.gmtvEnabled?DockStyle.Top:DockStyle.None;
            chatStatusGoha.Dock = settings.gohaEnabled?DockStyle.Top:DockStyle.None;
            chatStatusGohaWeb.Dock = settings.gohaEnabled?DockStyle.Top:DockStyle.None;
            chatStatusGoodgame.Dock = settings.goodgameEnabled?DockStyle.Top:DockStyle.None;
            chatStatusHashd.Dock = settings.hashdEnabled?DockStyle.Top:DockStyle.None;
            chatStatusSc2tv.Dock = settings.sc2tvEnabled?DockStyle.Top:DockStyle.None;
            chatStatusSkype.Dock = settings.skypeEnabled?DockStyle.Top:DockStyle.None;
            chatStatusSteamAdmin.Dock = settings.steamEnabled?DockStyle.Top:DockStyle.None;
            chatStatusSteamBot.Dock = settings.steamEnabled?DockStyle.Top:DockStyle.None;
            chatStatusTwitch.Dock = settings.twitchEnabled?DockStyle.Top:DockStyle.None;
            chatStatusOBS.Dock = settings.obsRemoteEnable ? DockStyle.Top : DockStyle.None;
        }
        void SetChatProperties(string propertyName)
        {
            switch (propertyName)
            {
                case "SteamBotPassword":
                    {
                        if( settings.SteamBotAccessToken != null )
                            settings.SteamBotAccessToken = null;
                    }
                    break;
                case "SteamBot":
                    {
                        if (settings.SteamBotAccessToken != null)
                            settings.SteamBotAccessToken = null;
                    }
                    break;
                case "globalChatFont":
                    {
                        textMessages.Font= settings.globalChatFont;
                    }
                    break;
                case "globalToolBoxBack":
                    {
                        textMessages.BackColor = settings.globalToolBoxBack;
                        panelTools.BackColor = settings.globalToolBoxBack;
                    }
                    break;
                case "globalChatTextColor":
                    {
                        textMessages.TextColor = settings.globalChatTextColor;
                        textMessages.ForeColor = settings.globalChatTextColor;
                    }
                    break;
                case "globalTimestampForeground":
                    {
                        textMessages.TimeColor = settings.globalTimestampForeground;
                    }
                    break;
                case "globalChatEnableTimestamps":
                    {
                        textMessages.TimeStamp = settings.globalChatEnableTimestamps;
                    }
                    break;
                case "globalPersonalMessageBack":
                    {
                        textMessages.PersonalMessageBack = settings.globalPersonalMessageBack;
                    }
                    break;
                case "globalPersonalMessageFont":
                    {
                        textMessages.PersonalMessageFont = settings.globalPersonalMessageFont;
                    }
                    break;
                case "personalMessageColor":
                    {
                        textMessages.PersonalMessageColor = settings.personalMessageColor;
                    }
                    break;
                case "generalHistoryLines":
                    {
                        uint lines = 0;
                        uint.TryParse(settings.generalHistoryLines, out lines);
                        textMessages.MaxLines = lines;
                    }
                    break;
                case "globalMouseTransparent":
                    {
                        textMessages.MouseTransparent = settings.globalMouseTransparent;
                    }
                    break;
                case "globalDefaultChat":
                    {
                        Enum.TryParse<EndPoint>(settings.globalDefaultChat, out currentChat);
                        var switchTo = chatAliases.FirstOrDefault(c => c.Endpoint == currentChat);
                        if (switchTo != null)
                            SwitchToChat(switchTo.Alias, false);
                    }
                    break;
                default:
                    {
                        if (propertyName.ToLower().Contains("enable"))
                        {
                            UpdateChatEnable();
                        }
                        textMessages.TextColor = settings.globalChatTextColor;
                    }
                    break;

            }

        }
        public void CopyChannelDescriptions()
        {
            
            if (twitchIrc != null && twitchWeb != null && settings.twitchEnabled && 
                twitchIrc.IsConnected )
            {
                twitchWeb.ShortDescription = settings.twitch_ShortDescription;
                twitchWeb.Game = settings.twitch_Game;
            }

            if (sc2tv != null && settings.sc2tvEnabled &&
                sc2tv.LoggedIn)
            {
                sc2tv.ShortDescription = settings.sc2tv_ShortDescription;
                sc2tv.LongDescription = settings.sc2tv_LongDescription;
                sc2tv.Game = settings.sc2tv_Game;
            }
            if (ggChat != null && settings.goodgameEnabled &&
                ggChat.isLoggedIn)
            {
                ggChat.ShortDescription = settings.goodgame_ShortDescription;
                ggChat.Game = settings.goodgame_Game;
            }

            if (cybergame != null && settings.cyberEnabled && cybergame.isLoggedIn)
            {
                cybergame.ShortDescription = settings.cybergame_ShortDescription;
                cybergame.Game = settings.cybergame_Game;
            }
            if (gohaTVstream != null && settings.gohaEnabled && gohaTVstream.LoggedIn)
            {
                gohaTVstream.ShortDescription = settings.goha_ShortDescription;
                gohaTVstream.Game = settings.goha_Game;
            }


        }
        void settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetChatProperties(e.PropertyName);
        }
        private void buttonFullscreen_Click(object sender, EventArgs e)
        {
            switchFullScreenMode();

        }
        private void buttonSettings_Click_1(object sender, EventArgs e)
        {

            var lastOnTopState = this.TopMost;
            settings.globalOnTop = false;
            SettingsDialog settingsForm = new SettingsDialog();
            settingsForm.EndPointList = endpointListBS;
            settingsForm.TopMost = true;
            //var state = this.WindowState;
            //this.WindowState = FormWindowState.Minimized;
            settingsForm.ShowDialog();
            //this.WindowState = state;
            this.Enabled = true;
            
            ProtectConfig();
            settings.globalOnTop = lastOnTopState;
            
        }

        private void comboSc2Channels_DropDown(object sender, EventArgs e)
        {
            if( settings.sc2tvEnabled )
                sc2tv.updateStreamList();
        }
        private void textCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var m = new UbiMessage(textCommand.Text, EndPoint.SteamAdmin, currentChat);
                SendMessage(m);
                textCommand.Text = "";
                e.Handled = true;
            }
        }
        private void textMessages_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            textMessages.LinkClick(e.LinkText);
        }

        private bool SwitchToChat(string alias, bool notice=true)
        {
            if (!String.IsNullOrEmpty(alias))
            {
                var chatAlias = chatAliases.Where(ca => ca.Alias.Trim().ToLower() == alias.Trim().ToLower()).FirstOrDefault();
                if (chatAlias == null)
                {
                    var knownAliases = "";
                    chatAliases.ForEach(ca => knownAliases += ca.Alias += " ");
                    knownAliases = knownAliases.Trim();
                    if( notice )
                        SendMessage(new UbiMessage(
                            String.Format("\"{0}\" is unknown chat alias. Use one of: {1}", alias, knownAliases),
                            EndPoint.Bot, EndPoint.SteamAdmin)
                    );
                    return false;
                }
                else
                {
                    currentChat = chatAlias.Endpoint;
                    try
                    {
                        pictureCurrentChat.Image = log.GetChatBitmap(chatAlias.Icon);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print(ex.Message + " " + ex.StackTrace);
                    }

                    if (notice)
                    {
                        var msg = new UbiMessage(String.Format("Switching to {0}...", currentChat.ToString()), EndPoint.Bot, EndPoint.Notice);
                        if (settings.steamCurrentChatNotify && settings.steamEnabled)
                        {
                            if (!isFlood(msg))
                                SendMessage(msg);
                        }
                    }

                    lastMessageSent.FromEndPoint = chatAlias.Endpoint;

                    return true;
                }
            }
            return false;
        }
        private Result StartStopStreamsCommand()
        {
            StartStopOBSStream();
            return Result.Successful;
        }
        private Result SetFormWidth(string width)
        {
            int _width;
            int.TryParse(width, out _width);

            if (_width > 0)
                Utils.SetProperty<Form, int>(this, "Width", _width);
            
            return Result.Successful;
        }
        private Result SetFormHeight(string height)
        {
            int _height;
            int.TryParse(height, out _height);

            if (_height > 0)
                Utils.SetProperty<Form, int>(this, "Height", _height);

            return Result.Successful;
        }
        private Result SetOBSScene(string sceneMask)
        {
            if (!settings.obsRemoteEnable)
            {
                SendMessage(new UbiMessage("OBS isn't enabled", EndPoint.Bot, EndPoint.Error));
                return Result.Failed;
            }

            List<Scene> scenes = obsRemote.Scenes.Where(s => s.name.ToLower().Contains(sceneMask.ToLower())).ToList();
            if( scenes.Count <= 0 )
                SendMessage(new UbiMessage("No scenes found!", EndPoint.Bot, EndPoint.Error));

            if (scenes.Count > 1)
            {
                var foundScenes = "";
                foreach (var scene in scenes)
                {
                    foundScenes += scene.name + ",";
                }
                if (!String.IsNullOrEmpty(foundScenes))
                    foundScenes = foundScenes.TrimEnd(',');


                SendMessage(new UbiMessage("Please clarify what scene:" + foundScenes, EndPoint.Bot, EndPoint.Notice));
            }
            else
            {
                obsRemote.SetCurrentScene(scenes[0].name);
            }

            return Result.Successful;
        }

        private Result GohaConfirmCommand(string confirmCode)
        {
            SendConfirmCodeToGohaIRC(confirmCode);
            return Result.Successful;
        }
        private Result GohaUpdatePassword()
        {
            SendUpdatePassToGohaIRC();
            return Result.Successful;
        }
        private Result ReplyCommand( string switchto, UbiMessage message)
        {
            if (!SwitchToChat(switchto))
                return Result.Failed;


            if (currentChat != lastMessageSent.FromEndPoint)
            {
                var chatAlias = chatAliases.Where(ca => ca.Endpoint == lastMessageSent.FromEndPoint).FirstOrDefault();
                if (chatAlias == null)
                {
                    SendMessage(new UbiMessage(
                        String.Format("I can't replay to a message from ({0})!", lastMessageSent.FromEndPoint.ToString()),
                        EndPoint.Bot, EndPoint.Error)
                    );
                }
                else
                {
                    currentChat = lastMessageSent.FromEndPoint;
                }
            }

            message.FromEndPoint = EndPoint.SteamAdmin;
            message.ToEndPoint = currentChat;
            if( message.Text.Length > 0 )
                SendMessage(message);

            return Result.Successful;
        }
        private bool isFlood( UbiMessage message)
        {
            try
            {
                if (lastMessagePerEndpoint.FirstOrDefault(m => (m.Text == message.Text && m.ToEndPoint == m.ToEndPoint)) != null)
                    return true;
                else
                    lastMessagePerEndpoint.RemoveAll(m => m.ToEndPoint == message.ToEndPoint);

                lastMessagePerEndpoint.Add(message);
            }
            catch {
                Debug.Print("Exception in isFlood()");
            }
            return false;

        }
        private void SendMessage(UbiMessage message)
        {
            
            lock (lockSendMessage)
            {
                if (message == null)
                    return;

                message.Text = HttpUtility.HtmlDecode( message.Text.Trim() );
                message.Text = Regex.Replace(message.Text, @"<.*?>(.*)<\/.*?>", "$1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                message.Text = Regex.Replace(message.Text, @"\[.*?\](.*)\[\/.*?\]", "$1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                message.Text = message.Text.Replace(@"\", @"\\");    
                if (message.FromEndPoint != EndPoint.Console &&
                    message.FromEndPoint != EndPoint.SteamAdmin &&
                    message.FromEndPoint != EndPoint.Bot)
                    lastMessageSent = message;

                if (message.Text.Length <= 0)
                    return;

                if (settings.generalSuppressErrors && message.ToEndPoint == EndPoint.Error)
                    return;

                if (settings.generalSuppressNotices && message.ToEndPoint == EndPoint.Notice)
                    return;

                // Execute command or write it to console
                if (message.FromEndPoint == EndPoint.Console ||
                    message.FromEndPoint == EndPoint.SteamAdmin)
                {
                    if (ParseAdminCommand(message.Text) == Result.Successful)
                    {
                        log.WriteLine(new UbiMessage( message.Text ), ChatIcon.Admin);
                        return;
                    }
                    if (!isFlood(message))
                        log.WriteLine(new UbiMessage( message.Text ), ChatIcon.Admin);

                    if (message.ToEndPoint == EndPoint.Console)
                        return;

                    message.ToEndPoint = currentChat;
                }


                /*var text = message.Text;

                if (text.Contains("gif-maniac.net"))
                    SendMessageToTwitchIRC(new UbiMessage("/ban " + message.FromName, EndPoint.Console));

                if (!String.IsNullOrEmpty(message.FromGroupName))
                    text = settings.appearanceGrpMessageFormat;
                else if (!String.IsNullOrEmpty(message.FromName))
                    text = settings.appearanceMsgFormat;

                */
                
                /*
                message.Text = text;
                */
                // Send message to specified chat(s)
                if (isClosing)
                    return;

                switch (message.ToEndPoint)
                {
                    case EndPoint.All:
                        {
                            ThreadPool.QueueUserWorkItem(arg => SendToAll(message));
                        }
                        break;
                    case EndPoint.Goodgame:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToGoodgame(message));
                        break;
                    case EndPoint.Sc2Tv:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSc2Tv(message));
                        break;
                    case EndPoint.Skype:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSkype(message));
                        break;
                    case EndPoint.SkypeGroup:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSkype(message));
                        break;
                    case EndPoint.SteamAdmin:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSteamAdmin(message));
                        break;
                    case EndPoint.TwitchTV:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToTwitchIRC(message));
                        break;
                    case EndPoint.Gohatv:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToGohaIRC(message));
                        break;
                    case EndPoint.Empiretv:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToEmpireTV(message));
                        break;
                    case EndPoint.Cybergame:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToCybergame(message));
                        break;
                    case EndPoint.Hashd:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToHashd(message));
                        break;
                    case EndPoint.JetSet:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToJetSet(message));
                        break;
                    case EndPoint.HitBox:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToHitBox(message));
                        break;
                    case EndPoint.Console:
                        log.WriteLine(new UbiMessage(message.Text));
                        break;
                    case EndPoint.Notice:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSteamAdmin(message));
                        break;
                    case EndPoint.Error:
                        ThreadPool.QueueUserWorkItem(f => SendMessageToSteamAdmin(message));
                        break;
                    default:
                        log.WriteLine(new UbiMessage( "Can't send a message. Chat is readonly!"));
                        break;
                }
                if (!isFlood(message))
                {
                    if (obsRemote != null && settings.obsRemoteEnable && settings.obsChatSwitch && !String.IsNullOrEmpty(settings.obsChatSourceName))
                    {
                        obsRemote.SetSourceRendererPart(settings.obsChatSourceName, true);
                        Int32 timeout = 0;
                        Int32.TryParse(settings.obsChatTimeout, out timeout);
                        if (timeout > 0)
                        {
                            obsChatSourceSwitch.Change( timeout * 1000, Timeout.Infinite );
                        }
                    }

                    bool highlight = false;
                    highlight = (!String.IsNullOrEmpty(message.ToName) && chatAliases.FirstOrDefault(c => c.MyNick == message.ToName && c.Endpoint == message.FromEndPoint) != null);

                    if (!highlight &&                        
                        chatAliases.FirstOrDefault(c => !String.IsNullOrEmpty(c.MyNick) && message.Text.Length > c.MyNick.Length && c.MyNick.ToLower() + "," == message.Text.ToLower().Substring(0, c.MyNick.Length) + ","
                        && c.Endpoint == message.FromEndPoint) != null)
                        highlight = true;

                    Color? foreColor = null;
                    Color? backColor = null;

                    
                    if (message.FromEndPoint == EndPoint.TwitchTV && message.Text.StartsWith("\x0001ACTION") )
                    {
                        message.Text = message.Text.Replace("\x0001","").Replace("ACTION ", "");
                        foreColor = settings.twitchMeForeColor;
                        backColor = settings.twitchMeBackcolor;
                    }


                    if (settings.webEnable && webChat != null)
                    {

                        webChat.AddMessage(
                            message.ImagePath,
                            message.Text,
                            message.FromName,
                            message.ToName,
                            DateTime.Now.GetDateTimeFormats('T')[0],
                            String.IsNullOrEmpty(message.ToName) ? "" : "->",
                            message.FromEndPoint.ToString(),
                            highlight
                            );
                    }
                    //Debug.Print(String.Format("{0} (highlight={1})", message.Text, highlight));
                    log.WriteLine( message, message.Icon, highlight, foreColor, backColor);
                    if (message.Icon == ChatIcon.Sc2Tv)
                    {
                        if (message.Text.Contains(":s:"))
                        {
                            String m = message.Text;
                            for (int i = 0; i < m.Length; i++)
                            {
                                int smilePos = -1;
                                if (m.Substring(i).IndexOf(":s:") == 0)
                                {
                                    foreach (Smile smile in sc2tv.smiles)
                                    {
                                        smilePos = m.Substring(i).IndexOf(":s" + smile.Code);
                                        if (smilePos == 0)
                                        {
                                            log.ReplaceSmileCode(":s" + smile.Code, smile.bmp);
                                            break;
                                        }
                                    }
                                }
                            }

                            textMessages.ScrollToEnd();
                        }
                    }
                    else if (message.Icon == ChatIcon.TwitchTv)
                    {
                        var regex = new Regex(@"[\s\t\r\n\p{P}]");
                        var msgText = message.Text;
                        var words = regex.Split(msgText).Where(x => !string.IsNullOrEmpty(x));
                        foreach (var smile in TwitchSmiles.Smiles)
                        {
                            log.ReplaceSmileCode(smile.Code, smile.Smile);
                        }

                    }
                }
            }
        }
        private void SendToAll(UbiMessage message)
        {
            try
            {
                SendMessageToEmpireTV(message);
                SendMessageToGohaIRC(message);
                SendMessageToTwitchIRC(message);
                SendMessageToSc2Tv(message);
                SendMessageToGoodgame(message);
                SendMessageToCybergame(message);
                SendMessageToHashd(message);
                SendMessageToJetSet(message);
                SendMessageToHitBox(message);
            }
            catch(Exception e )
            {
                Debug.Print("Error sending a message to all {0} {1}", e.Message, e.StackTrace);
            }
        }
        private void SendMessageToGoodgame(UbiMessage message)
        {
            if (ggChat == null)
                return;

            if ( ggChat.isLoggedIn && settings.goodgameEnabled)
            {
                ggChat.SendMessage(message.Text);
            }
        }
        private void SendMessageToSc2Tv(UbiMessage message)
        {
            if (sc2tv == null)
                return;

            if (sc2tv.LoggedIn && settings.sc2tvEnabled)
            {
                if (!sc2tv.SendMessage(message.Text))
                    SendMessage(new UbiMessage("Sc2tv: I didn't send a message. Try again!", EndPoint.Sc2Tv, EndPoint.Error));
            }
        }
        private void SendMessageToSkype(UbiMessage message)
        {
        }
        private void SendMessageToSteamAdmin(UbiMessage message)
        {
            if (steamAdmin == null || steamBot == null)
                return;

            if (steamBot.loginStatus == SteamAPISession.LoginStatus.LoginSuccessful)
            {
                if (settings.skypeSkipGroupMessages && message.FromEndPoint == EndPoint.SkypeGroup)
                    return;
                if (steamAdmin.status != SteamAPISession.UserStatus.Online)
                    return;

                var text = settings.appearanceMsgFormat;

                text = text.Replace(@"%t", message.Text);
                text = text.Replace(@"%s", message.FromName == null ? String.Empty : message.FromName);
                text = text.Replace(@"%d", message.ToName == null ? String.Empty : "->" + message.ToName);
                text = text.Replace(@"%c", message.FromEndPoint.ToString());
                text = text.Replace(@"%sg", message.FromGroupName == null ? String.Empty : message.FromGroupName);

                steamBot.SendMessage(steamAdmin, text);
            }
        }
        private void SendMessageToTwitchIRC(UbiMessage message)
        {
            if (!settings.twitchEnabled || twitchIrc == null)
                return;
            
            if( twitchIrc.IsRegistered &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                var channelName = "#" + settings.TwitchUser.ToLower();
                var twitchChannel = twitchIrc.Channels.SingleOrDefault(c => c.Name == channelName);
                twitchIrc.LocalUser.SendMessage(twitchChannel, message.Text);
            }

        }
        private void SendMessageToGohaIRC(UbiMessage message)
        {
            if (gohaIrc == null || !gohaIrc.IsRegistered)
                return;

            if (settings.gohaEnabled &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                var channelName = "#" + settings.GohaIRCChannel;
                var gohaChannel = gohaIrc.Channels.SingleOrDefault(c => c.Name == channelName);
                gohaIrc.LocalUser.SendMessage(gohaChannel, message.Text);
            }

        }
        private void SendMessageToCybergame(UbiMessage message)
        {
            if (cybergame == null || !cybergame.isLoggedIn )
                return;

            if (settings.cyberEnabled &&                
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                cybergame.SendMessage(message.Text);
            }

        }
        private void SendMessageToHitBox(UbiMessage message)
        {
            if (hitbox == null || !hitbox.IsLoggedIn)
                return;

            if (settings.hitboxEnable &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                hitbox.SendMessage(message.Text);
            }

        }

        private void SendMessageToJetSet(UbiMessage message)
        {
            if (jetset == null || !jetset.IsLoggedIn)
                return;

            if (settings.jetsetEnable &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                jetset.SendMessage(message.Text);
            }

        }
        private void SendMessageToHashd(UbiMessage message)
        {
            if (hashd == null)
                return;

            if (settings.hashdEnabled &&
                hashd.isLoggedIn &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                hashd.SendMessage(message.Text);
            }

        }
        private void SendRegisterInfoToGohaIRC(string email)
        {
            if (settings.gohaEnabled &&
                !string.IsNullOrEmpty(settings.GohaPassword) &&
                !string.IsNullOrEmpty(settings.GohaUser))
            {
                gohaIrc.LocalUser.SendMessage("NickServ", String.Format("REGISTER {0} {1}",settings.GohaPassword, email));
            }
        }
        private void SendConfirmCodeToGohaIRC(string confirmCode)
        {
            if (settings.gohaEnabled &&
                !string.IsNullOrEmpty(settings.GohaPassword) &&
                !string.IsNullOrEmpty(settings.GohaUser))
            {
                gohaIrc.LocalUser.SendMessage("NickServ", String.Format("VERIFY REGISTER {0} {1}",settings.GohaUser,confirmCode));
            }
        }
        private void SendUpdatePassToGohaIRC()
        {
            if (settings.gohaEnabled &&
                !string.IsNullOrEmpty(settings.GohaPassword) &&
                !string.IsNullOrEmpty(settings.GohaUser))
            {
                gohaIrc.LocalUser.SendMessage("NickServ", String.Format("SET PASSWORD {0}", settings.GohaPassword));
            }
        }
        private void SendMessageToEmpireTV(UbiMessage message)
        {
            if (empireTV == null)
                return;
            if (settings.empireEnabled && empireTV.LoggedIn &&
                (message.FromEndPoint == EndPoint.Console || message.FromEndPoint == EndPoint.SteamAdmin))
            {
                empireTV.SendMessage(message.Text);
            }
        }
        private void pictureCurrentChat_Click(object sender, EventArgs e)
        {
            pictureCurrentChat.ContextMenuStrip.Show();
        }
        private void SwitchPlayersOn(bool switchGoha, bool switchSc2tv)
        {
            Debug.Print("Switching players on...");
            if (switchGoha && gohaTVstream != null)
            {
                if (gohaTVstream.LoggedIn && gohaTVstream.StreamStatus == "off")
                {
                    gohaTVstream.SwitchStream();
                    if (gohaTVstream.StreamStatus == "off")
                    {
                        streamStatus.SetOff(pictureGohaStream);
                        MessageBox.Show("Goha Live Stream Player wasn't switched on! Do it manually!");
                    }
                    else
                    {
                        SendMessage(new UbiMessage(String.Format("Goha: Live Stream Player switched on!"), EndPoint.Gohatv, EndPoint.Notice));
                        streamStatus.SetOn(pictureGohaStream);
                    }
                }
            }

            if (switchSc2tv && sc2tv != null)
            {
                if (sc2tv.LoggedIn && !sc2tv.ChannelIsLive)
                {
                    sc2tv.setLiveStatus(true);                    
                    if (!sc2tv.ChannelIsLive)
                    {
                        streamStatus.SetOff(pictureSc2tvStream);
                        MessageBox.Show("Sc2tv Live Stream Player wasn't switched on! Do it manually!");                        
                    }
                    else
                    {
                        SendMessage(new UbiMessage(String.Format("Sc2Tv: Live Stream Player switched on!"), EndPoint.Sc2Tv, EndPoint.Notice));
                        streamStatus.SetOn(pictureSc2tvStream);
                    }
                }
            }

        }
        private void SwitchPlayersOff( bool switchGoha, bool switchSc2tv)
        {
            Debug.Print("Switching players off");
            if( switchGoha && gohaTVstream != null )
            {
                if (gohaTVstream.LoggedIn && gohaTVstream.StreamStatus == "on")
                {
                    gohaTVstream.SwitchStream();
                    if (gohaTVstream.StreamStatus == "on")
                    {
                        MessageBox.Show("Goha Live Stream Player wasn't switched off! Do it manually!");
                    }
                    else
                    {
                        SendMessage(new UbiMessage(String.Format("Goha: Live Stream Player switched off!"), EndPoint.Gohatv, EndPoint.Notice));
                        streamStatus.SetOff(pictureGohaStream);
                    }
                }
            }

            if (switchSc2tv && sc2tv != null)
            {
                if( sc2tv.LoggedIn && sc2tv.ChannelIsLive )
                {
                    sc2tv.setLiveStatus(false);
                    if (sc2tv.ChannelIsLive)
                    {
                        MessageBox.Show("Sc2tv Live Stream Player wasn't switched off! Do it manually!");
                    }
                    else
                    {
                        SendMessage(new UbiMessage(String.Format("Sc2Tv: Live Stream Player switched off!"), EndPoint.Sc2Tv, EndPoint.Notice));
                        streamStatus.SetOff(pictureSc2tvStream);
                    }
                }
            }

        }
        private void ForceClose(object o)
        {
            Process.GetCurrentProcess().Kill();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (isClosing)
                    return;
           

                //var profiles = new ChatProfiles();
                //profiles.Profiles = new List<ChatProfile>();

                //profiles.Profiles.Add(new ChatProfile() { Topic = "Test", ChatName = "Chat name", Game = "test", LongDescription = "test2", ShortDescription = "test3"});
                //settings.chatProfiles = profiles;


                isClosing = true;

                forceCloseTimer.Change(5000, Timeout.Infinite);
                currentChat = EndPoint.Console;
                SendMessage(new UbiMessage(String.Format("Leaving chats..."), EndPoint.Console, EndPoint.Notice));

                if( settings.globalDebug && debugForm != null)
                    debugForm.Hide();

                #region Save settings
                if (this.WindowState != FormWindowState.Minimized)
                {
                    settings.mainformWidth = Size.Width;
                    settings.mainformHeight = Size.Height;
                }

                if (settings.sc2tvEnabled)
                {
                    settings.sc2LastMsgId = sc2tv.LastMessageId;
                }
                if (settings.youtubeEnable)
                {
                    settings.youtubeLastTime = youtube.LastTime;
                }

                settings.chatProfiles.WriteProfile(settings.currentProfile, settings);
            
                settings.Save();
                #endregion

                //this.Visible = false;
                e.Cancel = true;
                timerEverySecond.Enabled = false;


                SwitchPlayersOff( true, true );

                var b1 = new BGWorker( StopTwitchIRC, null );
                var b2 = new BGWorker( StopGohaIRC, null );
                var b3 = new BGWorker( StopSteamBot, null );
                var b4 = new BGWorker(StopSc2Chat, null);
                var b5 = new BGWorker(StopGoodgame, null);

                if( twitchTV != null )
                    twitchTV.Stop();
                if(empireTV != null )
                    empireTV.Stop();
                if(hashd != null)
                    hashd.Stop();
                if(cybergame != null)
                    cybergame.Stop();
                if (youtube != null)
                    youtube.Stop();
                if (gamerstv != null)
                    gamerstv.Stop();
                if (jetset != null)
                    jetset.Stop();
                if (hitbox != null)
                    hitbox.Stop();
                
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + " " + ex.StackTrace);
            }
        }
        private void StopTwitchIRC()
        {
            if (twitchIrc != null)
            {
                if (twitchIrc.IsRegistered)
                {

                    twitchIrc.Quit();
                    SendMessage(new UbiMessage(String.Format("TwitchTV: disconnected!"), EndPoint.TwitchTV, EndPoint.Notice));
                    chatStatusTwitch.On = false;
                }
            }
        }
        private void StopGohaIRC()
        {
            if (gohaIrc != null)
            {
                if (gohaIrc.IsRegistered)
                {
                    gohaIrc.Disconnect(); //.Quit();
                    SendMessage(new UbiMessage(String.Format("GohaTV: disconnected."), EndPoint.Gohatv, EndPoint.Notice));
                    chatStatusGoha.On = false;
                }
            }
        }
        private void StopGoodgame()
        {
            if (ggChat == null || !settings.goodgameEnabled)
                return;

            ggChat.Stop();
            SendMessage(new UbiMessage(String.Format("Goodgame: disconnected."), EndPoint.Goodgame, EndPoint.Notice));
            chatStatusGoodgame.On = false;
        }
        private void StopSteamBot()
        {
            if (!settings.steamEnabled)
                return;

            bWorkerSteamPoll.CancelAsync();
            while (bWorkerSteamPoll.CancellationPending) Thread.Sleep(10);
            SendMessage(new UbiMessage(String.Format("Steam: disconnected."), EndPoint.Steam, EndPoint.Notice));
            chatStatusSteamBot.On = false;
        }
        private void StopSc2Chat()
        {
            if (settings.sc2tvEnabled)
            {
                sc2tv.Stop();
                SendMessage(new UbiMessage(String.Format("Sc2tv: disconnected."), EndPoint.Sc2Tv, EndPoint.Notice));
                chatStatusSc2tv.On = false;
            }  
        }
        private void RunWithTimeout(ParameterizedThreadStart start, int timeoutSec)
        {
            Thread t = new Thread(start);
            t.Start();
            if (!t.Join(TimeSpan.FromSeconds(timeoutSec)))
            {
                try
                {
                    t.Abort();
                }
                catch (Exception ex)
                {
                    Debug.Print(ex.Message + " " + ex.StackTrace);
                }
            }
        }
        private void textMessages_SizeChanged(object sender, EventArgs e)
        {
            textMessages.ScrollToEnd();

            var maxX = this.Width - panelTools.ClientRectangle.Width+1;
            var maxY = textMessages.Height - panelTools.Height+1;
            var minX = 0;
            var minY = 0;
            var newLocation = panelTools.Location;
            if (panelTools.Location.X < minX)
                newLocation.X = minX;
            if (panelTools.Location.X > maxX)
                newLocation.X = maxX;

            if (panelTools.Location.Y < minY)
                newLocation.Y = minY;
            if (panelTools.Location.Y > maxY)
                newLocation.Y = maxY;

            panelTools.Location = newLocation;
        }
        private void ShowSettings()
        {
            SettingsDialog settingsForm = new SettingsDialog();
            settingsForm.ShowDialog();
        }
        private Result ParseAdminCommand(string command)
        {

            var cmd = adminCommands.Where(ac => ac.isCommand(command)).FirstOrDefault();
            if (cmd != null)
            {
                cmd.Execute();
                return Result.Successful;
            }
            else
                return Result.Failed;
        }
        private void pictureGohaStream_Click(object sender, EventArgs e)
        {
            gohaTVstream.SwitchStream();
        }
        private void pictureSc2tvStream_Click(object sender, EventArgs e)
        {
            Debug.Print("Switching sc2tv player state...");
            if (!sc2tv.LoggedIn)
                return;

            var prevLiveStatus = sc2tv.ChannelIsLive;
            
            if (prevLiveStatus)
                sc2tv.setLiveStatus(false);
            else
                sc2tv.setLiveStatus(true);

            if (prevLiveStatus != sc2tv.ChannelIsLive)
            {
                if (prevLiveStatus)
                {
                    SendMessage(new UbiMessage(String.Format("Sc2Tv: Stream switched off!"), EndPoint.Sc2Tv, EndPoint.Notice));
                    streamStatus.SetOff(pictureSc2tvStream);
                }
                else
                {
                    streamStatus.SetOn(pictureSc2tvStream);
                    SendMessage(new UbiMessage(String.Format("Sc2Tv: Stream switched on!"), EndPoint.Sc2Tv, EndPoint.Notice));
                }
            }
            else
            {
                SendMessage(new UbiMessage(String.Format("Sc2Tv: Stream wasn't switched! Please try again!"), EndPoint.Sc2Tv, EndPoint.Error));
            }

        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            SwitchBorder();
        }
        private void SwitchBorder()
        {
            if (checkBoxBorder.Checked)
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            }
            else
            {
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }
        private void switchFullScreenMode()
        {
            if (panelMessages.Dock == DockStyle.Fill)
            {
                Size = settings.globalFullSize;
                settings.isFullscreenMode = false;
                panelBottom.Visible = true;
                panelMessages.Dock = DockStyle.None;
                panelMessages.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
                panelMessages.Height = panelBottom.Top - 2;
                panelMessages.Width = panelRight.Left - 2;
                
                textMessages.ScrollToEnd();
                button1.Show();
            }
            else
            {
                Size = settings.globalCompactSize;
                settings.isFullscreenMode = true;
                panelMessages.Dock = DockStyle.Fill;
                if (!settings.globalShowCommandBarInCompact)
                    panelBottom.Visible = false;
                button1.Hide();
                textMessages.ScrollToEnd();
            }

        }
        private void textMessages_DoubleClick(object sender, EventArgs e)
        {
            switchFullScreenMode();
        }
        private void hideTools()
        {
            if (_Offset != Point.Empty || _OffsetViewers != Point.Empty)
                return;
            if (checkBoxOnTop.Visible)
            {
                SetVisibility(panelTools, false);
                if (TransparencyKey != textMessages.BackColor)
                    SetTransparency(textMessages.BackColor, settings.globalUseChroma);
            }

        }
        private void showTools()
        {

            if (!checkBoxOnTop.Visible && settings.globalCompactSize != Size )
            {
                SetVisibility(panelTools, true);
            }
            if (!trackBarTransparency.Visible)
                SetVisibility(trackBarTransparency, true);

            SetTransparency(Color.Empty,settings.globalUseChroma);

           
        }
        private void SetTooltip(ToolTip tooltip, Control control, string value)
        {
            if (control.InvokeRequired)
            {
                SetTooltipCB d = new SetTooltipCB(SetTooltip);
                control.Parent.Invoke(d, new object[] { tooltip, control, value });
            }
            else
            {
                tooltip.SetToolTip(control, value);
            }
        }
        private void SetCheckedToolTip(ToolStripMenuItem item, bool state)
        {
            if (item == null)
                return;
            if (item.GetCurrentParent() == null)
                return;
            if (item.GetCurrentParent().InvokeRequired)
            {
                SetCheckedToolTipCB d = new SetCheckedToolTipCB(SetCheckedToolTip);
                item.GetCurrentParent().Invoke(d, new object[] { item, state });
            }
            else
            {
                item.Checked = state;
            }
        }
        private void SetTransparency(Color color, bool chroma = false)
        {
            if (this.InvokeRequired)
            {
                SetTransparencyCB d = new SetTransparencyCB(SetTransparency);
                this.Invoke(d, new object[] { color,chroma });
            }
            else
            {
                try
                {
                    if (chroma)
                    {
                        this.TransparencyKey = textMessages.BackColor;
                    }
                    else
                    {
                        this.TransparencyKey = new Color();
                        if (color == Color.Empty)
                        {
                            if (this.Opacity != 1)
                            {
                                this.AllowTransparency = false;
                                this.Opacity = 1;

                            }
                        }
                        else
                        {
                            if (this.Opacity != settings.globalTransparency / 100.0f)
                            {
                                this.AllowTransparency = true;
                                this.Opacity = settings.globalTransparency / 100.0f;
                            }
                        }
                    }
                }
                catch(Exception e) {
                    Debug.Print(e.Message + " " + e.StackTrace);
                }
            }
        }
        private void SetVisibility(Control control, bool visibility)
        {
            if (control.InvokeRequired)
            {
                SetVisibilityCB d = new SetVisibilityCB(SetVisibility);
                control.Parent.Invoke(d, new object[] { control, visibility });
            }
            else
            {
                control.Visible = visibility;
            }
        }
        private void SetTopMost(Form form, bool topmost)
        {
            if (form.InvokeRequired)
            {
                SetTopMostCB cb = new SetTopMostCB(SetTopMost);
                form.Invoke(cb, new object[] { form, topmost });
            }
            else
            {
                form.TopMost = topmost;
            }
        }
        private void SetComboValue(ComboBox combo, object value)
        {
            if (combo.InvokeRequired)
            {
                SetComboValueCB cb = new SetComboValueCB(SetComboValue);
                combo.Parent.Invoke(cb, new object[] {combo, value });
            }
            else
            {
                combo.SelectedValue = value;
            }
        }
        private void textMessages_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _OffsetForm = new Point(e.X, e.Y);
                Cursor = Cursors.SizeAll;
            }

        }
        private void textMessages_MouseMove(object sender, MouseEventArgs e)
        {
            if (_OffsetForm != Point.Empty)
            {
                Point newlocation = this.Location;

                newlocation.X += e.X - _OffsetForm.X;
                newlocation.Y += e.Y - _OffsetForm.Y;
                this.Location = newlocation;
            }

            showTools();
        }
        private void textMessages_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _OffsetForm = Point.Empty;
                Cursor = Cursors.Default;
            }
        }
        private void timerEverySecond_Tick(object sender, EventArgs e)
        {
            if (settings == null)
                return;
            try
            {
                UpdateStyles();
                UInt32 twitchViewers = 0, cybergameViewers = 0, hashdViewers = 0, youtubeViewers = 0, goodgameViewers = 0, hitboxViewers = 0;
                if (twitchChannel != null)
                    UInt32.TryParse(twitchChannel.Viewers, out twitchViewers);
                
                if (cybergame != null)
                    UInt32.TryParse(cybergame.Viewers, out cybergameViewers);

                if (hashd != null)
                    UInt32.TryParse(hashd.Viewers, out hashdViewers);

                if (youtube != null)
                    youtubeViewers = youtube.Viewers;

                if (hitbox != null)
                    hitboxViewers = hitbox.Viewers;


                if (ggChat != null && ggChat.isLoggedIn)
                {
                    UInt32.TryParse(ggChat.FlashViewers, out goodgameViewers);

                    uint nonGGCount = 0;
                    if (ggChat.ServiceNames.Contains("twitch.tv"))
                        nonGGCount += twitchViewers;
                    if (ggChat.ServiceNames.Contains("cybergame.tv"))
                        nonGGCount += cybergameViewers;
                    if (ggChat.ServiceNames.Contains("hashd.tv"))
                        nonGGCount += hashdViewers;
                    if (ggChat.ServiceNames.Contains("youtube.com"))
                        nonGGCount += youtubeViewers;
                    if (ggChat.ServiceNames.Contains("hitbox.tv"))
                        nonGGCount += hitboxViewers;
                    if (goodgameViewers >= nonGGCount)
                        goodgameViewers -= nonGGCount;
                    else
                        goodgameViewers = 0;
                }

                var totalViewers = cybergameViewers + twitchViewers + hashdViewers + youtubeViewers + goodgameViewers + hitboxViewers;

                if (MaxViewers < totalViewers)
                    MaxViewers = totalViewers;

                var viewersText = String.Format("{0}", totalViewers);

                if (counterCybergame.Visible)
                    counterCybergame.Counter = cybergameViewers.ToString();
                if (counterTwitch.Visible)
                    counterTwitch.Counter = twitchViewers.ToString();
                if (counterHash.Visible)
                    counterHash.Counter = hashdViewers.ToString();
                if (youtube != null && counterYoutube.Visible)
                    counterYoutube.Counter = youtubeViewers.ToString();
                if (ggChat != null && counterGoodgame.Visible)
                    counterGoodgame.Counter = goodgameViewers.ToString();
                if (counterHitBox.Visible)
                    counterHitBox.Counter = hitboxViewers.ToString();

                if (viewersText != labelViewers.Text)
                {
                    labelViewers.Text = viewersText;
                    SetTooltip(viewersTooltip, labelViewers, String.Format("Twitch.tv: {0}, Cybergame.tv: {1}, Hashd.tv: {2}, Max total: {3}", twitchViewers, cybergameViewers, hashdViewers, MaxViewers));
                }
                if (trackBarTransparency.ClientRectangle.Contains(trackBarTransparency.PointToClient(Cursor.Position)))
                    return;

                if ((!ClientRectangle.Contains(PointToClient(Cursor.Position))) || settings.globalCompactSize == Size)
                {
                    hideTools();
                }
                else if (settings.globalCompactSize != Size)
                {
                    showTools();
                }

                if (settings.webEnable && webChat != null)
                {
                    if (settings.obsRemoteEnable && obsRemote != null)
                    {
                        //webChat.MicOn = obsRemote.Status.
                        webChat.ObsBitrate = (obsRemote.Status.bitrate * 8 / 1024).ToString();
                        webChat.ObsFrameDrops = obsRemote.Status.framesDropped.ToString();
                        webChat.MicOn = !obsRemote.MicMuted;

                    }
                    webChat.Viewers = totalViewers.ToString();
                }
                if (settings.obsRemoteEnable && !checkBoxBorder.Checked)
                {
                    if (obsRemote != null)
                    {
                        try
                        {
                            var stats = String.Format(
                                " FPS: {0} RATE: {1}K DROPS: {2}",
                                obsRemote.Status.fps,
                                obsRemote.Status.bitrate / 1024 * 8,
                                obsRemote.Status.framesDropped);
                            this.Text = formTitle + stats;
                        }
                        catch (Exception ex)
                        {
                            Debug.Print(ex.Message + " " + ex.StackTrace);
                        }
                    }
                }
            }
            catch (Exception ex )
            {
                Debug.Print("Timer everysecond exception {0} {1}", ex.Message, ex.StackTrace);
            }

        }
        private void trackBarTransparency_MouseMove(object sender, MouseEventArgs e)
        {
            SetTransparency(textMessages.BackColor);
        }
        private void buttonFullscreen_Click_1(object sender, EventArgs e)
        {
            switchFullScreenMode();
        }
        private void contextMenuChat_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            SwitchToChat(chatAliases[contextMenuChat.Items.IndexOf(e.ClickedItem)].Alias);
        }
        private void pictureCurrentChat_Click_1(object sender, EventArgs e)
        {
            contextMenuChat.Show(Cursor.Position);
        }
        private void buttonStreamStartStop_Click(object sender, EventArgs e)
        {

            StartStopOBSStream();
        }
        private void contextSceneSwitch_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            //e.Cancel = !btnClose;
            //btnClose = true;
        }
        private void textMessages_TextChanged(object sender, EventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (settings != null && settings.globalDebug && debugForm != null)
                debugForm.Show();
        }
        private void textMessages_SelectionChanged(object sender, EventArgs e)
        {
            //if (textMessages.SelectionType == RichTextBoxSelectionTypes.Object)
            //{/
            //    textMessages.Select(textMessages.GetCharIndexFromPosition(textMessages.PointToClient(Cursor.Position)), 0);
            //}

        }
        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (panelMessages.Dock == DockStyle.Fill)
                settings.globalCompactSize = this.Size;
            else
                settings.globalFullSize = this.Size;
            
            //UpdateChatEnable();

        }
        #endregion
        #region HitBox
        private void ConnectHitBox()
        {
            if (isClosing)
                return;
            if (!settings.hitboxEnable)
                return;

            hitbox = new HitBox(settings.hitboxUser, settings.hitboxPassword);
            hitbox.LastTimeStamp = settings.hitboxLastTimeStamp;
            hitbox.OnMessageReceived += new EventHandler<HitBoxMessage>(hitbox_OnMessageReceived);
            hitbox.OnLogin += new EventHandler<EventArgs>(hitbox_OnLogin);

            hitbox.Start();
            if (!hitbox.Login())
            {
                SendMessage(new UbiMessage("Hitbox: login failed!", EndPoint.HitBox, EndPoint.Error));
            }
            else
            {
                SendMessage(new UbiMessage("Hitbox: logged in!", EndPoint.HitBox, EndPoint.Error));
            }

        }

        void hitbox_OnLogin(object sender, EventArgs e)
        {
            chatStatusHitBox.On = true;
        }

        void hitbox_OnMessageReceived(object sender, HitBoxMessage e)
        {
            if (!settings.hitboxEnable)
                return;

            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.HitBox, EndPoint.SteamAdmin)
            {
                FromName = e.User,
                NickColor = settings.hitboxNickColor,
                TextOnly = false
            });
            settings.hitboxLastTimeStamp = hitbox.LastTimeStamp;
        }

        #endregion

        #region JetSet
        private void ConnectJetSet()
        {
            if (isClosing)
                return;
            if (!settings.jetsetEnable)
                return;
            jetset = new JetSetPro(settings.jetsetUser, settings.jetsetPassword);
            jetset.LastTimeStamp = settings.jetsetLastTimeStamp;
            jetset.OnMessageReceived += new EventHandler<JetSetMessage>(jetset_OnMessageReceived);
            jetset.OnLogin += new EventHandler<EventArgs>(jetset_OnLogin);
            jetset.Start();
            jetset.Login();
        }

        void jetset_OnLogin(object sender, EventArgs e)
        {
            chatStatusJetSet.On = true;
        }

        void jetset_OnMessageReceived(object sender, JetSetMessage e)
        {
            if (!settings.jetsetEnable)
                return;

            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.JetSet, EndPoint.SteamAdmin)
            {
                FromName = e.User,
                NickColor = settings.jetsetNickColor,
                TextOnly = false
            });
            settings.jetsetLastTimeStamp = jetset.LastTimeStamp;
        }

        #endregion
        #region GamersTV
        private void ConnectGamersTV()
        {
            if (isClosing)
                return;
            if (!settings.gmtvEnabled)
                return;

            if (String.IsNullOrEmpty(settings.gmtChannelUrl))
                return;

            gamerstv = new GamersTv();
            gamerstv.ChannelUrl = settings.gmtChannelUrl;
            gamerstv.LastDate = settings.gmtLastdate;
            gamerstv.OnMessageReceived += new EventHandler<GamersTVMessage>(gamerstv_OnMessageReceived);
            gamerstv.OnLogin += new EventHandler<EventArgs>(gamerstv_OnLogin);
            gamerstv.Start();
        }

        void gamerstv_OnLogin(object sender, EventArgs e)
        {
            chatStatusGamerTv.On = true;
        }

        void gamerstv_OnMessageReceived(object sender, GamersTVMessage e)
        {
            if (!settings.gmtvEnabled)
                return;

            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.GamersTV, EndPoint.SteamAdmin)
            {
                FromName = e.User,
                NickColor = settings.gmtvColor,
                TextOnly = false
            });
            settings.gmtLastdate = gamerstv.LastDate;
        }

        #endregion
        #region Youtube
        private void ConnectYoutube()
        {
            if (isClosing)
                return;
            if (!settings.youtubeEnable)
                return;

            if (String.IsNullOrEmpty(settings.youtubeID))
                return;

            youtube = new YouTube(settings.youtubeID, settings.youtubeLastTime);
            youtube.OnMessageReceived += new EventHandler<YouTube.YoutubeMessage>(youtube_OnMessageReceived);

            youtube.Start();

        }

        void youtube_OnMessageReceived(object sender, YouTube.YoutubeMessage e)
        {
            if (!settings.youtubeEnable)
                return;

            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.Youtube, EndPoint.SteamAdmin) 
            { 
                FromName = e.User,
                NickColor = settings.youtubeNickColor,
                TextOnly = false
            });

        }
        #endregion


        #region Cybergame methods and events
        private void ConnectHashd()
        {
            if (isClosing)
                return; 
            if (!settings.hashdEnabled ||
                String.IsNullOrEmpty(settings.hashdUser) ||
                String.IsNullOrEmpty(settings.hashdPassword))
                return;
            hashd = new Hashd(settings.hashdUser, settings.hashdPassword);
            hashd.Live += new EventHandler<EventArgs>(hashd_Live);
            hashd.Offline += new EventHandler<EventArgs>(hashd_Offline);
            hashd.OnLogin += new EventHandler<EventArgs>(hashd_OnLogin);
            hashd.OnMessage += new EventHandler<Hashd.HashdMessageEventArgs>(hashd_OnMessage);

            if (!hashd.Login())
            {
                SendMessage(new UbiMessage("Hashd: login failed!", EndPoint.Hashd, EndPoint.Error));
            }
            else
            {
                hashd.Start();
            }
        }

        void hashd_OnMessage(object sender, Hashd.HashdMessageEventArgs e)
        {
            if (e.Message.User.ToLower() == hashd.User)
                return;

            SendMessage(new UbiMessage(String.Format("{0}", e.Message.Text), EndPoint.Hashd, EndPoint.SteamAdmin) 
            { 
                FromName = e.Message.User,
                NickColor = settings.hashdNickColor,
                TextOnly = false

            });
        }

        void hashd_OnLogin(object sender, EventArgs e)
        {
            chatStatusHashd.On = true;
            SendMessage(new UbiMessage("Hashd: logged in!", EndPoint.Hashd, EndPoint.Notice));
        }

        void hashd_Offline(object sender, EventArgs e)
        {
            streamStatus.SetOff(pictureHashdStream);
        }

        void hashd_Live(object sender, EventArgs e)
        {
            streamStatus.SetOn(pictureHashdStream);
        }
        #endregion

        #region Cybergame methods and events
        private void ConnectCybergame()
        {
            if (isClosing)
                return;

            if (!settings.cyberEnabled ||
                String.IsNullOrEmpty(settings.cyberUser) ||
                String.IsNullOrEmpty(settings.cyberPassword))
                return;
            cybergame = new Cybergame(settings.cyberUser, settings.cyberPassword);
            cybergame.Live += new EventHandler<EventArgs>(cybergame_Live);
            cybergame.Offline += new EventHandler<EventArgs>(cybergame_Offline);
            cybergame.OnChatMessage += new EventHandler<MessageReceivedEventArgs>(cybergame_OnMessage);
            cybergame.OnLogin += new EventHandler<EventArgs>(cybergame_OnLogin);
            cybergame.Start();

            if (!cybergame.Login())
            {
                SendMessage(new UbiMessage("Cybergame: login failed!", EndPoint.Cybergame, EndPoint.Error));
            }
        }

        void cybergame_OnLogin(object sender, EventArgs e)
        {
            chatStatusCybergame.On = true;
            SendMessage(new UbiMessage("Cybergame: logged in!", EndPoint.Cybergame, EndPoint.Notice));

            cybergame.GetDescription();

            if (String.IsNullOrEmpty(settings.cybergame_ShortDescription))
                settings.cybergame_ShortDescription = cybergame.ShortDescription;
            if (String.IsNullOrEmpty(settings.cybergame_Game))
                settings.cybergame_Game = cybergame.Game;
        }

        void cybergame_OnMessage(object sender, MessageReceivedEventArgs e)
        {
            SendMessage(new UbiMessage( String.Format("{0}",e.Message.message), EndPoint.Cybergame, EndPoint.SteamAdmin ) 
            {
                FromName = e.Message.alias,
                NickColor = settings.cyberNickColor,
                TextOnly = false

            });
        }
        void cybergame_Offline(object sender, EventArgs e)
        {
            streamStatus.SetOff(pictureCybergameStream);
            //throw new NotImplementedException();
        }
        void cybergame_Live(object sender, EventArgs e)
        {
            streamStatus.SetOn(pictureCybergameStream);
            //throw new NotImplementedException();
        }


        #endregion

        #region EmpireTV methods and events
        private void ConnectEmpireTV()
        {
            if (isClosing)
                return;

            if (!settings.empireEnabled || String.IsNullOrEmpty(settings.empireUser) || String.IsNullOrEmpty(settings.empirePassword))
                return;

            empireTV.OnLogin += OnEmpireLogin;
            empireTV.OnNewMessage += OnEmpireMessage;

            if( !empireTV.Login(settings.empireUser, settings.empirePassword) )
                SendMessage(new UbiMessage("EmpireTV: login failed!", EndPoint.Empiretv, EndPoint.Error));
        }
        private void OnEmpireLogin(object sender, EventArgs e)
        {
            chatStatusEmpire.On = true;
            empireTV.LoadHistory = settings.empireLoadHistory;
            empireTV.Start();
        }
        private void OnEmpireMessage(object sender, MessageArgs e)
        {
            if (e.Message.nick.ToLower() == settings.empireUser.ToLower())
                return;
            SendMessage(new UbiMessage(String.Format("{0}", e.Message.text), EndPoint.Empiretv, EndPoint.SteamAdmin) 
            {
                FromName = e.Message.nick,
                NickColor = settings.empireNickColor,
                TextOnly = false
            });
        }
        #endregion

        #region GohaTV Stream methods and events
        private void ConnectGohaStream()
        {
            if (isClosing)
                return;

            if (!settings.gohaEnabled || String.IsNullOrEmpty(settings.GohaUser) || String.IsNullOrEmpty(settings.GohaPassword))
                return;

            gohaTVstream.OnLogin += OnGohaStreamLogin;
            gohaTVstream.OnLive += OnGohaStreamLive;
            gohaTVstream.OnOffline += OnGohaStreamOffline;

            gohaTVstream.Login(settings.GohaUser, settings.GohaPassword);
        }
        private void OnGohaStreamLogin(object sender, EventArgs e)
        {
            GohaTVResult result;
            if (settings.gohaStreamControl && streamIsOnline && gohaTVstream.StreamStatus == "off")
            {
                 result = gohaTVstream.SwitchStream();
            }
            else if (settings.gohaStreamControlOnStartExit && gohaTVstream.StreamStatus == "off")
            {
                 result = gohaTVstream.SwitchStream();
            }
            chatStatusGohaWeb.On = true;

            gohaTVstream.GetDescription();
            if (String.IsNullOrEmpty(settings.goha_ShortDescription))
                settings.goha_ShortDescription = gohaTVstream.ShortDescription;
            if (String.IsNullOrEmpty(settings.goha_Game))
                settings.goha_Game = gohaTVstream.Game;

                
        }
        private void OnGohaStreamLive(object sender, EventArgs e)
        {
            streamStatus.SetOn(pictureGohaStream);
        }
        private void OnGohaStreamOffline(object sender, EventArgs e)
        {
            streamStatus.SetOff(pictureGohaStream);            
        }
        #endregion

        #region Twitch channel methods and events
        private void ConnectTwitchChannel()
        {
            if (isClosing)
                return;


            if (String.IsNullOrEmpty(settings.TwitchUser) ||
                !settings.twitchEnabled)
                return;
          
            twitchChannel = new Twitch(settings.TwitchUser.ToLower());
            twitchChannel.Live += OnGoLive;
            twitchChannel.Offline += OnGoOffline;
            twitchChannel.Start();
            adminCommands.Add(new AdminCommand(@"^/viewers\s*$", TwitchViewers));
            adminCommands.Add(new AdminCommand(@"^/bitrate\s*$", TwitchBitrate));
        }
        private Result TwitchViewers()
        {
            var m = new UbiMessage(String.Format("Twitch viewers: {0}", twitchChannel.Viewers), EndPoint.TwitchTV, EndPoint.SteamAdmin);
            SendMessage(m);
            return Result.Successful;
        }
        private Result TwitchBitrate()
        {
            var bitrate = (int)double.Parse(twitchChannel.Bitrate, NumberStyles.Float, CultureInfo.InvariantCulture);
            var m = new UbiMessage(String.Format("Twitch bitrate: {0}Kbit", bitrate), EndPoint.TwitchTV, EndPoint.SteamAdmin);
            SendMessage(m);
            return Result.Successful;
        }

        private void OnGoLive(object sender, EventArgs e)
        {
            if (streamIsOnline)
                return;

            Debug.Print("OnGoLive event");
            SendMessage(new UbiMessage(String.Format("Twitch: STREAM ONLINE!"), EndPoint.TwitchTV, EndPoint.Notice));
            if (settings.globalEnableSounds)
            {
                try
                {
                    WindowsMediaPlayer a = new WMPLib.WindowsMediaPlayer();

                    a.URL = settings.globalSoundOnlineFile;
                    a.controls.play();
                    var counter = 0;
                    while (a.playState == WMPPlayState.wmppsPlaying)
                    {
                        counter++;
                        if (counter > 300)
                            break;
                        Thread.Sleep(10);
                    }
                }
                catch
                {
                    Debug.Print("Exception in OnGoLive()");
                }
            }
           

            if (settings.gohaStreamControl)
            {                
                if (gohaTVstream.LoggedIn)
                {
                    if (gohaTVstream.StreamStatus == "off")
                    {
                        if (!streamIsOnline)
                        {
                            SendMessage(new UbiMessage(String.Format("Goha: Stream switched on!"), EndPoint.TwitchTV, EndPoint.Notice));
                            gohaTVstream.SwitchStream();
                        }
                    }
                }
            }
            if (settings.sc2StreamAutoSwitch)
            {
                if (sc2tv.LoggedIn)
                {
                    if (!sc2tv.ChannelIsLive )
                    {
                        sc2tv.setLiveStatus(true);
                        if (sc2tv.ChannelIsLive)
                        {
                            SendMessage(new UbiMessage(String.Format("Sc2Tv: Stream switched on (Twitch stream went online)!"), EndPoint.Sc2Tv, EndPoint.Notice));
                            streamStatus.SetOn(pictureSc2tvStream);
                        }
                        else
                        {
                            streamStatus.SetOff(pictureSc2tvStream);
                        }
                    }
                }
            }
            streamStatus.SetOn(pictureStream);

            streamIsOnline = true;
        }
        private void OnGoOffline(object sender, EventArgs e)
        {
            if (!streamIsOnline)
                return;

            Debug.Print("OnGoOffline event");
            SendMessage(new UbiMessage(String.Format("Twitch: STREAM OFFLINE!"), EndPoint.TwitchTV, EndPoint.Notice));
            if (settings.globalEnableSounds)
            {
                try
                {
                    WindowsMediaPlayer a = new WMPLib.WindowsMediaPlayer();
                    a.URL = settings.globalSoundOfflineFile;
                    a.controls.play();
                    var counter = 0;
                    while (a.playState == WMPPlayState.wmppsPlaying)
                    {
                        counter++;
                        if (counter > 300)
                            break;
                        Thread.Sleep(10);
                    }
                }
                catch
                {
                    Debug.Print("Exception in OnGoOffline()");
                }
            }
           
            if (settings.gohaStreamControl)
            {
                if (gohaTVstream.LoggedIn)
                {
                    if (gohaTVstream.StreamStatus == "on")
                    {
                        if (streamIsOnline)
                        {
                            SendMessage(new UbiMessage(String.Format("Goha: Stream switched off!"), EndPoint.TwitchTV, EndPoint.Notice));
                            gohaTVstream.SwitchStream();
                        }
                    }
                }
            }
            if (settings.sc2StreamAutoSwitch)
            {
                if (sc2tv.LoggedIn)
                {
                    if (sc2tv.ChannelIsLive)
                    {
                        sc2tv.setLiveStatus(false);
                        if (!sc2tv.ChannelIsLive)
                        {
                            SendMessage(new UbiMessage(String.Format("Sc2Tv: Stream switched off (Twitch stream went offline)!"), EndPoint.Sc2Tv, EndPoint.Notice));
                            streamStatus.SetOff(pictureSc2tvStream);
                        }
                        else
                        {
                            throw new Exception("Sc2tv stream wasn't switched! Do it manually!");
                        }
                    }
                }
            }
            streamStatus.SetOff(pictureStream);
            streamIsOnline = false;
            
        }
        #endregion

        #region Twitch IRC methods and events
        private void ConnectTwitchIRC()
        {
            lock (lockTwitchConnect)
            {
                if (isClosing)
                    return;

                //twitchIrc.FloodPreventer = new IrcStandardFloodPreventer(4, 1000);
                if (settings.TwitchUser.Length <= 0 ||
                    !settings.twitchEnabled)
                    return;


                try
                {
                    if (twitchIrc != null)
                    {
                        if (twitchIrc.IsConnected)
                            return;

                        twitchIrc.Disconnect();
                        twitchIrc = null;
                    }
                }
                catch
                {
                    Debug.Print("Exception in ConnectTwitchIRC()");
                }

                //TwitchAPI ttvApi = new TwitchAPI();
                //ttvApi.GetToken( settings.TwitchUser.ToLower(), settings.TwitchPassword);
                twitchPing = new System.Threading.Timer(new TimerCallback(TwitchPingTick), null, Timeout.Infinite, Timeout.Infinite);
                twitchDisconnect = new System.Threading.Timer(new TimerCallback(TwitchDisconnectNoPong), null, Timeout.Infinite, Timeout.Infinite);

                twitchIrc = new IrcClient();

                twitchIrc.Connected += OnTwitchConnect;
                twitchIrc.Registered += OnTwitchRegister;
                twitchIrc.Disconnected += OnTwitchDisconnect;
                twitchIrc.Error += new EventHandler<IrcErrorEventArgs>(twitchIrc_Error);
                twitchIrc.RawMessageReceived += new EventHandler<IrcRawMessageEventArgs>(twitchIrc_RawMessageReceived);
                
                
                var twitchDnsName = settings.TwitchUser.ToLower() + "." + twitchIRCDomain;

                try
                {
                    twitchServers = Dns.GetHostEntry("irc.twitch.tv");
                }
                catch
                {
                    SendMessage(new UbiMessage("Twitch: error resolving twitch hostname. Using harcoded list of IPs", EndPoint.TwitchTV, EndPoint.Error));
                    twitchServers = new IPHostEntry()
                    {
                        AddressList = new IPAddress[] {
                        IPAddress.Parse("199.9.253.199"),
                        IPAddress.Parse("199.9.253.210"),
                        IPAddress.Parse("199.9.250.239"),
                        IPAddress.Parse("199.9.250.229")
                    }
                    };
                }
                using (var connectedEvent = new ManualResetEventSlim(false))
                {
                    twitchIrc.Connected += (sender2, e2) => connectedEvent.Set();

                    var tmpNextServer = twitchServers.AddressList.SkipWhile(p => p.ToString() == nextTwitchIP.ToString()).FirstOrDefault();


                    if (tmpNextServer == null)
                        tmpNextServer = twitchServers.AddressList.FirstOrDefault();

                    if (nextTwitchIP.Address != 0)
                        SendMessage(new UbiMessage("Twitch: cycling to the next server " + tmpNextServer.ToString(), EndPoint.TwitchTV, EndPoint.Error));

                    nextTwitchIP = tmpNextServer;


                    if (!settings.TwitchPassword.ToLower().Contains("oauth:"))
                    {
                        twitchWeb = new TwitchWeb(settings.TwitchUser.ToLower(), settings.TwitchPassword);
                        twitchWeb.OnDescriptionSetError += new EventHandler<EventArgs>(twitchWeb_OnDescriptionSetError);
                        if (twitchWeb.Login())
                        {
                            settings.twitchOAuthKey = twitchWeb.ChatOAuthKey;
                            twitchWeb.GetDescription();
                            if( String.IsNullOrEmpty( settings.twitch_ShortDescription ) )
                                settings.twitch_ShortDescription = twitchWeb.ShortDescription;
                            if( String.IsNullOrEmpty( settings.twitch_Game ))
                                settings.twitch_Game = twitchWeb.Game;
                        }
                    }
                    else
                    {
                        settings.twitchOAuthKey = settings.TwitchPassword;
                    }

                    if (!String.IsNullOrEmpty(settings.twitchOAuthKey))
                    {
                        twitchIrc.Connect(nextTwitchIP, false, new IrcUserRegistrationInfo()
                        {
                            NickName = settings.TwitchUser.ToLower(),
                            UserName = settings.TwitchUser.ToLower(),
                            RealName = "Twitch bot of " + settings.TwitchUser.ToLower(),
                            Password = settings.twitchOAuthKey
                        });

                        if (!connectedEvent.Wait(60000))
                        {
                            SendMessage(new UbiMessage("Twitch: connection timeout!", EndPoint.TwitchTV, EndPoint.Error));
                            return;
                        }
                    }
                }
            }
        }

        void twitchWeb_OnDescriptionSetError(object sender, EventArgs e)
        {
            lock (lockTwitchConnect)
            {
                SendMessage(new UbiMessage("Twitch: error setting description", EndPoint.TwitchTV, EndPoint.Error));
            }
        }

        void twitchIrc_Error(object sender, IrcErrorEventArgs e)
        {
            lock (lockTwitchConnect)
            {
                SendMessage(new UbiMessage(String.Format("Twitch IRC error: {0}", e.Error.Message), EndPoint.TwitchTV, EndPoint.Error));
            }
        }

        void twitchIrc_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            lock (lockTwitchMessage)
            {

                if (e.RawContent.Contains("Login failed") || e.RawContent.ToLower().Contains("login unsuccessful"))
                {
                    if (!twitchTriedOAuth && !settings.TwitchPassword.Contains("oauth:"))
                    {
                        TwitchWeb toauth = new TwitchWeb(settings.TwitchUser.ToLower(), settings.TwitchPassword);
                        twitchTriedOAuth = true;

                        if (toauth.Login())
                        {
                            settings.twitchOAuthKey = toauth.ChatOAuthKey;
                            return;
                        }
                    }
                    SendMessage(new UbiMessage("Twitch login failed! Check settings!", EndPoint.TwitchTV, EndPoint.Error));
                }
                else if (settings.twitchDebugMessages)
                {
                    SendMessage(new UbiMessage(e.RawContent, EndPoint.TwitchTV, EndPoint.SteamAdmin));
                }
            }
        }

        private void OnTwitchDisconnect(object sender, EventArgs e)
        {
            lock (lockTwitchConnect)
            {

                if (!settings.twitchEnabled)
                    return;

                if (!isClosing)
                {
                    //SendMessage(new Message("Twitch bot disconnected. Trying to reconnect...", EndPoint.TwitchTV, EndPoint.SteamAdmin));
                    twitchBW.Stop();
                    twitchBW = new BGWorker(ConnectTwitchIRC, null);
                }
                else
                {
                    SendMessage(new UbiMessage("Twitch bot is disconnecting", EndPoint.TwitchTV, EndPoint.Notice));
                    twitchIrc.Disconnect();
                }
            }
        }
        private void OnTwitchConnect(object sender, EventArgs e)
        {
        }
        private void OnTwitchChannelList(object sender, IrcChannelListReceivedEventArgs e)
        {

        }
        private void OnTwitchChannelJoinLocal(object sender, IrcChannelEventArgs e)
        {
            lock (lockTwitchConnect)
            {
                e.Channel.UserJoined += OnTwitchChannelJoin;
                e.Channel.UserLeft += OnTwitchChannelLeft;
                e.Channel.MessageReceived += OnTwitchMessageReceived;
                chatStatusTwitch.On = true;
                SendMessage(new UbiMessage(String.Format("Twitch IRC: logged in!"), EndPoint.TwitchTV, EndPoint.Notice));
                twitchIrc.PongReceived += new EventHandler<IrcPingOrPongReceivedEventArgs>(twitchIrc_PongReceived);
                twitchPing.Change(0, 30000);

            }
        }

        void twitchIrc_PongReceived(object sender, IrcPingOrPongReceivedEventArgs e)
        {
            twitchDisconnect.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private void TwitchDisconnectNoPong(object x)
        {
            try
            {
                if (twitchIrc != null)
                    twitchIrc.Disconnect();
                
                twitchBW.Stop();
                twitchBW = new BGWorker(ConnectTwitchIRC, null);
                twitchDisconnect.Change(Timeout.Infinite, Timeout.Infinite);
                twitchPing.Change(Timeout.Infinite, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + " " + ex.StackTrace);
            }
        }

        private void TwitchPingTick(object x)
        {
            if (twitchIrc.IsConnected)
            {
                twitchIrc.Ping();
                twitchDisconnect.Change(30000, 0);
            }
        }
        private void OnTwitchChannelLeftLocal(object sender, IrcChannelEventArgs e)
        {            
            SendMessage(new UbiMessage(String.Format("Twitch: bot left!"), EndPoint.TwitchTV,
                EndPoint.Notice));
        }
        private void OnTwitchMessageReceivedLocal(object sender, IrcMessageEventArgs e)
        {
            lock (lockTwitchMessage)
            {
                if (e.Text.Contains("HISTORYEND") ||
                    e.Text.Contains("USERCOLOR") ||
                    e.Text.Contains("EMOTESET"))
                    return;

                SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.TwitchTV, EndPoint.SteamAdmin) 
                { 
                    FromName = e.Source.ToString(),
                    NickColor = settings.twitchNickColor,
                    TextOnly = false
                });
            }
        }
        private void OnTwitchNoticeReceivedLocal(object sender, IrcMessageEventArgs e)
        {
            lock (lockTwitchMessage)
            {

                SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.TwitchTV, EndPoint.SteamAdmin) { FromName = e.Source.ToString() });
            }
        }
        private void OnTwitchChannelJoin(object sender, IrcChannelUserEventArgs e)
        {
            lock (lockTwitchMessage)
            {

                if (settings.twitchLeaveJoinMessages)
                    SendMessage(new UbiMessage(String.Format("{0} joined " + settings.twitchChatAlias, e.ChannelUser.User.NickName), EndPoint.TwitchTV, EndPoint.SteamAdmin));
            }
        }
        private void OnTwitchChannelLeft(object sender, IrcChannelUserEventArgs e)
        {
            lock (lockTwitchMessage)
            {
                if (settings.twitchLeaveJoinMessages)
                    SendMessage(new UbiMessage(String.Format("{1}{0} left ", settings.twitchChatAlias, e.ChannelUser.User.NickName), EndPoint.TwitchTV, EndPoint.SteamAdmin));
            }
        }
        private void OnTwitchMessageReceived(object sender, IrcMessageEventArgs e)
        {
            lock (lockTwitchMessage)
            {

                SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.TwitchTV, EndPoint.SteamAdmin) 
                { 
                    FromName = e.Source.ToString(),
                    NickColor = settings.twitchNickColor,
                    TextOnly = false
                });
            }
        }
        private void OnTwitchNoticeReceived(object sender, IrcMessageEventArgs e)
        {
            lock (lockTwitchMessage)
            {

                SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.TwitchTV, EndPoint.SteamAdmin) { FromName = e.Source.ToString() });
            }
        }
        private void OnTwitchRegister(object sender, EventArgs e)        
        {
            lock (lockTwitchConnect)
            {
                twitchIrc.LocalUser.JoinedChannel += OnTwitchChannelJoinLocal;
                twitchIrc.LocalUser.LeftChannel += OnTwitchChannelLeftLocal;
                twitchIrc.LocalUser.NoticeReceived += OnTwitchNoticeReceivedLocal;
                twitchIrc.LocalUser.MessageReceived += OnTwitchMessageReceivedLocal;
                Thread.Sleep(3000);
                twitchIrc.Channels.Join(String.IsNullOrEmpty(settings.twitchChannel) ? "#" + settings.TwitchUser.ToLower() : settings.twitchChannel.Contains('#') ? settings.twitchChannel : "#" + settings.twitchChannel);
            }
        }
        #endregion

        #region Sc2Tv methods and events
        private void ConnectSc2tv()
        {
            if (isClosing)
                return;

            if (!settings.sc2tvEnabled)
                return;

            if (sc2ChannelId != 0)
            {
                sc2tv.ChannelId = sc2ChannelId;
            }


            if (String.IsNullOrEmpty(settings.Sc2tvUser) || String.IsNullOrEmpty(settings.Sc2tvPassword))
                return;

            sc2tv.Login(settings.Sc2tvUser, settings.Sc2tvPassword);


        }
        private void OnSc2TvLogin(object sender, Sc2Chat.Sc2Event e)
        {
            Debug.Print("OnSc2TvLogin event");
            if (sc2tv.LoggedIn)
            {
                ThreadPool.QueueUserWorkItem( f=> sc2tv.updateStreamList());
                ThreadPool.QueueUserWorkItem( f=>sc2tv.updateSmiles());
                UInt32.TryParse(sc2tv.GetStreamID(), out sc2ChannelId);
                settings.Sc2tvId = sc2ChannelId.ToString();
                sc2tv.ChannelId = sc2ChannelId;
                sc2tv.Start();

                if (sc2ChannelId != 0 )
                {                   
                    if (sc2tv.ChannelIsLive)
                        streamStatus.SetOn(pictureSc2tvStream);
                    else
                        streamStatus.SetOff(pictureSc2tvStream);
                    
                    //var currentVal = sc2tv.channelList.getById(sc2ChannelId);
                    //if(currentVal != null)
                    //    SetComboValue(comboSc2Channels, currentVal);
                }
                if (String.IsNullOrEmpty(settings.sc2tv_ShortDescription))
                    settings.sc2tv_ShortDescription = sc2tv.ShortDescription;
                if (String.IsNullOrEmpty(settings.sc2tv_LongDescription))
                    settings.sc2tv_LongDescription = sc2tv.LongDescription;
                if (String.IsNullOrEmpty(settings.sc2tv_Game))
                    settings.sc2tv_Game = sc2tv.Game;


                SendMessage(new UbiMessage(String.Format("Sc2tv: logged in!"), EndPoint.Sc2Tv, EndPoint.Notice));
                chatStatusSc2tv.On = true;

                if (obsRemote != null && settings.obsRemoteEnable && obsRemote.Status.streaming)
                {
                    ThreadPool.QueueUserWorkItem( f=> SwitchPlayersOn(true, true) );
                }
                
            }
            else
            {
                SendMessage(new UbiMessage(String.Format("Sc2tv: login failed!"), EndPoint.Sc2Tv, EndPoint.Error));
            }
        }
        private void OnSc2TvChannelList(object sender, Sc2Chat.Sc2Event e)
        {
            if (channelsSC2 == null)
            {
                channelsSC2 = new BindingSource();
                channelsSC2.DataSource = sc2tv.channelList.channels;
            }
            comboSc2Channels.SetDataSource(null);
            comboSc2Channels.SetDataSource(channelsSC2, "Title", "Id");


        }
        private void OnSc2TvMessageReceived(object sender, Sc2Chat.Sc2MessageEvent e)
        {
            if (e.message.name.ToLower() == settings.Sc2tvUser.ToLower())
               return;

            var message = sc2tv.sanitizeMessage(e.message.message,settings.sc2tvSanitizeSmiles);
            if (message.Trim().Length <= 0)
                return;
            
            var to = e.message.to;
            
            if( to == settings.Sc2tvUser && 
                settings.sc2tvPersonalizedOnly )
            {
                SendMessage(new UbiMessage(String.Format("{0}", message), EndPoint.Sc2Tv, EndPoint.SteamAdmin) 
                {
                    FromName = e.message.name,
                    NickColor = settings.sc2NickColor,
                    TextOnly = false

                });
            }
            else
            {
                SendMessage(new UbiMessage(String.Format("{0}", message), EndPoint.Sc2Tv, EndPoint.SteamAdmin) 
                { 
                    FromName = e.message.name, 
                    ToName = to,
                    NickColor = settings.sc2NickColor,
                    TextOnly = false

                });
            }
        }

        private void comboSc2Channels_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboSc2Channels.Items.Count <= 0)
                return;
            try
            {
                var channel = (dotSC2TV.Channel)comboSc2Channels.SelectedValue;
                SendMessage(new UbiMessage(String.Format("Switching sc2tv channel to: {0}", channel.Title), EndPoint.Console, EndPoint.Notice));
                sc2tv.ChannelId = channel.Id;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + " " + ex.StackTrace);
            }
        }
        #endregion
        
        #region Steam bot methods and events
        private void backgroundWorkerSteamPoll_DoWork(object sender, DoWorkEventArgs e)
        {
            if ((bWorkerSteamPoll.CancellationPending == true))
            {
                e.Cancel = true;
                return;
            }
            if (steamBot == null || !settings.steamEnabled)
                return;


            if (steamBot.loginStatus == SteamAPISession.LoginStatus.LoginSuccessful)
                updateList = steamBot.Poll();
            else
                ThreadPool.QueueUserWorkItem(c => ConnectSteamBot());
        }
        private void backgroundWorkerPoll_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            bWorkerSteamPoll.RunWorkerAsync();
        }
        private void ConnectSteamBot()
        {
            if (isClosing)
                return;

            
            string user = settings.SteamBot;
            var steamEnabled = settings.steamEnabled;
            if (String.IsNullOrEmpty(user) || !steamEnabled)
                return;

            steamBot = new SteamAPISession();
            steamBot.Logon += OnSteamLogin;
            steamBot.NewMessage += OnNewSteamMessage;
            steamBot.FriendStateChange += OnSteamFriendStatusChange;
            steamBot.Typing += OnSteamTyping;
            steamBot.SteamGuard += new EventHandler<SteamAPISession.SteamEvent>(steamBot_SteamGuard);
            chatStatusSteamBot.On = true;

            string password = settings.SteamBotPassword;
            string token = settings.SteamBotAccessToken;

            if (String.IsNullOrEmpty(token))
                token = steamBot.RSALogin(user, password);

            status = steamBot.Authenticate(token);

            if (status == SteamAPISession.LoginStatus.LoginSuccessful)
            {
                SendMessage(new UbiMessage(String.Format("Steam: logged in!"), EndPoint.Steam, EndPoint.Notice));
                settings.SteamBotAccessToken = steamBot.accessToken;
            }
            else
            {
                SendMessage(new UbiMessage(String.Format("Steam: login failed"), EndPoint.Steam, EndPoint.Error));
            }
            settings.Save();
        }

        void steamBot_SteamGuard(object sender, SteamAPISession.SteamEvent e)
        {
            string code = InputBox.Show("Check email and enter Steam Guard code:");
            steamBot.SteamGuardKey = code;
        }
        private void OnSteamTyping(object sender, SteamAPISession.SteamEvent e)
        {
            if( settings.steamCurrentChatNotify )
                SendMessage(new UbiMessage(String.Format("Replying to {0}", currentChat.ToString()), EndPoint.Steam, EndPoint.Notice));
        }
        private void OnSteamLogin(object sender, SteamAPISession.SteamEvent e)
        {
            chatStatusSteamBot.On = true;

            //Get Steam Admin ID
            if (String.IsNullOrEmpty(settings.SteamAdminId))
            {
                List<SteamAPISession.Friend> friends = steamBot.GetFriends();
                foreach (SteamAPISession.Friend f in friends)
                {
                    SteamAPISession.User user = steamBot.GetUserInfo(f.steamid);
                    if (user.nickname == settings.SteamAdmin)
                    {
                        steamAdmin = user;
                        settings.SteamAdminId = steamAdmin.steamid;
                        settings.Save();
                        break;
                    }
                }
            }
            else
            {
                steamAdmin = steamBot.GetUserInfo(settings.SteamAdminId);
            }


            if (steamAdmin != null)
            {
                SteamAPISession.User ui = steamBot.GetUserInfo(steamAdmin.steamid);
                if (ui.status != SteamAPISession.UserStatus.Offline)
                {
                    chatStatusSteamAdmin.On = true;
                    steamAdmin.status = SteamAPISession.UserStatus.Online;
                }

            }
            else
                SendMessage(new UbiMessage(String.Format("Can't find {0} in your friends! Check settings or add that account into friend list for bot!",
                    settings.SteamAdmin), EndPoint.Steam, EndPoint.Error));

            if( !bWorkerSteamPoll.IsBusy )
                bWorkerSteamPoll.RunWorkerAsync();

        }
        private void OnNewSteamMessage(object sender, SteamAPISession.SteamEvent e)
        {
            // Message or command from admin. Route it to chat or execute specified action
            if (e.update.origin == steamAdmin.steamid)
            {
                SendMessage( new UbiMessage(String.Format("{0}", e.update.message), EndPoint.SteamAdmin, currentChat) );
            }
        }
        private void OnSteamFriendStatusChange(object sender, SteamAPISession.SteamEvent e)
        {
            if (steamAdmin == null)
                return;

            if (e.update.origin == steamAdmin.steamid)
            {
                if (e.update.status == SteamAPISession.UserStatus.Offline)
                {
                    chatStatusSteamAdmin.On = false;
                    steamAdmin.status = SteamAPISession.UserStatus.Offline;
                }
                else
                {
                    chatStatusSteamAdmin.On = true;
                    steamAdmin.status = SteamAPISession.UserStatus.Online;
                }
            }
        }
        #endregion

        #region Skype methods and events
        public void ConnectSkype()
        {
            if (isClosing)
                return;

            var skypeEnabled = settings.skypeEnabled;
            if (!skypeEnabled)
                return;

            skype = new SkypeChat();
            if (skype == null)
                return;

            adminCommands.Add( new AdminCommand(@"^/hangup\s*(.*)$", skype.Hangup));
            adminCommands.Add( new AdminCommand(@"^/call\s*(.*)$", skype.Call));
            adminCommands.Add( new AdminCommand(@"^/answer\s*(.*)$", skype.Answer));
            adminCommands.Add( new AdminCommand(@"^/mute$",skype.SetMute,true));
            adminCommands.Add( new AdminCommand(@"^/unmute$",skype.SetMute,false));
            adminCommands.Add( new AdminCommand(@"^/speakoff$",skype.SetSpeakers,false));
            adminCommands.Add( new AdminCommand(@"^/speakon$",skype.SetSpeakers,true));

            skype.Connect += OnConnectSkype;
            try
            {
                if (!skype.Start())
                {
                    SendMessage(new UbiMessage(skype.LastError, EndPoint.Skype, EndPoint.SteamAdmin));
                }
            }
            catch {
                SendMessage(new UbiMessage("Skype: attach to process failed!", EndPoint.Skype, EndPoint.Error));
            }
        }
        private void OnGroupMessageReceived(object sender, ChatMessageEventArgs e)
        {
            if( !settings.skypeSkipGroupMessages )
                SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.SkypeGroup, EndPoint.SteamAdmin) { FromName = e.From, FromGroupName = e.GroupName });
        }
        public void OnMessageReceived(object sender, ChatMessageEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.Skype, EndPoint.SteamAdmin) { FromName = e.From });
        }
        public void OnIncomingCall(object sender, CallEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0} calling you on Skype. Type /answer to respond.", e.from), EndPoint.Skype, EndPoint.SteamAdmin));
        }
        public void OnConnectSkype( object sender, EventArgs e)
        {
            chatStatusSkype.On = true;
            skype.MessageReceived += OnMessageReceived;
            skype.GroupMessageReceived += OnGroupMessageReceived;
            skype.IncomingCall += OnIncomingCall;
        }
        #endregion

        #region Goodgame methods and events
        public void ConnectGoodgame()
        {
            if (isClosing)
                return;

            if (!settings.goodgameEnabled)
                return;

            ggChat = new Goodgame(settings.goodgameUser, settings.goodgamePassword, settings.goodgameLoadHistory);

            ggChat.OnMessageReceived += new EventHandler<Goodgame.Message>(ggChat_OnMessageReceived);
            ggChat.OnLogin += new EventHandler<EventArgs>(ggChat_OnLogin);
            ggChat.OnDisconnect += OnGGDisconnect;
            ggChat.OnError += OnGGError;
            if (ggChat.Login())
            {
                ggChat.Start();
                ggChat.GetDescription();
                if (String.IsNullOrEmpty(settings.goodgame_ShortDescription))
                    settings.goodgame_ShortDescription = ggChat.ShortDescription;
                if (String.IsNullOrEmpty(settings.goodgame_Game))
                    settings.goodgame_Game = ggChat.Game;
            }
            else
            {
                SendMessage(new UbiMessage(String.Format("Goodgame: login failed!"), EndPoint.Goodgame, EndPoint.Error));  
            }

        }

        void ggChat_OnLogin(object sender, EventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("Goodgame: logged in!"), EndPoint.Goodgame, EndPoint.Notice));
            chatStatusGoodgame.On = true;
        }

        void ggChat_OnMessageReceived(object sender, Goodgame.Message e)
        {           
            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.Goodgame, EndPoint.SteamAdmin) 
            { 
                FromName = e.User, 
                ToName = e.ToName,
                NickColor = settings.goodgameNickColor,
                TextOnly = false
            });
        }
        public void OnGGDisconnect(object sender, EventArgs e)
        {
            //goodgameBW.Stop();
            //goodgameBW = new BGWorker(ConnectGoodgame, null);
            chatStatusGoodgame.On = false;
        }
        private void OnGGError(object sender, Goodgame.TextEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("Goodgame error: {0}", e.Text), EndPoint.Goodgame, EndPoint.Error));
        }
        #endregion

        #region XSplit methods and events
        public void OnXSplitFrameDrops(object sender, EventArgs e)
        {
            if (!settings.enableXSplitStats)
                return;

            XSplit xapp = null;
            uint framesDropped = 0;
            try
            {
                xapp = (XSplit)sender;
                framesDropped = xapp.FrameDrops;
            }
            catch { }
            SendMessage(new UbiMessage(
                String.Format("Frame drops detected! {0} frame(s) dropped so far", framesDropped),
                EndPoint.Bot, EndPoint.Error)
            );
        }
        public void OnXSplitStatusRefresh(object sender, EventArgs e)
        {
            if( statusServer != null && settings.enableStatusServer )
                statusServer.Broadcast(xsplit.GetJson());
        }
        #endregion

        #region Battlelog methods and events

        //public void ConnectBattlelog()
        //{
        //    if (isClosing)
        //        return;

        //    if( settings.battlelogEnabled && 
        //        !String.IsNullOrEmpty(settings.battlelogEmail) &&
        //        !String.IsNullOrEmpty(settings.battlelogPassword))
        //    {
        //        battlelog.OnMessageReceive += OnBattlelogMessage;
        //        battlelog.OnConnect += OnBattlelogConnect;
        //        battlelog.OnUnknownJson += OnBattlelogJson;
        //        battlelog.Start(settings.battlelogEmail,settings.battlelogPassword);
                
        //    }

        //}
        //public void OnBattlelogConnect(object sender, EventArgs e)
        //{
        //    chatStatusBattlelog.On = true;
        //    SendMessage(new UbiMessage(String.Format("Battlelog: Logged In!"), EndPoint.Battlelog, EndPoint.SteamAdmin));          
        //}
        //public void OnBattlelogJson(object sender, StringEventArgs e)
        //{
        //    if( String.IsNullOrEmpty(e.Message))
        //        return;
        //    SendMessage(new UbiMessage(String.Format("Unknown JSON from the Battlelog: {0}", e.Message), EndPoint.Battlelog, EndPoint.SteamAdmin));
        //}
        //public void OnBattlelogMessage(object sender, BattleChatMessageArgs e)
        //{
        //    if (settings.battlelogEnabled)
        //    {
        //        if( e.message.fromUsername != settings.battlelogNick )
        //            SendMessage(new UbiMessage(String.Format("{0}", e.message.message), EndPoint.Battlelog, EndPoint.SteamAdmin) { FromName = e.message.fromUsername });                    
        //    }
        //}

        #endregion

        #region Goha.tv methods and events
        private void ConnectGohaIRC()
        {
            if (isClosing)
                return;

            //gohaIrc.FloodPreventer = new IrcStandardFloodPreventer(4, 1000);
            if (settings.GohaUser.Length <= 0 ||
                !settings.gohaEnabled)
                return;

            using (var connectedEvent = new ManualResetEventSlim(false))
            {
                gohaIrc.Connected += (sender2, e2) => connectedEvent.Set();
                gohaIrc.RawMessageReceived += new EventHandler<IrcRawMessageEventArgs>(gohaIrc_RawMessageReceived);
                gohaIrc.Error += new EventHandler<IrcErrorEventArgs>(gohaIrc_Error);
                gohaIrc.Connect(gohaIRCDomain, false, new IrcUserRegistrationInfo()
                {
                    NickName = settings.GohaUser,
                    UserName = settings.GohaUser,
                    RealName = "Goha bot of " + settings.GohaUser,
                    //Password = settings.GohaPassword
                });

                if (!connectedEvent.Wait(10000))
                {
                    SendMessage(new UbiMessage("Goha: connection timeout!", EndPoint.Gohatv, EndPoint.Error));
                    return;
                }

            }
        }

        void gohaIrc_Error(object sender, IrcErrorEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("Goha IRC error: {0}", e.Error.Message), EndPoint.Gohatv, EndPoint.Error));
            gohaBW = new BGWorker(ConnectGohaIRC, null);
        }

        void gohaIrc_RawMessageReceived(object sender, IrcRawMessageEventArgs e)
        {
            if (settings.gohaDebugMessages)
            {
                SendMessage(new UbiMessage(e.RawContent, EndPoint.Gohatv, EndPoint.SteamAdmin));
            }


            if (e.Message.Source == null)
                return;

            if (e.Message.Source.Name.ToLower() == "nickserv")
            {
                if (e.RawContent.Contains("Invalid password for"))
                {
                    SendMessage(new UbiMessage("Goha login failed! Check settings!", EndPoint.Gohatv, EndPoint.Error));

                }
                else if (e.RawContent.Contains("You are now identified"))
                {
                    SendMessage(new UbiMessage(String.Format("Goha IRC: logged in!"), EndPoint.Gohatv, EndPoint.Notice));
                    chatStatusGoha.On = true;

                }
                else if (e.RawContent.Contains("is not a registered nickname"))
                {
                    var email = InputBox.Show("Enter your email to receive Goha confirmation code:");
                    if (string.IsNullOrEmpty(email))
                        return;
                    SendRegisterInfoToGohaIRC(email);
                }
                else if (e.RawContent.Contains("An email containing nickname activation instructions"))
                {
                    var confirmCode = InputBox.Show("Check your mail and enter Goha confirmation code:");
                    if (string.IsNullOrEmpty(confirmCode))
                        return;

                    SendConfirmCodeToGohaIRC(confirmCode);
                }
                else if (e.RawContent.Contains("Verification failed. Invalid key for"))
                {
                    var confirmCode = InputBox.Show("Wrong confirmation code. Try again:");
                    if (string.IsNullOrEmpty(confirmCode))
                        return;

                    SendConfirmCodeToGohaIRC(confirmCode);
                }
                else if (e.RawContent.Contains("has now been verified."))
                {
                    SendMessage(new UbiMessage(String.Format("Goha IRC: email verified!"), EndPoint.Gohatv, EndPoint.Notice));
                }
            }
            
        }
        private void OnGohaDisconnect(object sender, EventArgs e)
        {
            if (!settings.gohaEnabled)
                return;

            gohaIrc.Quit();
        }
        private void OnGohaConnect(object sender, EventArgs e)
        {
            //SendMessage( new Message(String.Format("Goha: joining to the channel"),EndPoint.Gohatv, EndPoint.SteamAdmin));
        }
        private void OnGohaChannelList(object sender, IrcChannelListReceivedEventArgs e)
        {

        }
        private void OnGohaChannelJoinLocal(object sender, IrcChannelEventArgs e)
        {
            e.Channel.MessageReceived += OnGohaMessageReceived;
            e.Channel.UserJoined += OnGohaChannelJoin;
            e.Channel.UserLeft += OnGohaChannelLeft;

            //gohaIrc.SendRawMessage("NICK " + settings.GohaUser);
            gohaIrc.LocalUser.SendMessage("NickServ", String.Format("IDENTIFY {0}", settings.GohaPassword));
  
        }
        private void OnGohaChannelLeftLocal(object sender, IrcChannelEventArgs e)
        {
            //SendMessage(new Message(String.Format("Goha: logged in!"), EndPoint.Gohatv,EndPoint.SteamAdmin));
        }
        private void OnGohaMessageReceivedLocal(object sender, IrcMessageEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0}",e.Text), EndPoint.Gohatv, EndPoint.SteamAdmin) 
            { 
                FromName = e.Source.ToString(),
                NickColor = settings.gohaNickColor,
                TextOnly = false

            });
        }
        private void OnGohaNoticeReceivedLocal(object sender, IrcMessageEventArgs e)
        {
            
            //SendMessage(new Message(String.Format("{1} ({0}{2})", e.Source, e.Text, "@goha.tv"), EndPoint.Gohatv, EndPoint.SteamAdmin));
        }
        private void OnGohaChannelJoin(object sender, IrcChannelUserEventArgs e)
        {

            if (settings.gohaLeaveJoinMessages)
                SendMessage(new UbiMessage(String.Format("{1}{0} joined ",settings.gohaChatAlias, e.ChannelUser.User.NickName), EndPoint.Gohatv, EndPoint.SteamAdmin));

        }
        private void OnGohaChannelLeft(object sender, IrcChannelUserEventArgs e)
        {
            var nickName = e.ChannelUser.User.NickName;
            var chatAlias = settings.gohaChatAlias;
            var endPoint = EndPoint.Gohatv;

            if (!chatUsers.Exists(u => (u.NickName == nickName && u.EndPoint == endPoint)))
                chatUsers.Add(new ChatUser(null, nickName, endPoint));

            if (settings.gohaLeaveJoinMessages)
                SendMessage(new UbiMessage(String.Format("{1}{0} left ", chatAlias, nickName), endPoint, EndPoint.SteamAdmin));
        }
        private void OnGohaMessageReceived(object sender, IrcMessageEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.Gohatv, EndPoint.SteamAdmin) 
            { 
                FromName = e.Source.ToString(),
                NickColor = settings.gohaNickColor,
                TextOnly = false
            });
        }
        private void OnGohaNoticeReceived(object sender, IrcMessageEventArgs e)
        {
            SendMessage(new UbiMessage(String.Format("{0}", e.Text), EndPoint.Gohatv, EndPoint.SteamAdmin) { FromName = e.Source.ToString() });
        }
        private void OnGohaRegister(object sender, EventArgs e)
        {
            gohaIrc.Channels.Join("#" + settings.GohaIRCChannel);

            gohaIrc.LocalUser.NoticeReceived += OnGohaNoticeReceivedLocal;
            gohaIrc.LocalUser.MessageReceived += OnGohaMessageReceivedLocal;
            gohaIrc.LocalUser.JoinedChannel += OnGohaChannelJoinLocal;
            gohaIrc.LocalUser.LeftChannel += OnGohaChannelLeftLocal;
        }

        private void UnprotectConfig()
        {
            try
            {
                // Open the configuration file and retrieve 
                // the connectionStrings section.
                Configuration config = ConfigurationManager.
                    OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

                ConfigurationSectionGroup group = config.GetSectionGroup("userSettings") as ConfigurationSectionGroup;
                foreach (ConfigurationSection section in group.Sections)
                {
                    if (section.SectionInformation.IsProtected)
                    {
                        // Encrypt the section.
                        section.SectionInformation.UnprotectSection();
                    }
                }
                // Save the current configuration.
                config.Save();

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + " " + ex.StackTrace);
            }
        }
        static void ProtectConfig()
        {
            try
            {
                // Open the configuration file and retrieve 
                // the connectionStrings section.
                    Configuration config = ConfigurationManager.
                        OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

                    ConfigurationSectionGroup group = config.GetSectionGroup("userSettings") as ConfigurationSectionGroup;
                    foreach (ConfigurationSection section in group.Sections)
                    {
                        if (!section.SectionInformation.IsProtected)
                        {
                            // Encrypt the section.
                            section.SectionInformation.ProtectSection(
                                "DataProtectionConfigurationProvider");
                        }
                    }
                    // Save the current configuration.
                    config.Save();

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message + " " + ex.StackTrace);
            }
        }
        #endregion

        #region OBS Remote methods and events
        public void ConnectOBSRemote()
        {
            if (!settings.obsRemoteEnable)
                return;

            Debug.Print("OBSRemote connecting...");

            if (isClosing)
                return;
            obsRemote = new OBSRemote();
            obsRemote.OnLive += new EventHandler<EventArgs>(obsRemote_OnLive);
            obsRemote.OnOffline += new EventHandler<EventArgs>(obsRemote_OnOffline);
            obsRemote.OnError += new EventHandler<EventArgs>(obsRemote_OnError);
            obsRemote.OnDisconnect += new EventHandler<EventArgs>(obsRemote_OnDisconnect);
            obsRemote.OnSceneList += new EventHandler<OBSSceneStatusEventArgs>(obsRemote_OnSceneList);
            obsRemote.OnSceneSet += new EventHandler<OBSMessageEventArgs>(obsRemote_OnSceneSet);
            obsRemote.OnSourceChange += new EventHandler<OBSSourceEventArgs>(obsRemote_OnSourceChange);
            obsRemote.Connect(settings.obsHost);
        }

        void obsRemote_OnDisconnect(object sender, EventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;
            
            chatStatusOBS.On = false;
        }
        void obsRemote_OnSourceChange(object sender, OBSSourceEventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;

            foreach (ToolStripMenuItem item in contextSceneSwitch.Items)
            {
                if (item.Checked)
                {
                    foreach (ToolStripMenuItem subitem in item.DropDownItems)
                    {
                        if (subitem.Text == e.Source.name)
                        {
                            SetCheckedToolTip(subitem, e.Source.render);
                        }
                    }
                }
            }
            
        }
        private void StartStopOBSStream()
        {
            if (settings.obsRemoteEnable && obsRemote != null)
            {
                if (obsRemote.Opened)
                {
                    obsRemote.StartStopStream();
                }
                else
                {
                    SendMessage(new UbiMessage("No connection to OBS plugin!", EndPoint.Bot, EndPoint.Error));
                }
            }
            else if( !settings.obsRemoteEnable )
            {
                SendMessage(new UbiMessage("OBS control is not enabled. Check your settings!", EndPoint.Bot, EndPoint.Error));
            }
        }
        void obsRemote_OnSceneSet(object sender, OBSMessageEventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;

            var sceneName = e.Message;
            if (String.IsNullOrEmpty(sceneName))
                return;

            foreach (ToolStripMenuItem item in contextSceneSwitch.Items)
            {
                if (item.Text == sceneName)
                {
                    SetCheckedToolTip(item, true);
                    SendMessage(new UbiMessage("Scene set to " + sceneName, EndPoint.Bot, EndPoint.Notice));
                }
                else
                {
                    SetCheckedToolTip(item, false);
                }
            }
        }
        void obsRemote_OnSceneList(object sender, OBSSceneStatusEventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;

            try
            {
                contextSceneSwitch.Items.Clear();
                if (e.Status.scenes.Count <= 0)
                {
                    contextSceneSwitch.Items.Add("No scenes");
                    return;
                }

                foreach (Scene scene in e.Status.scenes)
                {
                    ToolStripMenuItem item = (ToolStripMenuItem)contextSceneSwitch.Items.Add(scene.name);
                    if (e.Status.currentScene == scene.name)
                        item.Checked = true;

                    foreach (Source source in scene.sources)
                    {
                        ToolStripMenuItem subitem = (ToolStripMenuItem)item.DropDownItems.Add(source.name,null, contextSceneSwitch_onClick);                    
                        if (source.render)
                            subitem.Checked = true;
                    }
                }
                            
                chatStatusOBS.On = true;
                }
            catch
            {
            }
        }
        void contextSceneSwitch_onClick(object sender, EventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;

            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            obsRemote.SetSourceRenderer(clickedItem.Text, !clickedItem.Checked);
        }
        private void contextSceneSwitch_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if( !settings.obsRemoteEnable )
                return;

            var menuItem = (ToolStripMenuItem)e.ClickedItem;
            obsRemote.SetCurrentScene(menuItem.Text);
        }
        void obsRemote_OnError(object sender, EventArgs e)
        {
            if (!settings.obsRemoteEnable )
                return;
            chatStatusOBS.On = false;
            Thread.Sleep(3000);
            ConnectOBSRemote();
        }
        void obsRemote_OnOffline(object sender, EventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;

            buttonStreamStartStop.Image = Properties.Resources.play;
            SendMessage(new UbiMessage("OBS doesn't streaming. Switching players off!", EndPoint.Bot, EndPoint.Notice));
            ThreadPool.QueueUserWorkItem( f => SwitchPlayersOff(true,true));
        }
        void obsRemote_OnLive(object sender, EventArgs e)
        {
            if (!settings.obsRemoteEnable)
                return;
            buttonStreamStartStop.Image = Properties.Resources.stop;
            SendMessage(new UbiMessage("OBS is streaming. Switching players on!", EndPoint.Bot, EndPoint.Notice));
            ThreadPool.QueueUserWorkItem( f => SwitchPlayersOn(true,true) );
        }
        #endregion

        private void Chat2Image()
        {
            Debug.Print("Screen capture");
            Control2Image.RtbToBitmap(textMessages, @"c:\test.jpeg");
        }
        public static void ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            var wc = new WebClient();
            var msg = "Error: " + e.Exception.Source + " " + e.Exception.Message + "\n\nStack Trace:\n" + e.Exception.StackTrace;

            wc.Headers["Content-Type"] = "application/x-www-form-urlencoded; charset=UTF-8";
            wc.UploadString("http://xedocproject.com/crashlog/index.php", String.Format(
                "p={0}&e={1}", "Ubiquitous", HttpUtility.UrlEncode(msg)));

            //System.IO.File.WriteAllText(@"C:\UbiquitousCrashLog.txt", msg);
            Debug.Print(msg);
        }

        private void pictureBoxMoveTools_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _Offset = new Point(e.X, e.Y);
            }
        }
        private void pictureBoxMoveTools_MouseUp(object sender, MouseEventArgs e)
        {
            _Offset = Point.Empty;

        }
        private void pictureBoxMoveTools_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Offset != Point.Empty)
            {
                Point newlocation = panelTools.Location;

                var newX = newlocation.X + e.X - _Offset.X;
                var newY = newlocation.Y + e.Y - _Offset.Y;
                var maxX = this.Width - panelTools.ClientRectangle.Width;
                var maxY = textMessages.Height - panelTools.Height;

                if (newX < maxX && newX >= 0)
                    newlocation.X += e.X - _Offset.X;
                else if (newX > maxX)
                    newlocation.X = maxX;
                else if (newX < 0)
                    newlocation.X = 0;

                if (newY >= 0 && newY < maxY)
                    newlocation.Y += e.Y - _Offset.Y;
                else if (newY > maxY)
                    newlocation.Y = maxY;
                else if (newY < 0)
                    newlocation.Y = 0;

                panelTools.Location = newlocation;
            }
        }
        private void labelViewers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _OffsetViewers = new Point(e.X, e.Y);

            }
        }
        private void labelViewers_MouseMove(object sender, MouseEventArgs e)
        {
            if (_OffsetViewers != Point.Empty)
            {
                Point newlocation = labelViewers.Location;

                var newX = newlocation.X + e.X - _OffsetViewers.X;
                var newY = newlocation.Y + e.Y - _OffsetViewers.Y;
                var maxX = this.Width - labelViewers.ClientRectangle.Width;
                var maxY = textMessages.Height - labelViewers.Height;

                if (newX < maxX && newX >= 0)
                    newlocation.X += e.X - _OffsetViewers.X;
                else if (newX > maxX)
                    newlocation.X = maxX;
                else if (newX < 0)
                    newlocation.X = 0;
                
                if (newY >= 0 && newY < maxY)
                    newlocation.Y += e.Y - _OffsetViewers.Y;
                else if (newY > maxY)
                    newlocation.Y = maxY;
                else if (newY < 0)
                    newlocation.Y = 0;

                labelViewers.Location = newlocation;
            }
        }
        private void labelViewers_MouseUp(object sender, MouseEventArgs e)
        {
            _OffsetViewers = Point.Empty;
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetTransparency(this.BackColor, checkBox1.Checked);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void chatStatus_VisibleChanged(object sender, EventArgs e)
        {
           /* var chatStatus = sender as ChatStatus;
            if (chatStatus.Visible)
                chatStatus.Dock = DockStyle.Top;
            else
                chatStatus.Dock = DockStyle.None;
            */
        }

        private void chatStatusOBS_Click(object sender, EventArgs e)
        {
            if( obsRemote != null )
            {
                if (settings.obsRemoteEnable)
                {
                    if (obsRemote.Opened)
                    {
                        ThreadPool.QueueUserWorkItem(f => obsRemote.Disconnect());
                    }
                    else
                    {
                        obsremoteBW = new BGWorker(ConnectOBSRemote, null);
                    }
                }
            }
        }
        private void SetupComboProfiles()
        {
            if (comboProfiles == null)
            {
                comboProfiles = new List<String>();
            }
            comboProfiles.Clear();
            comboProfiles.Add("<Edit...>");
            comboProfiles.Add("<New...>");
            comboProfiles.Add("<Delete...>");
            settings.chatProfiles.Profiles.ForEach(p => comboProfiles.Add(p.Name));

            if (profileListBS == null)
            {

                profileListBS = new BindingSource();
                profileListBS.DataSource = comboProfiles;
            }
            comboBoxProfiles.SetDataSource(null);
            comboBoxProfiles.SetDataSource(profileListBS);
            //comboBoxProfiles.SetDataSource(null);
            
            comboBoxProfiles.SelectedItem = settings.currentProfile;
        }
        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxProfiles_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var combo = sender as ComboBoxWithId;
            String selectedText = combo.SelectedItem.ToString();
            Debug.Print("Profile: {0}",selectedText);
            if (selectedText.Equals("<Edit...>", StringComparison.CurrentCultureIgnoreCase))
            {
                showProfileEditForm();
            }
            else if (selectedText.Equals("<Delete...>", StringComparison.CurrentCultureIgnoreCase))
            {
                if (MessageBox.Show("Delete " + settings.currentProfile + " ?", "Please confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    var curProfile = settings.chatProfiles.Profiles.FirstOrDefault(p => p.Name.Equals(settings.currentProfile, StringComparison.CurrentCultureIgnoreCase));
                    settings.chatProfiles.Profiles.Remove(curProfile);
                    SetupComboProfiles();
                }
            }
            else if (selectedText.Equals("<New...>", StringComparison.CurrentCultureIgnoreCase))
            {
                var curProfile = settings.chatProfiles.Profiles.FirstOrDefault(p => p.Name.Equals(settings.currentProfile, StringComparison.CurrentCultureIgnoreCase));
                if (curProfile == null)
                    return;

                settings.currentProfile = settings.chatProfiles.CreateNew(curProfile);
                SetupComboProfiles();
                showProfileEditForm();

            }
            else
            {
                if (settings.chatProfiles.Profiles.Any(p => p.Name.Equals(selectedText, StringComparison.CurrentCultureIgnoreCase)))
                {
                    settings.chatProfiles.WriteProfile(settings.currentProfile, settings);

                    settings.currentProfile = selectedText;
                    object param = settings;
                    settings.chatProfiles.ReadProfile(selectedText, ref param);
                    showProfileEditForm();
                }
            }
            
            comboBoxProfiles.SelectedItem = settings.currentProfile;
        }

        private void showProfileEditForm()
        {
            var lastOnTopState = this.TopMost;
            settings.globalOnTop = false;
            var descrForm = new Forms.Descriptions(this, settings);
            descrForm.TopMost = true;
            //var state = this.WindowState;
            //this.WindowState = FormWindowState.Minimized;
            descrForm.ShowDialog();
            //this.WindowState = state;
            settings.globalOnTop = lastOnTopState;
            SetupComboProfiles();
            settings.chatProfiles.WriteProfile(settings.currentProfile, settings);

            //String selectedText = comboBoxProfiles.SelectedItem.ToString();
            //ChatProfile cp = settings.chatProfiles.Profiles.FirstOrDefault(p => p.Name.Equals(selectedText, StringComparison.CurrentCultureIgnoreCase));
            //cp.Name = settings.currentProfile;
        }


    }


}

