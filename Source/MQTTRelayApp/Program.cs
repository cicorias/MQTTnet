namespace MQTTRelayApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Startup();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        async static void Startup()
        {

            Server server = Server.Instance;

            await Task.Run(server.Run);

        }
    }
}