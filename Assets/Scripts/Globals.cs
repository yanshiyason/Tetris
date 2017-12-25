using UnityEngine;

public enum MoveDirection { Down, Left, Right }
public enum RotateDirection { Left, Right }

public struct DirectionToVector {

    public static Vector3 For (MoveDirection direction) {
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
}