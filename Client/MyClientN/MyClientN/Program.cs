//Demonstrate Sockets
using System;
using System.Net.Sockets;
using System.IO;
public class Whois
{
    static void WriteToLog(string logtext) //writes to server log
    {
        System.IO.File.AppendAllText("clientlog.txt", string.Format("{0}{1}", logtext, Environment.NewLine));
    }
    static void Main(string[] args)
    {

        TcpClient client = new TcpClient();
        client.Connect("localhost", 43);
        StreamWriter sw = new StreamWriter(client.GetStream());
        StreamReader sr = new StreamReader(client.GetStream());
        int alength = args.Length;
        
        if (alength > 0)
        {
            string newline = args[0];
            for (int i = 1; i < alength; i++)
            {
                newline += " " + args[i];
            }
            sw.WriteLine(newline);
            WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Client]: " + newline);
            sw.Flush();
            Console.WriteLine(sr.ReadToEnd());
        }
        else
        {
            Console.WriteLine("ERROR: no arguments specified");
        }
        
    }
}