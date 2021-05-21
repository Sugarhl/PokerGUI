using System;
using System.Windows;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ControlsAndStyles
{
    public class ClientUtilits : IDisposable
    {
        int port = 8080; // порт сервера
        string address = "127.0.0.1"; // адрес сервера
        bool connectionIsOpen = false;
        byte[] buffer = new byte[2048];

        IPEndPoint ipPoint;

        Socket socket;

        public ClientUtilits(string address, int port)
        {
            this.address = address;
            this.port = port;
        }

        public bool OpenConnection()
        {
            try
            {
                ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);

                connectionIsOpen = true;
            }
            catch
            {
                connectionIsOpen = false;
            }
            return connectionIsOpen;
        }


        public void SendString(string message)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public bool ReciveResponse(out Response response)
        {
            StringBuilder builder = new StringBuilder();

            int bytes = 0;
            try
            {
                do
                {
                    bytes = socket.Receive(buffer, buffer.Length, 0);
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, bytes));
                }
                while (socket.Available > 0);
            }
            catch
            {
                response = null;
                return false;
            }
            string json = builder.ToString();
            string resp = json.Substring(0, json.IndexOf('\0'));

            return Response.ParseResponse(resp, out response);
        }

        ~ClientUtilits()
        {
            Dispose();
        }

        public void Dispose()
        {
            socket?.Shutdown(SocketShutdown.Both);
            socket?.Close();
            connectionIsOpen = false;
        }
    }
}
