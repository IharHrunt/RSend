using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RSend;

namespace Client
{
    public partial class GroupForm : Form
    {
        private Dictionary<String, String> SettingList;
        private IServerObject ServerObj;
        private String userName;
        private String groupName;
        private String mode;        
        
        public GroupForm()
        {
            InitializeComponent();
        }

        public GroupForm(Dictionary<String, String> SettingList, IServerObject ServerObj, String groupName, String groupMembers, String userName, String mode)
        {
            InitializeComponent();
            try
            {
                this.ServerObj = ServerObj;
                this.SettingList = SettingList;
                this.textBoxName.Text = groupName;
                this.textBoxMembers.Text = groupMembers;
                this.userName = userName;
                this.groupName = groupName;
                this.mode = mode;
                                
                if (this.mode == "edit")
                {
                    String line = "";
                    String[] arr = groupMembers.Split(';');
                    foreach (String a in arr)
                    {
                        if (a.Trim() != "")
                        {
                            line = line + a.ToString().Trim() + ";" + Environment.NewLine;
                        }
                    }
                    this.textBoxMembers.Text = line;
                }                
            }
            catch
            {
                //Error mesage will be catched in SelectForm  ???????????????????????
            }
        }

        private void SetSetting()
        {
            try
            {                
                this.Text = "RSend Client: " + SettingList["frmGroupCaption"];
                lblGroupName.Text = SettingList["lblGroupName"];
                lblGroupMembers.Text = SettingList["lblGroupMembers"];
                lblGroupComment.Text = SettingList["lblGroupComment"];
                btnSave.Text = SettingList["btnSave"];
                btnCancel.Text = SettingList["btnCancel"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " " + "(LoadConfigFile)", SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);                
            }
        }

        private void GroupForm_Load(object sender, EventArgs e)
        {
            SetSetting();
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxName.Text == "")
                {
                    MessageBox.Show(SettingList["MsgBoxEmptyField"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                    textBoxName.Focus();
                    return;
                }
                if (textBoxMembers.Text == "")
                {
                    MessageBox.Show(SettingList["MsgBoxEmptyField"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);
                    DialogResult = DialogResult.None;
                    textBoxMembers.Focus();
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;
                List<String> ListMembers = new List<string>();
                String[] members = (textBoxMembers.Text.Trim()).Split(';');
                textBoxMembers.Text = "";
                foreach (String mem in members)
                {
                    String tmp = mem.Trim();
                    if (tmp != "")
                    {                                             
                        ListMembers.Add(RemoveBadSymbols(tmp));
                    }
                }              
                if (ListMembers.Count > 0)
                {
                    ListMembers.Sort();
                    int index = 0;
                    while (index < ListMembers.Count - 1)
                    {
                        if (ListMembers[index] == ListMembers[index + 1])
                        {
                            ListMembers.RemoveAt(index);
                        }
                        else
                        {
                            index++;                            
                        }
                    }
                }             
               foreach (String str in ListMembers)
               {
                   textBoxMembers.Text = textBoxMembers.Text + str + ";";
               }

                textBoxName.Text = textBoxName.Text.Trim();
                textBoxMembers.Text = textBoxMembers.Text.Trim();
                if (mode == "add")
                {
                    if (!(ServerObj.AddPrivateGroup(userName, textBoxName.Text.Trim(), textBoxMembers.Text.Trim())))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(SettingList["MsgBoxErrSaveGroup"], SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
                        DialogResult = DialogResult.None;
                        return;
                    }
                }
                if (mode == "edit")
                {
                    if (!(ServerObj.EditPrivateGroup(userName, groupName, textBoxName.Text.Trim(), textBoxMembers.Text.Trim())))
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show(SettingList["MsgBoxErrSaveGroup"], SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
                        DialogResult = DialogResult.None;
                        return;
                    }
                }                
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(SettingList["MsgBoxErrSaveGroup"] + " " + ex.Message, SettingList["MsgBoxCaptionError"], MessageBoxButtons.OK);
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
        

    }
}
