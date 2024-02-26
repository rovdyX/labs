using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

public class DnaSequence
{
    public string Sequence { get; set; }
}

class Client
{
    private const string SERVER_IP = "127.0.0.1";
    private const int PORT = 8888;

    public static void Main(string[] args)
    {
        try
        {
            TcpClient client = new TcpClient(SERVER_IP, PORT);
            Console.WriteLine("Connected to server!");

            using (NetworkStream stream = client.GetStream())
            {
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string sequenceJson = Encoding.ASCII.GetString(data, 0, bytesRead);

                var sequenceObj = JsonSerializer.Deserialize<DnaSequence>(sequenceJson);
                string dnaSequence = sequenceObj.Sequence;
                Console.WriteLine($"Received DNA sequence: {dnaSequence}");

                string userAction = "save";
                byte[] userResponse = Encoding.ASCII.GetBytes(userAction);
                stream.Write(userResponse, 0, userResponse.Length);
                Console.WriteLine($"Sent user action: {userAction}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
