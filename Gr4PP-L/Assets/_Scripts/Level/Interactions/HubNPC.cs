using System;
using UnityEngine;

namespace Level
{
    /** Author: Nick Zimanski
    *   Version: 10/25/22
    */
    public class HubNPC : Interactable 
    {
        public override void InteractCallback() {
            Managers.DialogueManager _dm = GameManager.Instance.Get<Managers.DialogueManager>();
            
            if (_dm.IsConversationActive()) return;
            _dm.RunConversation(1337);
        }
    }
}