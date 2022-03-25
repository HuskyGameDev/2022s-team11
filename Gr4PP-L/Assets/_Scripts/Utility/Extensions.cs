using _Scripts.Dialogue;
namespace _Scripts.Utility
{
    public static class Extensions
    {
        public static void SortByBeatId(this DialogueBeat[] beats) {
            DialogueBeat[] newBeats = new DialogueBeat[beats.Length];
            int beatsSorted = 0;
            int trackToFind = 0;
            int beatToFind = 0;
            while (beatsSorted < beats.Length) {
                for (var i = 0; i < beats.Length; i++) {
                    if(beats[i].Track == trackToFind && beats[i].BeatId == beatToFind) {
                        newBeats[beatsSorted] = beats[i];
                        beatsSorted++;
                        trackToFind++;
                        i = 0;
                    }
                }
                beatToFind++;
                trackToFind = 0;
            }
            beats = newBeats;
        }
    }
}