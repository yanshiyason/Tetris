using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent {

}

public class MoveBlockGroupEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public MoveDirection Direction { get; private set; }

    public MoveBlockGroupEvent (Transform currentPosition, MoveDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}

public class RotateBlockGroupEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public RotateDirection Direction { get; private set; }

    public RotateBlockGroupEvent (Transform currentPosition, RotateDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}

public class MoveValidEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public MoveDirection Direction { get; private set; }

    public MoveValidEvent (Transform currentPosition, MoveDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}

public class MoveInvalidEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public MoveDirection Direction { get; private set; }

    public MoveInvalidEvent (Transform currentPosition, MoveDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}

public class RotateValidEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public RotateDirection Direction { get; private set; }

    public RotateValidEvent (Transform currentPosition, RotateDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}

public class RotateInvalidEvent : GameEvent {
    public Transform CurrentPosition { get; private set; }
    public RotateDirection Direction { get; private set; }

    public RotateInvalidEvent (Transform currentPosition, RotateDirection direction) {
        CurrentPosition = currentPosition;
        Direction = direction;
    }
}