using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using MetaTextBox;

using NUnit.Framework;


// ReSharper disable ObjectCreationAsStatement


namespace Tests
{
    [TestFixture]
    public class MetaTextBoxTests
    {
        private readonly MetaTextBox.MetaTextBox _systemUnderTest = new MetaTextBox.MetaTextBox ();

        [Test]
        public void MetaTextBox_Always_ThrowsNoException ()
        {
            try
            {
                new MetaTextBox.MetaTextBox ();
            }
            catch (Exception)
            {
                Assert.Fail ();
            }
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

        [Test]
        public void InitializeComponent_Always_ControlsAreValid ()
        {
            _systemUnderTest.InitializeComponent();
            foreach (var control in _systemUnderTest.Controls)
                Assert.AreNotEqual (null, control);
        }

        [Test]
        public void GetLineRangesTest ()
        {
            try
            {
                _systemUnderTest.Text = new ColoredString(new List<ColoredCharacter>
                {
                    new ColoredCharacter(Color.AliceBlue, Color.AntiqueWhite, ' '),
                    new ColoredCharacter(Color.AliceBlue, Color.AntiqueWhite, '_'),
                    
                    new ColoredCharacter(Color.AliceBlue, Color.AliceBlue, ' '),
                    new ColoredCharacter(Color.Aqua, Color.AliceBlue, ' '),
                    
                    new ColoredCharacter(Color.AntiqueWhite, Color.AliceBlue, '_'),
                    new ColoredCharacter(Color.AntiqueWhite, Color.AliceBlue, ' '),
                    new ColoredCharacter(Color.AntiqueWhite, Color.AliceBlue, ' '),
                });
                var count = _systemUnderTest.Text.Count ();
                var subString = _systemUnderTest.Text.Substring (0, count - 1);
                var ranges = MetaTextBox.MetaTextBox.GetLineRanges (subString);
                Assert.AreEqual(3, ranges.Count);
                Assert.AreEqual(2, ranges[0].Count());
                Assert.AreEqual(2, ranges[1].Count());
                Assert.AreEqual(3, ranges[2].Count());
            }
            catch (Exception e)
            {
                if (e is AssertionException)
                    throw;
                Assert.Fail(e.Message);
            }
        }
    }
}