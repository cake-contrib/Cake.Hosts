using System;
using System.Diagnostics;
using System.IO;


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

        internal static void FileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File {filePath} does not exist");
            }
        }
    }
}
