using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Xunit;

namespace Cake.Hosts.Tests
{
    public class GuardTests
    {
        [Fact]
        public void CheckIpAddress_ValidIP_DoesNotThrow()
        {
            // Act
            Action act = () => Guard.CheckIpAddress("127.0.0.1", "ip");

            // Assert
            act.Should().NotThrow();
        }


        [Fact]
        public void CheckIpAddress_InvalidIp_ThrowsArguemtnException()
        {
            // Act
            Action act = () => Guard.CheckIpAddress("blah.blah.blah.blah", "blah");

            // Assert
            act.Should().Throw<ArgumentException>();
        }
    }
}
