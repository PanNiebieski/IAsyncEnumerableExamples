int i = 0;
foreach (var item in Napisy())
{
    i++;
    Console.WriteLine(item);
}

IEnumerable<string> Napisy()
{
    yield return "Ala";
    yield return "ma";
    yield return "radioaktywnego";
    yield return "kota";
    yield return "z";
    yield return "Marsa";
}



class MyObjects : IEnumerable<MyObject>
{
    List<MyObject> mylist = new List<MyObject>();

    public MyObject this[int index]
    {
        get { return mylist[index]; }
        set { mylist.Insert(index, value); }
    }

    public IEnumerator<MyObject> GetEnumerator()
    {
        return mylist.GetEnumerator();
    }

    System.Collections.IEnumerator 
        System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}



