using TUnit.Core.Interfaces;

namespace tests;

public class DataClass : IAsyncInitializer, IAsyncDisposable
{
    public Task InitializeAsync()
    {
        return Console.Out.WriteLineAsync("Classes can be injected into tests, and they can perform some initialisation logic such as starting an in-memory server or a test container.");
    }

    public async ValueTask DisposeAsync()
    {
        await Console.Out.WriteLineAsync("And when the class is finished with, we can clean up any resources.");
    }

    public class FromClass
    {
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; } 
    }
    
    public class ToClass
    {
        public string Property1 { get; set; } = string.Empty;
        public int Property2 { get; set; }
        public bool Property3 { get; set; } 
    }
}