using PAW.Tasks;

Console.WriteLine("Hello, World!");

await Task.WhenAll(Sync.Execute(100, "AAA"), Sync.Execute(100, "BBB"));

Console.ReadLine();
Console.ReadKey();
