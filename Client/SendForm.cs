using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RSend;


namespace Client
{
    public partial class SendForm : Form
    {        
        private IServerObject ServerObj;
        private String userName;
        private String info;
        private Dictionary<String, String> SettingList;
        private delegate void SendMessageDelegate(String senderName, List<String> recieverNames, String message, Boolean confirmation);        
        private List<String> ListSendServer = new List<string>();
        private List<String> ListSendServerListView = new List<string>();        
        private List<String> ListReplyName = new List<string>();
        private ListViewItem[] ListViewToItem;
        //private String strSendTo;
                          
        public SendForm()
        {            
            InitializeComponent();
        }

        public SendForm(Dictionary<String, String> SettingList, IServerObject ServerObj, String userName, String info, List<String> ListReplyName)
        {
            InitializeComponent();            
            this.SettingList = SettingList;
            this.ServerObj = ServerObj;
            this.userName = userName;
            this.info = info;
            this.ListReplyName = ListReplyName;            
        }

        private void SendForm_Load(object sender, EventArgs e)
        {
            try
            {
                btnSelect.Select();
                btnSelect.Focus();
                SetSetting();
                ListSendServer = new List<string>();
                
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 15);
                listViewTo.SmallImageList = imgList;
                listViewTo.View = View.List;

                if (ListReplyName != null)
                {
                    ListSendServer = ListReplyName;
                    getReplyName();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(SendFormLoad)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void SetSetting()
        {
            try
            {
                this.Text = "RSend Client: " + SettingList["frmSendCaption"];
                lblMsgText.Text = SettingList["lblMsgText"];
                lblSendTo.Text = SettingList["lblSendTo"];
                cbConfirm.Text = SettingList["cbConfirm"];
                btnSelect.Text = SettingList["btnSelect"];
                btnSend.Text = SettingList["btnSend"];
                btnClear.Text = SettingList["btnClear"];
                btnClose.Text = SettingList["btnClose"];
                toolStripMenuItemCopy.Text = SettingList["btnCopy"];
                toolStripMenuItemInfo.Text = SettingList["frmInfoCaption"];                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(LoadConfigFile)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void getReplyName()
        {            
            try
            { 
                if (ListSendServer != null)
                {
                    if (ListSendServer.Count > 0)
                    {
                        List<String> ListClientsInfo = new List<string>();        
                        ListClientsInfo = ServerObj.GetClientsInfo();
                        foreach (String s1 in ListSendServer)
                        {                            
                            foreach (String s2 in ListClientsInfo)
                            {   
                                if (s1.Trim() == s2.Trim())
                                {                                    
                                    String strInfo = s2.Split(')')[1].ToString().Trim();
                                    String userReply = ""; 
                                    if ((strInfo == null) || (strInfo == ""))
                                    {
                                        userReply = s2.Split('(')[0].ToString().Trim();
                                    }
                                    else
                                    {
                                        userReply = strInfo.Split(';')[0].ToString().Trim();
                                    }                                    
                                    listViewTo.Items.Clear();
                                    ListViewToItem = new ListViewItem[1];
                                    ListSendServerListView.Add(userReply + ";" + ListReplyName[0].ToString().Trim());                                        
                                    ListViewToItem[0] = new ListViewItem(userReply);
                                    listViewTo.Items.AddRange(ListViewToItem);                                    
                                }
                            }
                        }                        
                        btnClear.Enabled = true;
                        txtMsg.Select();
                        txtMsg.Focus();
                    }
                }
                if (listViewTo.Items.Count == 0)
                {                    
                    MessageBox.Show(SettingList["MsgBoxNoReplyMsg"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    this.Opacity = 0;
                    this.Close();
                }
                Visible = true;
            }                        
            catch (Exception ex)
            {
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }             
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                SelectForm selectForm = new SelectForm(SettingList, ServerObj, userName);
                Cursor.Current = Cursors.Default;

                List<String> ListClientsInfo = new List<string>();
                ListClientsInfo = selectForm.ListClientsInfo;
                if (ListClientsInfo.Count < 1)
                {
                    MessageBox.Show(SettingList["MsgBoxNoRegisteredUsers"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    btnClose.Select();
                    btnClose.Focus();
                    return;
                }

                if (selectForm.ShowDialog() == DialogResult.OK)
                {

                    Cursor.Current = Cursors.WaitCursor;
                    ListClientsInfo.Sort();

                    List<String> ListAnySend = new List<string>();
                    switch (selectForm.tabSelection)
                    {
                        case "User":
                            ListAnySend = selectForm.ListUserSend;
                            break;
                        case "Host":
                            ListAnySend = selectForm.ListHostSend;
                            break;
                        case "ADGroup":
                            ListAnySend = selectForm.ListADGroupSend;
                            break;
                        case "PrivateGroup":
                            ListAnySend = selectForm.ListPrivateGroupSend;
                            break;
                    }

                    if (ListAnySend.Count > 0)
                    {
                        ListAnySend.Sort();
                        int index = 0;
                        while (index < ListAnySend.Count - 1)
                        {
                            if (ListAnySend[index] == ListAnySend[index + 1])
                            {
                                ListAnySend.RemoveAt(index);
                            }
                            else
                            {
                                index++;
                            }
                        }
                    }

                    int countLoop = 0;
                    bool breakLoop = false;
                    string[] strArr = new string[2];
                    String strInfoPart1 = "";
                    String strInfoPart2 = "";                    

                    //User accounts lookup
                    if ((selectForm.tabSelection == "User") || (selectForm.tabSelection == "ADGroup") || (selectForm.tabSelection == "PrivateGroup"))
                    {
                        String strUserName = "";
                        String strUserDisplay = "";
                        countLoop = 0;
                        breakLoop = false;

                        List<String> ListUserClients = new List<string>();
                        ListUserClients = ListClientsInfo;
                        ListUserClients.Sort();

                        for (int i = 0; i < ListAnySend.Count; i++)
                        {
                            for (int j = countLoop; j < ListUserClients.Count; j++)
                            {
                                strUserName = ListUserClients[j].ToString().Split('(')[0].Trim();
                                if (strUserName.ToLower() == ListAnySend[i].ToString().Trim().ToLower())
                                {                                    
                                    countLoop = j + 1;
                                    breakLoop = true;
                                    if (strUserName != "")
                                    {
                                        strArr = ListUserClients[j].ToString().Split(')');
                                        strUserDisplay = ListUserClients[j].ToString().Split(')')[1].Trim();                                                                                         
                                        ListSendServer.Add(strArr[0].Trim() + ")" + strUserDisplay);
                                        if (strUserDisplay == "")
                                        {
                                            ListSendServerListView.Add(strUserName + ";" + strArr[0].Trim() + ")" + strUserDisplay);
                                        }
                                        else
                                        {                                            
                                            ListSendServerListView.Add(strUserDisplay.Split(';')[0].Trim() + ";" + strArr[0].Trim() + ")" + strUserDisplay);
                                        }
                                    }
                                }
                                else
                                {
                                    if (breakLoop)
                                    {
                                        breakLoop = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // Host accounts lookup
                    if ((selectForm.tabSelection == "Host") || (selectForm.tabSelection == "ADGroup") || (selectForm.tabSelection == "PrivateGroup"))
                    {

                        String strHostName = "";
                        String strHostDisplay = "";
                        countLoop = 0;
                        breakLoop = false;

                        List<String> ListHostClients = new List<string>();

                        for (int i = 0; i < ListClientsInfo.Count; i++)
                        {
                            strInfoPart1 = ListClientsInfo[i].ToString().Split(')')[0] + ")";
                            strInfoPart2 = ListClientsInfo[i].ToString().Split(')')[1];
                            strArr = strInfoPart1.ToString().Split('(');
                            ListHostClients.Add(strArr[1].Substring(0, strArr[1].Length - 1) + " (" + strArr[0].Trim() + ")" + strInfoPart2);
                        }
                        ListHostClients.Sort();

                        for (int i = 0; i < ListAnySend.Count; i++)
                        {
                            for (int j = countLoop; j < ListHostClients.Count; j++)                            
                            {
                                strHostName = ListHostClients[j].ToString().Split('(')[0].Trim();
                                if (strHostName.ToLower() == ListAnySend[i].ToString().Trim().ToLower())
                                {
                                    countLoop = j + 1;
                                    breakLoop = true;

                                    if (strHostName != "")
                                    {
                                        strInfoPart1 = ListHostClients[j].ToString().Split(')')[0];                                        
                                        strArr = strInfoPart1.ToString().Trim().Split('(');
                                        strHostDisplay = ListHostClients[j].ToString().Split(')')[1].Trim();                                        
                                        ListSendServer.Add(strArr[1].Trim() + " (" + strArr[0].Trim() + ")" + strHostDisplay);                                        
                                        if (strHostDisplay == "")
                                        {
                                            ListSendServerListView.Add(strHostName + ";" + strArr[1].Trim() + " (" + strArr[0].Trim() + ")" + strHostDisplay);                                            
                                        }
                                        else
                                        {                                            
                                            ListSendServerListView.Add(strHostDisplay.Split(';')[1].Trim() + ";" + strArr[1].Trim() + " (" + strArr[0].Trim() + ")" + strHostDisplay);
                                        }
                                    }
                                }
                                else
                                {
                                    if (breakLoop)
                                    {
                                        breakLoop = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // remove duplicate names & sort
                    bool btnEnable = false;
                    if (ListSendServer.Count > 0)
                    {
                        btnEnable = true;
                        ListSendServer.Sort();
                        int index = 0;
                        while (index < ListSendServer.Count - 1)
                        {
                            if (ListSendServer[index] == ListSendServer[index + 1])
                            {
                                ListSendServer.RemoveAt(index);
                            }
                            else
                            {
                                index++;
                            }
                        }
                    }


                    ///////////////////////if (ListSendServer.Count == selectForm.ListClients.Count)
                    if (ListSendServer.Count == ListClientsInfo.Count)
                    {
                        listViewTo.Items.Clear();
                        ListViewToItem = new ListViewItem[1];
                        ListViewToItem[0] = new ListViewItem("<" + SettingList["sToAllUsers"] + ">");
                        listViewTo.Items.AddRange(ListViewToItem);
                    }
                    else
                    {
                        // remove duplicate names & sort
                        if (ListSendServerListView.Count > 0)   
                        {
                            ListSendServerListView.Sort();
                            int index = 0;
                            while (index < ListSendServerListView.Count - 1)
                            {
                                if (ListSendServerListView[index] == ListSendServerListView[index + 1])
                                {
                                    ListSendServerListView.RemoveAt(index);
                                }
                                else
                                {
                                    index++;
                                }
                            }
                        }
                        listViewTo.Items.Clear();
                        int count = ListSendServerListView.Count;
                        ListViewToItem = new ListViewItem[count];
                        for (int i = 0; i < count; i++)
                        {                            
                            ListViewToItem[i] = new ListViewItem(ListSendServerListView[i].Split(';')[0]);
                        }
                        listViewTo.Items.AddRange(ListViewToItem);
                    }

                    Cursor.Current = Cursors.Default;                    
                    if (btnEnable)
                    {
                        btnClear.Enabled = true;
                        if (txtMsg.Text.Trim() != "")
                        {
                            cbConfirm.Enabled = true;
                            btnSend.Enabled = true;
                        }
                        txtMsg.Select();
                        txtMsg.Focus();
                    }
                    else
                    {
                        btnClear.Enabled = false;
                        btnSend.Enabled = false;
                        cbConfirm.Enabled = false;
                        btnSelect.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            ListSendServer = null;
            ListSendServer = new List<string>();
            ListSendServerListView = new List<string>();
            listViewTo.Items.Clear();
            btnSelect.Enabled = true;
            btnClear.Enabled = false;
            btnSend.Enabled = false;
            cbConfirm.Enabled = false;
            btnSelect.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }        

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {
            try
            {
                
                if (listViewTo.Items.Count == 0)
                {
                    MessageBox.Show(SettingList["MsgBoxEmptySelectUserTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    btnSelect.Select();
                    btnSelect.Focus();
                    return;
                }                 
                if (txtMsg.Text.Trim() == "")
                {
                    MessageBox.Show(SettingList["MsgBoxEmptyMsgTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    txtMsg.Text = "";
                    txtMsg.Focus();
                    return;
                }

                Boolean confirmation = false;
                if (cbConfirm.Checked) 
                {
                    confirmation = true;
                }               
                Cursor.Current = Cursors.WaitCursor;                
                AsyncCallback SendMessageCallBack = new AsyncCallback(SendMessageReturned);
                SendMessageDelegate SMDel = new SendMessageDelegate(ServerObj.SendMessageToServer);
                IAsyncResult SendMessageAsyncResult = SMDel.BeginInvoke(userName + info, ListSendServer, txtMsg.Text, confirmation, SendMessageCallBack, SMDel);

                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxSendMsgOk"], SettingList["MsgBoxCaptionInfo"], MessageBoxButtons.OK);
                ListSendServer = null;
                ListSendServer = new List<string>();
                ListSendServerListView = new List<string>();
                listViewTo.Items.Clear();
                txtMsg.Text = "";
                cbConfirm.Checked = false;
                btnSelect.Enabled = true;
                btnClear.Enabled = false;
                btnSend.Enabled = false;
                cbConfirm.Enabled = false;
                btnClose.Focus();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrSendMsg"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }              
        }

        void SendMessageReturned(IAsyncResult result)
        {
            try
            {
                SendMessageDelegate dlgt = (SendMessageDelegate)result.AsyncState;
                dlgt.EndInvoke(result);
            }
            catch(Exception ex)
            {
                MessageBox.Show(SettingList["MsgBoxErrSendMsg"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void SendForm_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (((e.Modifiers & Keys.Control) == Keys.Control) && (e.KeyCode == Keys.Enter))
            {                
                SendMessage();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {            
            if ((listViewTo.Items.Count > 0) && (txtMsg.Text.Trim() != ""))
            {
                cbConfirm.Enabled = true;
                btnSend.Enabled = true;
            }
            else
            {
                cbConfirm.Enabled = false;
                btnSend.Enabled = false;                
            }              
        }

        private void listViewTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard(listViewTo);
            }
            if (e.KeyCode == Keys.F1)
            {
                showFormInfo(); 
            }
        }

        private void listViewTo_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewTo.SelectedItems.Count > 0)
                {
                    Point point = new Point(this.Location.X + listViewTo.Location.X + e.X + 10, this.Location.Y + listViewTo.Location.Y + e.Y + 30);
                    contextMenuStripListView.Items[0].Visible = false;
                    if (listViewTo.SelectedItems.Count == 1)
                    {
                        String strSelect = ListSendServer[listViewTo.Items.IndexOf(listViewTo.SelectedItems[0])].ToString();
                        if (strSelect != "")
                        {
                            contextMenuStripListView.Items[0].Visible = true;
                        }
                    }
                    contextMenuStripListView.Show(point);
                }
            }
        }

        private void toolStripMenuItemInfo_Click(object sender, EventArgs e)
        {
            showFormInfo();
        }

        private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            CopyToClipboard(listViewTo);
        }

        private void CopyToClipboard(ListView listViewToTmp)
        {
            try
            {
                var builder = new StringBuilder();
                foreach (ListViewItem item in listViewToTmp.SelectedItems)
                {                    
                    builder.AppendLine(item.SubItems[0].Text + ";");
                }
                Clipboard.SetText(builder.ToString());
            }
            catch 
            {  
            }              
        }

        private void showFormInfo()
        {
            try
            {
                if (listViewTo.SelectedItems.Count == 1)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    String strSelect = ListSendServerListView[listViewTo.Items.IndexOf(listViewTo.SelectedItems[0])].ToString();
                    if ((strSelect == null) || (strSelect == ""))
                    {
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    String strUser = strSelect.Split('(')[0].Trim();
                    strUser = strUser.Split(';')[1].Trim();
                    String strHost = strSelect.Split(')')[0].Trim();
                    strHost = strHost.Split('(')[1].Trim();
                    String strInfo = strSelect.Split(')')[1].Trim();

                    if ((strInfo == null) || (strInfo == ""))
                    {
                        strSelect = strUser + ";" + strHost + ";";
                    }
                    else
                    {
                        strSelect = strUser + ";" + strHost + ";" + strInfo.Split(';')[2].Trim();
                    }
                    InfoForm infoForm = new InfoForm(ServerObj, SettingList, strSelect);
                    infoForm.ShowDialog();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }


    }
}
