using UnityEngine;
namespace Utility
{
    public static class Extensions
    {

        public static Vector2[] ToVector2s(this GameObject[] go) {
            Vector2[] t = new Vector2[go.Length];
            for (int i = 0; i < go.Length; i++) {
                t[i] = (Vector2) go[i].transform.position;
            }
            return t;
        }
    }
}