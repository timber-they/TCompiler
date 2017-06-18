using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using MetaTextBox;

using NUnit.Framework;


// ReSharper disable InconsistentNaming
// ReSharper disable ObjectCreationAsStatement


namespace Tests
{
    [TestFixture]
    public class ColoredStringTests
    {
        [TestCase (0, true), TestCase (0, false), TestCase (1, false)]
        public void ColoredString_Always_Creatable (int count, bool @null)
        {
            try
            {
                new ColoredString (@null ? null : Enumerable.Repeat<ColoredCharacter> (null, count).ToList ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (true, null), TestCase (true, " "), TestCase (false, null), TestCase (false, ""),
         TestCase (false, " ")]
        public void ColoredStringTest1 (bool colorEmpty, string text)
        {
            try
            {
                new ColoredString (colorEmpty ? Color.Empty : Color.AliceBlue, colorEmpty ? Color.Empty : Color.AliceBlue, text);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (true, null), TestCase (true, " "), TestCase (false, null), TestCase (false, ""),
         TestCase (false, " ")]
        public void ColoredStringTest2 (bool colorEmpty, string text)
        {
            try
            {
                var old = new ColoredString (colorEmpty ? Color.Empty : Color.AliceBlue, colorEmpty ? Color.Empty : Color.AliceBlue, text);
                var @new = new ColoredString(old);
                Assert.AreEqual(old, @new);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', '_', true), TestCase (' ', ' ', false), TestCase (' ', '_', false), TestCase (' ', ' ', true)]
        public void ReplaceTest (char toReplace, char replacement, bool containsElements)
        {
            try
            {
                var sUT = new ColoredString (containsElements
                                                 ? new List<ColoredCharacter>
                                                 {
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, toReplace),
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, replacement)
                                                 }
                                                 : new List<ColoredCharacter> ());
                var n = sUT.Replace (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, toReplace),
                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, replacement));
                var n2 = sUT.Replace (new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, toReplace),
                                      new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, replacement));
                var oldToReplace = sUT.ColoredCharacters.Count (character => character.Character == toReplace);
                var newToReplace = n.ColoredCharacters.Count (character => character.Character == toReplace);
                var newToReplace2 = n2.ColoredCharacters.Count (character => character.Character == toReplace);
                var oldReplacements = sUT.ColoredCharacters.Count (character => character.Character == replacement);
                var newReplacements = n.ColoredCharacters.Count (character => character.Character == replacement);
                Assert.AreEqual (sUT.Count (), n.Count ());
                Assert.AreEqual (sUT.Count (), n2.Count ());

                if (toReplace == replacement)
                    return;
                Assert.AreEqual (0, newToReplace);
                Assert.AreEqual (oldToReplace, newToReplace2);
                Assert.AreEqual (oldToReplace, newReplacements - oldReplacements);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', true), TestCase ('_', true), TestCase ('.', true), TestCase (' ', false), TestCase ('_', false),
         TestCase ('.', false)]
        public void RemoveTest (char toRemove, bool containsElements)
        {
            try
            {
                var sUt = new ColoredString (containsElements
                                                 ? new List<ColoredCharacter>
                                                 {
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, ' '),
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, '_')
                                                 }
                                                 : new List<ColoredCharacter> ());
                var n = sUt.Remove (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, toRemove));
                var n2 = sUt.Remove (new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, toRemove));
                var oldToRemove = sUt.ColoredCharacters.Count (character => character.Character == toRemove);
                var newToRemove = n.ColoredCharacters.Count (character => character.Character == toRemove);
                var newToRemove2 = n2.ColoredCharacters.Count (character => character.Character == toRemove);
                Assert.AreEqual (0, newToRemove);
                Assert.AreEqual (oldToRemove, newToRemove2);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', true), TestCase ('_', true), TestCase ('.', true), TestCase (' ', false), TestCase ('_', false),
         TestCase ('.', false)]
        public void RemoveTest1 (char toRemove, bool containsElements)
        {
            try
            {
                var sUt = new ColoredString (containsElements
                                                 ? new List<ColoredCharacter>
                                                 {
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, ' '),
                                                     new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, '_')
                                                 }
                                                 : new List<ColoredCharacter> ());
                var n = sUt.Remove (toRemove);
                var newToRemove = n.ColoredCharacters.Count (character => character.Character == toRemove);
                if (newToRemove != 0)
                    Assert.Fail ();
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (0, 0, true), TestCase (-1, 0, false), TestCase (-1, -1, false), TestCase (1, 1, true),
         TestCase (2, 2, true), TestCase (3, 3, false)]
        public void RemoveTest2 (int index, int count, bool containsEnoughElements)
        {
            try
            {
                var sUT = new ColoredString (containsEnoughElements
                                                 ? Enumerable.Repeat (new ColoredCharacter (Color.AliceBlue,
                                                                                            Color.AliceBlue, ' '),
                                                                      index + count).ToList ()
                                                 : new List<ColoredCharacter> ());
                var n = sUT.Remove (index, count);
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have been thrown");
                Assert.AreEqual (count, sUT.Count () - n.ColoredCharacters.Count);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (0, 0, true), TestCase (-1, 0, false), TestCase (-1, -1, false), TestCase (1, 1, true),
         TestCase (2, 2, true), TestCase (3, 3, false)]
        public void InsertTest (int index, int count, bool containsEnoughElements)
        {
            try
            {
                var sUT = new ColoredString (containsEnoughElements
                                                 ? Enumerable.Repeat (new ColoredCharacter (Color.AliceBlue,
                                                                                            Color.AliceBlue, ' '),
                                                                      index).ToList ()
                                                 : new List<ColoredCharacter> ());
                var n = sUT.Insert (index, new ColoredString (
                                        Enumerable.Repeat (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, ' '),
                                                           count).ToList ()));
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have been thrown");
                Assert.AreEqual (count, n.ColoredCharacters.Count - sUT.Count ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (0, true), TestCase (-1, false), TestCase (1, true),
         TestCase (2, true), TestCase (3, false)]
        public void InsertTest1 (int index, bool containsEnoughElements)
        {
            try
            {
                var sUT = new ColoredString (containsEnoughElements
                                                 ? Enumerable.Repeat (new ColoredCharacter (Color.AliceBlue,
                                                                                            Color.AliceBlue, ' '),
                                                                      index).ToList ()
                                                 : new List<ColoredCharacter> ());
                var n = sUT.Insert (index, new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, ' '));
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have been thrown");
                Assert.AreEqual (1, n.ColoredCharacters.Count - sUT.Count ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', 0, false), TestCase (' ', 1, false), TestCase (' ', 2, false), TestCase (' ', 0, true),
         TestCase (' ', 1, true), TestCase (' ', 2, true)]
        public void SplitTest (char splitter, int splitterAmount, bool stuffBetween)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (stuffBetween ? "_" : "",
                                                          Enumerable.Repeat (splitter, splitterAmount)));
                var n = sUT.Split (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, splitter));
                var n2 = sUT.Split (new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, splitter));
                var splittedLength = n.Count;
                var splittedLength2 = n2.Count;
                Assert.AreEqual (splitterAmount + 1, splittedLength);
                Assert.AreEqual (1, splittedLength2);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', 0, false), TestCase (' ', 1, false), TestCase (' ', 2, false), TestCase (' ', 0, true),
         TestCase (' ', 1, true), TestCase (' ', 2, true)]
        public void SplitTest1 (char splitter, int splitterAmount, bool stuffBetween)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (stuffBetween ? "_" : "",
                                                          Enumerable.Repeat (splitter, splitterAmount)));
                var n = sUT.Split (splitter);
                var splittedLength = n.Count;
                Assert.AreEqual (splitterAmount + 1, splittedLength);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', 0, false), TestCase (' ', 1, false), TestCase (' ', 2, false), TestCase (' ', 0, true),
         TestCase (' ', 1, true), TestCase (' ', 2, true)]
        public void ContainsTest (char toContain, int containingAmount, bool stuffBetween)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (stuffBetween ? "_" : "",
                                                          Enumerable.Repeat (toContain, containingAmount)));
                var n = sUT.Contains (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, toContain));
                var n2 = sUT.Contains (new ColoredCharacter (Color.AntiqueWhite, Color.AntiqueWhite, toContain));
                var n3 = sUT.Contains (new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, (char) (toContain + 1)));
                if (containingAmount > 0)
                    Assert.IsTrue (n);
                else
                    Assert.IsFalse (n);
                Assert.IsFalse (n2);
                Assert.IsFalse (n3);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (' ', 0, false), TestCase (' ', 1, false), TestCase (' ', 2, false), TestCase (' ', 0, true),
         TestCase (' ', 1, true), TestCase (' ', 2, true)]
        public void ContainsTest1 (char toContain, int containingAmount, bool stuffBetween)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (stuffBetween ? "_" : "",
                                                          Enumerable.Repeat (toContain, containingAmount)));
                var n = sUT.Contains (toContain);
                var n3 = sUT.Contains ((char) (toContain + 1));
                if (containingAmount > 0)
                    Assert.IsTrue (n);
                else
                    Assert.IsFalse (n);
                Assert.IsFalse (n3);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (-1, false), TestCase (0, false), TestCase (1, false), TestCase (0, true), TestCase (1, true)]
        public void GetTest (int index, bool containsEnoughElements)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (
                                                 "",
                                                 Enumerable.Range ('a', containsEnoughElements ? index + 1 : 0).
                                                            Select (i => (char) i)));
                var n = sUT.Get (index);
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have got thrown");
                Assert.AreEqual (sUT.ColoredCharacters [index].Character, n.Character);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (0), TestCase (null), TestCase (1), TestCase (10)]
        public void CountTest (int? amount)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             amount == null
                                                 ? null
                                                 : string.Join ("", Enumerable.Repeat (' ', amount.Value)));
                var n = sUT.Count ();
                if (amount == null)
                    Assert.Fail ("Exception should have been thrown");
                Assert.AreEqual (amount.Value, n);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (amount != null)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (-1, false, 0), TestCase (1, false, 0), TestCase (0, true, 0), TestCase (1, true, 0),
         TestCase (10, true, 0), TestCase (0, true, 2), TestCase (1, true, 3),
         TestCase (10, true, 4)]
        public void SubstringTest (int index, bool containsEnoughElements, int itemsMore)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (
                                                 "",
                                                 containsEnoughElements
                                                     ? Enumerable.Repeat (' ', index + itemsMore)
                                                     : ""));
                var n = sUT.Substring (index);
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have been thrown");
                Assert.AreEqual (itemsMore, n.Count ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (-1, false, -1, 0), TestCase (1, false, -1, 0), TestCase (0, true, 1, 0), TestCase (1, true, 1, 0),
         TestCase (10, true, 2, 0), TestCase (0, true, 3, 2), TestCase (1, true, 0, 3),
         TestCase (10, true, 5, 4)]
        public void SubstringTest1 (int index, bool containsEnoughElements, int count, int itemsMore)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             string.Join (
                                                 "",
                                                 containsEnoughElements
                                                     ? Enumerable.Repeat (' ', index + count + itemsMore)
                                                     : ""));
                var n = sUT.Substring (index, count);
                if (!containsEnoughElements)
                    Assert.Fail ("An exception should have been thrown");
                Assert.AreEqual (count, n.Count ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                if (containsEnoughElements)
                    Assert.Fail (e.Message);
            }
        }

        [TestCase (0, true), TestCase (null, true), TestCase (1, false), TestCase (2, false)]
        public void LastOrDefaultTest (int? count, bool nullExpected)
        {
            try
            {
                var sUT = new ColoredString (Color.AliceBlue, Color.AliceBlue, 
                                             count == null
                                                 ? null
                                                 : string.Join ("", Enumerable.Repeat (' ', count.Value)));
                var n = sUT.LastOrDefault ();
                if (nullExpected)
                    Assert.IsNull (n);
                else
                {
                    Assert.IsNotNull(n);
                    Assert.AreEqual (' ', n.Character);
                }
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [Test]
        public void EqualsTest ()
        {
            try
            {
                var sUT = new ColoredString(Color.AliceBlue, Color.AliceBlue, " ");
                var c1 = new ColoredString(Color.AliceBlue, Color.AliceBlue, " ");
                var c2 = new ColoredString(Color.AntiqueWhite, Color.AntiqueWhite, " ");
                var c3 = new ColoredString(Color.AliceBlue, Color.AliceBlue, "  ");
                var c4 = new ColoredString(Color.AliceBlue, Color.AliceBlue, null);
                var c5 = new ColoredString (Color.AliceBlue, Color.AntiqueWhite, " ");
                var n1 = Equals (sUT, c1);
                var n2 = Equals (sUT, c2);
                var n3 = Equals (sUT, c3);
                var n4 = Equals (sUT, c4);
                var n5 = Equals (sUT, c5);
                Assert.IsTrue(n1);
                Assert.IsFalse(n2);
                Assert.IsFalse(n3);
                Assert.IsFalse(n4);
                Assert.IsFalse(n5);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (""), TestCase (null), TestCase ("foo")]
        public void ToStringTest (string s)
        {
            try
            {
                var sUT = new ColoredString(Color.AliceBlue, Color.AliceBlue, s);
                var n = sUT.ToString ();
                Assert.AreEqual(s ?? "", n);
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