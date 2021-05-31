using System.IO;

namespace Cake.Hosts.Tests
{
    public class TestsInvalidHostPathProvider : IHostsPathProvider
    {
        public string GetHostsFilePath()
        {
            return Directory.GetCurrentDirectory() + @"\testInvalidHosts";
        }
    }
}