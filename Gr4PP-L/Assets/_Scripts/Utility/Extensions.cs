using UnityEngine;
namespace Utility
{
    public static class Extensions
    {

        public static Vector2[] ToVector2s(this GameObject[] go)
        {
            Vector2[] t = new Vector2[go.Length];
            for (int i = 0; i < go.Length; i++)
            {
                t[i] = (Vector2)go[i].transform.position;
            }
            return t;
        }

        public static bool GetButton(this UnityEngine.InputSystem.InputAction action) => action.ReadValue<float>() > 0;
        public static bool GetButtonUp(this UnityEngine.InputSystem.InputAction action) => action.triggered && action.ReadValue<float>() == 0;
        public static bool GetButtonDown(this UnityEngine.InputSystem.InputAction action) => action.triggered && action.ReadValue<float>() > 0;
    }
}