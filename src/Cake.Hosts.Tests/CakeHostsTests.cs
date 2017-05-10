using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cake.Core;
using Cake.Core.Diagnostics;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Cake.Hosts.Tests
{
    public class CakeHostsTests : IDisposable
    {
        private readonly CakeHosts sut;
        private readonly string hostsPath;

        public CakeHostsTests()
        {
            var context = Substitute.For<ICakeContext>();
            var log = Substitute.For<ICakeLog>();
            sut = new CakeHosts(new TestsHostPathProvider(), context, log);
            hostsPath = new TestsHostPathProvider().GetHostsFilePath();
            File.Copy(hostsPath, hostsPath + ".backup", overwrite: true);
        }

        [Fact]
        public void RecordExists_WithRecord_True()
        {
            // Act
            var result = sut.HostsRecordExists("127.0.0.1", "NotFound");

            // Assert
            result.Should().BeTrue();
        }


        [Fact]
        public void NoRecordExists_False()
        {
            // Act
            var result = sut.HostsRecordExists("0.0.0.0", "NotFound");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void RecordExists_CommentedHost_False()
        {
            // Act
            var result = sut.HostsRecordExists("127.0.0.1", "DisabledHost.dev");

            // Assert
            result.Should().BeFalse();
        }

        //[Fact]
        //public void RemoveHostsRecord_Removes_Always()
        //{
        //    // Act
        //    var domainName = "ToBeRemoved.dev";
        //    sut.RemoveHostsRecord(domainName);

        //    // Assert
        //    // validate hosts file does not contain the record anymore
        //    var hostsLines = ReadHostsLines();
        //    var hasRecord = hostsLines.Any(l => l.ToLower().Contains(domainName.ToLower()));
        //    hasRecord.Should().BeFalse();
        //}


        //[Fact]
        //public void RemoveHostsRecord_Commented_DoesNotChange()
        //{
        //    // Act
        //    var domainName = "ToBeRemoved.disabled";
        //    sut.RemoveHostsRecord(domainName);

        //    // Assert
        //    // validate hosts file does not contain the record anymore
        //    var hostsLines = ReadHostsLines();
        //    var hasRecord = hostsLines.Any(l => l.ToLower().Contains(domainName.ToLower()));
        //    hasRecord.Should().BeTrue();
        //}


        public void Dispose()
        {
            File.Copy(hostsPath + ".backup", hostsPath, overwrite: true);
        }

        public List<String> ReadHostsLines()
        {
            var allLines = File.ReadAllLines(hostsPath).ToList();
            return allLines;
        }
    }
}
