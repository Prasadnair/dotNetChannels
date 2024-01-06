// See https://aka.ms/new-console-template for more information
using System.Threading.Channels;

Console.WriteLine("Channels In .NET");

// Create an unbounded channel
var channel = Channel.CreateUnbounded<int>();

// Producer writing data to the channel
async Task ProduceAsync()
{
    for (int i = 0; i < 5; i++)
    {
        await channel.Writer.WriteAsync(i);
        Console.WriteLine($"Produced: {i}");
    }
    channel.Writer.Complete();
}

// Consumer reading data from the channel
async Task ConsumeAsync()
{
    while (await channel.Reader.WaitToReadAsync())
    {
        while (channel.Reader.TryRead(out var item))
        {
            Console.WriteLine($"Consumed: {item}");
        }
    }
}

// Run producer and consumer asynchronously
var producerTask = ProduceAsync();
var consumerTask = ConsumeAsync();

// Wait for both tasks to complete
await Task.WhenAll(producerTask, consumerTask);

Console.ReadLine();

