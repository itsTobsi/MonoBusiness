try
{
    using var game = new MonoBusiness.Game1();
    game.Run();
}
catch (System.Exception ex)
{
    System.Console.WriteLine(ex);
    throw;
}
