
using UnityEngine;

namespace Level
{
    /** Author: Nick Zimanski
    *   Version: 10/25/22
    */
    public class LevelEntry : Interactable
    {
        [SerializeField]
        private string _levelName;



        public override void InteractCallback()
        {
            GameManager.Instance.Get<Managers.AudioManager>().Play("Level Entry");
            GameManager.Instance.Get<Managers.TimerManager>().LevelExit();
            GameManager.Instance.StartCoroutine(GameManager.Instance.Get<Managers.LevelManager>().LoadScene(_levelName));
        }
    }
}
