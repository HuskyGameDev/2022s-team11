using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Level {
    /**
    *   Author: Nick Zimanski
    *   Version: 10/31/22
    */
    [CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPC")]
    public class NPCData : ScriptableObject
    {   
        #region Serialized Fields
        [SerializeField]
        private CharacterName _character;

        [Header("Dialogue")]
        [SerializeField]
        private int[] _randomConversations;

        [SerializeField]
        private int[] _orderedConversations;
    
        [Header("Visuals")]
        [SerializeField]
        private Sprite _sprite;
        public Sprite Sprite => _sprite;

        [Header("Audio")]
        [SerializeField]
        private int[] _dialogueSounds;
        #endregion

        #region Private Fields
        private System.Random _rand;
        private int _orderedConversationIndex;
        private static Dictionary<int, string> s_nameDict = new Dictionary<int, string>(){
            {0, "Gr4PP-L"},
            {1, "E-CL4Ir3"},
            {2, "NAVI"},
            {3, "H4rD-HT"},
            {4, "BL3ND3r"},
            {5, "Gun"},
            {6, "M1KE"},
            {7, "D3L-V"},
            {8, "Tr4NS-P"},
            {9, "A1r-Dr0P"},
            {10, "S3C-T"},
        };
        #endregion

        public void OnEnable() {
            _rand = new System.Random();
        }

        public int RandomConversation() {
            //TODO: Only show conversations the player hasn't seen, if there are any they haven't seen
            return _randomConversations[_rand.Next(0, _randomConversations.Length)];
        }

        public int NextOrderedConversation() {
            if (!HasMoreOrderedConversations()) {
                return -1;
            }
            int temp = _orderedConversations[_orderedConversationIndex];
            _orderedConversationIndex++;
            return temp;
        }

        public bool HasMoreOrderedConversations() {
            return _orderedConversationIndex >= _orderedConversations.Length;
        }

        public string Name => GetNameFromCharacter(_character);

        public enum CharacterName : int {
            GR4PP_L = 0,
            E_CL4IR3 = 1,
            NAVI = 2,
            H4RD_HT = 3,
            BL3ND3R = 4,
            GUN = 5,
            M1KE = 6,
            D3L_V = 7,
            TR4NS_P = 8,
            A1R_DR0P = 9,
            S3C_T = 10
        }

        public static string GetNameFromCharacter(CharacterName cha) {
            return s_nameDict[(int) cha];
        } 
    }
}