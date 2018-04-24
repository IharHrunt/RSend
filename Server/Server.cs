using System;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Collections.Generic;
using System.Windows.Forms; //comment for console application
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Timers;
using RSend;

namespace Server
{
    public class ServerObject : MarshalByRefObject, IServerObject
    {
        public int maxThread;
        public int timeoutKeepAlive;
        public int timeoutConfirmation;
        public int sizeLog;
        public Boolean archiveLog;        
        public String domain="";
        public String domainGroups = "";
        public String domainShowGroupDisplay = "false";
        public String domainGroupSearchAttribute = "";
        public String domainGroupDisplayAttribute = "";        
        public String domainUserAttributes = "";
        public String domainHostAttributes = "";
        public StreamWriter objWriter;
        public static int iThread = 0;
        public String fileEncode = "UTF-8";
        public String fileDat = "RSend.dat";
        public String fileLog = "RSend.log";
        public String sBuffer = "";
        public List<String> ListBuffer = new List<String>();
        public Dictionary<String, ClientInfo> ClientList = new Dictionary<String, ClientInfo>();        
        public Dictionary<String, String> ListADGroup = new Dictionary<String, String>();
        public System.Timers.Timer timer1 = new System.Timers.Timer();
        
        public ServerObject()
        {
            maxThread = Convert.ToInt32(ConfigurationSettings.AppSettings["maxThread"].Trim());
            timeoutKeepAlive = Convert.ToInt32(ConfigurationSettings.AppSettings["timeoutKeepAlive"].Trim());
            timeoutConfirmation = Convert.ToInt32(ConfigurationSettings.AppSettings["timeoutConfirmation"].Trim());             
            sizeLog = Convert.ToInt32(ConfigurationSettings.AppSettings["sizeLog"].Trim());
            archiveLog = Convert.ToBoolean(ConfigurationSettings.AppSettings["archiveLog"].Trim());
            domainShowGroupDisplay = ConfigurationSettings.AppSettings["domainShowGroupDisplay"].Trim().ToLower();
            domainGroupSearchAttribute = ConfigurationSettings.AppSettings["domainGroupSearchAttribute"].Trim().ToLower();
            domainGroupDisplayAttribute = ConfigurationSettings.AppSettings["domainGroupDisplayAttribute"].Trim().ToLower();
            domainUserAttributes = ConfigurationSettings.AppSettings["domainUserAttributes"].Trim();
            domainHostAttributes = ConfigurationSettings.AppSettings["domainHostAttributes"].Trim();            
            domainGroups = ConfigurationSettings.AppSettings["domainGroups"].Trim();

            WriteBuffer("Server created");
            LoadADGroups();
            CreatefileDat();
            timer1.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer1.Interval = timeoutKeepAlive;
            timer1.Enabled = true;                                
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {
            RenewClientList();
        }

        public Boolean Logon(ClientInfo clientData)
        {   
            try
            {
                String sUrl = String.Format(@"tcp://{0}:{1}/ClientObject.soap", clientData.ipAddress.ToString(), clientData.portN.ToString());                
                IClientObject ClientObj = (IClientObject)Activator.GetObject(typeof(IClientObject), sUrl);                
                clientData.ClientObj = ClientObj;                
                clientData.DateTimeLogin = DateTime.Now;                
                if (ClientList.ContainsKey(clientData.name))
                {
                    Logoff(clientData.name, "has duplicate key in ClientList");
                }        
                lock (this)
                {
                    ClientList.Add(clientData.name, clientData);
                }                

                WriteBuffer("Logon: " + clientData.name + ", IP address: " + clientData.ipAddress.ToString() + ", Port: " + clientData.portN.ToString());
                return true;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: Logon() " + clientData.name + ", IP address: " + clientData.ipAddress.ToString() + ", Port: " + clientData.portN.ToString() + " " + ex.Message + " " + ex.StackTrace);
                return false;
            }            
        }

        public Boolean Logoff(String name, String reason)
        {
            try
            {
                lock (this)
                {
                    ClientList.Remove(name);
                }        
                WriteBuffer("Logoff:" + " " + name + " " + reason);
                return true;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: Logoff()" + " " + name + " " + ex.Message);
                return false;
            }
        }
        
        public Boolean KeepAliveLogon(ClientInfo clientData)
        {
            try
            {
                if (ClientList.ContainsKey(clientData.name))
                {
                    lock (this)
                    {
                        ClientList[clientData.name].DateTimeLogin = DateTime.Now;                    
                    }        
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: KeepAliveLogon()" + " " + ex.Message);
                return false;
            }
        }
        
        public void RenewClientList()
        {            
            WriteBuffer("RenewClientList:" + " (iThread = " + iThread + ")");
            
            // Correct iThread (time between 3:00 and 3:30) 
            Double dToday = DateTime.Today.Ticks / TimeSpan.TicksPerSecond;
            Double dCorrect = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            Double diff = dCorrect - dToday;
            if ((diff > 10800) && (diff < 12600))            
            {
                if ((iThread != 0) && (sBuffer == DateTime.Now.ToString() + " RenewClientList:" + " (iThread = " + iThread + ")\r\n"))
                {
                    iThread = 0;
                    WriteBuffer("RenewClientList:" + " (iThread = " + iThread + ") - correction iTread");                                       
                }
            }          

            String tmp = sBuffer;
            sBuffer = "";
            WriteLog(tmp);            
            try
            {
                List<String> ListLogoff = new List<String>();
                Double dLogon = 0;
                Double dNow = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                dNow = Math.Round(dNow, 0);
                Double dInterval = Math.Round((Double)timeoutKeepAlive / 1000);
                               
                foreach (var client in ClientList)
                {
                    dLogon = client.Value.DateTimeLogin.Ticks / TimeSpan.TicksPerSecond;                    
                    if ((dNow - dLogon) > dInterval)
                    {                        
                        ListLogoff.Add(client.Value.name);                                             
                    }
                }
                foreach (String sName in ListLogoff)
                {
                    Logoff(sName, "has exceeded timer interval (msec):" + " " + timeoutKeepAlive);
                }
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: RenewClientList()" + " " + ex.Message);
            }
        }

        public List<String> GetClients()
        {
            List<String> ListClients = new List<String>();
            try
            {
                ICollection<String> keys = ClientList.Keys;
                foreach (String strUserHost in keys)
                {
                    ListClients.Add(strUserHost);
                }
                return ListClients;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetClients()" + " " + ex.Message);
                return ListClients;
            }
        }

        public List<String> GetClientsInfo()
        {
            List<String> ListClients = new List<String>();
            try
            {
                ICollection<String> keys = ClientList.Keys;
                foreach (String strUserHost in keys)
                {
                    ListClients.Add(strUserHost + ClientList[strUserHost].info);
                }
                return ListClients;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetClientsInfo()" + " " + ex.Message);
                return ListClients;
            }
        }

        public List<String> GetObjectAttr(String obj)
        {
            List<String> ListObjectAttr = new List<String>();        
            try
            {
                String[] arrayAttr;
                switch (obj)
                {
                    case "domainUserAttributes":
                        arrayAttr = domainUserAttributes.Split(';');
                        break;
                    case "domainHostAttributes":
                        arrayAttr = domainHostAttributes.Split(';');
                        break;
                    default:
                        arrayAttr = domainUserAttributes.Split(';');
                        break;
                }                            
                foreach (String arr in arrayAttr)
                {
                    if (arr.Trim() != "")
                    {
                        ListObjectAttr.Add(arr.Trim());
                    }
                }
                return ListObjectAttr;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetObjectAttr()" + " " + ex.Message);
                return ListObjectAttr;
            }
        }

        //////////////////// AD Groups /////////////////////
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

        public void LoadADGroups()
        {
            try
            {
                domain = IPGlobalProperties.GetIPGlobalProperties().DomainName;                        

                if ((domain == null) || (domain == "") || (domainGroups == null) || (domainGroups == ""))
                {
                    return;
                }
                
                DirectoryEntry entry = new DirectoryEntry("LDAP://" + domain);
                DirectorySearcher search = new DirectorySearcher(entry);               
                String[] arrayADGroup = domainGroups.Split(';');
                
                foreach (String arr in arrayADGroup)
                {                    
                    String sName = arr.Trim();
                    String sGrpName = "";                    
                    String sGrpDesc = "";

                    if ((sName != "") && (domainGroupSearchAttribute != ""))
                    {   
                        search.Filter = "(&(objectClass=group)(" + domainGroupSearchAttribute + "=" + sName + "))";
                        foreach (SearchResult sResultSet in search.FindAll())
                        {
                            sGrpName = GetProperty(sResultSet, domainGroupSearchAttribute);
                            if (domainShowGroupDisplay == "true")
                            {
                                sGrpDesc = GetProperty(sResultSet, domainGroupDisplayAttribute);
                                sGrpDesc = RemoveBadSymbols(sGrpDesc);
                            }
                        }
                        if (sGrpName != "")
                        {                            
                            if (sGrpDesc == "")
                            {
                                sGrpDesc = sGrpName;
                            }
                            ListADGroup.Add(sGrpName, sGrpDesc);
                        }
                    }                                        
                }                               
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: LoadADGroup()" + " " + ex.Message);
            }
        }
        
        public List<String> GetADGroups()
        {
            List<String> ListADGroupNames = new List<string>();
            try
            {
                foreach (var grp in ListADGroup)
                {
                    ListADGroupNames.Add(grp.Value);                    
                }
                ListADGroupNames.Sort();
                return ListADGroupNames;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetADGroup()" + " " + ex.Message);
                return ListADGroupNames;                
            }
        }        

        public List<String> GetADGroupMembers(List<String> ListADGroupSelected)
        {
            List<String> ListADGroupClients = new List<String>();
            try
            {
                foreach (var grp in ListADGroup)
                {
                    foreach (String str in ListADGroupSelected)
                    {
                        if (str == grp.Value)
                        {
                            List<String> ListADGroupMembers = new List<String>();
                            ListADGroupMembers = QueryADGroupMembers(grp.Key);
                            foreach (String member in ListADGroupMembers)
                            {
                                ListADGroupClients.Add(member);
                            }
                        }
                    }
                }
                
                if (ListADGroupClients.Count > 0)
                {
                    ListADGroupClients.Sort();
                    int index = 0;
                    while (index < ListADGroupClients.Count - 1)
                    {
                        if (ListADGroupClients[index] == ListADGroupClients[index + 1])
                        {
                            ListADGroupClients.RemoveAt(index);
                        }
                        else
                        {
                            index++;
                        }
                    }
                }
                return ListADGroupClients;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetADGroupMembers()" + " " + ex.Message);
                return ListADGroupClients;
            }
        }

        public List<String> QueryADGroupMembers(String sGroup)
        {
            List<String> ListADGroupMembers = new List<String>();
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                GroupPrincipal grpObj = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, sGroup);
                
                if (grpObj != null)
                {
                    foreach (Principal p in grpObj.GetMembers(true))
                    {                        
                        String sObjName = p.SamAccountName.ToString().Trim();                        
                        if (sObjName.Substring(sObjName.Length - 1, 1) == "$")
                        {
                            sObjName = sObjName.Substring(0, sObjName.Length - 1);
                        }
                        ListADGroupMembers.Add(sObjName);                        
                    }
                    grpObj.Dispose();
                    ctx.Dispose();
                }
                return ListADGroupMembers;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: QueryADGroupMembers()" + " " + ex.Message);
                return ListADGroupMembers;
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

        //////////////////// Private Groups ////////////////////
        public List<String> GetPrivateGroups(String userName)
        {
            List<String> ListPrivateGroups = new List<String>();
            List<String> ListLines = new List<String>();
            try
            {
                if (!File.Exists(fileDat))
                {
                    WriteBuffer("Error Source: GetPrivateGroups() File " + fileDat + " not exists ");
                    return ListPrivateGroups;
                }
                ListLines = new List<string>(File.ReadAllLines(fileDat, Encoding.GetEncoding(fileEncode)));   
                foreach (String line in ListLines)
                {                 
                    if (line.Split(';')[0].Trim().ToLower() == userName.Trim().ToLower())
                    {
                        ListPrivateGroups.Add(line.Split(';')[1].Trim());
                    }
                }
                ListPrivateGroups.Sort();
                 
                return ListPrivateGroups;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetPrivateGroups() " + userName + " " + ex.Message);
                return ListPrivateGroups;
            }
        }

        public List<String> GetPrivateGroupMembers(String userName, List<String> ListPrivateGroupSelected)
        {
            List<String> ListPrivateGroupMembers = new List<String>();
            List<String> ListLines = new List<String>();
            int count = 0;
            try
            {
                if (!File.Exists(fileDat))
                {
                    WriteBuffer("Error source: GetPrivateGroupMembers() File " + fileDat + " not exists ");
                    return ListPrivateGroupMembers;
                }
                ListLines = new List<string>(File.ReadAllLines(fileDat, Encoding.GetEncoding(fileEncode)));
                foreach (String line in ListLines)
                {
                    if (line.Split(';')[0].Trim().ToLower() == userName.Trim().ToLower())
                    {
                        foreach (String sel in ListPrivateGroupSelected)
                        {
                            if (line.Split(';')[1].Trim().ToLower() == sel.Trim().ToLower())
                            {
                                String[] members = line.Split(';');
                                int i = 0;
                                foreach (String mem in members)
                                {
                                    String tmp = mem.Trim();
                                    if (tmp != "")
                                    {
                                        if (i > 1)
                                        {
                                            ListPrivateGroupMembers.Add(tmp);
                                            count++;
                                        }
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                }
                if (count == 0)
                {
                    WriteBuffer("Error source: GetPrivateGroupMembers() " + userName + " Record to fetch not found");
                }
                ListPrivateGroupMembers.Sort();
                return ListPrivateGroupMembers;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: GetPrivateGroupMembers() " + userName + " " + ex.Message);
                return ListPrivateGroupMembers;
            }
        }


        public Boolean AddPrivateGroup(String userName, String groupName, String groupMembers)
        {   
            List<String> ListLines = new List<String>();
            try
            {
                if (!File.Exists(fileDat))
                {
                    WriteBuffer("Error source: AddPrivateGroup() File " + fileDat + " not exists");
                    return false;
                }
                ListLines = new List<string>(File.ReadAllLines(fileDat, Encoding.GetEncoding(fileEncode)));
                foreach (String line in ListLines)
                {                    
                    if (line.Split(';')[0].Trim().ToLower() == userName.Trim().ToLower())                    
                    {
                        if (line.Split(';')[1].Trim().ToLower() == groupName.Trim().ToLower())
                        {
                            WriteBuffer("Error Source: AddPrivateGroup() " + userName + " Prevent duplicate group name");
                            return false;
                        }
                    }
                }
                ListLines.Add((userName + ";" + groupName + ";" + groupMembers).Trim());
                File.WriteAllLines(fileDat, ListLines.ToArray(), Encoding.GetEncoding(fileEncode));                
                return true;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: AddPrivateGroup() " + userName + " " + ex.Message);
                return false;
            }
        }

        public Boolean EditPrivateGroup(String userName, String groupNameOld, String groupNameNew, String groupMembers)
        {            
            List<String> ListLines = new List<String>();
            int count = 0;
            String match = "";
            try
            {
                if (!File.Exists(fileDat))
                {
                    WriteBuffer("Error Source: EditPrivateGroup() File " + fileDat + " not exists");
                    return false;
                }
                ListLines = new List<string>(File.ReadAllLines(fileDat, Encoding.GetEncoding(fileEncode)));
                foreach (String line in ListLines)
                {                 
                    if (line.Split(';')[0].Trim().ToLower() == userName.Trim().ToLower())                    
                    {                        
                        if (groupNameOld.ToLower() == groupNameNew.ToLower())
                        {
                            if (line.Split(';')[1].Trim().ToLower() == groupNameOld.Trim().ToLower())
                            {
                                match = line.Trim();
                                count++;
                            }
                        }
                        else
                        {
                            if (line.Split(';')[1].Trim().ToLower() == groupNameOld.Trim().ToLower())
                            {
                                match = line.Trim();
                                count++;
                            }
                            if (line.Split(';')[1].Trim().ToLower() == groupNameNew.Trim().ToLower())
                            {                                
                                count++;
                            }
                        }
                    }
                }
                if (count == 0)
                {
                    WriteBuffer("Error source: EditPrivateGroup() " + userName +  " Record to update not found");
                    return false;
                }
                if (count > 1)
                {
                    WriteBuffer("Error Source: EditPrivateGroup() " + userName + " Prevent duplicate group name");
                    return false;
                }                
                ListLines.Remove(match);
                ListLines.Add(userName + ";" + groupNameNew + ";" + groupMembers);
                File.WriteAllLines(fileDat, ListLines.ToArray(), Encoding.GetEncoding(fileEncode));
                return true;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: EditPrivateGroup() " + userName + " " + ex.Message);
                return false;
            }
        }

        public Boolean DeletePrivateGroup(String userName, List<String> ListPrivateGroupSelected)
        {
            List<String> ListDeleteGroups = new List<String>();
            List<String> ListLines = new List<String>();
            int count = 0;
            try
            {
                if (!File.Exists(fileDat))
                {
                    WriteBuffer("Error source: DeletePrivateGroup() File " + fileDat + " not exists ");
                    return false;
                }
                ListLines = new List<string>(File.ReadAllLines(fileDat, Encoding.GetEncoding(fileEncode)));
                foreach (String line in ListLines)
                {                    
                    if (line.Split(';')[0].Trim().ToLower() == userName.Trim().ToLower())                    
                    {
                        foreach (String sel in ListPrivateGroupSelected)
                        {
                            if (line.Split(';')[1].Trim().ToLower() == sel.Trim().ToLower())
                            {
                                ListDeleteGroups.Add(line);                                
                                count++;
                            }
                        }
                    }
                }
                if (count == 0)
                {
                    WriteBuffer("Error source: DeletePrivateGroup() " + userName + " Record to delete not found");
                    return false;
                }
                foreach (String del in ListDeleteGroups)
                {
                    ListLines.Remove(del);
                }
                File.WriteAllLines(fileDat, ListLines.ToArray(), Encoding.GetEncoding(fileEncode));
                return true;              
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: DeletePrivateGroup() " + userName + " " + ex.Message);
                return false;
            }
        }

        public String MessageNumber()
        {
            String strNumber = "00000000000000"; //63567716371039
            try
            {
                Double dNow = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                dNow = Math.Round(dNow, 0);
                strNumber = dNow.ToString();                
                return strNumber;
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: MessageNumber()" + " " + ex.Message);
                return strNumber;
            }
        }

        //////////////////// Send Message ////////////////////
        public void SendMessageToServer(String senderName_f, List<String> recieverNames_f, String message_f, Boolean confirmation_f)
        {
                        
            String senderName = "";
            String senderNameInfo = "";
            String info = senderName_f.Split(')')[1].Trim();
            if ((info == null) || (info == ""))
            {
                senderName = senderName_f;
                senderNameInfo = senderName_f;
            }
            else
            {
                senderName = senderName_f.Split(')')[0].Trim() + ")";
                senderNameInfo = senderName_f;
            }                      
            List<String> recieverNames = recieverNames_f;
            String message = message_f;
            Boolean confirmation = confirmation_f;            
            String msgNumber = "#";            
            String messageLog = message;
            if (message == "selftest")
            {
                msgNumber = "#selftest";            
            }
            else
            {
                msgNumber = "#" + MessageNumber();
                messageLog = message.Replace("\n", "(\\n)").Replace("\r", "(\\r)"); //\n = (LF) = 0x0A  \r = (CR) = 0x0D 
            }

            Thread thrMain = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    WriteBuffer("Send message " + msgNumber + " (iThread = " + iThread + ") from: " + senderName + " text: " + messageLog);                                            
                    int countTotal = recieverNames.Count;
                    int countSent = countTotal;
                    int countDelivered = 0;
                    List<String> deliveryNames = new List<String>();

                    foreach (String clname in recieverNames)
                    {
                        iThread++;//don't move
                        int i = 0;
                        while (iThread >= maxThread)
                        {
                            System.Threading.Thread.Sleep(100); //waiting for free thread
                            i++;
                        }
                        String _senderName = "";
                        String _senderNameInfo = "";
                        String _clname = "";                        
                        String _clnameInfo = "";    
                        String _info = clname.Split(')')[1].Trim();
                        if ((_info == null) || (_info == ""))
                        {
                            _senderName = senderName;
                            _senderNameInfo = senderName;
                            _clname = clname;                        
                            _clnameInfo = clname;    
                        }
                        else
                        {
                            _senderName = senderName;
                            _senderNameInfo = senderNameInfo;
                            _clname = clname.Split(')')[0].Trim() + ")";                        
                            _clnameInfo = _info.Split(';')[0].Trim() + " (" + _info.Split(';')[1].Trim() + ")";
                        }                                               
                        String _message = message;
                        Boolean _confirmation = confirmation;
                        int _iThread = iThread;//don't move

                        Thread thr = new Thread(new ThreadStart(delegate()
                        {
                            try
                            {
                                if (ClientList.ContainsKey(_clname))
                                {                                    
                                    ClientList[_clname].ClientObj.SendMessageToClient(_senderNameInfo, _message, false);
                                    WriteBuffer("Success delivery message " +  msgNumber + " (iThread = " + _iThread + ") from: " + _senderName + " to: " + _clname);
                                }
                                else
                                {
                                    WriteBuffer("Error: Fail delivery message " + msgNumber + " (iThread = " + _iThread + ") from: " + _senderName + " to: " + _clname + " client is offline");

                                    deliveryNames.Add(_clnameInfo);
                                }

                                if (_confirmation)
                                {
                                    countSent--;
                                }
                                iThread--;
                            }
                            catch (Exception ex)
                            {
                                WriteBuffer("Error: Fail delivery message " +  msgNumber + " (iThread = " + _iThread + ") from: " + _senderName + " to: " + _clname + " " + ex.Message);
                                if (_confirmation)
                                {
                                    countSent--;
                                    deliveryNames.Add(_clnameInfo);
                                }
                                iThread--;
                                Logoff(_clname, "failed to receive message");
                            }
                        }
                        ));
                        thr.Start();
                    }

                    // Start confirmation
                    if (confirmation)
                    {
                        iThread++;//don't move
                        String _senderName = senderName;
                        String _senderNameInfo = senderNameInfo;                        
                        String _message = message;
                        String _messageLog = messageLog;
                        String _respond = "";
                        int _iThread = 0;
                        
                        Thread thr = new Thread(new ThreadStart(delegate()
                        {
                            try
                            {
                                int confirmationDelaySec = (int)(Math.Round((double)timeoutConfirmation / 1000));
                                int j = 0;
                                while (j < confirmationDelaySec)
                                {
                                    j++;
                                    if ((countSent == 0))
                                    {
                                        j = confirmationDelaySec;
                                    }
                                    else
                                    {
                                        System.Threading.Thread.Sleep(1000); //waiting for "later" clients
                                    }
                                }
                                 
                                _iThread = iThread;
                                WriteBuffer("Send confirmation message " + msgNumber + " (iThread = " + _iThread + ") to: " + _senderName + " text: " + _messageLog);
                                String failedNames = "";
                                if (deliveryNames.Count > 0)
                                {
                                    failedNames = "sDeliveryFailed:\n";
                                    foreach (String fname in deliveryNames)                                    
                                    {                                        
                                        failedNames = failedNames + fname + "\n";
                                    }
                                }         
                                countDelivered = countTotal - deliveryNames.Count;
                                _respond = "sDeliveryTotal: " + countDelivered + " / " + countTotal + "\n" + failedNames + "\n" + _message;
                                ClientList[senderName].ClientObj.SendMessageToClient(_senderNameInfo, _respond, true);                                    
                                WriteBuffer("Success delivery confirmation message " + msgNumber + " (iThread = " + _iThread + ") to: " + _senderName);
                                iThread--;
                            }
                            catch (Exception ex)
                            {
                                WriteBuffer("Error: Fail delivery confirmation message " +  msgNumber + " (iThread = " + _iThread + ") to: " + _senderName + " " + ex.Message);
                                iThread--;
                                Logoff(_senderName, "failed to receive confirmation");
                            }
                        }
                        ));
                        thr.Start();
                    }
                    ///////////
                    WriteBuffer("Finish delivery message " +  msgNumber + " (iThread = " + iThread + ") from: " + senderName + ", confirmation status:" + " " + confirmation);
                }
                catch (Exception ex)
                {
                    WriteBuffer("Error source: SendMessageToServer()" + " (iThread = " + iThread + ") " + ex.Message);
                    iThread = 0;
                }
            }
            ));
            thrMain.Start();
        }

        public void CreatefileDat()
        {
            try
            {
                FileInfo f = new FileInfo(fileDat);
                if (!f.Exists)
                {            
                    objWriter = new StreamWriter(fileDat, true, Encoding.GetEncoding(fileEncode));                    
                    WriteBuffer(fileDat + " created");
                }
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: CreatefileDat()" + " " + ex.Message);
            }
            finally
            {
                if (!(objWriter == null))
                {
                    objWriter.Close();
                }
            }
        }

       public void WriteBuffer(String str)
        {
            try
            {   
                lock (this)
                {
                    sBuffer = sBuffer + DateTime.Now.ToString() + " " + str + "\r\n";
                }                
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: WriteBuffer()" + " " + ex.Message);                
            }
        }

        public void WriteLog(String str)
        {                    
            try
            {                
                FileInfo f = new FileInfo(fileLog);
                if (f.Exists)
                {
                    if (f.Length > sizeLog)
                    {
                        if (archiveLog)
                        {

                            String year = DateTime.Today.Year.ToString();
                            String month = DateTime.Today.Month.ToString();
                            if (month.Length == 1)
                            {
                                month = "0" + month;
                            }
                            String day = DateTime.Today.Day.ToString();
                            if (day.Length == 1)
                            {
                                day = "0" + day;
                            }
                            String fileArc = "archive-" + year + month + day + "-" + fileLog;
                            System.IO.File.Move(fileLog, fileArc);
                        }
                        else
                        {
                            System.IO.File.Delete(fileLog);
                        }
                    }
                }
                objWriter = new StreamWriter(fileLog, true, Encoding.GetEncoding(fileEncode));                
                objWriter.Write(str);                
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: WriteLog()" + " " + ex.Message);
            }
            finally
            {
                if (!(objWriter == null))
                {
                    objWriter.Close();
                }
            }
        }

        public void Close()
        {
            try
            {
                WriteBuffer("Server destroyed" + " (iThread = " + iThread + ")");
            }
            catch (Exception ex)
            {
                WriteBuffer("Error source: Close()" + " " + ex.Message);
            }
        }
 
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String proc = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(proc);
                if (processes.Length > 1)
                {
                    MessageBox.Show("One copy of RSend Server is already running on your computer!", "Warning",  MessageBoxButtons.OK);
                    Application.Exit();
                    //Console.WriteLine("One copy of RSend Server is already running on your computer!");
                    return;
                }

                ServerObject ServerObj;
                try
                {
                    RemotingConfiguration.Configure("RSendServer.exe.config", false);
                    ServerObj = new ServerObject();
                    RemotingServices.Marshal(ServerObj, "ServerObject.rem");
                } 
                catch (Exception ex)
                {                
                    MessageBox.Show("Configuration file not found or damaged! " + ex.Message, "Error", MessageBoxButtons.OK);
                    Application.Exit();
                    //Console.WriteLine("Configuration file not found or damaged!");
                    return;
                }
                                
                // Specify Console Application in Properties
                //ConsoleKeyInfo cki;
                //Console.TreatControlCAsInput = true; //Prevent example from ending if CTL+C is pressed.
                //do
                //{
                //    cki = Console.ReadKey();
                //} while (!(((cki.Modifiers & ConsoleModifiers.Control) != 0) && (cki.Key.ToString().ToLower() == "z")));
                Application.Run(new ServerForm(ServerObj));
                
                ServerObj.Close();
                ServerObj.WriteLog(ServerObj.sBuffer);
                ServerObj = null;
            }
            catch (Exception ex)
            {                
                MessageBox.Show("" + ex.Message, "Warning",  MessageBoxButtons.OK);
                Application.Exit();
                //Console.WriteLine(ex.Message);
                return;
            }                       
        }
    }
}

