using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    /** Author: Nick Zimanski
    * Version: 9/16/22
    */
    public class DialogueManager : Manager
    {
        private static Regex _regexCharacterChangeMatch = new Regex("\\[(?<=\\[)[^\\[\\]]*(?=\\])\\]");
        private static Regex _regexCharacterChangeNameGrab = new Regex("(?<=\\[)[^\\[\\]]*(?=\\])");
        private static Regex _regexInstructionMatch = new Regex("{(?<={)[^{}]*(?=})}");
        private static Regex _regexInstructionGrabContent = new Regex("(?<={)[^{}]*(?=})");
        private ConversationCollection _allConversations;
        public UsableConversation CurrentConversation = null;
        public ConvInstruction CurrentConvInstruction;
        public ConversationIterator CurrentConversationIterator;
        [SerializeField]
        private float _charsPerSecond = 10f;

        [Tooltip("How long between blinking the '█' character at the end of the text")]
        public float FullBlockBlinkInterval = 0.5f;

        public float Text_secondsPerChar { get => 1f / _charsPerSecond; }

        [HideInInspector]
        public bool Text_incomingNewText;

        private GameManager _gm;


        public bool IsConversationActive()
        {
            return !(CurrentConversation == null);
        }


        private UsableConversation GetConversationByID(int id)
        {
            if (!_allConversations.Contains(id)) return null;
            return _allConversations.GetConversation(id);
        }

        private static T ImportJson<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);
            return JsonUtility.FromJson<T>(textAsset.text);
        }

        public bool RunConversation(int id)
        {
            //We have a conversation running already
            if (IsConversationActive()) return false;
            //The requested conversation doesn't exist
            if (!_allConversations.Contains(id)) return false;

            CurrentConversation = GetConversationByID(id);

            CurrentConversationIterator = (ConversationIterator)CurrentConversation.GetEnumerator();
            if (CurrentConversationIterator.Current.data == null)
            {
                CurrentConversation = null;
                return false;
            }

            _gm.Get<TimerManager>().Pause();
            _gm.Get<Managers.InputManager>().LockInputType("player");

            return true;
        }

        public void NextConversationInstruction()
        {
            if (!CurrentConversationIterator.MoveNext())
            {
                EndConversation();
                return;
            }

            if (CurrentConversationIterator.Current.type == ConvInstruction.InstructionType.TEXT)
                Text_incomingNewText = true;
        }

        public void EndConversation()
        {
            Text_incomingNewText = false;

            CurrentConversation = null;

            _gm.Get<TimerManager>().Resume();
            _gm.Get<Managers.InputManager>().UnlockInputType("player");
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
        private class ConversationCollection
        {
            public RawConversation[] raw_conversations;
            /// <summary>
            /// Holds all usable conversations, indexed by their id. This is what you should use to get your conversation to play.
            /// </summary>
            private Dictionary<int, UsableConversation> _conversationDictionary;

            public void InitializeConversations()
            {
                _conversationDictionary = new Dictionary<int, UsableConversation>();
                foreach (RawConversation rc in raw_conversations)
                {
                    _conversationDictionary.Add(rc.id, new UsableConversation(rc));
                }
            }

            public UsableConversation GetConversation(int id)
            {
                if (!_conversationDictionary.ContainsKey(id)) return null;

                return _conversationDictionary[id];
            }

            public bool Contains(int id)
            {
                return _conversationDictionary.ContainsKey(id);
            }

        }
        [Serializable]
        public struct RawConversation
        {
            public int id;
            public string text;
            public string name;
        }

        /// <summary>
        /// Higher-level data storage for individual conversation data
        /// </summary>
        public class UsableConversation : IEnumerable<ConvInstruction>
        {
            private int id;
            private string name;
            public string[] characters;
            public LinkedList<ConvInstruction> conversationSteps;
            public UsableConversation(RawConversation rc)
            {
                this.id = rc.id;
                this.name = rc.name;
                conversationSteps = ParseTextIntoSteps(rc.text);
            }

            /// <summary>
            /// Parses all of the text in the json data for a conversation into discrete instruction for display
            /// </summary>
            /// <param name="s">the raw string from the json file</param>
            /// <returns>the formatted steps</returns>
            private LinkedList<ConvInstruction> ParseTextIntoSteps(string s)
            {
                LinkedList<ConvInstruction> list = new LinkedList<ConvInstruction>();
                string temp;

                while (s.Length > 0)
                {
                    //s = s.Trim();
                    /*
                    *character switch
                    */
                    if (s.StartsWith("["))
                    {
                        temp = _regexCharacterChangeMatch.Match(s).Value;
                        s = s.Remove(0, temp.Length);
                        temp = _regexCharacterChangeNameGrab.Match(temp).Value;

                        if (temp == null) temp = "";
                        list.AddLast(new LinkedListNode<ConvInstruction>(new ConvInstruction(ConvInstruction.InstructionType.CHARACTER_CHANGE, temp)));
                        temp = "";
                        continue;
                    }

                    /*
                    *instruction
                    */
                    if (s.StartsWith("{"))
                    {
                        temp = _regexInstructionMatch.Match(s).ToString();
                        s = s.Remove(0, temp.Length);
                        temp = _regexInstructionGrabContent.Match(temp).ToString();
                        string[] args = temp.Split("=");

                        switch (args[0].ToLower())
                        {
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
                    if (s.IndexOfAny(new char[] { '[', '{' }) != -1)
                    {
                        //There are commands after this text, cut up to them
                        temp = s.Substring(0, s.IndexOfAny(new char[] { '[', '{' }));
                        s = s.Remove(0, s.IndexOfAny(new char[] { '[', '{' }));
                    }
                    else
                    {
                        //there are no more commands after this text, end the parse
                        temp = s.Substring(0);
                        s = "";
                    }

                    list.AddLast(new LinkedListNode<ConvInstruction>(new ConvInstruction(ConvInstruction.InstructionType.TEXT, temp)));
                    temp = "";
                }
                return list;
            }

            public IEnumerator<ConvInstruction> GetEnumerator()
            {
                return new ConversationIterator(conversationSteps);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

        }

        /// <summary>
        /// Custom iterator for UsableConversation
        /// </summary>
        public class ConversationIterator : IEnumerator<ConvInstruction>
        {
            private LinkedList<ConvInstruction> _list;
            private LinkedListNode<ConvInstruction> _current;
            public ConversationIterator(LinkedList<ConvInstruction> ll)
            {
                _list = ll;
                _current = _list.First;
            }

            public bool MoveNext()
            {
                if (_current == null || _current.Next == null)
                    return false;

                _current = _current.Next;
                return true;
            }

            public void Reset()
            {
                _current = _list.First;
            }

            void IDisposable.Dispose() { }

            public ConvInstruction Current
            {
                get { return _current.Value; }
            }

            public bool IsNextOfType(ConvInstruction.InstructionType type)
            {
                if (_current.Next == null) return false;
                if (_current.Next.Value.type != type) return false;
                return true;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public struct ConvInstruction
        {
            public ConvInstruction(InstructionType t, string d)
            {
                type = t;
                data = d;
            }
            public enum InstructionType
            {
                TEXT,
                DELAY,
                CHARACTER_CHANGE
            }

            public InstructionType type;

            public string data;
        }

        public new void Initialize()
        {
            base.Initialize();

            _gm = GameManager.Instance;

            _allConversations = ImportJson<ConversationCollection>("Json/conversations");
            _allConversations.InitializeConversations();

            _charsPerSecond = _gm.Parameters.charsPerSecond;
        }

        public DialogueManager()
        {
            Initialize();
        }

        public override Manager GetNewInstance()
        {
            return new DialogueManager();
        }

        public override void Destroy()
        {

        }
    }


}
