using System;
using UnityEngine;

namespace Level
{
    /** Author: Nick Zimanski
    *   Version: 3/15/23
    */
    public class WildNPC : NPC
    {
        public override void InteractCallback()
        {
            Managers.DialogueManager _dm = GameManager.Instance.Get<Managers.DialogueManager>();

            Debug.Log(_dm.IsConversationActive());
            int dddd = _npc.NextOrderedConversation();
            if (_dm.IsConversationActive()) return;
            Debug.Log(dddd);
            _dm.RunConversation(dddd);
        }
    }
}