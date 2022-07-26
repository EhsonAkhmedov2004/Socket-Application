using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Help
{
    public class Help
    {
        public string Frombyte(byte[]bytes,int index,int count) 
        {
            return Encoding.ASCII.GetString(bytes,index,count);

        }
        public byte[] Fromstring(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }
        

    }

}
