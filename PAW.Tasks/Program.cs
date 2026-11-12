using PAW.Tasks;

Console.WriteLine("Hello, World!");

Sync.Execute(100, "AAA");

Sync.Execute(100, "BBB").Wait();

Console.ReadLine();
Console.ReadKey();
