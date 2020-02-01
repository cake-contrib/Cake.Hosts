using System.IO;
using System.Text.RegularExpressions;

namespace Cake.Hosts
{
    /// <summary>
    /// Validate and extract data from hosts file
    /// </summary>
    public class HostsFileUtil
    {
        /// <summary>
        /// Verify if line is from hosts file
        /// </summary>
        /// <param name="line">The line from hosts file</param>
        /// <returns>If is not empty and not starts with #, is valid</returns>
        static public bool IsLineAHostFile(string line) 
            => line.Trim().Length > 0 && !line.TrimStart().StartsWith("#");

        /// <summary>
        /// Convert line in object HostsFile
        /// </summary>
        /// <param name="line">The line from hosts file</param>
        /// <returns>If line is valid, return a object HostsFile</returns>
        static public HostsFile GetHostsFile(string line)
        {
            var result = TryGetHostsFileEntry(line);

            if (result == null)
                throw new InvalidDataException();

            return result;
        }

        /// <summary>
        /// Try convert line in object HostsFile
        /// </summary>
        /// <param name="line">The line from hosts file</param>
        /// <returns>If line is valid, return a object HostsFile</returns>
        static public HostsFile TryGetHostsFileEntry(string line)
        {
            var match = RegexHostsEntry.Match(line);

            if (!match.Success)
                return null;

            return new HostsFile(match.Groups["address"].Value, match.Groups["name"].Value);
        }

        static readonly Regex RegexHostsEntry = new Regex(@"^\s*(?<address>\S+)\s+(?<name>\S+)\s*($|#)", RegexOptions.Compiled);
    }
}
