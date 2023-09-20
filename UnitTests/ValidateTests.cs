using FluentAssertions;

namespace WpfMvvmToolkit.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void ValidateFile_ValidFilePath()
        {
            string path = "c:\\test.txt";

            var results = Validate.File(path);

            results.Should().BeEmpty();
        }
    }
}