using System.Net.Sockets;
using System.Text;

namespace Sonakaj2018
{
    class TCPClient
    {
        private int port = 0000;
        private string server = "0.0.0.0";
        private TcpClient tcpClient;

        public TCPClient()
        {
            tcpClient = new TcpClient();
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                tcpClient.Connect(server, port);
            }
            catch
            {
                ConnectToServer();
            }
        }

        public bool Write(string requestData)
        {
            try
            {
                NetworkStream stream = tcpClient.GetStream();
                byte[] utfBytes = Encoding.ASCII.GetBytes(requestData + "\n");
                byte[] data = Encoding.Convert(Encoding.ASCII, Encoding.GetEncoding("KOI8-R"), utfBytes);
                stream.Write(data, 0, data.Length);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public string Read()
        {
            try
            {
                byte[] data = new byte[256];
                StringBuilder response = new StringBuilder();
                NetworkStream stream = tcpClient.GetStream();

                do
                {
                    int bytes = stream.Read(data, 0, data.Length);
                    response.Append(Encoding.GetEncoding("KOI8-R").GetString(data, 0, bytes));
                }
                while (!response.ToString().Contains("\n"));

                stream.Close();
                tcpClient.Close();
                
                return response.ToString();

            }
            catch (SocketException e)
            {
                return "SocketException:" + e;
            }
        }
    }
}
