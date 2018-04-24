using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Media;
using RSend;

namespace Client
{
    public class ClientObject : MarshalByRefObject, IClientObject
    {
        public ClientForm clientForm = null;        
        private delegate void txtMessagesUpdateDelegate(String senderName, String text);
        
        public ClientObject()
        {            
        }        
        
        public ClientObject(ClientForm form)
        {
            clientForm = form;            
        }         
        
        public void SendMessageToClient(String senderNameInfo, String message, Boolean confirmation)
        {

            String aliasJS = senderNameInfo.Split('(')[0].Trim();
            String senderName = senderNameInfo.Split(')')[0].Trim() + ")";
            String info = senderNameInfo.Split(')')[1].Trim();
            String senderNameDispaly = "";            

            if ((info == null) || (info == ""))
            {
                senderNameDispaly = senderName;                                
            }
            else
            {
                senderNameDispaly = info.Split(';')[0].Trim() + " (" + info.Split(';')[1].Trim() + ")";                
            }
            // hide host nmae /////////////////////////////////////////////
            if (clientForm.hideHostName == "true")
            {
                senderNameDispaly = senderNameDispaly.Split('(')[0].Trim();                
            }
            ///////////////////////////////////////////////////////////////            
            
            String msgText;
            if (message == "selftest")
            {
                clientForm.selftestResult = true;
                return;
            }

            message = Regex.Replace(message, @"<", "&#60;");
            message = Regex.Replace(message, @">", "&#62;");
            message = Regex.Replace(message, @"\n", "<br>");
            message = Regex.Replace(message, @"  ", "&nbsp; ");

            message = Regex.Replace(message, @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                      "<span title='$1' style ='COLOR: #0000ff; TEXT-DECORATION: underline; CURSOR: pointer;' onclick=\"window.external.openLink('$1')\">$1</span>",            
                      RegexOptions.IgnoreCase);           
            
            message = Regex.Replace(message, @"(([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5}))",
                      "<span title='mailto:$1' style ='COLOR: #0000ff; TEXT-DECORATION: underline; CURSOR: pointer;' onclick=\"window.external.openLink('mailto:$1')\">$1</span>",                      
                      RegexOptions.IgnoreCase);

            if (confirmation)
            {
                message = Regex.Replace(message, @"sDeliveryTotal", clientForm.SettingList["sDeliveryTotal"]);
                message = Regex.Replace(message, @"sDeliveryFailed", clientForm.SettingList["sDeliveryFailed"]);
            }

            if (clientForm.modeReadOnly == "true")
                {
                msgText = "<tr><td align='center' height='2'></td></tr><tr><td align='left'>" +
                          "<div style='FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #000000; FONT-FAMILY: arial;'>" +
                          DateTime.Now.ToString() + " " + clientForm.SettingList["sMsgFrom"] + " " +
                          "<span style ='COLOR: #0000ff; TEXT-DECORATION: underline;'>" + senderNameDispaly + "</span></div><br>" +
                          "<font style='FONT-WEIGHT: normal; FONT-SIZE: 14px; COLOR: #000000; FONT-FAMILY: arial;'>" + message + "</font>" +
                          "<hr style = 'HEIGHT:1px;'></td></tr>";
            }
            else
            {
                msgText = "<tr><td align='center' height='2'></td></tr><tr><td align='left'>" +
                          "<div style='FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR: #000000; FONT-FAMILY: arial;'>" +
                          DateTime.Now.ToString() + " " + clientForm.SettingList["sMsgFrom"] + " " +
                          "<span title = '" + clientForm.SettingList["sTitleReply"] + "'" +
                          "style ='COLOR: #0000ff; TEXT-DECORATION: underline; CURSOR: pointer;'" +
                          "onclick=\"window.external.getReplyName('" + senderNameInfo + "')\"; onmouseup=\"mouseButton" + aliasJS + "(event)\";>" + senderNameDispaly + "</span></div><br>" +
                          "<script>var mouseButton" + aliasJS + " = function (e) {  var e = e || window.event;  var btn; if ('object' === typeof e) { btn = e.button; " +
                          "if (btn == 2) { window.external.showFormInfo('" + senderNameInfo + "'); } } }</script>" +
                          "<font style='FONT-WEIGHT: normal; FONT-SIZE: 14px; COLOR: #000000; FONT-FAMILY: arial;'>" + message + "</font>" +
                          "<hr style = 'HEIGHT:1px;'></td></tr>";
            }
                        
            txtMessagesUpdateDelegate WFUDelegate = new txtMessagesUpdateDelegate(SendMessageToClientReturned);
            clientForm.BeginInvoke(WFUDelegate, new object[] { senderName, msgText });
        }

