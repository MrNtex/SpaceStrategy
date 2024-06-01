using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularBuffer<T>
{
    private readonly T[] _buffer;

    private int _head;
    private int _tail;
    private int _count;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0) throw new System.ArgumentOutOfRangeException("capacity", "must be greater than zero");

        _buffer = new T[capacity];
        _count = 0;
        _head = 0;
        _tail = 0;
    }

    public int Count => _count;
    public int Capacity => _buffer.Length;

    public void Add(T item)
    {
        _buffer[_head] = item;

        if (++_head == _buffer.Length)
        {
            _head = 0;
        }

        if (_count == _buffer.Length)
        {
            if (++_tail == _buffer.Length)
            {
                _tail = 0;
            }
        }
        else
        {
            ++_count;
        }
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count) throw new System.ArgumentOutOfRangeException("index");

            return _buffer[(_tail + index) % _buffer.Length];
        }
        set
        {
            if (index < 0 || index >= _count) throw new System.ArgumentOutOfRangeException("index");

            _buffer[(_tail + index) % _buffer.Length] = value;
        }
    }

    public void Clear()
    {
        _count = 0;
        _head = 0;
        _tail = 0;
    }

    public T[] ToArray()
    {
        T[] array = new T[_count];
        for (int i = 0; i < _count; i++)
        {
            array[i] = _buffer[(_tail + i) % _buffer.Length];
        }
        return array;
    }
}
