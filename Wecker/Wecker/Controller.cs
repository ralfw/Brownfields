using System;
using System.Threading;

namespace Wecker
{
    public class Controller
    {
        private Timer timer;
        private DateTime endeZeit;

        public void Starten(DateTime endeZeit) {
            this.endeZeit = endeZeit;
            timer = new Timer(state => TimerTick());
            timer.Change(1000, 1000);
        }

        private void TimerTick() {
            ZeitAnzeigen(endeZeit);
            if (DateTime.Now >= endeZeit) {
                new Sound().AlarmStarten();
                timer.Dispose();
            }
        }

        public event Action<DateTime> ZeitAnzeigen;
    }
}