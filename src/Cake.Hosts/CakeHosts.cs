using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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


        public bool HostsRecordExists(string ipAddress, String domainName)
        {
            Guard.CheckIpAddress(ipAddress, nameof(ipAddress));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var regexPattern = $@"^\s*{Regex.Escape(ipAddress)}\s*{Regex.Escape(domainName)}\s*$";
            var regexp = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var path = hostsPathProvider.GetHostsFilePath();
            log.Debug("Using Hosts file at location {0}", path);
            log.Debug("Using regex to check Paths file: {0}", regexp);
            var fileContents = File.ReadAllText(path);

            var result = regexp.IsMatch(fileContents);
            
            log.Information("Checking if hosts file contains record with ip {0} and domain {1}. Result: {2}", ipAddress, domainName, result);

            return result;
        }


        // Does not throw if this domain name already in the file
        public void AddHostsRecord(String ipAddress, String domainName)
        {
            Guard.CheckIpAddress(ipAddress, nameof(ipAddress));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var path = hostsPathProvider.GetHostsFilePath();
            log.Debug("Using Hosts file at location {0}", path);

            if (HostsRecordExists(ipAddress, domainName))
            {
                log.Debug("Hosts file already contains record with IP {0} and domain {1}. Ignoring this operation", ipAddress, domainName);
                return;
            }

            File.AppendAllText(path, Environment.NewLine + ipAddress + " " + domainName);
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
