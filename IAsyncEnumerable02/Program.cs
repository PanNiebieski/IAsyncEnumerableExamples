
List<string> foldersExampleLinqAsync = 
    await GetFolders().ToListAsync();


List<string> folders = new List<string>();
await foreach (var folder in GetFolders())
{
    folders.Add(folder);
}


var collection1_1 = GetFolders()
                    .Select(f => GetFilesInfoAsync(f))
                    .Select(f => f.Where(file => IsSus(file)));


await foreach(var fileinfos in collection1_1)
{
    await foreach(var item in fileinfos)
    {
        Console.WriteLine(item.Name);
        Console.Write($" -> {item.Length}\n");
    }

}

var collection1_2 = GetFolders()
                    .Select(f => GetFilesInfoAsync(f))
                    .Select
                    (files => files.Select
                    (file =>
                        new
                        {
                            IsSus = IsSus(file),
                            Name = file.Name,
                            FullName = file.FullName,
                            Lenght = file.Length,
                        }
                    ));





var collection2_2 = folders
                    .Select(f => GetFilesInfoAsync(f))
                    .Select
                    (files => files.Select
                    (async file =>
                        new
                        {
                            IsSus = await IsSusAsync(file),
                            Name = file.Name,
                            FullName = file.FullName,
                            Lenght = file.Length,
                        }
                    ));


//var collection2_1 = folders
//                    .Select(f => GetFilesInfoAsync(f))
//                    .Select
//                    (f => f.Where
//                    (async file => await IsSusAsync(file)));




var collection2_1_1 = folders
                    .Select(f => GetFilesInfoAsync(f))
                    .Select
                    (f => f.
                    Where(file => IsSusAsync(file).Result));



var collection2_2_1 = folders
                    .Select(f => new DirectoryInfo(f).
                    GetFiles("*", SearchOption.TopDirectoryOnly))
                    .Select
                    (files => files.Select
                    (file => 
                        new 
                        {
                            IsSus = IsSusAsync(file).Result,
                            Name = file.Name,
                            FullName = file.FullName,
                            Lenght = file.Length,
                        }
                    ));
                       





async Task<bool> IsSusAsync(FileInfo file)
{
    await Task.Delay(2000);
    Random r = new Random();

    return r.Next(1, 10) > 8;
}

bool IsSus(FileInfo file)
{
    Random r = new Random();

    return r.Next(1, 10) > 8;
}

async IAsyncEnumerable<string> GetFolders()
{
    List<string> folders = new List<string>()
    {
        @"D:\__zdjecia",
        @"D:\_smieciedoanalizy",
        @"D:\----------------",
        @"D:\00",
        @"D:\0Prezentacja",
        @"D:\Camera",
    };

    foreach (var item in folders)
    {
        await Task.Delay(4000);
        yield return item;
    }
}

async IAsyncEnumerable<FileInfo> 
    GetFilesInfoAsync(string folder)
{
    var directory = new DirectoryInfo(folder);

    foreach (var item in directory.
           GetFiles("*", SearchOption.TopDirectoryOnly))
    {
        await Task.Delay(100);
        yield return item;
    }

}


async Task<FileInfo[]> GetFilesInfoTaskAsync(string folder)
{
    await Task.Delay(100);
    var directory = new DirectoryInfo(folder);

    return directory.
           GetFiles("*", SearchOption.TopDirectoryOnly);
}


