using System.Threading.Tasks;

namespace Pipelines.Generator.Test;

public sealed class IgnoreAttributeTests : TestBase
{
    [Test]
    public Task SimpleHandler()
    {
        return Verify();
    }
}
