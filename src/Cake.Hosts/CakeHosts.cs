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

        public CakeHosts(ICakeContext cakeContext, IHostsPathProvider hostsPathProvider, ICakeLog log)
        {
            this.hostsPathProvider = hostsPathProvider;
            this.cakeContext = cakeContext;
            this.log = log;
        }


        internal bool HostsRecordExists(string ipAddress, String domainName)
        {
            Guard.CheckIpAddress(ipAddress, nameof(ipAddress));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var regexp = GetRegex(ipAddress, domainName);

            var path = hostsPathProvider.GetHostsFilePath();
            log.Debug("Using Hosts file at location {0}", path);
            var fileContents = File.ReadAllText(path);

            var result = regexp.IsMatch(fileContents);
            
            log.Information("Checking if hosts file contains record with ip {0} and domain {1}. Result: {2}", ipAddress, domainName, result);

            return result;
        }


        // Does not throw if this domain name already in the file
        internal void AddHostsRecord(String ipAddress, String domainName)
        {
            Guard.CheckIpAddress(ipAddress, nameof(ipAddress));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var path = hostsPathProvider.GetHostsFilePath();
            Guard.FileExists(path);
            log.Debug("Using Hosts file at location {0}", path);

            if (HostsRecordExists(ipAddress, domainName))
            {
                log.Debug("Hosts file already contains record with IP {0} and domain {1}. Ignoring this operation", ipAddress, domainName);
                return;
            }

            File.AppendAllText(path, Environment.NewLine + ipAddress + " " + domainName);
        }



        internal void RemoveHostsRecord(String ipAddress, String domainName)
        {
            Guard.CheckIpAddress(ipAddress, nameof(ipAddress));
            Guard.ArgumentIsNotNull(domainName, nameof(domainName));

            var path = hostsPathProvider.GetHostsFilePath();
            Guard.FileExists(path);
            log.Debug("Using Hosts file at location {0}", path);

            if (!HostsRecordExists(ipAddress, domainName))
            {
                return;
            }

            var regex = GetRegex(ipAddress, domainName);

            var allLines = File.ReadAllLines(path).ToList();

            for (int i = allLines.Count - 1; i >= 0; i--)
            {
                var line = allLines[i];
                if (regex.IsMatch(line))
                {
                    allLines.RemoveAt(i);
                }
            }

            File.WriteAllLines(path, allLines);
        }


        private Regex GetRegex(string ipAddress, string domainName)
        {
            var regexPattern = $@"^\s*{Regex.Escape(ipAddress)}\s*{Regex.Escape(domainName)}\s*$";
            var regexp = new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            log.Debug("Using regex to check Paths file: {0}", regexp);
            return regexp;
        }

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
