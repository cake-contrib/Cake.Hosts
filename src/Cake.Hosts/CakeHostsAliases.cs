using System;
using Cake.Core;
using Cake.Core.Annotations;

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
            return cakeHosts.HostsRecordExists(ipAddress, domainName);
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
