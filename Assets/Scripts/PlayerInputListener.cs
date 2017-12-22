using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Tetris.Extensions;

class PlayerInputListener : MonoBehaviour {
    void Update() {
        ListenToPlayerInput();
    }

    public static Dictionary <string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>()
    {
        { "RotateRight", new UnityEvent() },
        { "RotateLeft",  new UnityEvent() },
        { "MoveRight",   new UnityEvent() },
        { "MoveLeft",    new UnityEvent() },
        { "MoveDown",    new UnityEvent() },
    };
    
    void ListenToPlayerInput() {
        if (TetrisManager.fallingBlockGroup == null)
            return;

		if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.RightArrow))
		{
            InvokeEvent("RotateRight");
		} else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.LeftArrow))
		{
            InvokeEvent("RotateLeft");
		} else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
            InvokeEvent("MoveLeft");
		} else if (Input.GetKeyDown(KeyCode.RightArrow))
		{ 
            InvokeEvent("MoveRight");
		} else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
            InvokeEvent("MoveDown");
		}
	}

    void InvokeEvent(string eventName)
    {
        Debug.Log("Invoking event: " + eventName);
        UnityEvent unityEvent = null;
        eventDictionary.TryGetValue(eventName, out unityEvent);

        if (unityEvent == null)
            return;

        unityEvent.Invoke();
    }
}
