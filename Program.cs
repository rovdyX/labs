using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

public class DnaSequence
{
    public string Sequence { get; set; }
}

class Server
{
    private const int PORT = 8888;

    public static void Main(string[] args)
    {
        TcpListener server = null;
        try
        {
            server = new TcpListener(IPAddress.Any, PORT);
            server.Start();

            Console.WriteLine("Server started. Waiting for clients...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Client connected!");

                HandleClient(client);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            server?.Stop();
        }
    }

    private static void HandleClient(TcpClient client)
    {
        try
        {
            using (NetworkStream stream = client.GetStream())
            {

                string randomSequence = GenerateRandomDnaSequence();

  
                var sequenceObj = new DnaSequence{ Sequence = randomSequence };
                string sequenceJson = JsonSerializer.Serialize(sequenceObj);
                byte[] dataToSend = System.Text.Encoding.ASCII.GetBytes(sequenceJson);
                stream.Write(dataToSend, 0, dataToSend.Length);

 
                byte[] data = new byte[1024];
                int bytesRead = stream.Read(data, 0, data.Length);
                string response = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
                Console.WriteLine($"Client response: {response}");
                Console.WriteLine("Привет ")
            }
        }
        finally
        {
            client.Close();
        }
    }

    private static string GenerateRandomDnaSequence()
    {
        Random random = new Random();
        const string nucleotides = "ACGT";
        int length = random.Next(50, 100);
        char[] sequence = new char[length];
        for (int i = 0; i < length; i++)
        {
            sequence[i] = nucleotides[random.Next(nucleotides.Length)];
        }
        return new string(sequence);
    }
}
