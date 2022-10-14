using UnityEngine;
namespace Managers
{
    /** Author: Nick Zimanski
    * Version 9/16/22
    */
    public abstract class Manager : IGameService
    {
        private string _name;

        public string GetName() {
            return this._name;
        }

        public void Initialize() {
            _name = this.GetType().Name;
        }

        public abstract void Destroy();

        public abstract Manager GetNewInstance();

    }
}