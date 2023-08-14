using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T: IHeapElem<T>
{
    T[] elements;
    int currentElemCount;

    public Heap(int maxHeapSize)
    {
        elements = new T[maxHeapSize];
    }

    public void Add(T elem)
    {
        elem.HeapIndex = currentElemCount;
        elements[currentElemCount] = elem;
        SortUp(elem);
        currentElemCount++;
    }

    public T RemoveFirstElem()
    {
        T firstElem = elements[0];
        currentElemCount--;
        elements[0] = elements[currentElemCount];
        elements[0].HeapIndex = 0;
        SortDown(elements[0]);
        return firstElem;
    }

    public void UpdateElem(T elem)
    {
        SortUp(elem);
    }
    public int Count
    {
        get
        {
            return currentElemCount;
        }
    }
    public bool Contains(T elem)
    {
        return Equals(elements[elem.HeapIndex], elem);
    }
    void SortDown(T elem)
    {
        while (true)
        {
            int leftChildIndex = elem.HeapIndex * 2 + 1;
            int rightChildIndex = elem.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (leftChildIndex < currentElemCount)
            {
                swapIndex = leftChildIndex;
                if (rightChildIndex < currentElemCount)
                {
                    if (elements[leftChildIndex].CompareTo(elements[rightChildIndex]) < 0)
                    {
                        swapIndex = rightChildIndex;
                    }
                }

                if (elem.CompareTo(elements[swapIndex]) < 0)
                {
                    SwapElems(elem, elements[swapIndex]);
                }
                else
                {
                    return;

                }
                    
            }
            else
            {
                return;
            }
                
        }
    }

    void SortUp(T elem)
    {
        int parentIndex = (elem.HeapIndex - 1)/ 2;

        while (true)
        {
            T parentElem = elements[parentIndex];
            if (elem.CompareTo(parentElem) > 0)
            {
                SwapElems(elem, parentElem);
            }
            else
                break;

            parentIndex = (elem.HeapIndex - 1) / 2;
        }

    }

    void SwapElems(T elemA, T elemB)
    {
        elements[elemA.HeapIndex] = elemB;
        elements[elemB.HeapIndex] = elemA;

        int elemAIndex = elemA.HeapIndex;
        elemA.HeapIndex = elemB.HeapIndex;
        elemB.HeapIndex = elemAIndex;

    }
}

public interface IHeapElem<T>: IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
