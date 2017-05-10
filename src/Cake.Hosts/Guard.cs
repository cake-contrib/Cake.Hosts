using System;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace Cake.Hosts
{
    internal static class Guard
    {
        [DebuggerHidden]
        internal static void ArgumentIsNotNull(object value, string argumentName)
        {
            if (value is String)
            {
                var stringValue = value as String;
                if (String.IsNullOrWhiteSpace(stringValue))
                {
                    throw new ArgumentNullException(argumentName);
                }
            }

            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerHidden]
        internal static void FileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File {filePath} does not exist");
            }
        }

        [DebuggerHidden]
        internal static void CheckIpAddress(String ipString, string arguementName)
        {
            if (String.IsNullOrWhiteSpace(ipString))
            {
                throw new ArgumentNullException(arguementName);
            }

            var splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                throw new ArgumentNullException(arguementName);
            }


            var isValid = splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
            if (!isValid)
            {
                throw new ArgumentException("IP Address is not valid");
            }
        }
    }
}
