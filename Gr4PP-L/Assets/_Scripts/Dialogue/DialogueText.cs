/**using UnityEngine;
namespace _Scripts.Dialogue { 

    //[CreateAssetMenu(fileName = "DialogueTextData", menuName = "ScriptableObjects/Conversation/DialogueTextScriptableObject")]
    [System.Serializable]
    public class DialogueText : DialogueBeat
    {
        [SerializeField]
        public string Text;

        public DialogueText() : base() {
            Text = "";
        }
    }
}
*/