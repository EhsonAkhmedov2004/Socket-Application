// See https://aka.ms/new-console-template for more informat
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;


//Let's start !.....



AsynchronousSocketListener socket = new AsynchronousSocketListener();
public class ArrConfig
{
    public string[] array = { };
}
public class StateObject
{
    public const int bufferSize = 1024;

    public byte[] buffer = new byte[bufferSize];

    // Received data string.
    public StringBuilder sb = new StringBuilder();

    // Client socket.
    public Socket workSocket = null;

    public ICollection<Socket> connections = new List<Socket>{};
    

}


/*
 * ArrayList connections =- new ArrayList(25);
//when guy connects
connections.add(socket) //this is the current socket

for (int i = 0 ; i < connections.count; i++)
{
    Socket temp = (Socket) connectionsIdea;
    temp.Send(byte[]);
}

[/code]
 
 
 
 */










class AsynchronousSocketListener
{
    public StateObject STATEOBJECT = new StateObject();
    public AsynchronousSocketListener() {

        this.StartListening();
    }
    public static ManualResetEvent allDone = new ManualResetEvent(false);



    /*    void Send(Socket listener, int bytes)
        {
            StateObject state = new StateObject();
            byte[] bigData = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(state.buffer, 0, bytes));

            listener.BeginSend(
              bigData,
              0,
              bigData.Length,
              0,
              new AsyncCallback(SendCallback), listener);

        }*/


    /*
        void SendCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            int bytesSent = listener.EndSend(ar);

            Console.WriteLine("{0} bytes were sent !...", bytesSent);



        }*/

    void Send(Socket handler, String data)
    {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), handler);
    }

    void SendCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.  
            Socket handler = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    void ReadCallback(IAsyncResult ar) 
    {
        var ArrConfiguration = new ArrConfig();

        string content = String.Empty;
        StateObject state = (StateObject)ar.AsyncState;
        Socket handler = state.workSocket;
        

        int bytesRead = handler.EndReceive(ar);
        Console.WriteLine("READ DATA!....");

        if (bytesRead > 0)
        {    
            var m = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
      
            state.sb.Append(m);
            Console.WriteLine(m);
            foreach(Socket client in state.connections)
            {
                client.Send(Encoding.ASCII.GetBytes(m));
            }

            
            /*foreach(Socket socket in state.connections)
            {

                socket.Send(Encoding.ASCII.GetBytes("Message"));
                Console.WriteLine("SENT");
            }*/

     
           handler.BeginReceive(state.buffer, 0, StateObject.bufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
        }

    }
    void AcceptCallback(IAsyncResult ar)
    {
        allDone.Set();
        Socket listener = (Socket)ar.AsyncState;
        Socket handler = listener.EndAccept(ar);

        StateObject s = new StateObject();
        s.workSocket = handler;

        s.connections = STATEOBJECT.connections;

        
        STATEOBJECT.workSocket = handler;
        STATEOBJECT.connections.Add(handler);
        handler.Send(Encoding.ASCII.GetBytes("WELCOME"));
        Console.WriteLine(STATEOBJECT.connections.ToArray().Length);


      /*  if (STATEOBJECT.connections.ToArray().Length > 0)
        {
            foreach (Socket socket in STATEOBJECT.connections)
            {

                socket.Send()
            }
        }*/

        var message = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(STATEOBJECT.buffer));

   
        
        handler.BeginReceive(
            s.buffer,
            0,
            StateObject.bufferSize,
            0,
            new AsyncCallback(ReadCallback),
            s
            );
        

    }
    void StartListening()
    {
        const string ip = "127.0.0.1";
        Console.WriteLine(IPAddress.Parse(ip));
        const int port = 8000;

        IPEndPoint IPendpoint = new IPEndPoint(IPAddress.Parse(ip), port);

        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        

        try
        {
            listener.Bind(IPendpoint);
            listener.Listen(100);
            while (true)
            {
                allDone.Reset();

                Console.WriteLine("Waiting for connection .....");

                listener.BeginAccept(
                    new AsyncCallback(AcceptCallback),
                    listener);

                allDone.WaitOne();
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }

    }

}


















































































/*const string ip = "127.0.0.1";

TcpListener listener = new TcpListener(IPAddress.Parse(ip), 8000);
listener.Start();
Console.WriteLine("UP");

while (true)
{

    Console.WriteLine("Waiting for connection!...");
    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine("Client accepted.");

    NetworkStream stream = client.GetStream();
    StreamReader reader = new StreamReader(client.GetStream());
    StreamWriter writer = new StreamWriter(client.GetStream());

    byte[] buffer = new byte[1024];
    stream.Read(buffer, 0, buffer.Length);
    int recv = 0;

    foreach (byte b in buffer) {
        if (b != 0)
        {
            recv++;
        }
    }

    string req = Encoding.UTF8.GetString(buffer, 0, recv);
    Console.WriteLine(req);
    writer.WriteLine(req);
    writer.Flush();
    
    

}

*/



































/*using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

const string ip = "127.0.0.1";
const int port = 8000;


var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);



tcpSocket.Bind(tcpEndPoint);
Console.WriteLine("CHECK POINT BEFORE LISTEN METHOD !....");
tcpSocket.Listen(10);
Console.WriteLine("CHECK POINT AFTER LISTEN METHOD !....");



while (true)
{

    var buff = new byte[256];
    var size = 0;
    var builder = new StringBuilder();

    var listener = tcpSocket.Accept();
    
    Console.WriteLine("Message from client sent.....");


    do
    {


        size = listener.Receive(buff);
        builder.Append(Encoding.Unicode.GetString(buff, 0, size)); // got data -- message



    } while (listener.Available > 0);

    Console.WriteLine(builder.ToString());
    listener.Send(Encoding.Unicode.GetBytes(builder.ToString()));
    listener.Shutdown(SocketShutdown.Both);
    listener.Close();

}*/


