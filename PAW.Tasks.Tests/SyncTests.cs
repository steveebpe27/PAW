namespace PAW.Tasks.Tests;

[TestClass]
[DoNotParallelize]
public class SyncTests
{
    [TestMethod]
    public async Task Execute_PrintsEachIterationInOrder()
    {
        var lines = await CaptureOutput(() => Sync.Execute(3, "AAA"));

        CollectionAssert.AreEqual(
            new[] { "AAA Second 1", "AAA Second 2", "AAA Second 3" },
            lines);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task Execute_WithNonPositiveCount_PrintsNothing(int count)
    {
        var lines = await CaptureOutput(() => Sync.Execute(count, "AAA"));

        Assert.IsEmpty(lines);
    }

    [TestMethod]
    public async Task ExecuteEval_BelowNextReportingInterval_PrintsOnlyFirstIteration()
    {
        var lines = await CaptureOutput(() => Sync.ExecuteEval(3m, "BBB"));

        CollectionAssert.AreEqual(new[] { "BBB Second 1" }, lines);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task ExecuteEval_WithNonPositiveCount_PrintsNothing(int count)
    {
        var lines = await CaptureOutput(() => Sync.ExecuteEval(count, "BBB"));

        Assert.IsEmpty(lines);
    }

    [TestMethod]
    public async Task ExecuteItems_ReturnsAllItemsInOrder()
    {
        var items = await Sync.ExecuteItems(3, "Sismos");

        CollectionAssert.AreEqual(
            new[] { "Sismos Second 1", "Sismos Second 2", "Sismos Second 3" },
            items.ToArray());
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public async Task ExecuteItems_WithNonPositiveCount_ReturnsEmptyCollection(int count)
    {
        var items = await Sync.ExecuteItems(count, "Sismos");

        Assert.IsEmpty(items);
    }

    private static async Task<string[]> CaptureOutput(Func<Task> action)
    {
        var originalOutput = Console.Out;
        using var output = new StringWriter();
        Console.SetOut(output);

        try
        {
            await action();
            return output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }
        finally
        {
            Console.SetOut(originalOutput);
        }
    }
}
