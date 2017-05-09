using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Hosts
{
    [CakeAliasCategory("Hosts")]
    public static class CakeHostsAliases
    {
        public static void AddHostsRecord(this ICakeContext context, String ipAddress, String domainName)
        {
            throw new NotImplementedException();
        }


        public static void AddOrReplaceHostsRecord(this ICakeContext context, String ipAddress, String domainName)
        {
            throw new NotImplementedException();
        }

        
        public static void RemoveHostsRecord(this ICakeContext context, String domainName)
        {
            throw new NotImplementedException();
        }


        public static void HostsRecordExists(this ICakeContext context, String domainName)
        {
            throw new NotImplementedException();
        }
    }
}
