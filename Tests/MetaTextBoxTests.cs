using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using MetaTextBoxLibrary;

using NUnit.Framework;

using Mtb = MetaTextBoxLibrary.MetaTextBox;


// ReSharper disable ObjectCreationAsStatement


namespace Tests
{
    [TestFixture, Apartment (ApartmentState.STA)]
    public class MetaTextBoxTests
    {
        private readonly Mtb _systemUnderTest = new Mtb ();

        [Test]
        public void MetaTextBox_Always_ThrowsNoException ()
        {
            try
            {
                new Mtb ();
            }
            catch (Exception)
            {
                Assert.Fail ();
            }
        }

        [TestCase (1), TestCase (100)]
        public void TextTest (int count)
        {
            var sUt = new Mtb ();
            sUt.SetText (new string ('\n', count));
            Assert.AreEqual (sUt.Text.ToString (), new string ('\n', count));
        }

        [TestCase (' '), TestCase ('m'), TestCase ('i')]
        public void GetCharacterWidth_Always_ReturnsExpectedValue (char testCharacter)
        {
            Assert.AreEqual (7, _systemUnderTest.GetCharacterWidth ());
        }

        [TestCase (""), TestCase (" "), TestCase ("foo")]
        public void GetStringWidth_Always_ReturnsExpectedValue (string testString)
        {
            Assert.AreEqual (testString.Length * _systemUnderTest.GetCharacterWidth (),
                             _systemUnderTest.GetStringWidth (testString));
        }

        [TestCase (new [] {Keys.A}, true),
         TestCase (new [] {Keys.Shift}, false),
         TestCase (new [] {Keys.G, Keys.Alt, Keys.Control}, false),
         TestCase (new [] {Keys.Q, Keys.Alt, Keys.Control}, true),
         TestCase (new [] {Keys.D5, Keys.Shift}, true),
         TestCase (new [] {Keys.D9, Keys.Shift, Keys.Alt, Keys.Control}, false)]
        public void PerformInput_Always_ValidatesExpected (Keys [] keys, bool expectedValidation)
        {
            var keysValue = keys.Aggregate (0, (current, key) => current | (int) key);
            Assert.AreEqual (expectedValidation,
                             _systemUnderTest.PerformInput (keys.First (), new KeyEventArgs ((Keys) keysValue)));
        }

