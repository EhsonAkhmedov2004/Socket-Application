// See https://aka.ms/new-console-template for more information

var array = new List<string>();

var accept = true;

void Insert(ref string[] arr,string value,int index)
{
    string[] data = new string[arr.Length + 1];
    data[index] = value;

}

do
{
    var m = Console.ReadLine();
    if (m == "EXIT") accept = false;
    var a = out array;
    Console.WriteLine(array.ToArray().Length);
    

} while (accept == true);


































/*using System.Net;
using System.Net.Sockets;
using System.Text;

const string ip = "127.0.0.1";
const int port = 8100;



var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);



while (true)
{


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

}
*/