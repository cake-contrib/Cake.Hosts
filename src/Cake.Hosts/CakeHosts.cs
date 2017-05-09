using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        // throws if this domain name already in the file
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


        public bool HostsRecordExists(String domainName)
        {
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var allLines = ReadHostsFile();

            domainName = domainName.ToLower();
            var recordExists = allLines.Any(l => l.ToLower().Contains(domainName));

            return recordExists;
        }

        private IEnumerable<String> ReadHostsFile()
        {
            var hostsPath = hostsPathProvider.GetHostsFilePath();
            Guard.FileExists(hostsPath);

            var allLines = File.ReadAllLines(hostsPath);

            // need to exclude comments, so ignoring everything after #
            foreach (var line in allLines)
            {
                var lineParts = line.Split('#');
                var firstPart = lineParts[0].Trim();
                if (!String.IsNullOrWhiteSpace(firstPart))
                {
                    yield return firstPart;
                }
            }
        }
    }
}
