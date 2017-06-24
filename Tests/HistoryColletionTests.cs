using System;
using System.Collections.Generic;

using NUnit.Framework;

using MetaTextBoxLibrary;


namespace Tests
{
    [TestFixture]
    public class HistoryColletionTests
    {
        [Test]
        public void HistoryCollectionTestRun ()
        {
            try
            {
                var sut = new HistoryCollection<int> (5);
                Assert.AreEqual(5, sut._count);
                Assert.AreEqual(new List<int>
                {
                    0,
                    0,
                    0,
                    0,
                    0
                }, sut._items);
                Assert.AreEqual(0, sut._index);
                Assert.AreEqual(0, sut._currentHeight);

                sut.Push(0);
                Assert.AreEqual (new List<int>
                {
                    0,
                    0,
                    0,
                    0,
                    0
                }, sut._items);
                Assert.AreEqual (1, sut._index);
                Assert.AreEqual (1, sut._currentHeight);

                sut.Push(1);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    0,
                    0,
                    0
                }, sut._items);
                Assert.AreEqual (2, sut._index);
                Assert.AreEqual (2, sut._currentHeight);

                sut.Push(2);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    0,
                    0
                }, sut._items);
                Assert.AreEqual (3, sut._index);
                Assert.AreEqual (3, sut._currentHeight);

                sut.Push(3);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (4, sut._index);
                Assert.AreEqual (4, sut._currentHeight);

                Assert.AreEqual(2, sut.Undo ());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (3, sut._index);
                Assert.AreEqual (4, sut._currentHeight);

                Assert.AreEqual(1, sut.Undo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (2, sut._index);
                Assert.AreEqual (4, sut._currentHeight);

                Assert.AreEqual(0, sut.Undo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (1, sut._index);
                Assert.AreEqual (4, sut._currentHeight);

                Assert.AreEqual(1, sut.Redo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (2, sut._index);
                Assert.AreEqual (4, sut._currentHeight);

                sut.Push(4);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    4,
                    3,
                    0
                }, sut._items);
                Assert.AreEqual (3, sut._index);
                Assert.AreEqual (3, sut._currentHeight);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }
    }
}