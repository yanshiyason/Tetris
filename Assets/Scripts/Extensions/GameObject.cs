using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris {
    namespace Extensions {
        public static class ExtendedGameObject {
            public static void MoveDown (this Transform transform) {
                transform.Translate (0, -1, 0, Space.World);
            }

            public static void MoveLeft (this Transform transform) {
                transform.Translate (-1, 0, 0, Space.World);
            }

            public static void MoveRight (this Transform transform) {
                transform.Translate (1, 0, 0, Space.World);
            }

            public static void RotateRight (this Transform transform) {
                transform.RotateAround (transform.position, Vector3.back, 90);
            }

            public static void RotateLeft (this Transform transform) {
                transform.RotateAround (transform.position, Vector3.back, -90);
            }

            public static void LogTransforms (this Transform transform) {
                foreach (Transform t in transform) {
                    Debug.Log (t.position);
                }
            }

        }
    }
}