using UnityEngine;

namespace Tetris {
    namespace Extensions {
        public static class ExtendedVector3 {
            public static Vector3 Rounded(this Vector3 v) {
                return new Vector3(
                    Mathf.Round(v.x),
                    Mathf.Round(v.y),
                    Mathf.Round(v.z)
                );
            }
        }
    }
}