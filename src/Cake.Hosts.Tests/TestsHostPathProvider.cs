using System.IO;

namespace Cake.Hosts.Tests
{
    public class TestsHostPathProvider : IHostsPathProvider
    {
        public string GetHostsFilePath()
        {
            return Directory.GetCurrentDirectory() + @"\testHosts";
        }
    }
}
