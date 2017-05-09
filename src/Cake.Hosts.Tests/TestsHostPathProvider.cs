using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.Hosts.Tests
{
    public class TestsHostPathProvider : IHostsPathProvider
    {
        public string GetHostsFilePath()
        {
            return Directory.GetCurrentDirectory() + "testHosts";
        }
    }
}
