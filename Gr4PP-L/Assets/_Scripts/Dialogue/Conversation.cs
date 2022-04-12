using System.Collections.Generic;
using UnityEngine;
using _Scripts.Utility;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace _Scripts.Dialogue {
    [CreateAssetMenu(fileName = "ConversationData", menuName = "ScriptableObjects/Conversation/ConversationScriptableObject")]
    public class Conversation : ScriptableObject
    {
        private List<DialogueBeat> _beats = new List<DialogueBeat>();
        private bool _showBeats;
        private string _name = "Conversation Name";
        private List<NPC.Character> _characters => GetCharactersInConversation();

        private List<NPC.Character> GetCharactersInConversation() {
            List<NPC.Character> c = new List<NPC.Character>();
            for (int i = 0; i < _beats.Count; i++) {
                if (!c.Contains(_beats[i].SpeakingCharacter)) {
                    c.Add(_beats[i].SpeakingCharacter);
                }
            }
            return c;
        }

        #region Editor
        #if UNITY_EDITOR
        [CustomEditor(typeof(Conversation))]
        public class ConversationEditor: Editor {
            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                Conversation convo = (Conversation)target;

                convo._name = EditorGUILayout.TextField("Conversation Name", convo._name);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Characters in this convo: ", GUILayout.MaxWidth(200));
                for (int i = 0; i < convo._characters.Count; i++) {
                    GUILayout.Label(convo._characters[i] + ", ", GUILayout.MaxWidth(100));
                }
                GUILayout.EndHorizontal();

                convo._showBeats = EditorGUILayout.Foldout(convo._showBeats, "Dialogue Beats", true);
                EditorGUI.indentLevel ++;
                if (convo._showBeats) {
                    List<DialogueBeat> beats = convo._beats;
                    int size = beats.Count;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Size: " + size);
                    if (GUILayout.Button(" + ")) {
                        size++;
                    }
                    if (GUILayout.Button(" - ")) {
                        size--;
                    }
                    GUILayout.EndHorizontal();
                    EditorGUI.indentLevel ++;

                    while (size > beats.Count) { beats.Add(new DialogueBeat()); }
                    while (size < beats.Count) { beats.RemoveAt(beats.Count - 1); }
                    
                    for (int i = 0; i < beats.Count; i++) {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(28);
                        GUILayout.Label("Beat " + i);
                        GUILayout.EndHorizontal();

                        beats[i] = DrawDialogueBeat(beats[i]);

                        GUILayout.Space(28);
                    }
                }
            }

            private DialogueBeat DrawDialogueBeat(DialogueBeat beat) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(28);
                GUILayout.Label("Type");
                beat.Type = (BeatType) EditorGUILayout.EnumPopup(beat.Type);
                GUILayout.Label("Speaking Character");
                beat.SpeakingCharacter = (NPC.Character) EditorGUILayout.EnumPopup(beat.SpeakingCharacter);
                GUILayout.Label("Mood");
                beat.Mood = (NPC.Mood) EditorGUILayout.EnumPopup(beat.Mood);
                EditorGUILayout.EndHorizontal();
        
                beat.DialogueSound = (AudioClip) EditorGUILayout.ObjectField("Sound File", beat.DialogueSound, typeof(AudioClip), false);

                if (beat.Type == BeatType.TEXT) {
                    EditorGUILayout.LabelField("Text Body:");
                    beat.Text = EditorGUILayout.TextArea(beat.Text, GUILayout.MinHeight(50));
                }

                return beat;
            }
        }
        #endif
        #endregion
    }
}
