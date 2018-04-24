using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using RSend;


namespace Client
{
    public partial class SelectForm : Form
    {
        private Dictionary<String, String> SettingList;
        private IServerObject ServerObj;
                
        private ListViewItem[] ListViewItemUser;        
        private ListViewItem[] ListViewItemUserDisplay;
        private ListViewItem[] ListViewItemUserInfo;
        private ListViewItem[] ListViewItemHost;        
        private ListViewItem[] ListViewItemHostDisplay;
        private ListViewItem[] ListViewItemHostInfo;
        private ListViewItem[] ListViewItemADGroup;
        private ListViewItem[] ListViewItemPrivateGroup;
                
        public List<String> ListClientsInfo = new List<string>();
        public List<String> ListADGroups = new List<string>();
        public List<String> ListPrivateGroup = new List<string>();

        public List<String> ListUserSend = new List<string>();
        public List<String> ListHostSend = new List<string>();
        public List<String> ListADGroupSend = new List<string>();
        public List<String> ListPrivateGroupSend = new List<string>();        
        
        public String userNameCurrent;
        public String hideHostName = "false"; // hide host name 
        public String tabSelection = "Nobody";
        public String strError;               
                
        public SelectForm()
        {
            InitializeComponent();
        }

        public SelectForm(Dictionary<String, String> SettingList, IServerObject ServerObj, String userName)
        {
            InitializeComponent();
            try
            {
                this.SettingList = SettingList;
                this.ServerObj = ServerObj;                
                this.ListClientsInfo = ServerObj.GetClientsInfo();                
                this.ListADGroups = ServerObj.GetADGroups();
                userNameCurrent = userName.Split('(')[0].Trim();
                this.ListPrivateGroup = ServerObj.GetPrivateGroups(userNameCurrent);                                
            }
            catch 
            {                
            }            
        }

        private void SelectForm_Load(object sender, EventArgs e)
        {
            try
            {
                cbFilterUser.SelectedIndex = 0;
                cbFilterHost.SelectedIndex = 0;
                SetSetting();
                LoadListViewUser("", 0);
                LoadListViewHost("", 0);
                LoadListViewADGroup();
                LoadListViewPrivateGroup();                
                listViewUser.Focus();
                // hide host name ////////////////////////////
                if (hideHostName == "true")
                {
                    tabControlList.TabPages.Remove(tabPageHost);

                }
                ///////////////////////////////////////////   
            }                        
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(SelectFormLoad)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void SetSetting()
        {
            try
            {
                this.Text = "RSend Client: " + SettingList["frmSelectCaption"];
                tabPageUser.Text = " " + SettingList["tabPageUser"] + " ";
                tabPageHost.Text = " " + SettingList["tabPageHost"] + " ";
                tabPagePrivateGroup.Text = " " + SettingList["tabPagePrivateGroup"] + " ";
                tabPageADGroup.Text = " " + SettingList["tabPageADGroup"] + " ";
                btnOkUser.Text = SettingList["btnOk"];
                btnOkHost.Text = SettingList["btnOk"];
                btnOkPrivateGroup.Text = SettingList["btnOk"];
                btnOkADGroup.Text = SettingList["btnOk"];
                btnCancelUser.Text = SettingList["btnCancel"];
                btnCancelHost.Text = SettingList["btnCancel"];
                btnCancelPrivateGroup.Text = SettingList["btnCancel"];
                btnCancelADGroup.Text = SettingList["btnCancel"];
                btnAddGroup.Text = SettingList["btnAddGroup"];
                btnEditGroup.Text = SettingList["btnEditGroup"];
                btnDelGroup.Text = SettingList["btnDelGroup"];
                btnFilterUser.Text = SettingList["btnFilter"];
                btnFilterHost.Text = SettingList["btnFilter"];
                btnClearUser.Text = SettingList["btnClear"];
                btnClearHost.Text = SettingList["btnClear"];
                lblFilterUser.Text = SettingList["lblFilter"];
                lblFilterHost.Text = SettingList["lblFilter"];
                cbSelectAllUser.Text = SettingList["cbSelectAll"];
                cbSelectAllHost.Text = SettingList["cbSelectAll"];
                cbFilterUser.Items[0] = SettingList["cbOptionStart"];
                cbFilterUser.Items[1] = SettingList["cbOptionExist"];
                cbFilterUser.Items[2] = SettingList["cbOptionNotStart"];
                cbFilterUser.Items[3] = SettingList["cbOptionNotExist"];
                cbFilterHost.Items[0] = SettingList["cbOptionStart"];
                cbFilterHost.Items[1] = SettingList["cbOptionExist"];
                cbFilterHost.Items[2] = SettingList["cbOptionNotStart"];
                cbFilterHost.Items[3] = SettingList["cbOptionNotExist"];
                toolStripMenuItemCopy.Text = SettingList["btnCopy"];
                toolStripMenuItemInfo.Text = SettingList["frmInfoCaption"];
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

        private void SelectForm_Shown(object sender, EventArgs e)
        {
            listViewUser.Focus();
        }

        private void tabControlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlList.SelectedTab == tabControlList.TabPages["tabPageUser"])
            {
                listViewUser.Focus();
            }
            if (tabControlList.SelectedTab == tabControlList.TabPages["tabPageHost"])
            {
                listViewHost.Focus();
            }
            if (tabControlList.SelectedTab == tabControlList.TabPages["tabPageADGroup"])
            {
                listViewADGroup.Focus();
            }
            if (tabControlList.SelectedTab == tabControlList.TabPages["tabPagePrivateGroup"])
            {
                listViewPrivateGroup.Focus();
            }
        }

        private void UsersForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.None)
                e.Cancel = false;
        }

