using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris {
    namespace Extensions {
        public static class ExtendedGameObject {
            public static void Rounded (this Transform transform) {
                transform.position = transform.position.Rounded ();

                foreach (Transform t in transform) {
                    t.position = t.position.Rounded ();
                }
            }

            public static Vector3 ToVector3 (this MoveDirection direction) {
                switch (direction) {
                    case MoveDirection.Down:
                        return Vector3.down;
                    case MoveDirection.Left:
                        return Vector3.left;
                    case MoveDirection.Right:
                        return Vector3.right;
                    default:
                        return Vector3.down;
                }
            }

            public static void Move (this Transform transform, MoveDirection direction) {
                switch (direction) {
                    case MoveDirection.Down:
                        transform.Translate (Vector3.down, Space.World);
                        break;
                    case MoveDirection.Left:
                        transform.Translate (Vector3.left, Space.World);
                        break;
                    case MoveDirection.Right:
                        transform.Translate (Vector3.right, Space.World);
                        break;
                }
                transform.Rounded ();
            }

            public static void RotateInDirection (this Transform transform, RotateDirection direction) {
                switch (direction) {
                    case RotateDirection.Left:
                        transform.Rotate (0, 0, -90);
                        break;
                    case RotateDirection.Right:
                        transform.Rotate (0, 0, 90);
                        break;
                }
                transform.Rounded ();
            }

            public static void LogTransforms (this Transform transform) {
                foreach (Transform t in transform) {
                    Debug.Log (t.position);
                }
            }

        }
    }
}