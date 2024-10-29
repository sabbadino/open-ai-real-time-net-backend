using System.Net.WebSockets;
using System.Text;
Encoding encoding = Encoding.UTF8;

try
{
    Console.WriteLine("hit enter to connect");
    Console.ReadLine(); 
    var ws = new ClientWebSocket();
    //await ws.ConnectAsync(new Uri("ws://localhost:5241/realtime"),
        //CancellationToken.None);
    await ws.ConnectAsync(new Uri("wss://localhost:7106/realtime"),
        CancellationToken.None);
    Console.WriteLine("connected");
    Task.Run(async () => {
        var rcvBuff = new byte[1 << 10];
        while (true)
        {
            var rcvMessage = await ws.ReceiveAsync(rcvBuff, CancellationToken.None);
            if (rcvMessage.MessageType == WebSocketMessageType.Text)
            {
                var rcvText = encoding.GetString(rcvBuff, 0, rcvMessage.Count);
                Console.WriteLine($"Received message: \"{rcvText}\"");
            }
        }
    });

    while (true)
    {
        var line = Console.ReadLine();
        var sndBuff = encoding.GetBytes(line);
        await ws.SendAsync(sndBuff, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}
catch (Exception ex) {
    Console.WriteLine(ex.ToString());
    Console.Read();

}