        public void Test(String message)
        {
            MessageBox.Show(message, "client code");
        }

        public void SendMessageToClientReturned(String senderName, String text)
        {
            try
            {
                clientForm.sHTML = text + clientForm.sHTML;
                clientForm.webBrowser1.Document.OpenNew(true);
                clientForm.webBrowser1.Document.Write(clientForm.sHead + clientForm.sHTML + clientForm.sBottom);                
                clientForm.Opacity = 1;
                clientForm.menuItemClear.Enabled = true;
                if (clientForm.Visible == false)
                {
                    clientForm.Visible = true;
                    clientForm.Activate();                    
                }
                NotifySound();
            }
            catch
            {
            }
        }

        public void NotifySound()
        {
            try
            {
                if (clientForm.notifySound.ToLower() == "true")
                {                    
                    SoundPlayer player = new SoundPlayer(Environment.GetEnvironmentVariable("windir") + "\\Media\\notify.wav");
                    player.Play();
                }
            }
            catch
            {
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    class Program 
    {        
        [STAThread]
        static void Main(string[] args)
        {
            Dictionary<String, String> SettingList = new Dictionary<String, String>(); 
            try
            {                
                SettingList.Add("ipAddressServer", ConfigurationSettings.AppSettings["ipAddressServer"].Trim());
                SettingList.Add("portServer", ConfigurationSettings.AppSettings["portServer"].Trim());
                SettingList.Add("portClient", ConfigurationSettings.AppSettings["portClient"].Trim());
                SettingList.Add("timeoutKeepAlive", ConfigurationSettings.AppSettings["timeoutKeepAlive"].Trim());
                SettingList.Add("timeoutSelftest", ConfigurationSettings.AppSettings["timeoutSelftest"].Trim());                
                SettingList.Add("domainShowUserDisplay", ConfigurationSettings.AppSettings["domainShowUserDisplay"].Trim().ToLower());
                SettingList.Add("domainShowHostDisplay", ConfigurationSettings.AppSettings["domainShowHostDisplay"].Trim().ToLower());
                SettingList.Add("domainUserSearchAttribute", ConfigurationSettings.AppSettings["domainUserSearchAttribute"].Trim().ToLower());
                SettingList.Add("domainHostSearchAttribute", ConfigurationSettings.AppSettings["domainHostSearchAttribute"].Trim().ToLower());
                SettingList.Add("domainUserDisplayAttribute", ConfigurationSettings.AppSettings["domainUserDisplayAttribute"].Trim().ToLower());
                SettingList.Add("domainHostDisplayAttribute", ConfigurationSettings.AppSettings["domainHostDisplayAttribute"].Trim().ToLower());    
                SettingList.Add("notifySound", ConfigurationSettings.AppSettings["notifySound"].Trim().ToLower());
                SettingList.Add("modeReadOnly", ConfigurationSettings.AppSettings["modeReadOnly"].Trim().ToLower());
                SettingList.Add("urlHelp", ConfigurationSettings.AppSettings["urlHelp"].Trim());
                SettingList.Add("frmSendCaption", ConfigurationSettings.AppSettings["frmSendCaption"].Trim());
                SettingList.Add("frmSelectCaption", ConfigurationSettings.AppSettings["frmSelectCaption".Trim()]);
                SettingList.Add("frmGroupCaption", ConfigurationSettings.AppSettings["frmGroupCaption".Trim()]);
                SettingList.Add("frmInfoCaption", ConfigurationSettings.AppSettings["frmInfoCaption".Trim()]);
                
                SettingList.Add("lblStatusOk", ConfigurationSettings.AppSettings["lblStatusOk"].Trim());
                SettingList.Add("lblStatusErr", ConfigurationSettings.AppSettings["lblStatusErr"].Trim());

                SettingList.Add("lblMsgText", ConfigurationSettings.AppSettings["lblMsgText"].Trim());
                SettingList.Add("lblSendTo", ConfigurationSettings.AppSettings["lblSendTo"].Trim());
                SettingList.Add("lblFilter", ConfigurationSettings.AppSettings["lblFilter"].Trim());
                SettingList.Add("lblGroupName", ConfigurationSettings.AppSettings["lblGroupName"].Trim());
                SettingList.Add("lblGroupMembers", ConfigurationSettings.AppSettings["lblGroupMembers"].Trim());
                SettingList.Add("lblGroupComment", ConfigurationSettings.AppSettings["lblGroupComment"].Trim());

                SettingList.Add("menuItemSendMsg", ConfigurationSettings.AppSettings["menuItemSendMsg"].Trim());
                SettingList.Add("menuItemClear", ConfigurationSettings.AppSettings["menuItemClear"].Trim());                
                SettingList.Add("menuItemClose", ConfigurationSettings.AppSettings["menuItemClose"].Trim());
                SettingList.Add("menuItemHelp", ConfigurationSettings.AppSettings["menuItemHelp"].Trim());

                SettingList.Add("btnClear", ConfigurationSettings.AppSettings["btnClear"].Trim());
                SettingList.Add("btnSelect", ConfigurationSettings.AppSettings["btnSelect"].Trim());
                SettingList.Add("btnSend", ConfigurationSettings.AppSettings["btnSend"].Trim());
                SettingList.Add("btnOk", ConfigurationSettings.AppSettings["btnOk"].Trim());
                SettingList.Add("btnSave", ConfigurationSettings.AppSettings["btnSave"].Trim());
                SettingList.Add("btnCancel", ConfigurationSettings.AppSettings["btnCancel"].Trim());
                SettingList.Add("btnFilter", ConfigurationSettings.AppSettings["btnFilter"].Trim());
                SettingList.Add("btnClose", ConfigurationSettings.AppSettings["btnClose"].Trim());
                SettingList.Add("btnAddGroup", ConfigurationSettings.AppSettings["btnAddGroup"].Trim());
                SettingList.Add("btnEditGroup", ConfigurationSettings.AppSettings["btnEditGroup"].Trim());
                SettingList.Add("btnDelGroup", ConfigurationSettings.AppSettings["btnDelGroup"].Trim());
                SettingList.Add("btnCopy", ConfigurationSettings.AppSettings["btnCopy"].Trim());

                SettingList.Add("cbConfirm", ConfigurationSettings.AppSettings["cbConfirm"].Trim());
                SettingList.Add("cbSelectAll", ConfigurationSettings.AppSettings["cbSelectAll"].Trim());

                SettingList.Add("tabPageUser", ConfigurationSettings.AppSettings["tabPageUser"].Trim());
                SettingList.Add("tabPageHost", ConfigurationSettings.AppSettings["tabPageHost"].Trim());
                SettingList.Add("tabPagePrivateGroup", ConfigurationSettings.AppSettings["tabPagePrivateGroup"].Trim());
                SettingList.Add("tabPageADGroup", ConfigurationSettings.AppSettings["tabPageADGroup"].Trim());

                SettingList.Add("cbOptionStart", ConfigurationSettings.AppSettings["cbOptionStart"].Trim());
                SettingList.Add("cbOptionExist", ConfigurationSettings.AppSettings["cbOptionExist"].Trim());
                SettingList.Add("cbOptionNotStart", ConfigurationSettings.AppSettings["cbOptionNotStart"].Trim());
                SettingList.Add("cbOptionNotExist", ConfigurationSettings.AppSettings["cbOptionNotExist"].Trim());

                SettingList.Add("sToAllUsers", ConfigurationSettings.AppSettings["sToAllUsers"].Trim());
                SettingList.Add("sMsgFrom", ConfigurationSettings.AppSettings["sMsgFrom"].Trim());
                SettingList.Add("sDeliveryTotal", ConfigurationSettings.AppSettings["sDeliveryTotal"].Trim());
                SettingList.Add("sDeliveryFailed", ConfigurationSettings.AppSettings["sDeliveryFailed"].Trim());
                SettingList.Add("sTitleReply", ConfigurationSettings.AppSettings["sTitleReply"].Trim());                             
                                
                SettingList.Add("MsgBoxCaptionInfo", ConfigurationSettings.AppSettings["MsgBoxCaptionInfo"].Trim());
                SettingList.Add("MsgBoxCaptionWarning", ConfigurationSettings.AppSettings["MsgBoxCaptionWarning"].Trim());
                SettingList.Add("MsgBoxCaptionError", ConfigurationSettings.AppSettings["MsgBoxCaptionError"].Trim());

                SettingList.Add("MsgBoxSendMsgOk", ConfigurationSettings.AppSettings["MsgBoxSendMsgOk"].Trim());
                SettingList.Add("MsgBoxTwoAppRun", ConfigurationSettings.AppSettings["MsgBoxTwoAppRun"].Trim());                

                SettingList.Add("MsgBoxExit", ConfigurationSettings.AppSettings["MsgBoxExit"].Trim());
                SettingList.Add("MsgBoxClear", ConfigurationSettings.AppSettings["MsgBoxClear"].Trim());
                SettingList.Add("MsgBoxNoReplyMsg", ConfigurationSettings.AppSettings["MsgBoxNoReplyMsg"].Trim());
                SettingList.Add("MsgBoxNoReplySelect", ConfigurationSettings.AppSettings["MsgBoxNoReplySelect"].Trim());
                SettingList.Add("MsgBoxNoRegisteredUsers", ConfigurationSettings.AppSettings["MsgBoxNoRegisteredUsers"].Trim());
                SettingList.Add("MsgBoxNoReplyAlert", ConfigurationSettings.AppSettings["MsgBoxNoReplyAlert"].Trim());
                SettingList.Add("MsgBoxNoInfoAlert", ConfigurationSettings.AppSettings["MsgBoxNoInfoAlert"].Trim());  
                SettingList.Add("MsgBoxEmptyMsgTextBox", ConfigurationSettings.AppSettings["MsgBoxEmptyMsgTextBox"].Trim());
                SettingList.Add("MsgBoxEmptySelectUserTextBox", ConfigurationSettings.AppSettings["MsgBoxEmptySelectUserTextBox"].Trim());
                SettingList.Add("MsgBoxEmptyField", ConfigurationSettings.AppSettings["MsgBoxEmptyField"].Trim());                
                SettingList.Add("MsgBoxOneGroupEdit", ConfigurationSettings.AppSettings["MsgBoxOneGroupEdit"].Trim());
                SettingList.Add("MsgBoxEmptyGroupEdit", ConfigurationSettings.AppSettings["MsgBoxEmptyGroupEdit"].Trim());
                SettingList.Add("MsgBoxEmptyGroupDelete", ConfigurationSettings.AppSettings["MsgBoxEmptyGroupDelete"].Trim());
                SettingList.Add("MsgBoxConfirmDeleteGroup", ConfigurationSettings.AppSettings["MsgBoxConfirmDeleteGroup"].Trim());                
                SettingList.Add("MsgBoxNoPing", ConfigurationSettings.AppSettings["MsgBoxNoPing"].Trim());
                SettingList.Add("MsgBoxNoRemoting", ConfigurationSettings.AppSettings["MsgBoxNoRemoting"].Trim());
                SettingList.Add("MsgBoxErrSendMsg", ConfigurationSettings.AppSettings["MsgBoxErrSendMsg"].Trim());
                SettingList.Add("MsgBoxErrListSendTo", ConfigurationSettings.AppSettings["MsgBoxErrListSendTo"].Trim());
                SettingList.Add("MsgBoxErrSaveGroup", ConfigurationSettings.AppSettings["MsgBoxErrSaveGroup"].Trim());
                SettingList.Add("MsgBoxErrDeleteGroup", ConfigurationSettings.AppSettings["MsgBoxErrDeleteGroup"].Trim());
                SettingList.Add("MsgBoxErrOpenHelp", ConfigurationSettings.AppSettings["MsgBoxErrOpenHelp"].Trim());
                // hide host name /////////////////////////////////////////////////////////////////////////////////////
                try
                {
                    SettingList.Add("hideHostName", ConfigurationSettings.AppSettings["hideHostName"].Trim().ToLower());
                }
                catch
                {
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
            }
            catch
            {
                MessageBox.Show("Configuration file not found or damaged!", "Error", MessageBoxButtons.OK);
                Application.Exit();
                return;
            }

            String proc = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(proc);
            if (processes.Length > 1)
            {
                MessageBox.Show(SettingList["MsgBoxTwoAppRun"], SettingList["MsgBoxCaptionWarning"], MessageBoxButtons.OK);  
                Application.Exit();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ClientForm clientFormHide = new ClientForm(SettingList);
            clientFormHide.Opacity = 0;
            clientFormHide.Visible = true;            
            Application.Run();
        }
    }
}