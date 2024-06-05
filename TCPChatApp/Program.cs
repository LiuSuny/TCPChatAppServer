using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace TCPChatApp
{
    /// <summary>
    /// Creating server side TCP App
    /// </summary>
    public class Program
    {
        /// <summary>
        /// creating a global TcpListener server that accept incoming connection requests
        /// </summary>
        public static TcpListener listener;

        /// <summary>
        /// TCPclient class that enable a connection, send and receive data over a network.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=net-8.0
        /// </summary>
        public static List<TcpClient> clients = new List<TcpClient>();

        static void Main(string[] args)
        {
            //create a listerning port
            int port = 3000;

            //connect the port to the IPAddress
            listener = new TcpListener(System.Net.IPAddress.Any, port);

            //start listerning
            listener.Start();

            Console.WriteLine("Server listening at port:" + port);

            while (true)
            {
                //accept a pending request
                TcpClient client = listener.AcceptTcpClient();

                //next we use lock thread to add our client to our server 
                lock (clients)
                {
                    //add our client
                    clients.Add(client);
                }
                Thread threadClient = new Thread(TakeClient);
                threadClient.Start(client);
            }

        }

        /// <summary>
        /// Method that handle clients
        /// </summary>
        /// <param name="obj"></param>
        private static void TakeClient(object obj)
        {
            TcpClient tcpClient = (TcpClient)obj; //converting 

            //using a one-way, point-to-point buffered communication model
            //for sending and receiving data over Stream sockets in blocking mode
            NetworkStream stream = tcpClient.GetStream();

            //just creating a byte array
            byte[] buffer = new byte[1024];

            int byteReadCount; //count of client

            while ((byteReadCount= stream.Read(buffer, 0, buffer.Length)) !=0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, byteReadCount);
                Console.WriteLine($"Recieved: " + message);
                TransmitMessage(message, tcpClient);
            }

            //once done we can remove our client and close the connection
            lock (clients)
            {
                clients.Remove(tcpClient);
            }
            tcpClient.Close();
            
        }

        private static void TransmitMessage(string message, TcpClient tcpClient)
        {
            //creating an array 
            byte[] buffer = Encoding.ASCII.GetBytes(message);

            //thread
            lock (clients)
            {
                //checking client 
                foreach (var client in clients)
                {  
                    //if the client is not 
                    if (client != tcpClient)
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
       
    }
}

#region TCPCLIENT1

//NOTE: to run this project kindly create another ConsoleApp, copy and paste the following code to run 
//Ability to send and receive message from client side 

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TCPChatClientSide
//{
//    public class Program
//    {
//        private static TcpClient client;
//        private static NetworkStream stream;
//        private static string username;
//        static void Main(string[] args)
//        {
//            Console.Write("Enter your username: ");
//            username = Console.ReadLine();

//            //connect to specific IPAddress
//            string server = "127.0.0.1";
//            int port = 5000; //connect to server port

//            client = new TcpClient(server, port); //tap into the port

//            stream = client.GetStream(); //data streaming

//            //thread for receiving message
//            Thread receiveThread = new Thread(ReceiveMessages);

//            receiveThread.Start(); //start thread

//            Console.WriteLine("Connected to the TcpChatApp server."); //connect to server
//            SendMessages(); //calling our sendMessage method
//        }

//        //Send message method
//        private static void SendMessages()
//        {
//            while (true)
//            {
//                //string message = Console.ReadLine();
//                //byte[] buffer = Encoding.ASCII.GetBytes(message);
//                //stream.Write(buffer, 0, buffer.Length);

//                string message = Console.ReadLine();
//                string fullMessage = $"{username}: {message}";
//                byte[] buffer = Encoding.ASCII.GetBytes(fullMessage);
//                stream.Write(buffer, 0, buffer.Length);
//            }

//        }

//        //receive Message method
//        private static void ReceiveMessages()
//        {
//            byte[] buffer = new byte[1024];
//            int byteCount;

//            while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
//            {
//                string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
//                Console.WriteLine("Received: " + message);
//            }

//        }
//    }
//}

#endregion

#region TCPCLIENT1
//NOTE: to add more client to our chat tcp project to connect create another ConsoleApp, copy and paste the following code to run 
//Ability to send and receive message from client side and all the client will view the chat message sent and receive 
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace TCPChatClientSide2
//{
//    public class Program
//    {
//        private static TcpClient client;
//        private static NetworkStream stream;
//        private static string username;
//        static void Main(string[] args)
//        {
//            Console.Write("Enter your username: ");
//            username = Console.ReadLine();

//            //connect to specific IPAddress
//            string server = "127.0.0.1";
//            int port = 3000; //connect to server port

//            client = new TcpClient(server, port); //tap into the port

//            stream = client.GetStream(); //data streaming

//            //thread for receiving message
//            Thread receiveThread = new Thread(ReceiveMessages);

//            receiveThread.Start(); //start thread

//            Console.WriteLine("Connected to the TcpChatApp server."); //connect to server
//            SendMessages(); //calling our sendMessage method
//        }

//        //Send message method
//        private static void SendMessages()
//        {
//            while (true)
//            {
//                //string message = Console.ReadLine();
//                //byte[] buffer = Encoding.ASCII.GetBytes(message);
//                //stream.Write(buffer, 0, buffer.Length);

//                string message = Console.ReadLine();
//                string fullMessage = $"{username}: {message}";
//                byte[] buffer = Encoding.ASCII.GetBytes(fullMessage);
//                stream.Write(buffer, 0, buffer.Length);
//            }

//        }

//        //receive Message method
//        private static void ReceiveMessages()
//        {
//            byte[] buffer = new byte[1024];
//            int byteCount;

//            while ((byteCount = stream.Read(buffer, 0, buffer.Length)) != 0)
//            {
//                string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
//                Console.WriteLine("Meassage Received: " + message);
//            }

//        }
//    }
//}

#endregion