        [TestCase (0, 0, 0, 0), TestCase (1, 1, 0, 0), TestCase (10, 20, 1, 1),
         TestCase (100, 100, 2, 1), TestCase (-1, -1, 0, 0)]
        public void GetCursorLocationToPointTest (int x, int y, int expectedX, int expectedY)
        {
            try
            {
                var sUt = new Mtb ();
                sUt.SetText ("bla\nhi");
                var result = sUt.GetCursorLocationToPoint (new Point (x, y));
                Assert.AreEqual (expectedX, result.X);
                Assert.AreEqual (expectedY, result.Y);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [TestCase (0, 0, 10, null, null), TestCase (1, 0, 10, null, null), TestCase (1, 2, 1, 2, 7),
         TestCase (3, -2, 1, 2, 7)]
        public void ColorSelectionInTextTest (
            int selectionStart, int selectionLength, int firstLength, int? secondLength, int? thirdLength)
        {
            try
            {
                var sUt = new Mtb ();
                var s = new string ('\n', firstLength + (secondLength ?? 0) + (thirdLength ?? 0));
                sUt.SetText (s);
                sUt.SetSelection (selectionStart, selectionLength);
                sUt.ColorSelectionInText ();
                var ranges = Mtb.GetLineRanges (sUt.Text);
                Assert.True (ranges.Count > 0);
                Assert.AreEqual (firstLength, ranges [0].Count ());
                if (secondLength != null)
                {
                    Assert.True (ranges.Count > 1);
                    Assert.AreEqual (secondLength.Value, ranges [1].Count ());
                    if (thirdLength != null)
                    {
                        Assert.AreEqual (3, ranges.Count);
                        Assert.AreEqual (thirdLength.Value, ranges [2].Count ());
                    }
                    else
                        Assert.AreEqual (2, ranges.Count);
                }
                else
                    Assert.AreEqual (1, ranges.Count);
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [Test]
        public void InitializeComponent_Always_ControlsAreValid ()
        {
            _systemUnderTest.InitializeComponent ();
            foreach (var control in _systemUnderTest.Controls)
                Assert.AreNotEqual (null, control);
        }

        [Test]
        public void GetLineRangesTest ()
        {
            try
            {
                _systemUnderTest.Text = new ColoredString (new List<ColoredCharacter>
                {
                    new ColoredCharacter (Color.AliceBlue, Color.AntiqueWhite, ' '),
                    new ColoredCharacter (Color.AliceBlue, Color.AntiqueWhite, '_'),

                    new ColoredCharacter (Color.AliceBlue, Color.AliceBlue, ' '),
                    new ColoredCharacter (Color.Aqua, Color.AliceBlue, ' '),

                    new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, '_'),
                    new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, ' '),
                    new ColoredCharacter (Color.AntiqueWhite, Color.AliceBlue, ' '),
                });
                var count = _systemUnderTest.Text.Count ();
                var subString = _systemUnderTest.Text.Substring (0, count - 1);
                var ranges = Mtb.GetLineRanges (subString);
                Assert.AreEqual (3, ranges.Count);
                Assert.AreEqual (2, ranges [0].Count ());
                Assert.AreEqual (2, ranges [1].Count ());
                Assert.AreEqual (3, ranges [2].Count ());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail (e.Message);
            }
        }

        [Test]
        public void PerformShortcutTest ()
        {
            try
            {
                Application.EnableVisualStyles ();
                Clipboard.Clear ();
                var sUt = new MetaTextBox ();
                var startText = sUt.Text.ToString ();
                sUt.SetText ("Hallo");
                Assert.AreEqual ("Hallo\n", sUt.Text.ToString ());
                sUt.PerformShortcut (Keys.Z, new KeyEventArgs (Keys.Control));
                Assert.AreEqual (startText, sUt.Text.ToString ());
                sUt.PerformShortcut (Keys.Y, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("Hallo\n", sUt.Text.ToString ());
                sUt.SetCursorIndex ("Hallo".Length);
                sUt.PerformInput (Keys.A, new KeyEventArgs (Keys.None));
                Assert.AreEqual ("Halloa\n", sUt.Text.ToString ());
                sUt.PerformShortcut (Keys.Z, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("Hallo\n", sUt.Text.ToString ());
                sUt.SetCursorIndex ("Hallo".Length);
                sUt.PerformInput (Keys.B, new KeyEventArgs (Keys.None));
                Assert.AreEqual ("Hallob\n", sUt.Text.ToString ());
                sUt.PerformShortcut (Keys.Y, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("Hallob\n", sUt.Text.ToString ());
                sUt.SetSelection (1, 2);
                Assert.AreEqual ("al", sUt.GetSelectedText ());
                sUt.PerformShortcut (Keys.C, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("al", Clipboard.GetText ());
                Assert.AreEqual ("Hallob\n", sUt.Text.ToString ());
                sUt.SetSelection (2, 2);
                Assert.AreEqual ("ll", sUt.GetSelectedText ());
                sUt.PerformShortcut (Keys.X, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("Haob\n", sUt.Text.ToString ());
                Assert.AreEqual ("ll", Clipboard.GetText ());
                Assert.AreEqual(2, sUt.CursorIndex);
                sUt.SetSelection (0, 2);
                Assert.AreEqual ("Ha", sUt.GetSelectedText ());
                Assert.AreEqual (0, sUt.CursorIndex);
                sUt.PerformShortcut (Keys.V, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("llob\n", sUt.Text.ToString ());
                Assert.AreEqual (2, sUt.CursorIndex);
                Assert.AreEqual (0, sUt.SelectionLength); 
                sUt.SetSelection (1, 2);
                Assert.AreEqual (1, sUt.CursorIndex);
                Assert.AreEqual (2, sUt.SelectionLength);
                sUt.PerformShortcut (Keys.Z, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("Haob\n", sUt.Text.ToString ());
                Assert.AreEqual (2, sUt.CursorIndex);
                Assert.AreEqual (0, sUt.SelectionLength);
                sUt.PerformShortcut (Keys.Y, new KeyEventArgs (Keys.Control));
                Assert.AreEqual ("llob\n", sUt.Text.ToString ());
                Assert.AreEqual (2, sUt.CursorIndex);
                Assert.AreEqual (0, sUt.SelectionLength);
                sUt.PerformShortcut (Keys.Z, new KeyEventArgs (Keys.None));
                Assert.AreEqual ("llob\n", sUt.Text.ToString ());
                Assert.AreEqual (2, sUt.CursorIndex);
                Assert.AreEqual (0, sUt.SelectionLength);
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