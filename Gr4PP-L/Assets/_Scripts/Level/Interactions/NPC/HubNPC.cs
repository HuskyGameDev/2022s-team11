using System;
using UnityEngine;

namespace Level
{
    /** Author: Nick Zimanski
    *   Version: 10/25/22
    */
    public class HubNPC : Interactable 
    {
        [SerializeField]
        private NPCData _npc;

        private SpriteRenderer _spriteRenderer;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _npc.Sprite;
        }

        public override void InteractCallback() {
            Managers.DialogueManager _dm = GameManager.Instance.Get<Managers.DialogueManager>();
            
            if (_dm.IsConversationActive()) return;
            _dm.RunConversation(_npc.RandomConversation());
        }
    }
}