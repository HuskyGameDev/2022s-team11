using UnityEngine;
namespace _Scripts.Managers
{
    /** Author: Nick Zimanski
    * Version 9/09/22
    */
    public abstract class Manager : MonoBehaviour
    {
        void Awake()
        {
            //DontDestroyOnLoad(this.gameObject);
        }
    }
}