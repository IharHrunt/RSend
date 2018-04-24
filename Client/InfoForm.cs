using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using RSend;


namespace Client
{
    public partial class InfoForm : Form
    {
        private Dictionary<String, String> SettingList;
        public String strInfo;
        public String domainUserAttr;
        public String domainHostAttr;
        public String ipAddressConnect;
        public List<String> ListUserAttr = new List<String>();
        public List<String> ListHostAttr = new List<String>();
        public String domainUserSearchAttribute = "";
        public String domainHostSearchAttribute = "";
        
        public InfoForm()
        {
            InitializeComponent();
        }
        public InfoForm(IServerObject ServerObj, Dictionary<String, String> SettingList, String strInfo)
        {
            InitializeComponent();
            try
            {
                this.SettingList = SettingList;
                this.strInfo = strInfo;
                this.ListUserAttr = ServerObj.GetObjectAttr("domainUserAttributes");                                                             
                this.ListHostAttr = ServerObj.GetObjectAttr("domainHostAttributes");
            }
            catch
            {
            }
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            SetSetting();
            ShowInfo();
            btnClose.Select();
            btnClose.Focus();

        }

        private void SetSetting()
        {
            try
            {                
                this.Text = "RSend Client: " + SettingList["frmInfoCaption"];
                btnClose.Text = SettingList["btnClose"];
                textBoxInfo.WordWrap = false;
                domainUserSearchAttribute = SettingList["domainUserSearchAttribute"];
                domainHostSearchAttribute = SettingList["domainHostSearchAttribute"];                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(LoadConfigFile)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void ShowInfo()
        {
            try
            {
                if ((strInfo == null) || (strInfo == ""))
                {
                    textBoxInfo.Text = "No information";
                    return;
                }
                String strUserInfo = strInfo.Split(';')[0].Trim();                
                String strHostInfo = strInfo.Split(';')[1].Trim();
                String strIpInfo = strInfo.Split(';')[2].Trim();
                ipAddressConnect = strIpInfo;

                String domain = "";                
                try
                {
                    domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;
                }
                catch
                {                    
                }
                                
                if ((domain == null) || (domain == ""))
                {                    
                    textBoxInfo.Text = textBoxInfo.Text + "Domain Name   : not found" + Environment.NewLine;
                    textBoxInfo.Text = textBoxInfo.Text + "User Name     : " + strUserInfo + Environment.NewLine;
                    textBoxInfo.Text = textBoxInfo.Text + "Computer Name : " + strHostInfo + Environment.NewLine;
                    textBoxInfo.Text = textBoxInfo.Text + "Ip Address    : " + strIpInfo;
                    return;
                }
                                
                String space = "";
                int length = 0;
                ////////////////user length/////////////////
                for (int i = 0; i < ListUserAttr.Count; i++)
                {
                    if (ListUserAttr[i].Contains("/"))
                    {
                        if (ListUserAttr[i].Split('/')[1].Trim().Length > length)
                        {
                            length = ListUserAttr[i].Split('/')[1].Trim().Length;
                        }
                    }
                    else
                    {
                        if (ListUserAttr[i].Trim().Length > length)
                        {
                            length = ListUserAttr[i].Trim().Length;
                        }
                    }
                }
                ////////////////host length/////////////////
                for (int i = 0; i < ListHostAttr.Count; i++)
                {
                    if (ListHostAttr[i].Contains("/"))
                    {
                        if (ListHostAttr[i].Split('/')[1].Trim().Length > length)
                        {
                            length = ListHostAttr[i].Split('/')[1].Trim().Length;
                        }
                    }
                    else
                    {
                        if (ListHostAttr[i].Trim().Length > length)
                        {
                            length = ListHostAttr[i].Trim().Length;
                        }
                    }

                }
                if ("IpAddress".Length > length)
                {
                    length = strIpInfo.Length;
                }
                length = length + 1;

                
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
                DirectorySearcher search = new DirectorySearcher(entry);

                ///////////////////////////user search////////////////////////////
                textBoxInfo.Text = "";
                if (domainUserSearchAttribute != "")
                {                    
                    search.Filter = "(&(objectClass=user)(" + domainUserSearchAttribute + "=" + strUserInfo + "))";
                    foreach (SearchResult sResultSet in search.FindAll())
                    {   

                        //////////////////////////////////////////////
                        if (sResultSet.Properties.Contains("jpegPhoto"))
                        {
                            byte[] data = sResultSet.Properties["jpegPhoto"][0] as byte[];
                            pictureBox1.Image = ByteToImage(data);
                            pictureBox1.Cursor = Cursors.Hand;
                            ToolTip tt = new ToolTip();
                            tt.SetToolTip(pictureBox1, "Zoom image");                            
                        }
                        else
                        {
                            pictureBox1.Cursor = Cursors.Default;
                            pictureBox1.Visible = false;                            
                        }

                        ////////////////////////////////////////////

                        for (int i = 0; i < ListUserAttr.Count; i++)
                        {
                            if (ListUserAttr[i].Contains("/"))
                            {
                                space = new string(' ', length - ListUserAttr[i].Split('/')[1].Trim().Length);
                                space = space.ToString();
                                textBoxInfo.Text = textBoxInfo.Text + ListUserAttr[i].Split('/')[1].Trim() + space + ": " + GetProperty(sResultSet, ListUserAttr[i].Split('/')[0].Trim()) + Environment.NewLine;
                            }
                            else
                            {
                                space = new string(' ', length - ListUserAttr[i].Trim().Length);
                                space = space.ToString();
                                textBoxInfo.Text = textBoxInfo.Text + ListUserAttr[i].Trim() + space + ": " + GetProperty(sResultSet, ListUserAttr[i].Trim()) + Environment.NewLine;
                            }
                        }
                    }
                }
                
                //////////////////////////////computer search/////////////////////////
                if (textBoxInfo.Text.Trim() != "")
                {
                    textBoxInfo.Text = textBoxInfo.Text + Environment.NewLine;
                }
                if (domainHostSearchAttribute != "")
                {
                    search.Filter = "(&(objectClass=computer)(" + domainHostSearchAttribute + "=" + strHostInfo + "))";
                    foreach (SearchResult sResultSet in search.FindAll())
                    {
                        for (int i = 0; i < ListHostAttr.Count; i++)
                        {
                            if (ListHostAttr[i].Contains("/"))
                            {
                                space = new string(' ', length - ListHostAttr[i].Split('/')[1].Trim().Length);
                                space = space.ToString();
                                textBoxInfo.Text = textBoxInfo.Text + ListHostAttr[i].Split('/')[1].Trim() + space + ": " + GetProperty(sResultSet, ListHostAttr[i].Split('/')[0].Trim()) + Environment.NewLine;
                            }
                            else
                            {
                                space = new string(' ', length - ListHostAttr[i].Trim().Length);
                                space = space.ToString();
                                textBoxInfo.Text = textBoxInfo.Text + ListHostAttr[i].Trim() + space + ": " + GetProperty(sResultSet, ListHostAttr[i].Trim()) + Environment.NewLine;
                            }
                        }
                    }
                }

                if (textBoxInfo.Text.Trim() != "")
                {
                    space = new string(' ', length - "Ip Address".Length);
                    space = space.ToString();
                    textBoxInfo.Text = textBoxInfo.Text + "Ip Address" + space + ": " + strIpInfo;
                }
                else
                {                    
                    textBoxInfo.Text = textBoxInfo.Text + "User Name     : " + strUserInfo + Environment.NewLine;
                    textBoxInfo.Text = textBoxInfo.Text + "Computer Name : " + strHostInfo + Environment.NewLine;
                    textBoxInfo.Text = textBoxInfo.Text + "Ip Address    : " + strIpInfo;
                    return;
                }
                //////////////////////end computer search//////////////////////////                                  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InfoForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();                
            }
        }

        public static string GetProperty(SearchResult searchResult,  string PropertyName)
        {
            if(searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                FormCollection fc = Application.OpenForms;
                if (fc != null && fc.Count > 0)
                {

                    for (int i = fc.Count - 1; i >= 0; i--)
                    {
                        if ((fc[i].Name == "InfoForm") || (fc[i].Name == "SendForm") || (fc[i].Name == "SelectForm") || (fc[i].Name == "ClientForm"))
                        {
                            fc[i].Close();
                        }
                    }
                }
                //System.Diagnostics.Process.Start("C:\\Windows\\System32\\mstsc.exe", "/v:" + " " + ipAddressConnect);
                //System.Diagnostics.Process.Start("C:\\Program Files\\Ultravnc\\Vncviewer.exe", "-connect " + " " + ipAddressConnect);
                //System.Diagnostics.Process.Start("C:\\Program Files\\IntelliAdmin5\\Client5\\IntelliAdmin.exe", "hostname=" + " " + ipAddressConnect);                  
                 
                Cursor.Current = Cursors.Default;
                Close();
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
            }
        }

        private void btnPhoto_Click(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode.ToString() == "Zoom")
            {
                pictureBox1.Visible = true;
                pictureBox1.Dock = DockStyle.Fill;
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                btnPhoto.Text = "Hide Photo";
            }
            else
            {
                pictureBox1.Visible = false;
                pictureBox1.Dock = DockStyle.None;
                pictureBox1.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom);
                pictureBox1.ClientSize = new System.Drawing.Size(75, 90);
                pictureBox1.Location = new Point(12, this.Height - 140);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                btnPhoto.Text = "Show Photo";
            }
        }        
    }
}
