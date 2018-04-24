using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Security.Permissions;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using RSend;


namespace Client
{    
    public partial class ClientForm : Form
    { 
        public Dictionary<String, String> SettingList = new Dictionary<String, String>();        
        public String ipAddressServer;
        public int portServer;
        public int portClient;
        public int timeoutKeepAlive;      
        private int timeoutSelftest;        
        public String info = "";
        public String domain = "";
        public String domainShowUserDisplay = "false";
        public String domainShowHostDisplay = "false";        
        public String domainUserSearchAttribute = "";
        public String domainHostSearchAttribute = "";
        public String domainUserDisplayAttribute = "";
        public String domainHostDisplayAttribute = "";
        public String modeReadOnly = "false";
        public String notifySound = "false";
        public String hideHostName = "false"; // hide host name 
        private ClientObject ClientObj;
        private IServerObject ServerObj;
        private ObjRef m_obj;
        private String userName;
        private String userNameInfo;
        private IPAddress m_compIP;
        private TcpChannel clientChannel;
        private Boolean b_IsConnected = false;
        private Boolean b_IsClosed = false;
        private Boolean selftest = true;        
        public Boolean selftestResult = false;
        private String urlHelp;        
        private int height;
        private int width;          
        public String sHTML;
        public String sHead   = "<html style = 'border:0px'><head></head><body style = 'margin: 5px; padding:0px;'>" +
                                "<table border='0' cellspacing='0' cellpadding='0' width='100%'><tr><td align='center' height='1'></td></tr>";
        public String sBottom = "</table></body></html>";
        public HotKey hotKey;
        
        public ClientForm()
        {            
            InitializeComponent();            
        }

        public ClientForm(Dictionary<String, String> SettingList)
        {            
            InitializeComponent();
            this.SettingList = SettingList;            
        }

