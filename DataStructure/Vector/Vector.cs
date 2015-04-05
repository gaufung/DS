﻿using System;

namespace Vector
{
    public class Vector<T> :IVector<T> 
    {
        private T[] _elem;
        private const int DefaultCapactiry = 4;
        private int _size;
        private int _capacity;

        #region 构造函数
        public Vector()
        {
            _capacity = DefaultCapactiry;
            _elem = new T[_capacity];
            _size = 0;
        }
        private void CopyFrom(T[] source, int lo, int hi)
        {
            _elem=new T[3*(hi-lo)];
            _size = 0;
            while (lo<hi)
            {
                _elem[_size++] = source[lo++];
            }
        }
       

        public Vector(T[] source, int lo, int hi)
        {
            CopyFrom(source,lo,hi);
        }

        public Vector(T[] source,int n)
        {
            CopyFrom(source,0,n);
        }

        public Vector(Vector<T> v,int lo,int hi)
        {
            CopyFrom(v._elem,lo,hi);
        }

        public Vector(Vector<T> v)
        {
            CopyFrom(v._elem,0,v._size);
        }
        #endregion

        #region 扩容和缩小
        /// <summary>
        /// Expand the Vector's Capacity
        /// </summary>
        private void Expand()
        {
            if (_size < _capacity) return;
            if (_capacity < DefaultCapactiry) _capacity = DefaultCapactiry;
            var newElem = new T[_capacity <<= 1];
            for (var i = 0; i < _size; i++)
            {
                newElem[i] = _elem[i];
            }
            _elem = newElem;
        }

        /// <summary>
        /// Shrink the Vector's Capacity
        /// </summary>
        private void Shrink()
        {
            if (_capacity < DefaultCapactiry << 1) return;
            if (_size << 2 > _capacity) return;
            var newElem=new T[_capacity>>=1];
            for (var i = 0; i < _size; i++)
            {
                newElem[i] = _elem[i];
            }
            _elem = newElem;
        } 
        #endregion

        #region 属性
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index">序号</param>
        /// <returns>返回值</returns>
        public T this[int index]
        {

            get
            {
                return _elem[index];
            }
            set
            {
                _elem[index] = value;
            }
        }

        public int Size
        {
            get { return _size; }
        }

        public bool Empty
        {
            get { return _size == 0; }
        }

        #endregion

        #region 比较操作符
        /// <summary>
        /// Greater Than
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static  bool Gt(T a, T b)
        {
            if (a is IComparable)
            {
                return (a as IComparable).CompareTo(b) == 1;
            }
            throw new InvalidCastException("T is not IComparable");
        }
        /// <summary>
        /// Equal to
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Eq(T a, T b)
        {
            if (a is IComparable)
            {
                return (a as IComparable).CompareTo(b) == 0;
            }
            throw new InvalidCastException("T is not IComparable");
        }
        /// <summary>
        /// Litter than
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static bool Lt(T a, T b)
        {
            if (a is IComparable)
            {
                return (a as IComparable).CompareTo(b) == -1;
            }
            throw new InvalidCastException("T is not IComparable");
        }
        #endregion
                
        #region 无序查找
        /// <summary>
        /// Find  a　elemt  int the whole vector. If Failed return -1. the vector is not sorted.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public int Find(T e)
        {
            return Find(e, 0, _size);
        }

        /// <summary>
        /// Find  a　elemt  int part vector. If Failed return lo-1. the vector is not sorted.
        /// </summary>
        /// <param name="e">to find elem</param>
        /// <param name="lo">lo</param>
        /// <param name="hi">hi</param>
        /// <returns>found index</returns>
        public int Find(T e, int lo, int hi)
        {
            while (lo < hi-- && !Eq(_elem[hi], e))
            {
            }
            return hi;
        }
        #endregion

        #region 有序查找

        public int Search(T e)
        {
            return Search(e, 0, _size);
        }

        public int Search(T e, int lo, int hi)
        {
            var random = new Random();
            return (random.Next()%2 == 0) ? BinSearch(_elem, lo, hi, e) : FibSearch(_elem, lo, hi, e);
        }

        /// <summary>
        /// Binary Search
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static int BinSearch(T[] elem, int lo, int hi, T e)
        {
            while (lo < hi)
            {
                int mi = (lo + hi) >> 1;
                if (Lt(e, elem[mi]))
                {
                    hi = mi;
                }
                else
                {
                    if (Lt(elem[mi], e)) lo = mi + 1;
                    else
                    {
                        return mi;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Fibonacci Search
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private static int FibSearch(T[] elem, int lo, int hi, T e)
        {
            Fib fib = new Fib(hi - lo);
            while (lo < hi)
            {
                while (hi - lo < fib.Get())
                {
                    fib.Prev();
                }
                int mi = lo + fib.Get() - 1;
                if (Lt(e, elem[mi]))
                {
                    hi = mi;
                }
                else
                {
                    if (Lt(elem[mi], e))
                    {
                        lo = mi + 1;
                    }
                    else
                    {
                        return mi;
                    }
                }
            }
            return -1;
        }

        #endregion

        #region 删除元素

        /// <summary>
        /// remove a elem in r index;
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public T Remove(int r)
        {
            T e = _elem[r];
            Remove(r, r + 1);
            return e;
        }

        /// <summary>
        /// remove elemes index range [lo,hi)
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>the length to remove</returns>
        public int Remove(int lo, int hi)
        {
            while (hi < _size)
            {
                _elem[lo] = _elem[hi];
                lo++;
                hi++;
            }
            _size -= hi - lo;
            Shrink();
            return hi - lo;
        }

        #endregion

        #region 插入元素

        /// <summary>
        /// Insert a elem in the Vector
        /// </summary>
        /// <param name="r">the index to insert</param>
        /// <param name="e">item's value</param>
        /// <returns>the index</returns>
        public int Insert(int r, T e)
        {
            Expand();
            for (int i = _size; i > r; i--)
            {
                _elem[i] = _elem[i - 1];
            }
            _elem[r] = e;
            _size++;
            return r;
        }

        /// <summary>
        /// Insert a elem in the vector tailer
        /// </summary>
        /// <param name="e">elem value</param>
        /// <returns>insert index</returns>
        public int Insert(T e)
        {
            return Insert(_size, e);
        }

        #endregion

        /// <summary>
        /// Calculate the reverse count
        /// </summary>
        /// <returns></returns>
        public int DisOrdered()
        {
            int coount = 0;
            for (int i = 0; i < _size - 1; i++)
            {
                if (Lt(_elem[i + 1], _elem[i]))
                {
                    coount++;
                }
            }
            return coount;
        }

        #region 去除重复

        /// <summary>
        /// Unique the vectro
        /// </summary>
        /// <returns></returns>
        public int Deduplicate()
        {
            int oldSize = _size;
            int i = 1;
            while (i < _size)
            {
                if (Find(_elem[i], 0, i) < 0)
                {
                    i++;
                }
                else
                {
                    Remove(i);
                }
            }
            Shrink();
            return oldSize - _size;
        }

        public int Uniquify()
        {
            int i = 0, j = 0;
            while (++j < _size)
            {
                if (!Eq(_elem[i], _elem[j]))
                {
                    i++;
                    _elem[i] = _elem[j];
                }
            }
            _size = ++i;
            Shrink();
            return j - i;
        }

        #endregion


        /// <summary>
        /// Traverse the element in the vector
        /// </summary>
        /// <param name="action"></param>
        public void Traverse(System.Action<T> action)
        {
            for (int i = 0; i < _size; i++)
            {
                action(_elem[i]);
            }
        }



      
    }
}