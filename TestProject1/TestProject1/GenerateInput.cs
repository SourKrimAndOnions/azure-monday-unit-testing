namespace TestProject1;

public class GenerateInput
{
    [Theory, AutoNSubstituteData]
    public void RandomInput(
        string expected)
        => expected
        .Should()
        .NotBeNullOrEmpty();

    [Theory, AutoNSubstituteData]
    public void StringBuilder_Should_Append(
        StringBuilder sut,
        string str1,
        string str2)
        => sut
        .Append(str1)
        .Append(str2)
        .Build()
        .Should()
        .BeEquivalentTo(str1 + str2);

    [Theory, AutoNSubstituteData]
    public void StringBuilder_Should_Append_correctly(
        StringBuilder sut,
        string str1,
        string str2)
        => sut
        .Append(str1)
        .Append(str2)
        .Build()
        .Should()
        .NotBeEquivalentTo(str2 + str1);
}


public class StringBuilder
{
    string _value = string.Empty;
    public StringBuilder Append(string value)
    {
        _value += value;
        return this;
    }

    public string Build()
        => _value;
}
