


using System.Runtime.CompilerServices;

IAsyncEnumerable<string> folders = GetFolders();


var collection3_1 = folders
                    .Select( f => GetFilesInfoAsync(f))
                    .Select(f => f.WhereAwait
                    (async file => await IsSusAsync(file)));

await foreach (var fileinfos in collection3_1)
{
    await foreach (var item in fileinfos)
    {
        Console.WriteLine(item.Name);
        Console.Write($" -> {item.Length}\n");
    }
}

//var collection3_2 = folders
//                    .SelectAwait(async f => await GetFilesInfoAsync2(f))
//                    .Select(f => f.WhereAwait
//                    (async file => await IsSusAsync(file)));








//var collection3_3 = folders
//                    .SelectAwait(async f => await GetFilesInfoAsync2(f))
//                    .Select(f => f.ToAsyncEnumerable().WhereAwait
//                    (async file => await IsSusAsync(file)));


//await foreach (var fileinfos in collection3_3)
//{
//    await foreach (var item in fileinfos)
//    {
//        Console.WriteLine(item.Name);
//        Console.Write($" -> {item.Length}\n");
//    }
//}











//IAsyncEnumerable<TResult> Select<TSource, TResult>
//    (this IAsyncEnumerable<TSource> source, Func<TSource, TResult> selector)
//{

//}

//IAsyncEnumerable<TSource> Where<TSource>
//    (this IAsyncEnumerable<TSource> source, Func<TSource, bool> predicate)
//{

//}



async Task<bool> IsSusAsync(FileInfo file)
{
    await Task.Delay(500);
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
        await Task.Delay(2000);
        yield return item;
    }
}

async IAsyncEnumerable<FileInfo> GetFilesInfoAsync(string folder)
{

    var directory = new DirectoryInfo(folder);

    foreach (var item in directory.
           GetFiles("*", SearchOption.TopDirectoryOnly))
    {
        await Task.Delay(100);
        yield return item;
    }

}

async Task<FileInfo[]> GetFilesInfoAsync2(string folder)
{
    await Task.Delay(100);
    var directory = new DirectoryInfo(folder);

    return directory.
           GetFiles("*", SearchOption.TopDirectoryOnly);
}



public static class A
{
    static IAsyncEnumerable<T> Where<T>(this IAsyncEnumerable<T> source, 
        Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        // Using the CancellationToken parameter's default value
        return Core();

        async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in source.WithCancellation(cancellationToken))
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }
    static IAsyncEnumerable<T> WhereConfigureAwait<T>(this IAsyncEnumerable<T> source, Func<T, bool> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return Core();

        async IAsyncEnumerable<T> Core([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var item in source.WithCancellation(cancellationToken).
                ConfigureAwait(false))
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }

    static IAsyncEnumerable<T> WhereAwait<T>(this IAsyncEnumerable<T> source, Func<T, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return Core();

        async IAsyncEnumerable<T> Core([EnumeratorCancellation] 
        CancellationToken cancellationToken = default)
        {
            await foreach (var item in source.WithCancellation
                (cancellationToken).ConfigureAwait(false))
            {
                // Await the ValueTask<bool> returned from the predicate
                if (await predicate(item).ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
    }

    static IAsyncEnumerable<T> WhereAwaitWithCancellation<T>(this IAsyncEnumerable<T> source, Func<T, CancellationToken, ValueTask<bool>> predicate)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        return Core();

        async IAsyncEnumerable<T> Core([EnumeratorCancellation] 
        CancellationToken cancellationToken = default)
        {
            await foreach (var item in source.WithCancellation
                (cancellationToken).ConfigureAwait(false))
            {
                // Pass the CancellationToken to the predicate itself,
                // on top of the enumerable
                if (await predicate(item, cancellationToken).
                    ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }
    }
}