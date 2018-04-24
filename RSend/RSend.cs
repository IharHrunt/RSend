using System;
using System.Net;
using System.Collections.Generic;

namespace RSend
{
    public interface IServerObject
    {
        Boolean Logon(ClientInfo clientdata);        
        Boolean Logoff(String name, String reason);
        Boolean KeepAliveLogon(ClientInfo name);
        List<String> GetObjectAttr(String obj);        
        List<String> GetClients();
        List<String> GetClientsInfo();          
        List<String> GetADGroups();
        List<String> GetADGroupMembers(List<String> ListADGroupSelected);
        List<String> GetPrivateGroups(String userName);        
        List<String> GetPrivateGroupMembers(String userName, List<String> ListPrivateGroupSelected);
        Boolean AddPrivateGroup(String userName, String groupName, String groupMembers);
        Boolean EditPrivateGroup(String userName, String groupNameOld, String groupNameNew, String groupMembers);
        Boolean DeletePrivateGroup(String userName, List<String> ListPrivateGroupSelected);
        void SendMessageToServer(String senderName, List<String> ListRecieverNames, String message, Boolean confirmation);        
    }
    
    public interface IClientObject
    {
        void SendMessageToClient(String senderName, String message, Boolean confirmation);        
    }     
    
    [Serializable]
    public class ClientInfo
    {
        public String name;
        public String info;
        public IPAddress ipAddress;
        public int portN;
        public DateTime DateTimeLogin;
        public IClientObject ClientObj = null;
        public ClientInfo(String name, String info, IPAddress ipAddress, int portN, DateTime DateTimeLogin)        
        {
            this.portN = portN;
            this.name = name;
            this.info = info;
            this.ipAddress = ipAddress;
            this.DateTimeLogin = DateTimeLogin;
        }
    }    

}