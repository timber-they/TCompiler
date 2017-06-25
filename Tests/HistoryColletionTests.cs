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
                Assert.AreEqual(5, sut.Count);
                Assert.AreEqual(new List<int>
                {
                    0,
                    0,
                    0,
                    0,
                    0
                }, sut.Items);
                Assert.AreEqual(0, sut.Index);
                Assert.AreEqual(0, sut.CurrentHeight);

                sut.Push(0);
                Assert.AreEqual (new List<int>
                {
                    0,
                    0,
                    0,
                    0,
                    0
                }, sut.Items);
                Assert.AreEqual (1, sut.Index);
                Assert.AreEqual (1, sut.CurrentHeight);

                sut.Push(1);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    0,
                    0,
                    0
                }, sut.Items);
                Assert.AreEqual (2, sut.Index);
                Assert.AreEqual (2, sut.CurrentHeight);

                sut.Push(2);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    0,
                    0
                }, sut.Items);
                Assert.AreEqual (3, sut.Index);
                Assert.AreEqual (3, sut.CurrentHeight);

                sut.Push(3);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (4, sut.Index);
                Assert.AreEqual (4, sut.CurrentHeight);

                Assert.AreEqual(2, sut.Undo ());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (3, sut.Index);
                Assert.AreEqual (4, sut.CurrentHeight);

                Assert.AreEqual(1, sut.Undo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (2, sut.Index);
                Assert.AreEqual (4, sut.CurrentHeight);

                Assert.AreEqual(0, sut.Undo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (1, sut.Index);
                Assert.AreEqual (4, sut.CurrentHeight);

                Assert.AreEqual(1, sut.Redo());
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    2,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (2, sut.Index);
                Assert.AreEqual (4, sut.CurrentHeight);

                sut.Push(4);
                Assert.AreEqual (new List<int>
                {
                    0,
                    1,
                    4,
                    3,
                    0
                }, sut.Items);
                Assert.AreEqual (3, sut.Index);
                Assert.AreEqual (3, sut.CurrentHeight);
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