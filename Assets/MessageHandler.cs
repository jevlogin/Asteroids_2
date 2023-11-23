namespace WORLDGAMEDEVELOPMENT
{
    public class MessageHandler
    {
        public string Source { get; private set; }
        public string Data { get; private set; }

        public MessageHandler(string source, string data)
        {
            Source = source;
            Data = data;
        }
    }
}