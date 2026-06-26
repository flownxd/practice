using Xunit;
using task03;

namespace task03tests;

public class IteratorTests
{
    [Fact]
    public void CustomCollection_GetEnumerator_ReturnsAllItems()
    {
        var collection = new CustomCollection<int>();
        collection.Add(1);
        collection.Add(2);

        var result = new List<int>();
        foreach (var item in collection)
        {
            result.Add(item);
        }

        Assert.Equal(new[] { 1, 2 }, result);
    }

    [Fact]
    public void GetReverseEnumerator_ReturnsItemsInReverseOrder()
    {
        var collection = new CustomCollection<int>();
        collection.Add(1);
        collection.Add(2);
        collection.Add(3);

        var result = collection.GetReverseEnumerator().ToList();

        Assert.Equal(new[] { 3, 2, 1 }, result);
    }

    [Fact]
    public void GenerateSequence_ReturnsCorrectSequence()
    {
        var sequence = CustomCollection<int>.GenerateSequence(5, 3).ToList();

        Assert.Equal(new[] { 5, 6, 7 }, sequence);
    }

    [Fact]
    public void FilterAndSort_ReturnsFilteredAndSortedItems()
    {
        var collection = new CustomCollection<int>();
        collection.Add(3);
        collection.Add(1);
        collection.Add(4);
        collection.Add(2);

        var result = collection.FilterAndSort(x => x > 1, x => x).ToList();

        Assert.Equal(new[] { 2, 3, 4 }, result);
    }

    [Fact]
    public void Remove_RemovesItem()
    {
        var collection = new CustomCollection<string>();
        collection.Add("Яблоко");
        collection.Add("Банан");

        var removed = collection.Remove("Яблоко");

        Assert.True(removed);
        Assert.Single(collection);
        Assert.Equal("Банан", collection[0]);
    }

    [Fact]
    public void Count_ReturnsCorrectCount()
    {
        var collection = new CustomCollection<int>();
        collection.Add(1);
        collection.Add(2);
        collection.Add(3);

        Assert.Equal(3, collection.Count);
    }

    [Fact]
    public void Indexer_GetSet_WorksCorrectly()
    {
        var collection = new CustomCollection<string>();
        collection.Add("first");
        collection.Add("second");

        Assert.Equal("first", collection[0]);

        collection[0] = "changed";

        Assert.Equal("changed", collection[0]);
    }
}