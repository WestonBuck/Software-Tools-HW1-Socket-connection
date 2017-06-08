//
// Weston Buck     Colby Dial
// Program 1
//

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// Note: This code is slightly modified from https://msdn.microsoft.com/en-us/library/kb5kfec7(v=vs.110).aspx

public class SynchronousSocketClient
{

    public static void StartClient(IPHostEntry ipHostInfo, String input)
    {
        // Data buffer for incoming data.
        byte[] bytes = new byte[1024];

        // Connect to a remote device.
        try
        {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(input + "<EOF>");

                // Send the data through the socket.
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.
                //if(string.Compare(input,"quit") != 0)
                //{
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

               // }

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();

            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        
        IPHostEntry ipHostInfo;
        string Input = "", quit = "quit",dnsName = "";

        Console.WriteLine("Please enter a dns name to connect to:");
        dnsName = Console.ReadLine();
        while (string.Compare(Input, quit) != 0)
        { 
            ipHostInfo = Dns.GetHostEntry(dnsName);
            Console.WriteLine("Please enter a phrase to have echoed to the server:");
            Input = Console.ReadLine();
            StartClient(ipHostInfo, Input);
            
        }
        
        return 0;
    }
}