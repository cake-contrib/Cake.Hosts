using System;


namespace Cake.Hosts
{
    internal interface IHostsPathProvider
    {
        /// <summary>
        /// Returns full path to the hosts file
        /// </summary>
        /// <returns></returns>
        string GetHostsFilePath();
    }


    internal class WindowsHostsPathProvider : IHostsPathProvider
    {
        public string GetHostsFilePath()
        {
            var windir = Environment.GetEnvironmentVariable("windir");
            Guard.ArgumentIsNotNull(windir, nameof(windir));

            var hosts = windir + @"\System32\drivers\etc\hosts";

            return hosts;
        }
    }


    internal class LinuxHostsPathProvider : IHostsPathProvider
    {
        public string GetHostsFilePath()
        {
            return @"/etc/hosts";
        }
    }
}