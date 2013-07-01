using System.Media;

namespace Wecker
{
    public class Sound
    {
        public void AlarmStarten() {
            using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\chimes.wav")) {
                soundPlayer.Play();
            }
        }
    }
}