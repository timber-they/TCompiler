using System;
using System.Linq;
using System.Windows.Forms;

using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class MetaTextBoxTests
    {
        private readonly MetaTextBox.MetaTextBox SystemUnderTest = new MetaTextBox.MetaTextBox ();

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
            Assert.AreEqual (7, SystemUnderTest.GetCharacterWidth ());
        }

        [TestCase (""), TestCase (" "), TestCase ("foo")]
        public void GetStringWidth_Always_ReturnsExpectedValue (string testString)
        {
            Assert.AreEqual (testString.Length * SystemUnderTest.GetCharacterWidth (),
                             SystemUnderTest.GetStringWidth (testString));
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
                             SystemUnderTest.PerformInput (keys.First (), new KeyEventArgs ((Keys) keysValue)));
        }

        [Test]
        public void InitializeComponent_Always_ControlsAreValid ()
        {
            SystemUnderTest.InitializeComponent();
            foreach (var control in SystemUnderTest.Controls)
                Assert.AreNotEqual (null, control);
        }
    }
}