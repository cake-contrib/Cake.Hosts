namespace Cake.Hosts
{
    /// <summary>
    /// 
    /// </summary>
    public class HostsFile
    {
        /// <summary>
        /// 
        /// </summary>
        public string Ip { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string Hostname { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="hostname"></param>
        public HostsFile(string ip, string hostname)
        {
            Ip = ip;
            Hostname = hostname;
        }
    }
}
