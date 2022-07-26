

using System.Diagnostics;

var urls = new List<string>()
{
    "https://cezarywalenciuk.pl/",
    "https://docs.microsoft.com/pl-pl/aspnet/core/blazor/",
    "https://angular.io/",
    "https://pl.reactjs.org/",
    "https://vuejs.org/",
    "https://svelte.dev/",
    "https://www.javascript.com/",
    "https://www.youtube.com/",
    "https://azure.microsoft.com/",
    "https://www.amazon.pl/",
    "https://zetcode.com/csharp/httpclient/",
    "https://stackoverflow.com/questions/55686928/using-stopwatch-in-c-sharp",
    "https://www.programmingwithwolfgang.com/replace-rabbitmq-azure-service-bus-queue/",
    "https://medium.com/dotnet-hub/use-azure-key-vault-with-net-or-asp-net-core-applications-read-azure-key-vault-secret-in-dotnet-fca293e9fbb3",
    "https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/attribute-mapping.html",
    "https://www.nuget.org/packages/System.Linq.Async",
    "https://github.com/dotnet/reactive",
    "https://www.udemy.com/",
    "https://jquery.com/",
    "https://www.php.net/",
    "https://www.python.org/",
    "https://go.dev/",
    "https://docs.microsoft.com/pl-pl/dotnet/csharp/"

};

while (true)
{
    var options = Console.ReadKey();

    if (options.KeyChar == '1')
        await SolutionOne();
    else if (options.KeyChar == '2')
        await SolutionTwo();
    else if (options.KeyChar == '3')
        await SolutionThree();
}


//Sekwencyjnie przetwarzamy
async Task SolutionOne()
{
    using var client = new HttpClient();
    Console.WriteLine();
    Console.WriteLine("Wpisz szukane słowo");
    var word = Console.ReadLine();
    if (string.IsNullOrEmpty(word))
        return;

    var timer = new Stopwatch(); timer.Start();

    var results = urls.ToAsyncEnumerable()
            .SelectAwait(async url =>
                new {
                    Url = url,
                    Html = await client.GetStringAsync(url)
                })
            .Where(x => x.Html.Contains(word));


    await foreach (var result in results)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Znalezione {result.Url}");
        Console.ResetColor();
    }

    timer.Stop();
    Console.WriteLine(timer.ElapsedMilliseconds);
}

//Przetwarzanie sekwencji równolegle
async Task SolutionTwo()
{
    using var client = new HttpClient();
    Console.WriteLine();
    Console.WriteLine("Wpisz szukane słowo");
    var word = Console.ReadLine();
    if (string.IsNullOrEmpty(word))
    return;

    var tasks = urls
        .Select(async url => new
        {
            Url = url,
            Html = await client.GetStringAsync(url)
        });

    var timer = new Stopwatch(); timer.Start();

    var results2 = await Task.WhenAll(tasks);

    foreach (var result in results2.Where(x => 
        x.Html.Contains(word)))
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Znalezione {result.Url}");
        Console.ResetColor();
    }

    timer.Stop();
    Console.WriteLine(timer.ElapsedMilliseconds);

}



//Równolegle
async Task SolutionThree()
{
    using var client = new HttpClient();
    Console.WriteLine();
    Console.WriteLine("Wpisz szukane słowo");
    var word = Console.ReadLine();
    if (string.IsNullOrEmpty(word))
        return;

    var parallelOptions = new ParallelOptions() 
    { MaxDegreeOfParallelism = 4 };

    var timer = new Stopwatch(); timer.Start();

    await Parallel.ForEachAsync(urls, parallelOptions,
                async (url, ct) 
                => await FindMatch(url, word, client));

    timer.Stop();
    Console.WriteLine(timer.ElapsedMilliseconds);
}

async Task FindMatch(string url, string searchTerm, HttpClient client)
{
    var html = await client.GetStringAsync(url);
    if (html.Contains(searchTerm))
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Znalezione {url}");
        Console.ResetColor();
    }
}


