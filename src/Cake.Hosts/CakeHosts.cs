using System;
using System.Text;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Hosts
{
    internal class CakeHosts
    {
        private readonly IHostsPathProvider hostsPathProvider;
        private readonly ICakeContext cakeContext;
        private readonly ICakeLog log;

        public CakeHosts(IHostsPathProvider hostsPathProvider, ICakeContext cakeContext, ICakeLog log)
        {
            this.hostsPathProvider = hostsPathProvider;
            this.cakeContext = cakeContext;
            this.log = log;
        }

        public void AddHostsRecord(String ipAddress, String domainName)
        {
            var fullPath = hostsPathProvider.GetHostsFilePath();
            throw new NotImplementedException();
        }

        public void AddOrReplaceHostsRecord(String ipAddress, String domainName)
        {
            var fullPath = hostsPathProvider.GetHostsFilePath();
            throw new NotImplementedException();
        }


        public void RemoveHostsRecord(String domainName)
        {
            throw new NotImplementedException();
        }


        public void HostsRecordExists(String domainName)
        {
            throw new NotImplementedException();
        }
    }
}
