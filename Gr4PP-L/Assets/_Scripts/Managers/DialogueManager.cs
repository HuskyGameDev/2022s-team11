
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace _Scripts.Managers {
    /** Author: Nick Zimanski
    * Version: 9/10/22
    */
    public class DialogueManager : Manager
    {
        private ConversationCollection _allConversations;
        private UsableConversation _currentConversation = null;
        private bool _awaitingPlayerInput = false;
        private bool _conversationInProgress = false;
        private ConversationIterator _currentConversationIterator = null;
        private int _text_charactersTyped = 0;
        private float _text_lastCharacterTypedTime = 0;
        private string _text_currentString = "";
        private bool _text_incomingNewText = false;
        private bool _delay_hasStarted = false;
        private float _delay_endTime = 0;
        [SerializeField]
        private float _charsPerSecond = 7f;
        [SerializeField]
        private Canvas _dialogueCanvas;
        [SerializeField]
        private TextMeshProUGUI _dialogueText;
        [SerializeField]
        private TextMeshProUGUI _characterNameText;

        private float _text_secondsPerChar { get => 1f / _charsPerSecond; }
        void Awake()
        {
            _allConversations = ImportJson<ConversationCollection>("Json/conversations");
            _allConversations.InitializeConversations();
        }

        private void Start()
        {
        }

        void Update()
        {
            if (_currentConversation == null) return;

            UpdateConversation();

            //TODO Actually make the conversation display
        }


        
        public bool RunConversation(int id) {
            _currentConversation = GetConversationByID(id);
            if (_currentConversation == null) return false;

            _currentConversationIterator = (ConversationIterator) _currentConversation.GetEnumerator();
            if (_currentConversationIterator.Current.data == null) {
                _currentConversation = null;
                return false;
            }

            _dialogueCanvas.enabled = true;

            //Reset conversation-specific state
            _text_charactersTyped = 0;
            _text_lastCharacterTypedTime = 0;
            _delay_hasStarted = false;
            _delay_endTime = 0;

            return true;
        }

        private void UpdateConversation() {
            Debug.Log(_currentConversationIterator.Current.type);
            switch (_currentConversationIterator.Current.type) {
                case ConvInstruction.InstructionType.TEXT:
                    UpdateText();
                    break;
                case ConvInstruction.InstructionType.DELAY:
                    UpdateDelay();
                    break;
                case ConvInstruction.InstructionType.CHARACTER_CHANGE:
                    UpdateCharacterChange();
                    break;
                default:
                    break;
            }

            if (!_awaitingPlayerInput) return;
            //Input

            if (Input.GetAxisRaw("Submit") != 0) {
                NextConversationInstruction();
                _awaitingPlayerInput = false;
            }

        }

        private void UpdateText() {
            //Instantiate the current string
            if (_text_incomingNewText) {
                _text_currentString = _text_currentString + _currentConversationIterator.Current.data;
                _text_incomingNewText = false;
            }

            //Check if we've reached the end of the string
            if (_text_charactersTyped >= _text_currentString.Length) { 
                _awaitingPlayerInput = true;

                //End the text instruction automatically if the next up is a delay.
                if (_currentConversationIterator.IsNextOfType(ConvInstruction.InstructionType.DELAY)) NextConversationInstruction();
                return;
            }

            if (_text_lastCharacterTypedTime + _text_secondsPerChar > Time.time) return;

            _text_lastCharacterTypedTime = Time.time;
            _text_charactersTyped++;

            _dialogueText.text = _text_currentString.Substring(0, _text_charactersTyped);
            
        }

        private void UpdateDelay() {
            if (!_delay_hasStarted) {
                _delay_endTime = Time.time + (float) (Int32.Parse(_currentConversationIterator.Current.data)/1000f);
                _awaitingPlayerInput = true;
                _delay_hasStarted = true;
                return;
            }

            if (_delay_endTime > Time.time) return;

            _delay_endTime = 0;
            _delay_hasStarted = false;

            //Conversations shouldn't end on a delay
            NextConversationInstruction();
            
        }

        private void UpdateCharacterChange() {
            //If the character actually changes. Empty square brackets are treated as a passage change and will wipe the text box.
            if (_currentConversationIterator.Current.data != "")    {
                _characterNameText.SetText(_currentConversationIterator.Current.data);

                //TODO: set character portrait
                //TODO: ready character voice

            }

            _dialogueText.text = "";
            _text_currentString = "";
            _text_charactersTyped = 0;
            _text_lastCharacterTypedTime = 0;

            //Conversations shouldn't end on a character change
            NextConversationInstruction();
        }

        private void EndConversation() {
            _dialogueCanvas.enabled = false;
            _dialogueText.text = "";
            _characterNameText.text = "";
            _delay_endTime = 0;
            _delay_hasStarted = false;
            _text_currentString = "";
            _text_charactersTyped = 0;
            _text_lastCharacterTypedTime = 0;
        }

        private void NextConversationInstruction() {
            if (!_currentConversationIterator.MoveNext()) {
                _currentConversation = null;
                EndConversation();
            }

            if (_currentConversationIterator.Current.type == ConvInstruction.InstructionType.TEXT)
                _text_incomingNewText = true;
        }

        private UsableConversation GetConversationByID (int id) {
            if (!_allConversations.ConversationDictionary.ContainsKey(id)) return null;
            return _allConversations.ConversationDictionary.GetValueOrDefault(id);
        }

        private static T ImportJson<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);

            Debug.Log(textAsset);
            return JsonUtility.FromJson<T>(textAsset.text);
        }


        /*
        *
        *   Other classes
        *
        */

        /// <summary>
        /// A utility class for converting raw json data into conversations. Only for use in DialogueManager
        /// </summary>
        [SerializableAttribute]
        private class ConversationCollection {
            public RawConversation[] raw_conversations;
            /// <summary>
            /// Holds all usable conversations, indexed by their id. This is what you should use to get your conversation to play.
            /// </summary>
            public Dictionary<int, UsableConversation> ConversationDictionary;

            public void InitializeConversations() {
                ConversationDictionary = new Dictionary<int, UsableConversation>();
                foreach (RawConversation rc in raw_conversations) {
                    ConversationDictionary.Add(rc.id, new UsableConversation(rc));
                }
            }

        }
        [Serializable]
        public struct RawConversation {
            public int id;
            public string text;
            public string name;
        }

        /// <summary>
        /// Higher-level data storage for individual conversation data
        /// </summary>
        private class UsableConversation : IEnumerable<ConvInstruction> {
                private int id;
                private string name;
                public string[] characters;
                public LinkedList<ConvInstruction> conversationSteps;
                public UsableConversation(RawConversation rc) {
                    this.id = rc.id;
                    this.name = rc.name;
                    conversationSteps = ParseTextIntoSteps(rc.text); 
                }

                /// <summary>
                /// Parses all of the text in the json data for a conversation into discrete instruction for display
                /// </summary>
                /// <param name="s">the raw string from the json file</param>
                /// <returns>the formatted steps</returns>
                private LinkedList<ConvInstruction> ParseTextIntoSteps(string s) {
                    Regex characterChangeMatch = new Regex("\\[(?<=\\[)[^\\[\\]]*(?=\\])\\]");
                    Regex characterChangeNameGrab = new Regex("(?<=\\[)[^\\[\\]]*(?=\\])");
                    Regex instructionMatch = new Regex("{(?<={)[^{}]*(?=})}");
                    Regex instructionGrabContent = new Regex("(?<={)[^{}]*(?=})");
                    LinkedList<ConvInstruction> list = new LinkedList<ConvInstruction>();
                    string temp;

                    while (s.Length > 0) {
                        s = s.Trim();
                        /*
                        *character switch
                        */
                        if (s.StartsWith("[")) {
                            temp = characterChangeMatch.Match(s).Value;
                            s = s.Remove(0, temp.Length);
                            temp = characterChangeNameGrab.Match(temp).Value;

                            if (temp == null) temp = "";
                            list.AddLast(new LinkedListNode<ConvInstruction>(new ConvInstruction(ConvInstruction.InstructionType.CHARACTER_CHANGE, temp)));
                            temp = "";
                            continue;
                        }

                        /*
                        *instruction
                        */
                        if (s.StartsWith("{")) {
                            temp = instructionMatch.Match(s).ToString();
                            s = s.Remove(0, temp.Length);
                            temp = instructionGrabContent.Match(temp).ToString();
                            string[] args = temp.Split("=");

                            switch(args[0].ToLower()) {
                                case "delay":
                                    list.AddLast(new LinkedListNode<ConvInstruction>(new ConvInstruction(ConvInstruction.InstructionType.DELAY, args[1])));
                                    break;
                                default:
                                    break;
                            }
                            temp = "";
                            continue;
                        }

                        /*
                        *text
                        */
                        if (s.IndexOfAny(new char[]{'[','{'}) != -1) {
                            //There are commands after this text, cut up to them
                            temp = s.Substring(0, s.IndexOfAny(new char[]{'[','{'}));
                            s = s.Remove(0, s.IndexOfAny(new char[]{'[','{'}));
                        }
                        else {
                            //there are no more commands after this text, end the parse
                            temp = s.Substring(0);
                            s = "";
                        }

                        list.AddLast(new LinkedListNode<ConvInstruction>(new ConvInstruction(ConvInstruction.InstructionType.TEXT, temp)));
                        temp = "";
                    }
                    return list;
                }

                public IEnumerator<ConvInstruction> GetEnumerator() {
                    return new ConversationIterator(conversationSteps);
                }

                IEnumerator IEnumerable.GetEnumerator() {
                    return GetEnumerator();
                }

            }

        /// <summary>
        /// Custom iterator for UsableConversation
        /// </summary>
        private class ConversationIterator : IEnumerator<ConvInstruction> {
            private LinkedList<ConvInstruction> _list;
            private LinkedListNode<ConvInstruction> _current;
            public ConversationIterator(LinkedList<ConvInstruction> ll) {
                _list = ll;
                _current = _list.First;
            }

            public bool MoveNext() {
                if (_current == null || _current.Next == null) 
                    return false;

                _current = _current.Next;
                return true;
            }

            public void Reset()
            {
                _current = _list.First;
            }

            void IDisposable.Dispose() {}

            public ConvInstruction Current {
                get {return _current.Value;}
            }

            public bool IsNextOfType(ConvInstruction.InstructionType type) {
                if (_current.Next == null) return false;
                if (_current.Next.Value.type != type) return false;
                return true;
            }

            object IEnumerator.Current {
                get {return Current;}
            }
        }

        public struct ConvInstruction {
            public ConvInstruction(InstructionType t, string d) {
                type = t;
                data = d;
            }
            public enum InstructionType {
                TEXT,
                DELAY,
                CHARACTER_CHANGE
            }

            public InstructionType type;

            public string data;
        }

    }

    
}