        void RegistryHotKey()
        {
            try
            {  
                hotKey = new HotKey();
                hotKey.KeyPressed += new EventHandler<KeyPressedEventArgs>(HotKey_Pressed);                            
                hotKey.RegisterHotKey((ModifierKeys)2, Keys.Oemtilde);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(HotKey)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        void HotKey_Pressed(object sender, KeyPressedEventArgs e)
        {
            try
            {
                Opacity = 1;
                Visible = true;
                Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(HotKey)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }
        
        public void Test(String s)
        {
            MessageBox.Show("Test", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
        }

        public static string GetProperty(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {

            try
            {
                height = this.Height;
                width = this.Width;
                webBrowser1.Navigate("about:blank");
                webBrowser1.ObjectForScripting = this;
                SetSetting();
                RegistryHotKey();
                SetWindowPosition();
                
                String userNameCurrent = WindowsIdentity.GetCurrent().Name.Split('\\')[1].Trim();                
                String hostNameCurrent = Dns.GetHostName().Trim();
                userName = userNameCurrent + " (" + hostNameCurrent + ")";

                try
                {
                    domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                    if ((domain != null) && (domain != ""))
                    {

                        DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
                        DirectorySearcher search = new DirectorySearcher(entry);

                        try
                        {
                            if ((domainShowUserDisplay == "true") && (domainUserDisplayAttribute != "") && (domainUserSearchAttribute != ""))
                            {
                                String sUserDesc = "";
                                search.Filter = "(&(objectClass=user)(" + domainUserSearchAttribute + "=" + userNameCurrent + "))";
                                foreach (SearchResult sResultSet in search.FindAll())
                                {
                                    sUserDesc = GetProperty(sResultSet, domainUserDisplayAttribute);
                                }
                                if (sUserDesc != "")
                                {
                                    userNameCurrent = RemoveBadSymbols(sUserDesc);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("RSendClient cannot get user description from domain \"" + domain + "\".", SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                        }

                        try
                        {
                            if ((domainShowHostDisplay == "true") && (domainHostDisplayAttribute != "") && (domainHostSearchAttribute != ""))
                            {
                                String sHostDesc = "";
                                search.Filter = "(&(objectClass=computer)(" + domainHostSearchAttribute + "=" + hostNameCurrent + "))";
                                foreach (SearchResult sResultSet in search.FindAll())
                                {
                                    sHostDesc = GetProperty(sResultSet, domainHostDisplayAttribute);
                                }
                                if (sHostDesc != "")
                                {
                                    hostNameCurrent = RemoveBadSymbols(sHostDesc);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("RSendClient cannot get computer description from domain \"" + domain + "\".", SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("RSendClient cannot get domain \"" + domain + "\".", SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                }                    
                                
                info = userNameCurrent + ";" + hostNameCurrent + ";" + GetCompIP(Dns.GetHostName().ToLower().ToString());
                userNameInfo = userNameCurrent + " (" + hostNameCurrent + ")";
                
                // hide host name //////////////////////////
                if (hideHostName == "true")
                {
                    this.Text = this.Text + userNameCurrent;
                }
                else
                {
                    this.Text = this.Text + userNameInfo;
                }
                ///////////////////////////////////////////                
                
                Offline();

                ///////////////
                if (CheckPing())
                {
                    LogOnServer();
                }
                timer1.Interval = timeoutKeepAlive;
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(ClientFormLoad)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        public String RemoveBadSymbols(String s)
        {
            String[] arr;
            String line = "";
            int count = 0;
            if (s != "")
            {
                s = s.Replace("\n", " ").Replace("\t", " ").Replace("\r", " ").Replace(";", " ").Replace("(", " ").Replace(")", " ");
                arr = s.Split(' ');
                line = "";
                foreach (String a in arr)
                {
                    if (a.Trim() != "")
                    {
                        if (count == 0)
                        {
                            line = a.Trim();
                        }
                        else
                        {
                            line = line + " " + a.Trim();
                        }
                        count++;
                    }
                }
            }
            return line;
        }
        
        private void SetSetting()
        {
            try
            {
                ipAddressServer = SettingList["ipAddressServer"];
                portServer = Convert.ToInt32(SettingList["portServer"]);
                portClient = Convert.ToInt32(SettingList["portClient"]);
                timeoutKeepAlive = Convert.ToInt32(SettingList["timeoutKeepAlive"]);
                timeoutSelftest = Convert.ToInt32(SettingList["timeoutSelftest"]);               
                domainShowUserDisplay = SettingList["domainShowUserDisplay"];
                domainShowHostDisplay = SettingList["domainShowHostDisplay"];
                domainUserSearchAttribute = SettingList["domainUserSearchAttribute"];
                domainHostSearchAttribute = SettingList["domainHostSearchAttribute"];
                domainUserDisplayAttribute = SettingList["domainUserDisplayAttribute"];
                domainHostDisplayAttribute = SettingList["domainHostDisplayAttribute"];
                modeReadOnly = SettingList["modeReadOnly"];
                notifySound = SettingList["notifySound"];
                urlHelp = SettingList["urlHelp"].Trim();
                if (urlHelp != "")
                {
                    menuItemHelp.Enabled = true;
                }
                menuItemSendMsg.Text = SettingList["menuItemSendMsg"];
                menuItemClear.Text = SettingList["menuItemClear"];
                menuItemHelp.Text = SettingList["menuItemHelp"];
                menuItemClose.Text = SettingList["menuItemClose"];
                menuStripMain.ShowItemToolTips = true;
                menuItemSendMsg.ToolTipText = "Ctrl-N - " + menuItemSendMsg.Text;
                menuItemClear.ToolTipText = "Del - " + menuItemClear.Text;
                menuItemHelp.ToolTipText = "F1 - " + menuItemHelp.Text;
                menuItemClose.ToolTipText = "Esc - " + menuItemClose.Text;
                // hide host name ////////////////////////////
                try
                {
                    hideHostName = SettingList["hideHostName"];
                }
                catch
                {
                    hideHostName = "false";
                }
                //////////////////////////////////////////////
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(LoadConfigFile)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        public void SetWindowPosition()
        {
            this.Height = height;
            this.Width = width;            
            Rectangle r = Screen.PrimaryScreen.WorkingArea;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 1, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 1);
        }
        
        private void LogOnServer()
        {
            try 
            {                                
                m_compIP = GetCompIP(Dns.GetHostName().ToLower());                
       
                BinaryServerFormatterSinkProvider serverProv = new BinaryServerFormatterSinkProvider();
                serverProv.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProv = new BinaryClientFormatterSinkProvider();

                IDictionary props = new Hashtable();
                props["port"] = portClient;
                props["name"] = "";                
                clientChannel = new TcpChannel(props, clientProv, serverProv);
                ChannelServices.RegisterChannel(clientChannel, false);
                ClientObj = new ClientObject(this);
                m_obj = RemotingServices.Marshal(ClientObj, "ClientObject.soap");

                String serverURL = "tcp://" + ipAddressServer + ":" + portServer.ToString() + "/ServerObject.rem";
                ServerObj = (IServerObject)Activator.GetObject(typeof(IServerObject), serverURL);                
                RSend.ClientInfo ClData = new RSend.ClientInfo(userName, info, m_compIP, portClient, DateTime.Now);

                if (ServerObj.Logon(ClData))
                {
                    b_IsConnected = true;

                    // selftest block begin 
                    if (selftest)
                    {
                        List<String> ListSendServer = new List<string>();
                        ListSendServer.Add(userName);
                        ServerObj.SendMessageToServer(userName, ListSendServer, "selftest", false);

                        int i = 0;
                        int count = (int)(Math.Round((double)timeoutSelftest / 1000 * 2));

                        while (i < count)
                        {
                            if (selftestResult)
                            {
                                i = count;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(500);
                                i++;
                            }
                        }

                        if (selftestResult)
                        {
                            Online();
                        }
                        else
                        {
                            LogOffServer();
                        }
                    }
                    // selftest block end 
                    else
                    {
                        Online();
                    }
                }
                else
                {
                    b_IsConnected = false;
                    LogOffServer();
                }
            }
            catch 
            {
                Offline();
            }
        }
        
        private void LogOffServer()
        {
            try
            {
                if (b_IsConnected)
                { 
                    ServerObj.Logoff(userName, "has closed the program");                    
                }                
                RemotingServices.Unmarshal(m_obj);
                RemotingServices.Disconnect(ClientObj);
                ChannelServices.UnregisterChannel(clientChannel);
                Offline();
            }
            catch
            {
                Offline();
            }
        }
        
        private void ReLogOnServer()
        {
            try
            {
                if (!KeepAliveLogon())
                {
                    LogOffServer();
                    if (CheckPing())
                    {
                        Thread thr = new Thread(new ThreadStart(LogOnServer));
                        thr.Start();
                    }
                }
            }
            catch
            {
            }
        }
        
        private bool KeepAliveLogon()
        {
            try
            {
                if (!CheckPing())
                {
                    return false;
                }
                RSend.ClientInfo ClData = new RSend.ClientInfo(userName, info, m_compIP, portClient, DateTime.Now);
                if (ServerObj.KeepAliveLogon(ClData))
                {
                    Online();
                    return true;
                }
                else
                {
                    Offline();
                    return false;
                }
            }
            catch 
            {
                Offline();
                return false;
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            ReLogOnServer();            
        }

        private bool CheckPing()
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(ipAddressServer);
                ping.Send(ipAddressServer, 1000);
                if (pingReply.Status.ToString() == "Success")
                {
                    return true;
                }
                else
                {
                    Offline();
                    return false;
                }
            }
            catch 
            {
                Offline();
                return false;
            }
        }
        
        private IPAddress GetCompIP(String hostName)
        {            
            IPAddress tmpIP = IPAddress.Parse("127.0.0.1");
            try
            {
                IPAddress[] addresses = Dns.GetHostEntry(hostName).AddressList;
                foreach (IPAddress address in addresses)
                {
                    String strIP = address.ToString();
                    String pattern = @"\d\d?\d?\.\d\d?\d?\.\d\d?\d?\.\d\d?\d?";
                    Regex regex = new Regex(pattern);
                    if (regex.IsMatch(strIP))                    
                    {
                        tmpIP = address;
                        break;
                    }
                }
                return tmpIP;              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(GetIPAddress)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
                Offline();
                return tmpIP;
            }
        }

        private void Offline()
        {
            b_IsConnected = false;
            selftestResult = false;
            menuItemSendMsg.Enabled = false;
            lblStatus.Text = SettingList["lblStatusErr"];
            lblStatus.ForeColor = Color.Red;
            notifyIcon1.Icon = notifyIconOff.Icon;
            notifyIcon1.Text = "RSend Client - Offline";
        }

        private void Online()
        {
            b_IsConnected = true;
            selftestResult = true;
            if (modeReadOnly == "true")
            {
                menuItemSendMsg.Enabled = false;
            }        
            else
            {
                menuItemSendMsg.Enabled = true;
            }
            lblStatus.Text = SettingList["lblStatusOk"];
            lblStatus.ForeColor = Color.Black;
            notifyIcon1.Icon = notifyIconOn.Icon;
            notifyIcon1.Text = "RSend Client - Online";
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && (!b_IsClosed))
            {
                Opacity = 0;                                
                Visible = false;
                Activate();                
                SetWindowPosition();
                webBrowser1.Document.OpenNew(true);
                webBrowser1.Document.Write(sHead + sHTML + sBottom);
                e.Cancel = true;
            }
            else
            {
                if (b_IsConnected)
                {
                    try
                    {
                        LogOffServer();
                    }
                    catch 
                    {
                    }
                }
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            Opacity = 1;
            Visible = true;
            Activate();
        }

        private void showSendForm(List<String> listReplyName)
        {
            try
            {
                if (Visible == false)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                if (!CheckPing())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxNoPing"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    return;
                }                
                if (!KeepAliveLogon())
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxNoRemoting"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    return;
                }
                Cursor.Current = Cursors.Default;

                SendForm sendForm = new SendForm(SettingList, ServerObj, userName, info, listReplyName);                
                sendForm.ShowDialog();                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }        

        public void getReplyName(String linkUser)
        {
            try
            {                                              
                if (linkUser.Contains("RSendAlert"))
                {
                    MessageBox.Show(SettingList["MsgBoxNoReplyAlert"], SettingList["MsgBoxCaptionInfo"], MessageBoxButtons.OK);
                    return;
                }
                List<string> listReplyName = new List<string>();
                listReplyName.Add(linkUser);
                showSendForm(listReplyName);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK); 
            }
        }

        public void showFormInfo(String linkUser)
        {
            try
            {                                              
                if (linkUser.Contains("RSendAlert"))
                {
                    MessageBox.Show(SettingList["MsgBoxNoInfoAlert"], SettingList["MsgBoxCaptionInfo"], MessageBoxButtons.OK);                    
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                String tmp = linkUser.Split(')')[0].Trim();
                String strSelect = tmp.Split('(')[0].Trim() + ";" + tmp.Split('(')[1].Trim() + ";";                
                String info = linkUser.Split(')')[1].Trim();

                if ((info == null) || (info == ""))
                {                    
                }
                else
                {
                    strSelect = strSelect + info.Split(';')[2].Trim();
                }
                InfoForm infoForm = new InfoForm(ServerObj, SettingList, strSelect);
                infoForm.ShowDialog();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK); 
            }
        }
        
        public void openLink(String link)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                System.Diagnostics.Process.Start(link);                
                Cursor.Current = Cursors.Default;
                Close();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void Clear()
        {
            try
            {
                if (sHTML != null)
                {
                    if (MessageBox.Show(SettingList["MsgBoxClear"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        webBrowser1.Visible = false;
                        sHTML = null;
                        webBrowser1.Document.OpenNew(true);
                        webBrowser1.Document.Write(sHead + sHTML + sBottom);
                        webBrowser1.Visible = true;
                        menuItemClear.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK); 
            }

        }

        private void menuItemSendMsg_Click(object sender, EventArgs e)
        {
            showSendForm(null);
        }

        private void menuItemClear_Click(object sender, EventArgs e)
        {
            Clear();            
        }

        private void menuItemClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void menuItemHelp_Click(object sender, EventArgs e)
        {
            try
            {
                if (Visible == true)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    System.Diagnostics.Process.Start(urlHelp);
                    Cursor.Current = Cursors.Default;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrOpenHelp"], SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }
 
        private void ClientForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (((e.Modifiers & Keys.Control) == Keys.Control) && (e.KeyCode == Keys.Z))
            {
                if (MessageBox.Show(SettingList["MsgBoxExit"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (Visible == false)
                    {
                        if (MessageBox.Show(SettingList["MsgBoxExit"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            b_IsClosed = true;
                            Close();
                            Application.Exit();
                        }
                    }
                }
            }
        }

        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {            
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (((e.Modifiers & Keys.Control) == Keys.Control) && (e.KeyCode == Keys.Z))
            {
                if (Visible == true)
                {
                    if (MessageBox.Show(SettingList["MsgBoxExit"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        b_IsClosed = true;
                        Close();
                        Application.Exit();
                    }
                }
            }
        }

    }
}



