using UnityEngine;
namespace _Scripts.Dialogue {    
    [System.Serializable]
    public class DialogueBeat : UnityEngine.Object
    {
        [SerializeField]
        public BeatType Type;
        [SerializeField]
        public int Track;
        [SerializeField]
        public NPC.Mood Mood;
        [SerializeField]
        public NPC.Character SpeakingCharacter;
        [SerializeField]
        public UnityEngine.AudioClip DialogueSound;

        [SerializeField]
        public string Text;

        public DialogueBeat() {
            DialogueSound = null;
            Track = 0;
            Mood = NPC.Mood.HAPPY;
            SpeakingCharacter = NPC.Character.GR4PP_L;
            Text = "";
            Type = BeatType.TEXT;
        }

    }

    public enum BeatType {
        TEXT,
        CHOICE
    }
}