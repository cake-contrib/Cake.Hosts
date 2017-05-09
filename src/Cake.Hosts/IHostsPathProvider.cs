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
}