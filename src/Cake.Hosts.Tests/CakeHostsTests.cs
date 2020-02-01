using Cake.Core;
using Cake.Core.Diagnostics;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            sut = new CakeHosts(context, new TestsHostPathProvider(), log);
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

        [Fact]
        public void GetAllRecords_Find_Records()
        {
            // Act
            var result = sut.GetAllRecords();

            // Assert
            result.Should().NotBeEmpty();
        }

        [Fact]
        public void GetAllRecords_Not_Find_Records()
        {
            var context = Substitute.For<ICakeContext>();
            var log = Substitute.For<ICakeLog>();
            var sut = new CakeHosts(context, new TestsInvalidHostPathProvider(), log);
            var hostsPath = new TestsInvalidHostPathProvider().GetHostsFilePath();
            File.Copy(hostsPath, hostsPath + ".backup", overwrite: true);

            // Act
            var result = sut.GetAllRecords();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void AddRecord_Always_Adds()
        {
            // Act
            sut.AddHostsRecord("127.0.0.1", "MyTest.dev");

            // Assert
            var hostsLines = ReadHostsLines();
            var hasRecord = hostsLines.Any(l => l == "127.0.0.1 MyTest.dev");
            hasRecord.Should().BeTrue();
        }


        [Fact]
        public void AddRecord_CommentedAlready_AddsAnyway()
        {
            // Act
            sut.AddHostsRecord("127.0.0.1", "DisabledHost.dev");

            // Assert
            var hostsLines = ReadHostsLines();

            var hasRecord = hostsLines.Any(l => l == "127.0.0.1 DisabledHost.dev");
            var numberOfMentions = hostsLines.Count(l => l.Contains("DisabledHost.dev"));

            hasRecord.Should().BeTrue();
            numberOfMentions.Should().Be(2);
        }


        [Fact]
        public void AddRecord_AlreadyExist_DoesNotAdd()
        {
            // Act
            sut.AddHostsRecord("127.0.0.1", "ImSpecial");
            sut.AddHostsRecord("127.0.0.1", "ImSpecial");

            // Assert
            var hostsLines = ReadHostsLines();
            var numberOfMentions = hostsLines.Count(l => l.Contains("ImSpecial"));
            numberOfMentions.Should().Be(1);
        }

        [Fact]
        public void RemoveHostsRecord_Removes_Always()
        {
            // Act
            sut.RemoveHostsRecord("127.0.0.1", "ToBeRemoved.dev");

            // Assert
            // validate hosts file does not contain the record anymore
            var hostsLines = ReadHostsLines();
            var hasRecord = hostsLines.Any(l => l.ToLower().Contains("ToBeRemoved.dev".ToLower()));
            hasRecord.Should().BeFalse();
        }


        [Fact]
        public void RemoveHostsRecord_Commented_DoesNotChange()
        {
            // Act
            sut.RemoveHostsRecord("127.0.0.1", "ToBeRemoved.disabled");

            // Assert
            // validate hosts file does not contain the record anymore
            var hostsLines = ReadHostsLines();
            var hasRecord = hostsLines.Any(l => l.ToLower().Contains("ToBeRemoved.disabled".ToLower()));
            hasRecord.Should().BeTrue();
        }


        [Fact]
        public void RemoveHosts_WithoutMathingHost_DoesNotChangeFile()
        {
            // Arrange 
            var fileContentsBefore = File.ReadAllText(hostsPath);

            // Act
            sut.RemoveHostsRecord("8.8.8.8", "blah");

            // Assert
            var contentsAfter = File.ReadAllText(hostsPath);
            contentsAfter.Should().Be(fileContentsBefore);
        }



        public void Dispose()
        {
            File.Copy(hostsPath + ".backup", hostsPath, overwrite: true);
        }

        private List<String> ReadHostsLines()
        {
            var allLines = File.ReadAllLines(hostsPath).ToList();
            return allLines;
        }
    }
}
