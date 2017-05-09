using System;
using FluentAssertions;
using Xunit;

namespace Cake.Hosts.Tests
{
    public class WindowsHostsPathProviderTests
    {
        [Fact]
        public void Returns_Standard_Path()
        {
            //NOTE: no wonder this will explode on appVeyor because they probably have windows installed on a differnt drive
            // Arrange
            var sut = new WindowsHostsPathProvider();

            // Act
            var result = sut.GetHostsFilePath();

            result.Should().BeEquivalentTo(@"C:\Windows\System32\drivers\etc\hosts");
        }
    }
}
