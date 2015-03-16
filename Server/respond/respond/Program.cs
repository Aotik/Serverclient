using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;
public class Respond
{
    public static Hashtable storePerson = new Hashtable(); //declare hashtable
    static void WriteToLog(string logtext) //writes to server log
    {
        System.IO.File.AppendAllText("serverlog.txt", string.Format("{0}{1}", logtext, Environment.NewLine));
    }
    static void Main(string[] args)
    { 
        runServer();
    }
    static void runServer()
    {
        TcpListener listener;
        Socket connection;
        NetworkStream socketStream;
        try
        {
            listener = new TcpListener(IPAddress.Any,43);
            listener.Start();
            Console.WriteLine("Server Is Running...");
            while (true)
            {
                connection = listener.AcceptSocket();
                socketStream = new NetworkStream(connection);
                Console.WriteLine("Connection Established");
                doRequest(socketStream);
                socketStream.Close();
                connection.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }
    static void doRequest(NetworkStream socketStream)
    {

        try
        {

            StreamWriter sw = new StreamWriter(socketStream);
            StreamReader sr = new StreamReader(socketStream);
            String line = sr.ReadLine().Trim();
            if (line != "")
            {
            String[] commands = line.Split(new char[] { ' ' });
            int clength = commands.Length;
            Console.WriteLine("New input: " + line);
            
                switch (commands[0])
                {
                    case "GET":
                        if (commands[1].StartsWith("/"))
                        {
                            if (clength != 2)
                            {
                                if (commands[2] == "HTTP/1.0")
                                {
                                    string uname = commands[1].TrimStart('/');
                                    if (!storePerson.ContainsKey(uname))
                                    {

                                        sw.WriteLine("HTTP/1.0 404 Not Found");
                                        sw.WriteLine("Content-Type: text/plain");
                                        string response = "<HTTP/1.0 404 Not Found> for request: " + line;
                                        WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                        sw.Close();
                                    }
                                    else
                                    {
                                        sw.WriteLine("HTTP/1.0 200 OK");
                                        sw.WriteLine("Content-Type: text/plain");
                                        sw.WriteLine(storePerson[uname]);
                                        string response = "<HTTP/1.0 200 OK> for request: " + line;
                                        WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                        sw.Close();
                                    }
                                }
                                else if (commands[2] == "HTTP/1.1")
                                {
                                    string uname = commands[1].TrimStart('/');
                                    if (!storePerson.ContainsKey(uname))
                                    {

                                        sw.WriteLine("HTTP/1.1 404 Not Found");
                                        sw.WriteLine("Content-Type: text/plain");
                                        string response = "<HTTP/1.1 404 Not Found> for request: " + line;
                                        WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                        sw.Close();
                                    }
                                    else
                                    {
                                        sw.WriteLine("HTTP/1.1 200 OK");
                                        sw.WriteLine("Content-Type: text/plain");
                                        sw.WriteLine(storePerson[uname]);
                                        string response = "<HTTP/1.1 200 OK> for request: " + line;
                                        WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                        sw.Close();
                                    }
                                }
                            }
                            else
                            {
                                string uname = commands[1].TrimStart('/');
                                if (!storePerson.ContainsKey(uname))
                                {

                                    sw.WriteLine("HTTP/0.9 404 Not Found");
                                    sw.WriteLine("Content-Type: text/plain");
                                    string response = "<HTTP/0.9 404 Not Found> for request: " + line;
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                    sw.Close();
                                }
                                else
                                {
                                    sw.WriteLine("HTTP/0.9 200 OK");
                                    sw.WriteLine("Content-Type: text/plain");
                                    sw.WriteLine(storePerson[uname]);
                                    string response = "<HTTP/0.9 200 OK> for request: " + line;
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                    sw.Close();
                                }
                            }
                        }
                        else
                        {
                            sw.WriteLine("Name has to be in '/name' format. e.g GET /name");
                            sw.Close();
                        }
                        break;

                    case "PUT":
                        if (commands[1].StartsWith("/"))
                        {

                            string uname = commands[1].TrimStart('/');
                            string uloc = commands[2];
                            storePerson.Add(uname, uloc);
                            sw.WriteLine("HTTP/0.9 200 OK");
                            sw.WriteLine("Content-Type: text/plain");
                            sw.Close();
                        }
                        else
                        {
                            sw.WriteLine("Name has to be in '/name' format. e.g PUT /name location"); //error message
                            sw.Close();
                        }
                        break;

                    case "POST":
                        if (commands[1].StartsWith("/"))
                        {
                            string uname = commands[1].TrimStart('/');
                            string uloc = commands[3];
                            if (commands[2] == "HTTP/1.0")
                            {
                                if (!storePerson.ContainsKey(uname))
                                {
                                    //add 1.0 loc
                                    
                                    storePerson.Add(uname, uloc);
                                    string response = "New name: " + uname + " Location: " + uloc;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                    sw.WriteLine("HTTP/1.0 200 OK");
                                    sw.WriteLine("Content-Type: text/plain");
                                    sw.Close();
                                }
                                else
                                {
                                    //update 1.0 loc
                                    storePerson[uname] = uloc;
                                    string response = "Location updated for " + uname + ": " + uloc;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);

                                    sw.WriteLine("HTTP/1.0 200 OK");
                                    sw.WriteLine("Content-Type: text/plain");
                                    sw.Close();
                                }
                            }
                            else if (commands[2] == "HTTP/1.1")
                            {
                                if (!storePerson.ContainsKey(uname))
                                {
                                    //add 1.1 loc
                                    storePerson.Add(uname, uloc);
                                    string response = "New name: " + uname + " Location: " + uloc;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);

                                    sw.WriteLine("HTTP/1.1 200 OK");
                                    sw.WriteLine("Content-Type: text/plain");
                                    sw.Close();
                                }
                                else
                                {
                                    //update 1.1 loc
                                    storePerson[uname] = uloc;
                                    string response = "Location updated for " + uname + ": " + uloc;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);

                                    sw.WriteLine("HTTP/1.1 200 OK");
                                    sw.WriteLine("Content-Type: text/plain");
                                    sw.Close();
                                }
                            }
                        }
                        else
                        {
                            sw.WriteLine("Name has to be in '/name' format. e.g PUT /name location"); //error message
                            sw.Close();
                        }
                        break;

                    case "-h1":
                        if ((clength >= 3) && (commands[2] != "PUT") && (commands[2] != "GET"))
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //Store new location and name
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson.Add(commands[1], line);
                                string response = "New name: " + commands[1] + " Location: " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/1.1 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                            else
                            {
                                //Update location
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson[commands[1]] = line;
                                string response = commands[1] + " location changed to be " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/1.1 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                        }
                        else
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //No location found
                                sw.WriteLine("HTTP/1.1 404 Not Found");
                                sw.WriteLine("Content-Type: text/plain");
                                string response = "<HTTP/1.1 404 Not Found> for request: " + line;
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.Close();
                            }
                            else
                            {
                                //Retrieve location
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + commands[1] + " is " + storePerson[commands[1]]);
                                Console.WriteLine(storePerson[commands[1] + " located at " + storePerson[commands[1]]]);
                                sw.WriteLine("HTTP/1.1 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.WriteLine(storePerson[commands[1]]);
                                sw.Close();
                            }
                        } 
                        break;

                    case "-h9":
                        if ((clength >= 3) && (commands[2] != "PUT") && (commands[2] != "GET"))
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //Store new location and name
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson.Add(commands[1], line);
                                string response = "New name: " + commands[1] + " Location: " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/0.9 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                            else
                            {
                                //Update location
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson[commands[1]] = line;
                                string response = commands[1] + " location changed to be " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/0.9 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                        }
                        else
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //No location found
                                sw.WriteLine("HTTP/0.9 404 Not Found");
                                sw.WriteLine("Content-Type: text/plain");
                                string response = "<HTTP/0.9 404 Not Found> for request: " + line;
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.Close();
                            }
                            else
                            {
                                //Retrieve location
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + commands[1] + " is " + storePerson[commands[1]]);
                                Console.WriteLine(storePerson[commands[1] + " located at " + storePerson[commands[1]]]);
                                sw.WriteLine("HTTP/0.9 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.WriteLine(storePerson[commands[1]]);
                                sw.Close();
                            }
                        }
                        break;

                    case "-h0":
                        if ((clength >= 3) && (commands[2] != "PUT") && (commands[2] != "GET"))
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //Store new location and name
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson.Add(commands[1], line);
                                string response = "New name: " + commands[1] + " Location: " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/1.0 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                            else
                            {
                                //Update location
                                int fwl = commands[1].Length + 5;
                                line = line.Remove(0, fwl);
                                storePerson[commands[1]] = line;
                                string response = commands[1] + " location changed to be " + line;
                                Console.WriteLine(response);
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.WriteLine("HTTP/1.0 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.Close();
                            }
                        }
                        else
                        {
                            if (!storePerson.ContainsKey(commands[1]))
                            {
                                //No location found
                                sw.WriteLine("HTTP/1.0 404 Not Found");
                                sw.WriteLine("Content-Type: text/plain");
                                string response = "<HTTP/1.0 404 Not Found> for request: " + line;
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                sw.Close();
                            }
                            else
                            {
                                //Retrieve location
                                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + commands[1] + " is " + storePerson[commands[1]]);
                                Console.WriteLine(storePerson[commands[1] + " located at " + storePerson[commands[1]]]);
                                sw.WriteLine("HTTP/1.0 200 OK");
                                sw.WriteLine("Content-Type: text/plain");
                                sw.WriteLine(storePerson[commands[1]]);
                                sw.Close();
                            }
                        }
                        break;

                    default:

                        if ((clength >= 2) && (commands[0] != "PUT") && (commands[0] != "GET") && (commands[0] != "POST"))
                            {
                                if (!storePerson.ContainsKey(commands[0]))
                                {
                                    //Store new location and name
                                    int fwl = commands[0].Length + 1;
                                    line = line.Remove(0, fwl);
                                    storePerson.Add(commands[0], line);
                                    sw.WriteLine(commands[0] + " location changed to be " + line);
                                    string response = "New name: " + commands[0] + " Location: " + line;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                    sw.Close();
                                }
                                else
                                {
                                    //Update location
                                    int fwl = commands[0].Length + 1;
                                    line = line.Remove(0, fwl);
                                    storePerson[commands[0]] = line;
                                    string response = commands[0] + " location changed to be " + line;
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);

                                    sw.WriteLine(commands[0] + " location changed to be " + line);
                                    sw.Close();
                                }
                            }
                            else
                            {
                                if (!storePerson.ContainsKey(commands[0]))
                                {
                                    //No location found
                                    sw.WriteLine("ERROR: no entries found");
                                    string response = "ERROR: no entries found for: " + commands[0];
                                    Console.WriteLine(response);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + response);
                                    sw.Close();
                                }
                                else
                                {
                                    //Retrieve location
                                    sw.WriteLine(commands[0] + " is " + storePerson[commands[0]]);
                                    WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + commands[0] + " is " + storePerson[commands[0]]);
                                    Console.WriteLine(storePerson[commands[0]]);
                                    sw.Close();
                                }
                            } 
                        break;
                }
                

            }
            
            else
            {
                Console.WriteLine("No arguments specified");
                WriteToLog("[" + DateTime.Now.ToString("h:mm:ss tt") + "] [Server]: " + "No arguements specified");
                sw.Close();
            }
        }
        
        catch
        {
            Console.WriteLine("Something went wrong with the command");
        }
    }
}