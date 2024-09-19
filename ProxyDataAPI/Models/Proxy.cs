namespace ProxyDataAPI.Models
{
    public class Proxy
    {
        public int Id { get; set; }
        public string Protocol { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string Anonymity { get; set; }
        public bool Https { get; set; }
        public string Latency { get; set; }
        public string LastChecked { get; set; }
    }
}
