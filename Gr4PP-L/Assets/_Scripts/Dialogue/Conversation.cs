using UnityEngine;
using _Scripts.Utility;
namespace _Scripts.Dialogue {
    [CreateAssetMenu(fileName = "ConversationData", menuName = "ScriptableObjects/Conversation/ConversationScriptableObject")]
    public class Conversation : ScriptableObject
    {
        public DialogueBeat[] Beats = new DialogueBeat[1];
    }
}
