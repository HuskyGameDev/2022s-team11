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
        if (!_dm.IsConversationActive()) {
            //Ending a conversation
            if (_conversationInProgress) {
                EndConversation();
            }
            return;
        }

        //Starting a new conversation
        if (!_conversationInProgress) {
            _dialogueCanvas.enabled = true;

            //Reset conversation-specific state
            _text_charactersTyped = 0;
            _text_lastCharacterTypedTime = 0;
            _delay_hasStarted = false;
            _delay_endTime = 0;

            _conversationInProgress = true;
        }

        UpdateConversation();
    }

        private void UpdateConversation() {
            switch (_dm.CurrentConversationIterator.Current.type) {
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
                _dm.NextConversationInstruction();
                _awaitingPlayerInput = false;
            }

        }

        private void UpdateText() {
            //Instantiate the current string
            if (_dm.Text_incomingNewText) {
                _text_currentString = _text_currentString + _dm.CurrentConversationIterator.Current.data;
                _dm.Text_incomingNewText = false;
            }

            //Check if we've reached the end of the string
            if (_text_charactersTyped >= _text_currentString.Length) { 
                _awaitingPlayerInput = true;

                //End the text instruction automatically if the next up is a delay.
                if (_dm.CurrentConversationIterator.IsNextOfType(ConvInstruction.InstructionType.DELAY)) _dm.NextConversationInstruction();
                return;
            }

            if (_text_lastCharacterTypedTime + _dm.Text_secondsPerChar > Time.time) return;

            _text_lastCharacterTypedTime = Time.time;
            _text_charactersTyped++;

            _dialogueText.text = _text_currentString.Substring(0, _text_charactersTyped);
            
        }

        private void UpdateDelay() {
            if (!_delay_hasStarted) {
                _delay_endTime = Time.time + (float) (Int32.Parse(_dm.CurrentConversationIterator.Current.data)/1000f);
                _awaitingPlayerInput = true;
                _delay_hasStarted = true;
                return;
            }

            if (_delay_endTime > Time.time) return;

            _delay_endTime = 0;
            _delay_hasStarted = false;

            //Conversations shouldn't end on a delay
            _dm.NextConversationInstruction();
            
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

        private void UpdateCharacterChange() {
            //If the character actually changes. Empty square brackets are treated as a passage change and will wipe the text box.
            if (_dm.CurrentConversationIterator.Current.data != "")    {
                _characterNameText.SetText(_dm.CurrentConversationIterator.Current.data);

                //TODO: set character portrait, if the name matches a character
                //TODO: Gray out other character portraits
                //TODO: ready character voice

            }

            _dialogueText.text = "";
            _text_currentString = "";
            _text_charactersTyped = 0;
            _text_lastCharacterTypedTime = 0;

            //Conversations shouldn't end on a character change
            _dm.NextConversationInstruction();
        }
}
