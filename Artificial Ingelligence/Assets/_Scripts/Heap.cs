using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T> {
    
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    public void Add(T item) {
        item.heapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    void SortUp(T item) {

    }
}

public interface IHeapItem<T> : IComparable {
    int heapIndex {
        get;
        set;
    }
}
