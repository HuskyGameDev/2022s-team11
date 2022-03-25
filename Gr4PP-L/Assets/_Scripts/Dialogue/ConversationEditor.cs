using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace _Scripts.Dialogue
{
    [CustomEditor(typeof(DialogueBeat))]
    public class DialogueEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var type = serializedObject.FindProperty("Type");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Type"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("BeatID"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Track"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("SpeakingCharacter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Mood"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DialogueSound"));


            switch (type.intValue) {
                case (int) BeatType.TEXT:
                    //EditorGUILayout.PropertyField(serializedObject.FindProperty("Text"));
                    break;
                case (int) BeatType.CHOICE:
                    break;
                default:
                    break;
            } 
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}