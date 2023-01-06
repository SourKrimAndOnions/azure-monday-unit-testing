namespace TestProject1;

public class Mocking
{
    [Theory, AutoNSubstituteData]
    public void Should_Return_Correct_Item(
        [Frozen] IDataBase db,
        DbReader sut,
        int id,
        Dude expected)
    {
        db
        .GetById(default)
        .ReturnsForAnyArgs(expected);

        sut
       .GetEmployeeById(id)
       .Should()
       .BeOfType<Dude>()
       .And
       .Be(expected);
    }

    [Theory, AutoNSubstituteData]
    public void Should_Call_DB(
        [Frozen] IDataBase db,
        DbReader sut,
        int id,
        Dude expected)
    {
        sut
       .GetEmployeeById(id);

        db
        .Received(1)
        .GetById(id);
    }
}


public class DbReader
{
    private readonly IDataBase db;

    public DbReader(IDataBase db)
    {
        this.db = db;
    }

    public Dude? GetEmployeeById(int id)
        => db.GetById(id);

    public void changeName(int id, string name)
        => db.ChangeNameById(id, name);
}

public interface IDataBase
{
    Dude? GetById(int id);
    void ChangeNameById(int id, string name);
}

public record Dude(
    int Id,
    string Name);
