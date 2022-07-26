
using System.Net;
using System.Net.Sockets;
using System.Text;





 new AsyncClient(); // Start client

 internal struct StateObject
 {
    public StateObject() { }  
    public const int BufferSize = 1024;
    public byte[]    buffer     = new byte[BufferSize];

 }

public class AsyncClient
{

    public Socket       connected_client = null;
    public const int    port = 8000;
    public const string ip = "127.0.0.1";

    public ManualResetEvent connectDone = new ManualResetEvent(false);

    public AsyncClient()
    {
        StartClient();
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            StateObject state = (StateObject)ar.AsyncState;

            

            int bytesRead = connected_client.EndReceive(ar);


            if (bytesRead > 0)
            {

                
                Console.WriteLine(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                connected_client.BeginReceive(state.buffer,
                                        0,
                                        StateObject.BufferSize,
                                        0,
                                        new AsyncCallback(ReceiveCallback), state);
            }
                

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    void Receive()
    {
        try
        {
  
            StateObject state = new StateObject();
  
 
            connected_client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    void SendCallback(IAsyncResult ar) 
    {
        try
        {     
            int bytesSent = connected_client.EndSend(ar);

            Console.WriteLine("Sent {0} bytes to server.", bytesSent);

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    void Send(string data)
    {

  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        connected_client.BeginSend(
            byteData,
            0,
            byteData.Length,
            0,
            new AsyncCallback(SendCallback), null); 

    }

 

    void ConnectCallback(IAsyncResult ar)
    {
        try
        {
           

            Socket client = (Socket)ar.AsyncState;


            client!.EndConnect(ar);

            Console.WriteLine("Connected");

            connectDone.Set();


        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    } 
    
  
    void StartClient()
    {

        try
        {
            IPEndPoint IPendpoint = new IPEndPoint(IPAddress.Parse(ip), port);



            Socket client = new Socket(AddressFamily.InterNetwork,
                                       SocketType   .Stream,
                                       ProtocolType .Tcp);


            connected_client = client;


            connected_client.BeginConnect(IPendpoint,
                new AsyncCallback(ConnectCallback),client);
           
         

            connectDone.WaitOne();
            

            

            Console.WriteLine("Name");
            var Name = Console.ReadLine();
            while (true)
            {
                Receive();
                var message = $"{Name}: {Console.ReadLine()}";
                Send(message);
                Receive();



            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

    }





}




































































/*while (true)
{
TcpClient tcpClient = new TcpClient("127.0.0.1", 8000);
    NetworkStream stream = tcpClient.GetStream();

    string message = Console.ReadLine();

    int byteCount = Encoding.ASCII.GetByteCount(message + 1);
    byte[] sendData = new byte[byteCount];
    sendData = Encoding.ASCII.GetBytes(message);

    
    stream.Write(sendData, 0, sendData.Length);
    Console.WriteLine("Sending data");

  StreamReader streamReader = new StreamReader(stream);
    string response = streamReader.ReadLine();

    Console.WriteLine(response);
    stream.Close();
 
}
*/


















































/*const string ip = "127.0.0.1";
const int port = 8000;






while (true)
{

    var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
    var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    tcpSocket.Connect(tcpEndPoint);




    Console.Write("Type Message > ");
    var message = Console.ReadLine();

    var data = Encoding.Unicode.GetBytes(message);

    tcpSocket.Send(data);

    var buff = new byte[256];
    var size = 0;
    var builder = new StringBuilder();



    do
    {
        size = tcpSocket.Receive(buff);

        builder.Append(Encoding.Unicode.GetString(buff, 0, size));

    } while (tcpSocket.Available > 0);


    Console.WriteLine(builder.ToString());
    tcpSocket.Shutdown(SocketShutdown.Both);
    tcpSocket.Close();

}*/
