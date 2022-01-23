using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers {
    public class GameManager : MonoBehaviour
    {
        private static Camera s_mainCamera = null;
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public static Camera MainCamera() {
            if (s_mainCamera == null) s_mainCamera = Camera.main;
            return s_mainCamera;
        }
    }   
}