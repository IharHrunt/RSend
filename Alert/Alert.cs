using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using RSend;

namespace Alert
{
    public class AlertObject
    {
        private delegate void SendMessageDelegate(String senderName, List<String> resieverNames, String message, Boolean confirmation);
        
        public AlertObject()
        {
            SendMessage();
        }

        private void SendMessage()
        {
            try
            {
                List<String> ListClientsInfo = new List<string>();
                List<String> ListSend = new List<string>();
                List<String> ListSendServer = new List<string>();
                
                String ipAddressServer = ConfigurationSettings.AppSettings["ipAddressServer"].Trim();
                int portServer = Convert.ToInt32(ConfigurationSettings.AppSettings["portServer"].Trim());
                String to = ConfigurationSettings.AppSettings["to"].Trim();
                String text = ConfigurationSettings.AppSettings["Text"].Trim();

                if (ipAddressServer == "")
                {
                    Console.Write("Parameter 'ipAddressServer' cannot be empty! Please check configuration file!");
                    return;
                }
                if (to == "")
                {
                    Console.Write("Parameter 'to' cannot be empty! Please check configuration file!");
                    return;
                }
                if (text == "")
                {
                    Console.Write("Parameter 'text' cannot be empty! Please check configuration file!");
                    return;
                }              
                String hostName = Dns.GetHostName().ToLower();                
                if (hostName != "")
                {
                    if (hostName.Length > 1)
                    {
                        hostName = hostName.ToLower();
                        hostName = hostName.Remove(1).ToUpper() + hostName.Substring(1);
                    }
                    else
                    {
                        hostName = hostName.ToUpper();
                    }
                }
                
                String userName = "RSendAlert (" + hostName + ")";                

                String serverURL = "tcp://" + ipAddressServer + ":" + portServer.ToString() + "/ServerObject.rem";
                IServerObject ServerObj = (IServerObject)Activator.GetObject(typeof(IServerObject), serverURL);
                ListClientsInfo = ServerObj.GetClientsInfo();

                if (to == "*")
                {
                    ListSendServer = ListClientsInfo;
                    if (ListSendServer.Count == 0)
                    {
                        Console.Write("Sorry. There is nobody to send this message.");
                        return;
                    }
                }
                else
                {
                    String[] arrayTo = to.Split(';');
                    foreach (String arr in arrayTo)
                    {
                        String tmp = arr.Trim();
                        if (tmp == "")
                        {
                            continue;
                        }
                        ListSend.Add(tmp);                    }


                    for (int i = 0; i < ListSend.Count; i++)
                    {
                        for (int j = 0; j < ListClientsInfo.Count; j++)
                        {
                            if (Regex.IsMatch(ListClientsInfo[j].ToString(), ListSend[i].ToString(), RegexOptions.IgnoreCase)) 
                            {
                                ListSendServer.Add(ListClientsInfo[j].ToString().Split(')')[0].Trim() + ")");
                            }
                        }
                    }
                    if (ListSendServer.Count > 0)
                    {
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
                    else
                    {
                        Console.Write("Sorry. There is nobody to send this message.");
                        return;
                    }
                }
                                
                AsyncCallback SendMessageCallBack = new AsyncCallback(SendMessageReturned);
                SendMessageDelegate SMDel = new SendMessageDelegate(ServerObj.SendMessageToServer);
                IAsyncResult SendMessageAsyncResult = SMDel.BeginInvoke(userName, ListSendServer, text, false, SendMessageCallBack, SMDel);
                Console.Write("The message has been sent."); 
            }
            catch (Exception ex)
            {
                Console.Write("Cannot send this message. Either RSend server is offline or incorrect parameters in configuration file."); // + ex.ToString());                   
            }
        }

        void SendMessageReturned(IAsyncResult result)
        {
            try
            {
                SendMessageDelegate dlgt = (SendMessageDelegate)result.AsyncState;
                dlgt.EndInvoke(result);
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }
        }
    }

    class Program
    {        
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                if (File.Exists(args[0].Trim()))
                {
                    AppDomain.CurrentDomain.SetData("APP_CONFIG_FILE", args[0].Trim());                    
                    AlertObject AlObj = new AlertObject();
                }
                else
                {
                    Console.Write("File " + args[0] +" does not exist. Plese check command line parameter.");
                }
            }
            else
            {
                Console.Write("Please specify command line parameter. Example: RSendAlert.exe Filename");
            }
        }
    }
}
