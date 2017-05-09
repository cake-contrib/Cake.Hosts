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
        [Fact]
        public void RecordExists_WithRecord_True()
        {
            //Arrange
            var context = Substitute.For<ICakeContext>();
            var log = Substitute.For<ICakeLog>();
            var sut = new CakeHosts(new TestsHostPathProvider(), context, log);

            // Act
            var result = sut.HostsRecordExists("NotFound");

            // Assert
            result.Should().BeTrue();
        }
    }
}
