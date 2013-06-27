using NUnit.Framework;

namespace Taschenrechner
{
    [TestFixture]
    public class test_Zahlenspeicher
    {
        [Test]
        public void Ziffern_hinzufügen()
        {
            var sut = new Zahlenspeicher();

            Assert.AreEqual(1, sut.Ziffer_zu_Zahl_hinzufügen("1"));
            Assert.AreEqual(12, sut.Ziffer_zu_Zahl_hinzufügen("2"));
            Assert.AreEqual(123, sut.Ziffer_zu_Zahl_hinzufügen("3"));
        }


        [Test]
        public void Neubeginn_nach_Reset()
        {
            var sut = new Zahlenspeicher();
            sut.Ziffer_zu_Zahl_hinzufügen("1");
            sut.Ziffer_zu_Zahl_hinzufügen("2");

            sut.Reset();

            Assert.AreEqual(4, sut.Ziffer_zu_Zahl_hinzufügen("4"));
        }
    }
}