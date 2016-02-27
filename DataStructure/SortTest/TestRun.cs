﻿using System;
using NUnit.Framework;
using Sequence;

namespace SortTest
{
    [TestFixture]
    public class TestRun
    {
        private readonly int[] _nums;

        public TestRun()
        {
            _nums = new int[]
            {
                4, 1, 5, 3, 2
            };
        }

        private void Print()
        {
            foreach (var num in _nums)
            {
                Console.Write(num + "\t");
            }
        }

        [Test]
        public void TestBubbleSort()
        {
            Sort<int>.BubbleSort(_nums);
            Print();
        }

        [Test]
        public void TestSelectionSort()
        {
            Sort<int>.SelectionSort(_nums);
            Print();
        }

        [Test]
        public void TestInsertSort()
        {
            Sort<int>.InsertSort(_nums);
            Print();
        }

        [Test]
        public void TestMergeSort()
        {
            Sort<int>.MergeSort(_nums);
            Print();
        }

        [Test]
        public void TestQuickSort()
        {
            Sort<int>.QuickSort(_nums);
            Print();
        }

        [Test]
        public void TestHeapSort()
        {
            Sort<int>.HeapSort(_nums);
            Print();
        }
    }
}