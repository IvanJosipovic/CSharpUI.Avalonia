using Avalonia;
using Avalonia.Collections;
using System.Collections;
using System.Collections.Specialized;

namespace Tests;

public class AvaloniaListTest : AvaloniaObject, IAvaloniaList<AvaloniaListTest>
{
    private List<AvaloniaListTest> items;

    public AvaloniaListTest this[int index] { get => items[index]; set => items[index] = value; }

    public int Count => 1;

    public bool IsReadOnly => false;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public void Add(AvaloniaListTest item)
    {
    }

    public void AddRange(IEnumerable<AvaloniaListTest> items)
    {
    }

    public void Clear()
    {
    }

    public bool Contains(AvaloniaListTest item)
    {
        return false;
    }

    public void CopyTo(AvaloniaListTest[] array, int arrayIndex)
    {
    }

    public IEnumerator<AvaloniaListTest> GetEnumerator()
    {
        return null;
    }

    public int IndexOf(AvaloniaListTest item)
    {
        return 1;
    }

    public void Insert(int index, AvaloniaListTest item)
    {
    }

    public void InsertRange(int index, IEnumerable<AvaloniaListTest> items)
    {
    }

    public void Move(int oldIndex, int newIndex)
    {
    }

    public void MoveRange(int oldIndex, int count, int newIndex)
    {
    }

    public bool Remove(AvaloniaListTest item)
    {
        return true;
    }

    public void RemoveAll(IEnumerable<AvaloniaListTest> items)
    {
    }

    public void RemoveAt(int index)
    {
    }

    public void RemoveRange(int index, int count)
    {
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}