        private void SelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        
        private void btnFilterUser_Click(object sender, EventArgs e)
        {
            listViewUser.Items.Clear();
            cbSelectAllUser.Checked = false;
            LoadListViewUser(txtFilterUser.Text, cbFilterUser.SelectedIndex);
        }

        private void btnFilterHost_Click(object sender, EventArgs e)
        {
            listViewHost.Items.Clear();
            cbSelectAllHost.Checked = false;
            LoadListViewHost(txtFilterHost.Text, cbFilterHost.SelectedIndex);
        }

        private void btnClearUser_Click(object sender, EventArgs e)
        {
            listViewUser.Items.Clear();
            cbSelectAllUser.Checked = false;
            txtFilterUser.Text="";
            cbFilterUser.SelectedIndex = 0;
            LoadListViewUser("", 0);
        }

        private void btnClearHost_Click(object sender, EventArgs e)
        {
            listViewHost.Items.Clear();
            cbSelectAllHost.Checked = false;
            txtFilterHost.Text = "";
            cbFilterHost.SelectedIndex = 0;
            LoadListViewHost("", 0);
        }

        private void txtFilterUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listViewUser.Items.Clear();
                cbSelectAllUser.Checked = false;
                LoadListViewUser(txtFilterUser.Text, cbFilterUser.SelectedIndex);
            }
        }

        private void cbFilterUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listViewUser.Items.Clear();
                cbSelectAllUser.Checked = false;
                LoadListViewUser(txtFilterUser.Text, cbFilterUser.SelectedIndex);
            }

        }

        private void txtFilterHost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listViewHost.Items.Clear();
                cbSelectAllHost.Checked = false;
                LoadListViewHost(txtFilterHost.Text, cbFilterHost.SelectedIndex);
            }
        }

        private void cbFilterHost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                listViewHost.Items.Clear();
                cbSelectAllHost.Checked = false;
                LoadListViewHost(txtFilterHost.Text, cbFilterHost.SelectedIndex);
            }
        }

        private void LoadListViewUser(String txtFilter, int cbFilter)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                txtFilter = txtFilter.Trim();
                List<String> listUser = new List<string>();
                int count = ListClientsInfo.Count;
                String strUser = "";
                String strHost = "";                
                String strDisplay = "";
                String strInfo = "";
                int newcount = 0;

                for (int i = 0; i < count; i++)
                {
                    strUser = ListClientsInfo[i].ToString().Split('(')[0].Trim();
                    strHost = ListClientsInfo[i].ToString().Split(')')[0].Trim();
                    strHost = strHost.Split('(')[1].Trim();
                    strInfo = strUser + ";" + strHost + ";";
                    strDisplay = ListClientsInfo[i].ToString().Split(')')[1].Trim();                    
                    
                    if ((strDisplay == null) || (strDisplay == ""))
                    {                        
                        strDisplay = strUser;                        
                    }
                    else
                    {
                        strInfo = strInfo + strDisplay.Split(';')[2].Trim();
                        strDisplay = strDisplay.Split(';')[0].Trim();                        
                    }                    

                    if (txtFilter != "")
                    {
                        if (cbFilter == 0)
                        {                            
                            if (Regex.IsMatch(strDisplay, "^" + txtFilter, RegexOptions.IgnoreCase))
                            {
                                listUser.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                        if (cbFilter == 1)
                        {
                            if (Regex.IsMatch(strDisplay, txtFilter, RegexOptions.IgnoreCase))
                            {
                                listUser.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                        if (cbFilter == 2)
                        {
                            if (!Regex.IsMatch(strDisplay, "^" + txtFilter, RegexOptions.IgnoreCase))
                            {
                                listUser.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                        if (cbFilter == 3)
                        {
                            if (!Regex.IsMatch(strDisplay, txtFilter, RegexOptions.IgnoreCase))
                            {
                                listUser.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                    }
                    else
                    {
                        listUser.Add(strDisplay + ";" + strInfo);                                
                    }
                }
                listUser.Sort();
                if (txtFilter != "")
                {
                    count = newcount;
                }
                ListViewItemUser = new ListViewItem[count];
                ListViewItemUserDisplay = new ListViewItem[count];
                ListViewItemUserInfo = new ListViewItem[count];
                bool btnEnable = false;
                for (int i = 0; i < count; i++)
                {
                    ListViewItemUser[i] = new ListViewItem(listUser[i].Split(';')[1].Trim());
                    ListViewItemUserDisplay[i] = new ListViewItem(listUser[i].Split(';')[0].Trim());
                    ListViewItemUserInfo[i] = new ListViewItem(listUser[i].Split(';')[1].Trim() + ";" + listUser[i].Split(';')[2].Trim() + ";" + listUser[i].Split(';')[3].Trim());                                        
                    btnEnable = true;
                }
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 16);
                listViewUser.SmallImageList = imgList;
                listViewUser.View = View.List;
                listViewUser.CheckBoxes = true;
                listViewUser.Items.AddRange(ListViewItemUserDisplay);
                Cursor.Current = Cursors.Default;
                if (btnEnable)
                {
                    cbSelectAllUser.Enabled = true;
                    btnOkUser.Enabled = true;
                }
                else
                {
                    cbSelectAllUser.Enabled = false;
                    btnOkUser.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);             
            }
        }

        private void LoadListViewHost(String txtFilter, int cbFilter)
        {
            try
            {
                
                Cursor.Current = Cursors.WaitCursor;
                txtFilter = txtFilter.Trim();
                List<String> listHost = new List<string>();
                int count = ListClientsInfo.Count;
                String strUser = "";
                String strHost = "";
                String strDisplay = "";
                String strInfo = "";
                int newcount = 0;

                
                for (int i = 0; i < count; i++)
                {

                    strUser = ListClientsInfo[i].ToString().Split('(')[0].Trim();
                    strHost = ListClientsInfo[i].ToString().Split(')')[0].Trim();
                    strHost = strHost.Split('(')[1].Trim();
                    strInfo = strUser + ";" + strHost + ";";
                    strDisplay = ListClientsInfo[i].ToString().Split(')')[1].Trim();

                    if ((strDisplay == null) || (strDisplay == ""))
                    {                        
                        strDisplay = strHost;
                    }
                    else
                    {
                        strInfo = strInfo + strDisplay.Split(';')[2].Trim();
                        strDisplay = strDisplay.Split(';')[1].Trim();
                    }

                    if (txtFilter != "")
                    {
                        if (cbFilter == 0)
                        {
                            if (Regex.IsMatch(strDisplay, "^" + txtFilter, RegexOptions.IgnoreCase))
                            {
                                listHost.Add(strDisplay + ";" + strInfo); 
                                newcount++;
                            }
                        }
                        if (cbFilter == 1)
                        {
                            if (Regex.IsMatch(strDisplay, txtFilter, RegexOptions.IgnoreCase))
                            {
                                listHost.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                        if (cbFilter == 2)
                        {
                            if (!Regex.IsMatch(strDisplay, "^" + txtFilter, RegexOptions.IgnoreCase))
                            {
                                listHost.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                        if (cbFilter == 3)
                        {
                            if (!Regex.IsMatch(strDisplay, txtFilter, RegexOptions.IgnoreCase))
                            {
                                listHost.Add(strDisplay + ";" + strInfo);                                
                                newcount++;
                            }
                        }
                    }
                    else
                    {
                        listHost.Add(strDisplay + ";" + strInfo);                                
                    } 
                }
  
                listHost.Sort();
                if (txtFilter != "")
                {
                    count = newcount;
                }
                
                ListViewItemHost = new ListViewItem[count];
                ListViewItemHostInfo = new ListViewItem[count];
                ListViewItemHostDisplay = new ListViewItem[count];
                bool btnEnable = false;
                for (int i = 0; i < count; i++)
                {                    
                    ListViewItemHost[i] = new ListViewItem(listHost[i].Split(';')[2].Trim());
                    ListViewItemHostInfo[i] = new ListViewItem(listHost[i].Split(';')[1].Trim() + ";" + listHost[i].Split(';')[2].Trim() +  ";" + listHost[i].Split(';')[3].Trim());             
                    ListViewItemHostDisplay[i] = new ListViewItem(listHost[i].Split(';')[0].Trim());
                    btnEnable = true;
                }
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 16);
                listViewHost.SmallImageList = imgList;
                listViewHost.View = View.List;
                listViewHost.CheckBoxes = true;
                listViewHost.Items.AddRange(ListViewItemHostDisplay);
                Cursor.Current = Cursors.Default;
                if (btnEnable)
                {
                    cbSelectAllHost.Enabled = true;
                    btnOkHost.Enabled = true;
                }
                else
                {
                    cbSelectAllHost.Enabled = false;
                    btnOkHost.Enabled = false;
                } 
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void LoadListViewADGroup()
        {
            try
            {
                int count = ListADGroups.Count;
                ListViewItemADGroup = new ListViewItem[count];
                bool btnEnable = false;
                for (int i = 0; i < count; i++)
                {
                    ListViewItemADGroup[i] = new ListViewItem(ListADGroups[i]);
                    btnEnable = true;
                }
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 16);
                listViewADGroup.SmallImageList = imgList;
                listViewADGroup.View = View.List;
                listViewADGroup.CheckBoxes = true;
                listViewADGroup.Items.AddRange(ListViewItemADGroup);
                Cursor.Current = Cursors.Default;
                if (btnEnable)
                {
                    btnOkADGroup.Enabled = true;
                }
                else
                {
                    btnOkADGroup.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void LoadListViewPrivateGroup()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int count = ListPrivateGroup.Count;
                ListViewItemPrivateGroup = new ListViewItem[count];
                bool btnEnable = false;
                for (int i = 0; i < count; i++)
                {
                    ListViewItemPrivateGroup[i] = new ListViewItem(ListPrivateGroup[i]);
                    btnEnable = true;
                }
                ImageList imgList = new ImageList();
                imgList.ImageSize = new Size(1, 16);
                listViewPrivateGroup.SmallImageList = imgList;
                listViewPrivateGroup.View = View.List;
                listViewPrivateGroup.CheckBoxes = true;
                listViewPrivateGroup.Items.AddRange(ListViewItemPrivateGroup);
                Cursor.Current = Cursors.Default;
                if (btnEnable)
                {
                    btnEditGroup.Enabled = true;
                    btnDelGroup.Enabled = true;
                    btnOkPrivateGroup.Enabled = true;
                }
                else
                {
                    btnEditGroup.Enabled = false;
                    btnDelGroup.Enabled = false;
                    btnOkPrivateGroup.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void cbSelectAllUser_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cbSelectAllUser.Checked)
                {
                    for (int i = 0; i < ListViewItemUser.Length; i++)
                    {
                        ListViewItemUser[i].Checked = true;
                        ListViewItemUserDisplay[i].Checked = true;
                        ListViewItemUserInfo[i].Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < ListViewItemUser.Length; i++)
                    {
                        ListViewItemUser[i].Checked = false;
                        ListViewItemUserDisplay[i].Checked = false;
                        ListViewItemUserInfo[i].Checked = false;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + "  (Select All)  " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void cbSelectAllHost_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (cbSelectAllHost.Checked)
                {
                    for (int i = 0; i < ListViewItemHost.Length; i++)
                    {
                        ListViewItemHost[i].Checked = true;
                        ListViewItemHostDisplay[i].Checked = true;
                        ListViewItemHostInfo[i].Checked = true;
                    }
                }
                else
                {
                    for (int i = 0; i < ListViewItemHost.Length; i++)
                    {
                        ListViewItemHost[i].Checked = false;
                        ListViewItemHostDisplay[i].Checked = false;
                        ListViewItemHostInfo[i].Checked = false;
                    }
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }
        
        private void btnOkUser_Click(object sender, EventArgs e)
        {           
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int j = 0;
                for (int i = 0; i < ListViewItemUserDisplay.Length; i++)
                {
                    if (ListViewItemUserDisplay[i].Checked)
                    {
                        ListUserSend.Add(ListViewItemUser[i].Text);
                        j++;
                    }
                }                
                tabSelection = "User";
                if (j == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxEmptySelectUserTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                }
                else
                {                    
                    ListUserSend.Sort();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }            
        }        

        private void btnOkHost_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int j = 0;
                for (int i = 0; i < ListViewItemHostDisplay.Length; i++)
                {
                    if (ListViewItemHostDisplay[i].Checked)
                    {
                        ListHostSend.Add(ListViewItemHost[i].Text);
                        j++;
                    }
                }                
                tabSelection = "Host";
                if (j == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxEmptySelectUserTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                }     
                else
                {                    
                    ListHostSend.Sort();
                    Cursor.Current = Cursors.Default;    
                }       
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }            
        }
                
        private void btnOkADGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ListADGroupSend = new List<string>();
                List<String> ListADGroupSelected = new List<string>();
                for (int i = 0; i < ListViewItemADGroup.Length; i++)
                {
                    if (ListViewItemADGroup[i].Checked)
                    {
                        ListADGroupSelected.Add(ListViewItemADGroup[i].Text.ToString());
                    }
                }                
                tabSelection = "ADGroup";
                if (ListADGroupSelected.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxEmptySelectUserTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                }
                else
                {
                    ListADGroupSend = ServerObj.GetADGroupMembers(ListADGroupSelected);
                    ListADGroupSend.Sort();
                    Cursor.Current = Cursors.Default;
                }                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void btnOkPrivateGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ListPrivateGroupSend = new List<string>();
                List<String> ListPrivateGroupSelected = new List<string>();
                for (int i = 0; i < ListViewItemPrivateGroup.Length; i++)
                {
                    if (ListViewItemPrivateGroup[i].Checked)
                    {
                        ListPrivateGroupSelected.Add(ListViewItemPrivateGroup[i].Text.ToString());
                    }
                }
                tabSelection = "PrivateGroup";
                if (ListPrivateGroupSelected.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxEmptySelectUserTextBox"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                }
                else
                {
                    //////////////////////////////////////////////////////////////
                    List<String> ListPrivateGroupMembers = new List<String>();                    
                    ListPrivateGroupMembers = ServerObj.GetPrivateGroupMembers(userNameCurrent, ListPrivateGroupSelected);
                                    
                    String strUser = "";
                    String strUserInfo = "";
                    String strHost = "";
                    String strHostInfo = "";                    

                    foreach (String sMember in ListPrivateGroupMembers)
                    {
                        foreach (String strUserHost in ListClientsInfo)
                        {   
                            
                            strUser = strUserHost.Split('(')[0].Trim();
                            strUserInfo = strUserHost.Split(')')[1].Trim();
                            if ((strUserInfo == null) || (strUserInfo == ""))
                            {
                                strUserInfo = "";
                            }
                            else
                            {
                                strUserInfo = strUserInfo.Split(';')[0].Trim();
                            }

                            strHost = strUserHost.Split(')')[0].Trim();
                            strHost = strHost.Split('(')[1].Trim();
                            strHostInfo = strUserHost.Split(')')[1].Trim();
                            if ((strUserInfo == null) || (strUserInfo == ""))
                            {
                                strHostInfo = "";
                            }
                            else
                            {
                                strHostInfo = strHostInfo.Split(';')[1].Trim();
                            }


                            if (strUser.Trim().ToLower() == sMember.ToLower())
                            {
                                ListPrivateGroupSend.Add(strUser);                             
                            }
                            if (strUserInfo.Trim().ToLower() == sMember.ToLower())
                            {
                                ListPrivateGroupSend.Add(strUser);
                            }                           
                            if (strHost.Trim().ToLower() == sMember.ToLower())
                            {
                                ListPrivateGroupSend.Add(strHost);
                            }
                            if (strHostInfo.Trim().ToLower() == sMember.ToLower())
                            {
                                ListPrivateGroupSend.Add(strHost);
                            }             
                        }
                    }                     
                    //////////////////////////////////////////////////////
                    ListPrivateGroupSend.Sort();
                    Cursor.Current = Cursors.Default;
                }                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }        

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            try
            {
                GroupForm groupForm = new GroupForm(SettingList, ServerObj, null, null, userNameCurrent, "add");
                if (groupForm.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ListPrivateGroup = ServerObj.GetPrivateGroups(userNameCurrent);                                      
                }
                listViewPrivateGroup.Items.Clear();
                LoadListViewPrivateGroup();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);                
            }
        }

        private void btnEditGroup_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ListPrivateGroupSend = new List<string>();
                List<String> ListPrivateGroupMembers = new List<string>();
                int j = 0;
                for (int i = 0; i < ListViewItemPrivateGroup.Length; i++)
                {
                    if (ListViewItemPrivateGroup[i].Checked)
                    {
                        ListPrivateGroupSend.Add(ListViewItemPrivateGroup[i].Text);
                        j++;
                    }
                }                
                if (j == 0)
                {
                    Cursor.Current = Cursors.Default;                    
                    MessageBox.Show(SettingList["MsgBoxEmptyGroupEdit"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    return;
                }
                if (j > 1)
                {
                    Cursor.Current = Cursors.Default;                    
                    MessageBox.Show(SettingList["MsgBoxOneGroupEdit"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    return;
                }
                String groupName = ListPrivateGroupSend[0];
                String groupMembers = "";
                ListPrivateGroupMembers = ServerObj.GetPrivateGroupMembers(userNameCurrent, ListPrivateGroupSend);
                foreach (String tmp in ListPrivateGroupMembers)
                {
                    groupMembers = groupMembers + " " + tmp + ";";
                }                   
                GroupForm groupForm = new GroupForm(SettingList, ServerObj, groupName.Trim(), groupMembers.Trim(), userNameCurrent, "edit");
                if (groupForm.ShowDialog() == DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ListPrivateGroup = ServerObj.GetPrivateGroups(userNameCurrent);
                }
                listViewPrivateGroup.Items.Clear();
                LoadListViewPrivateGroup();
                Cursor.Current = Cursors.Default;                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);                                
            }            
        }

        private void btnDelGroup_Click(object sender, EventArgs e)
        {
            try
            {
                ListPrivateGroupSend = new List<string>();
                int j = 0;
                for (int i = 0; i < ListViewItemPrivateGroup.Length; i++)
                {
                    if (ListViewItemPrivateGroup[i].Checked)
                    {                        
                        ListPrivateGroupSend.Add(ListViewItemPrivateGroup[i].Text);                        
                        j++;
                    }
                }
                if (j == 0)
                {
                    MessageBox.Show(SettingList["MsgBoxEmptyGroupDelete"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    return;
                }
                if (MessageBox.Show(SettingList["MsgBoxConfirmDeleteGroup"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    listViewPrivateGroup.Items.Clear();
                    LoadListViewPrivateGroup();
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                bool res = false;
                try
                {
                    res = ServerObj.DeletePrivateGroup(userNameCurrent, ListPrivateGroupSend);
                }
                catch (Exception ex)
                {
                    res = false;
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxEmptyGroupDelete"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
                    return;
                }
                if (res)
                {
                    ListPrivateGroup = ServerObj.GetPrivateGroups(userNameCurrent);
                    listViewPrivateGroup.Items.Clear();
                    LoadListViewPrivateGroup();
                    Cursor.Current = Cursors.Default;                
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show(SettingList["MsgBoxErrDeleteGroup"], SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);                    
                }                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrListSendTo"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);                
            }
        }

        private void listViewUser_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (listViewUser.SelectedItems.Count > 0)
                    {
                        tabSelection = "User";
                        Point point = new Point(this.Location.X + listViewUser.Location.X + e.X + 10, this.Location.Y + listViewUser.Location.Y + e.Y + 60);

                        contextMenuStripListView.Items[0].Visible = false;
                        if (listViewUser.SelectedItems.Count == 1)
                        {
                            String strInfo = ListViewItemUserDisplay[listViewUser.Items.IndexOf(listViewUser.SelectedItems[0])].Text;
                            if (strInfo != "")
                            {
                                contextMenuStripListView.Items[0].Visible = true;
                            }
                        }
                        contextMenuStripListView.Show(point);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }

        }

        private void listViewHost_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (listViewHost.SelectedItems.Count > 0)
                    {
                        tabSelection = "Host";
                        Point point = new Point(this.Location.X + listViewUser.Location.X + e.X + 10,
                                                this.Location.Y + listViewUser.Location.Y + e.Y + 60);
                        contextMenuStripListView.Items[0].Visible = false;
                        if (listViewHost.SelectedItems.Count == 1)
                        {

                            String strInfo = ListViewItemHostDisplay[listViewHost.Items.IndexOf(listViewHost.SelectedItems[0])].Text;
                            if (strInfo != "")
                            {
                                contextMenuStripListView.Items[0].Visible = true;
                            }
                        }
                        contextMenuStripListView.Show(point);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void listViewADGroup_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (listViewADGroup.SelectedItems.Count > 0)
                    {
                        tabSelection = "ADGroup";
                        Point point = new Point(this.Location.X + listViewUser.Location.X + e.X + 10,
                                                this.Location.Y + listViewUser.Location.Y + e.Y + 60);
                        contextMenuStripListView.Items[0].Visible = false;
                        contextMenuStripListView.Show(point);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void listViewPrivateGroup_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (listViewPrivateGroup.SelectedItems.Count > 0)
                    {
                        tabSelection = "PrivateGroup";
                        Point point = new Point(this.Location.X + listViewUser.Location.X + e.X + 10,
                                                this.Location.Y + listViewUser.Location.Y + e.Y + 60);
                        contextMenuStripListView.Items[0].Visible = false;
                        contextMenuStripListView.Show(point);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void listViewUser_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (listViewUser.SelectedItems.Count > 0)
                {
                    if (e.Control && e.KeyCode == Keys.C)
                    {
                        tabSelection = "User";
                        CopyToClipboard(listViewUser);
                    }
                    if (e.KeyCode == Keys.F1)
                    {
                        if (listViewUser.SelectedItems.Count == 1)
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            String strSelect = ListViewItemUserInfo[listViewUser.Items.IndexOf(listViewUser.SelectedItems[0])].Text;
                            if ((strSelect == null) || (strSelect == ""))
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            InfoForm infoForm = new InfoForm(ServerObj, SettingList, strSelect);
                            infoForm.ShowDialog();
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void listViewHost_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (listViewHost.SelectedItems.Count > 0)
                {

                    if (e.Control && e.KeyCode == Keys.C)
                    {
                        tabSelection = "Host";
                        CopyToClipboard(listViewHost);
                    }
                    if (e.KeyCode == Keys.F1)
                    {
                        if (listViewHost.SelectedItems.Count == 1)
                        {
                            Cursor.Current = Cursors.WaitCursor; 
                            String strSelect = ListViewItemUserInfo[listViewUser.Items.IndexOf(listViewUser.SelectedItems[0])].Text;
                            if ((strSelect == null) || (strSelect == ""))
                            {
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                            InfoForm infoForm = new InfoForm(ServerObj, SettingList, strSelect);
                            infoForm.ShowDialog();
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void listViewADGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                tabSelection = "ADGroup";
                CopyToClipboard(listViewADGroup);
            }
        }

        private void listViewPrivateGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                tabSelection = "PrivateGroup";
                CopyToClipboard(listViewPrivateGroup);
            }
        }

        private void toolStripMenuItemInfo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                String strSelect = "";                
                switch (tabSelection)
                {
                    case "User":
                        strSelect = ListViewItemUserInfo[listViewUser.Items.IndexOf(listViewUser.SelectedItems[0])].Text;
                        break;
                    case "Host":
                        strSelect = ListViewItemHostInfo[listViewHost.Items.IndexOf(listViewHost.SelectedItems[0])].Text;
                        break;
                    default:
                        break;
                }
                if ((strSelect == null) || (strSelect == ""))
                {
                    Cursor.Current = Cursors.Default;
                    return;
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

        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            switch (tabSelection)
            {
                case "User":
                    CopyToClipboard(listViewUser);
                    break;
                case "Host":
                    CopyToClipboard(listViewHost);
                    break;
                case "ADGroup":
                    CopyToClipboard(listViewADGroup);                    
                    break;
                case "PrivateGroup":
                    CopyToClipboard(listViewPrivateGroup);
                    break;
                default:                    
                    break;
            }            
        }           

        private void CopyToClipboard(ListView listViewTmp)
        {
            try
            {
                var builder = new StringBuilder();
                foreach (ListViewItem item in listViewTmp.SelectedItems)
                {
                    builder.AppendLine(item.SubItems[0].Text + ";");
                }
                Clipboard.SetText(builder.ToString());
            }
            catch
            {              
            }
        }

    }
}


