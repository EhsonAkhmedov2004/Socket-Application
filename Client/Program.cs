
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


var CLIENT = new AsynchronousClient();
public class StateObject
{
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();

    public int increment = 0;
}

public class AsynchronousClient
{
    
    public const int port = 8000;
    public const string ip = "127.0.0.1";

    public ManualResetEvent connectDone = new ManualResetEvent(false);
    public ManualResetEvent sendDone    = new ManualResetEvent(false);
    public ManualResetEvent receiveDone = new ManualResetEvent(false);

    public string response = String.Empty;

    public AsynchronousClient()
    {
        StartClient();
    }
    void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

          
                state.workSocket.EndReceive(ar);
                if (bytesRead > 0)
                {

                    // There might be more data, so store the data received so far.  


                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));



                    Console.WriteLine(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    

                    /* Console.WriteLine(state.sb.ToString());*/

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }

            


        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
    void Receive(Socket client)
    {
        try
        {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = client;
            
           
            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
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
            Socket client = (Socket)ar.AsyncState;
           
           
          
            
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            

            

                sendDone.Set();


        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }

    }

    void Send(Socket client, string data)
    {

       
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        client.BeginSend(
            byteData,
            0,
            byteData.Length,
            0,
            new AsyncCallback(SendCallback), client); 

    }

    // OWN 
    void MessageGetData(StateObject state,Socket client)
    {
        
    }

    void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Console.WriteLine(ar);

            Socket client = (Socket) ar.AsyncState;
            client.EndConnect(ar);
            

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());


            // Signal that the connection has been made.  
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



            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            StateObject state = new StateObject();
            state.workSocket = client;
            // state.connections.Append(client);


            //state.connections.Concat(new Socket[] { state.workSocket }).ToArray(); // increment connections !...
            // Console.WriteLine(state.connections.ToArray().Length);
            //state.connections[state.connections.Length] = client;

            client.BeginConnect(IPendpoint,
                new AsyncCallback(ConnectCallback), client);
           
         

            connectDone.WaitOne();
            

            

                Console.WriteLine("Name");
                var Name = Console.ReadLine();
            while(true)
            {
                Receive(client);
                Console.WriteLine("Type > ");
                var message = $"{Name}: {Console.ReadLine()}";
                Send(client, message);
                Receive(client);
                /*         sendDone.WaitOne();*/
                /* receiveDone.WaitOne();
                 client.Shutdown(SocketShutdown.Both);
                 client.Close();*/

                /*Console.Write("Type > ");
                var message = Console.ReadLine();
                Send(state, message);*/

                /*sendDone.WaitOne();

               
                receiveDone.WaitOne();*/
            }
       
            
         /*   Send(client, "This is a test2<EOF>");
            sendDone.WaitOne();*/
           

            // Receive the response from the remote device.  
            

            

           

            // Write the response to the console.  
            Console.WriteLine("Response received : {0}", response);
            Console.WriteLine(state.sb.ToString());

            // Release the socket.  
            client.Shutdown(SocketShutdown.Both);
            client.Close();





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
