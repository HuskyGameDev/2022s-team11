using System.Collections.Generic;
using UnityEngine;
using Level;
using System;
using static Level.Respawnable;

namespace Managers
{
    public class InteractiveManager : Manager
    {
        public static event Action OnObjectRespawn;

        public new void Initialize()
        {
            base.Initialize();

            OnObjectRespawn = () => { };
            Managers.LevelManager.OnLevelExit += DestroyRespawnableReferences;
        }

        public InteractiveManager()
        {
            Initialize();
        }

        public static void RespawnAll()
        {
            OnObjectRespawn();
        }

        public override Manager GetNewInstance()
        {
            return new InteractiveManager();
        }

        public override void Destroy()
        {

        }

        public void DestroyRespawnableReferences()
        {
            OnObjectRespawn = () => { };
        }
    }
}
