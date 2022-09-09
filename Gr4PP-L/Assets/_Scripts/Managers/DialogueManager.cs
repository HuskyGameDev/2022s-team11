
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers {
    /** Author: Nick Zimanski
    * Version: 9/09/22
    */
    public class DialogueManager : Manager
    {
        ConversationCollection conversationCollection;
        void Awake()
        {
            conversationCollection = ImportJson<ConversationCollection>("Json/conversations");
            conversationCollection.InitializeConversations();

            
            ConversationIterator ie = (ConversationIterator) conversationCollection.ConversationDictionary[0].GetEnumerator();
            int g = 0;
            do {
                g++;
                Debug.Log(ie.Current.type + " " + ie.Current.data);
            } while (ie.MoveNext() && g < 1000);
        }

        public UsableConversation GetConversationByID (int id) {
            if (!conversationCollection.ConversationDictionary.ContainsKey(id)) return null;
            return conversationCollection.ConversationDictionary.GetValueOrDefault(id);
        }

        private static T ImportJson<T>(string path)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(path);

            Debug.Log(textAsset);
            return JsonUtility.FromJson<T>(textAsset.text);
        }

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
        public class UsableConversation : IEnumerable<ConvInstruction> {
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
                    Regex characterChangeMatch = new Regex("\\[(?<=\\[).*(?=\\])\\]");
                    Regex characterChangeNameGrab = new Regex("(?<=\\[).*(?=\\])");
                    Regex instructionMatch = new Regex("{(?<={).*(?=})}");
                    Regex instructionGrabContent = new Regex("(?<={).*(?=})");
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
        public class ConversationIterator : IEnumerator<ConvInstruction> {
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
