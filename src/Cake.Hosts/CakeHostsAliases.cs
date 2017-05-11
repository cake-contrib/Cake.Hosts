using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Hosts
{
    [CakeAliasCategory("Hosts")]
    public static class CakeHostsAliases
    {
        public static bool HostsRecordExists(this ICakeContext context, String ipAddress, String domainName)
        {
            var cakeHosts = GetCakeHosts(context);
            return cakeHosts.HostsRecordExists(ipAddress, domainName);
        }

        public static void AddHostsRecord(this ICakeContext context, String ipAddress, String domainName)
        {
            var cakeHosts = GetCakeHosts(context);
            cakeHosts.AddHostsRecord(ipAddress, domainName);
        }


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
