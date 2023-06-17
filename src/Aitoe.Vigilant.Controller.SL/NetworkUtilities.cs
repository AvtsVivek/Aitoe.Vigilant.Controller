using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Aitoe.Vigilant.Controller.SL
{
    internal class NetworkUtilities
    {
        internal static async Task<List<string>> GetSoapResponsesFromCamerasAsync()
        {
            IPAddress broadCastAddress = GetBroadcastIP();
            var result = new List<string>();
            using (var client = new UdpClient())
            {
                var ipEndpoint = new IPEndPoint(broadCastAddress, 3702);
                client.EnableBroadcast = true;
                try
                {
                    var soapMessage = GetBytes(CreateSoapRequest());
                    var timeout = DateTime.Now.AddSeconds(3);
                    await client.SendAsync(soapMessage, soapMessage.Length, ipEndpoint);

                    while (timeout > DateTime.Now)
                    {
                        if (client.Available > 0)
                        {
                            var receiveResult = await client.ReceiveAsync();
                            var text = GetText(receiveResult.Buffer);
                            result.Add(text);
                        }
                        else
                        {
                            await Task.Delay(10);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            return result;
        }

        internal static string GetCameraIpXmlFromResponseMessage(string soapResponseMessage)
        {
            var xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
            xmlNamespaceManager.AddNamespace("g", "http://schemas.xmlsoap.org/ws/2005/04/discovery");

            var element = XElement.Parse(soapResponseMessage).XPathSelectElement("//g:XAddrs[1]", xmlNamespaceManager);
            return element?.Value ?? string.Empty;
        }
        
        private static string CreateSoapRequest()
        {
            Guid messageId = Guid.NewGuid();
            const string soap = @"
            <?xml version=""1.0"" encoding=""UTF-8""?>
            <e:Envelope xmlns:e=""http://www.w3.org/2003/05/soap-envelope""
            xmlns:w=""http://schemas.xmlsoap.org/ws/2004/08/addressing""
            xmlns:d=""http://schemas.xmlsoap.org/ws/2005/04/discovery""
            xmlns:dn=""http://www.onvif.org/ver10/device/wsdl"">
            <e:Header>
            <w:MessageID>uuid:{0}</w:MessageID>
            <w:To e:mustUnderstand=""true"">urn:schemas-xmlsoap-org:ws:2005:04:discovery</w:To>
            <w:Action a:mustUnderstand=""true"">http://schemas.xmlsoap.org/ws/2005/04/discovery/Probe</w:Action>
            </e:Header>
            <e:Body>
            <d:Probe>
            <d:Types>dn:Device</d:Types>
            </d:Probe>
            </e:Body>
            </e:Envelope>
            ";

            var result = string.Format(soap, messageId);
            return result;
        }

        private static byte[] GetBytes(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }

        private static string GetText(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }

        private static IPAddress GetBroadcastIP()
        {
            var maskIP = GetHostMask();
            var hostIP = GetHostIP();

            if (maskIP == null || hostIP == null)
                return null;

            var complementedMaskBytes = new byte[4];
            var broadcastIPBytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                complementedMaskBytes[i] = (byte)~(maskIP.GetAddressBytes().ElementAt(i));
                broadcastIPBytes[i] = (byte)((hostIP.GetAddressBytes().ElementAt(i)) | complementedMaskBytes[i]);
            }

            return new IPAddress(broadcastIPBytes);
        }

        private static IPAddress GetHostIP()
        {
            foreach (IPAddress ip in (Dns.GetHostEntry(Dns.GetHostName())).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip;
            }
            return null;
        }

        private static IPAddress GetIPAddress()
        {
            // Not sure what this method is doing currently. We may have to remove this in future.
            IPAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (!ip.IsIPv6LinkLocal)
                {
                    return (ip);
                }
            }
            return addr.Length > 0 ? addr[0] : null;
        }

        private static PhysicalAddress GetMacAddress()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet && nic.OperationalStatus == OperationalStatus.Up)
                {
                    return nic.GetPhysicalAddress();
                }
            }
            return null;
        }

        private static void GetIPAndSubnetMask()
        {
            // Currently not very sure of the purpose of this method.
            // This is curretly printing some info to console. This not what we want.
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface netInterface in Interfaces)
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                if (netInterface.OperationalStatus != OperationalStatus.Up) continue;               
                Console.WriteLine(netInterface.Description + ": Mac Add " + netInterface.GetPhysicalAddress());
                UnicastIPAddressInformationCollection UnicastIPInfoCol = netInterface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    Console.WriteLine("\tIP Address is {0}", UnicatIPInfo.Address);
                    Console.WriteLine("\tSubnet Mask is {0}", UnicatIPInfo.IPv4Mask);
                }
            }
        }

        private static IPAddress GetHostMask()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface Interface in Interfaces)
            {

                IPAddress hostIP = GetHostIP();

                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;

                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    if (UnicatIPInfo.Address.ToString() == hostIP.ToString())
                    {
                        return UnicatIPInfo.IPv4Mask;
                    }
                }
            }
            return null;
        }


    }
}