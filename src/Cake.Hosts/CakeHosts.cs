using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        public bool HostsRecordExists(string ip, String domainName)
        {
            Guard.ArgumentIsNotNull(ip, nameof(ip));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var regexPattern = $@"^\s*{Regex.Escape(ip)}\s*{Regex.Escape(domainName)}\s*$";
            var regexp = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var path = hostsPathProvider.GetHostsFilePath();
            var fileContents = File.ReadAllText(path);

            return regexp.IsMatch(fileContents);
        }

        // Does not throw if this domain name already in the file
        public void AddHostsRecord(String ipAddress, String domainName)
        {
            var fullPath = hostsPathProvider.GetHostsFilePath();
            throw new NotImplementedException();
        }



        //public void RemoveHostsRecord(String domainName)
        //{
        //    Guard.ArgumentIsNotNull(domainName, nameof(domainName));

        //    var hostsPath = hostsPathProvider.GetHostsFilePath();
        //    Guard.FileExists(hostsPath);

        //    var allLines = File.ReadAllLines(hostsPath).ToList();

        //    //foreach (var line in allLines)
        //    for (int i = allLines.Count-1; i >= 0; i--)
        //    {
        //        var line = allLines[i];
        //        // check if no comment
        //        var split = line.Split('#');
        //        var contents = split[0];
        //        if (contents.Trim().Equals(domainName, StringComparison.OrdinalIgnoreCase))
        //        {
        //            allLines.RemoveAt(i);
        //        }
        //    }

        //    File.WriteAllLines(hostsPath, allLines);
        //}


        //public bool HostsRecordExists(String domainName)
        //{
        //    Guard.ArgumentIsNotNull(domainName, nameof(domainName));

        //    var allLines = ReadLinexExcludeComments().ToList();

        //    domainName = domainName.ToLower();
        //    var recordExists = allLines.Any(l => l.ToLower().Contains(domainName));

        //    return recordExists;
        //}


        private IEnumerable<String> ReadLinexExcludeComments()
        {
            var hostsPath = hostsPathProvider.GetHostsFilePath();
            Guard.FileExists(hostsPath);

            var allLines1 = File.ReadAllLines(hostsPath).ToList();
            var allLines = allLines1;

            // need to exclude comments, so ignoring everything after '#'
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
