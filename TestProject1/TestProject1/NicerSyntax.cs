namespace TestProject1;

public class NicerSyntax
{
    [Fact]
    public void RegularAssertion()
    {
        var a = 10;
        a++;
        Assert.Equal(11, a);
    }

    [Fact]
    public void RegularAssertions()
    {
        var a = 10;
        a++;
        Assert.Equal(11, a);
        Assert.Equal(typeof(int), a.GetType());
    }

    [Fact]
    public void FluentAssertion()
    {
        var a = 10;
        a++;
        a
        .Should()
        .Be(11);
    }

    [Fact]
    public void FluentAssertions()
    {
        var a = 10;
        a++;
        a
        .Should()
        .Be(11)
        .And
        .BeOfType(typeof(int));
    }

    [Fact]
    public void ErrorRaisedByRegularAssertions()
    {
        var a = "hello";
        Assert.Equal("hrello", a);
    }

    [Fact]
    public void ErrorRaisedByFluentAssertions()
    {
        var a = "hello";
        a
        .Should()
        .Be("hrello");
    }

    [Fact]
    public void ErrorRaisedByRegularAssertions2()
    {
        var a = "hello";
        Assert.Equal(typeof(int), a.GetType());
    }

    [Fact]
    public void ErrorRaisedByFluentAssertions2()
    {
        var a = "hello";
        a
        .Should()
        .BeOfType<int>();
    }
}
