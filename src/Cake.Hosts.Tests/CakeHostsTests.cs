using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core;
using Cake.Core.Diagnostics;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Cake.Hosts.Tests
{
    public class CakeHostsTests
    {
        private readonly CakeHosts sut;

        public CakeHostsTests()
        {
            var context = Substitute.For<ICakeContext>();
            var log = Substitute.For<ICakeLog>();
            sut = new CakeHosts(new TestsHostPathProvider(), context, log);
        }

        [Fact]
        public void RecordExists_WithRecord_True()
        {
            // Act
            var result = sut.HostsRecordExists("NotFound");

            // Assert
            result.Should().BeTrue();
        }


        [Fact]
        public void RecordExists_CommentedHost_False()
        {
            // Act
            var result = sut.HostsRecordExists("DisabledHost.dev");

            // Assert
            result.Should().BeFalse();
        }
    }
}
