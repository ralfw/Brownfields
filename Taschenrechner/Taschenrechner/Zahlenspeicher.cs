namespace Taschenrechner
{
    class Zahlenspeicher
    {
        private int _zahl;
        private bool _neueZahlBeginnen = true;


        public int Ziffer_zu_Zahl_hinzufügen(string ziffer)
        {
            if (_neueZahlBeginnen)
                _zahl = int.Parse(ziffer);
            else
            {
                _zahl = 10*_zahl + int.Parse(ziffer);
            }
            _neueZahlBeginnen = false;

            return _zahl;
        }


        public void Reset()
        {
            _neueZahlBeginnen = true;
        }
    }
}