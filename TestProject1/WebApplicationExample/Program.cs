var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();


namespace WebApplicationExample
{
    
/// <summary>
/// We need to make this class public and partial if we want to run integration tests with the same setup.
/// </summary>
public partial class Program
{
}}
