using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Hosts
{
    /// <summary>
    /// <para>
    /// Functionalty to operate hosts-file. Requres admin permissions and will probably fail on your general CI server.
    /// </para>
    /// <para>
    /// Mostly aimed on automation scripts rather than actual project builds.
    /// </para>
    /// Install via:
    /// <code>
    ///     #addin "nuget:?package=Cake.Hosts"
    /// </code>
    /// </summary>
    [CakeNamespaceImport("Cake.Hosts")]
    [CakeAliasCategory("Hosts")]
    public static class CakeHostsAliases
    {
        /// <summary>
        /// Checks if a record already exists in hosts file
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="ipAddress">IP address to check</param>
        /// <param name="domainName">Domain name to check</param>
        /// <returns>If given parameters already constitue a record in hosts file</returns>
        [CakeMethodAlias]
        public static bool HostsRecordExists(this ICakeContext context, String ipAddress, String domainName)
        {
            var cakeHosts = GetCakeHosts(context);
            var result = cakeHosts.HostsRecordExists(ipAddress, domainName);
            context.Log.Write(Verbosity.Normal, LogLevel.Information, "HOSTS record {0} with IP {1} exists: {2}", ipAddress, domainName, result);
            return result;
        }

        /// <summary>
        /// Lista all records in hosts file
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <returns>If contains records, return all records</returns>
        [CakeMethodAlias]
        public static IEnumerable<HostsFile> GetAllRecords(this ICakeContext context)
        {
            var cakeHosts = GetCakeHosts(context);
            var result = cakeHosts.GetAllRecords();

            if (result.Any())
            {
                context.Log.Write(Verbosity.Normal, LogLevel.Information, "List all records in hosts file");
                foreach (var item in result)
                {
                    context.Log.Write(Verbosity.Normal, LogLevel.Information, $"IP {item.Ip} with Hostname {item.Hostname}");
                }
            }
            else
            {
                context.Log.Write(Verbosity.Normal, LogLevel.Information, "Hosts file does not contains records");
            }
            
            return result;
        }

        /// <summary>
        /// Appends a record to the end of hosts file. If this value already exist, no modifications applied
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="ipAddress">IP address to add</param>
        /// <param name="domainName">Domain name to add</param>
        [CakeMethodAlias]
        public static void AddHostsRecord(this ICakeContext context, String ipAddress, String domainName)
        {
            context.Log.Write(Verbosity.Normal, LogLevel.Information, "Adding HOSTS record {0} with IP {1}", ipAddress, domainName);
            var cakeHosts = GetCakeHosts(context);
            cakeHosts.AddHostsRecord(ipAddress, domainName);
        }


        /// <summary>
        /// Removes a line with given IP address and domain name. If given pair does not exist in hosts file, no modifications applied
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="ipAddress">IP address to check</param>
        /// <param name="domainName">Domain name to check</param>
        [CakeMethodAlias]
        public static void RemoveHostsRecord(this ICakeContext context, String ipAddress, String domainName)
        {
            context.Log.Write(Verbosity.Normal, LogLevel.Information, "Removing HOSTS record {0} with IP {1}", ipAddress, domainName);
            var cakeHosts = GetCakeHosts(context);
            cakeHosts.RemoveHostsRecord(ipAddress, domainName);
        }



        private static CakeHosts GetCakeHosts(ICakeContext context)
        {
            IHostsPathProvider pathProvider = new WindowsHostsPathProvider();
            if (context.Environment.Platform.IsUnix())
            {
                pathProvider = new LinuxHostsPathProvider();
            }

            var cakeHosts = new CakeHosts(context, pathProvider, context.Log);
            return cakeHosts;
        }
    }
}
