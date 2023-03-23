using System;
using UnityEngine;

namespace Level
{
    /** Author: Nick Zimanski
    *   Version: 3/15/23
    */
    public class HubNPC : NPC
    {
        public override void InteractCallback()
        {
            Managers.DialogueManager _dm = GameManager.Instance.Get<Managers.DialogueManager>();

            if (_dm.IsConversationActive()) return;
            _dm.RunConversation(_npc.RandomConversation());
        }
    }
}