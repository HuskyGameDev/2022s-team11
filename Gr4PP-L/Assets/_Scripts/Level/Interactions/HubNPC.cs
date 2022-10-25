using System;
using UnityEngine;

namespace Level
{
    public class HubNPC : Interactable 
    {
        public override void InteractCallback() {
            GameManager.Instance.Get<Managers.DialogueManager>().RunConversation(80085);
        }
    }
}