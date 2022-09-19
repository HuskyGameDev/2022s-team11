using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Managers;
using static Managers.DialogueManager;

public class DialogueController : MonoBehaviour
{

    [SerializeField]
    private Canvas _dialogueCanvas;
    [SerializeField]
    private TextMeshProUGUI _dialogueText;
    [SerializeField]
    private TextMeshProUGUI _characterNameText;

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
    private GameManager _gm;
    private DialogueManager _dm;
    // Start is called before the first frame update
    void Start()
    {
        _dialogueCanvas = this.gameObject.GetComponent<Canvas>();
        _dialogueCanvas.enabled = false;

        _gm = GameManager.Instance;
        _dm = _gm.dialogueManager;

    }

    // Update is called once per frame
    void Update()
    {
        if (_currentConversation == null) return;

        UpdateConversation();
        //TODO: Store active conversation in the manager.
    }

    private bool RunConversation(int id) {
            _currentConversation = _dm.GetConversationByID(id);
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

        private void NextConversationInstruction() {
            if (!_currentConversationIterator.MoveNext()) {
                _currentConversation = null;
                EndConversation();
            }

            if (_currentConversationIterator.Current.type == ConvInstruction.InstructionType.TEXT)
                _text_incomingNewText = true;
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

            if (_gm.inputManager.GetAxisRaw("Submit") != 0) {
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

            if (_text_lastCharacterTypedTime + _dm.Text_secondsPerChar > Time.time) return;

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
}
