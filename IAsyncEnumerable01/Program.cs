using System.Net.Http.Json;

await GetTexts();


await foreach (var c in GetTexts3())
{
    Console.WriteLine(c);
}


async Task<List<string>> GetTexts()
{
    using var client = new HttpClient();

    var texts = await client.
        GetFromJsonAsync<List<string>>
        ("/api/texts");

    return texts;
}



//async Task<IEnumerable<string>> GetTexts2()
//{
//    using var client = new HttpClient();
//    var ids = new int[] { 11, 22, 33 };
//    foreach (var id in ids)
//    {
//        var text = await client.
//        GetFromJsonAsync<string>
//            ($"/api/customers?id={id}");

//        yield return text;
//    }
//}

async IAsyncEnumerable<string> 
    GetTexts3()
{
    using var client = new HttpClient();

    var ids = new int[] { 11, 22, 33, 44,
        55, 66, 77 ,88, 99 };

    foreach (var id in ids)
    {
        var text = await client.
            GetFromJsonAsync<string>
            ($"/api/texts?id={id}");

        yield return text;
    }
}

async IAsyncEnumerable<string> 
    GetTexts4(string game)
{
    using var client = new HttpClient();

    var ids = new int[] { 11, 22, 33 };

    foreach (var id in ids)
    {
        var text = await client.
            GetFromJsonAsync<string>
            ($"/api/texts?id={id}&game={game}");

        yield return text;
    }
}





//BAD IDEA
async Task<IAsyncEnumerable<string>> 
    GetTextsFromGame()
{
    var game = await GetGame();
    return GetTexts4(game);
}


//GOOD IDEA
async IAsyncEnumerable<string> 
    GetTexts5()
{
    var game = await GetGame();
    await foreach (var c in GetTexts4(game))
    {
        yield return c;
    }
}




async Task<string> GetGame()
{
    await Task.Delay(4000);
    return "Mortal Kombat 2";
}