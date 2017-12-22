using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventManager : MonoBehaviour {

	// Use this for initialization
	private Dictionary<string, UnityEvent> eventDictionary;

	private static EventManager _instance;

	public static EventManager instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType (typeof (EventManager)) as EventManager;
				if (_instance) {
					_instance.Initialize ();
				} else {
					Debug.LogError ("No EventManager script found in the scene.");
				}
			}
			return _instance;
		}
	}

	void Initialize () {
		if (eventDictionary != null)
			return;

		eventDictionary = new Dictionary<string, UnityEvent> ();
	}

	void Start () { }

	// Update is called once per frame
	void Update () { }

	public static void RegisterListener (string eventName, UnityAction listener) {
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.AddListener (listener);
		} else {
			thisEvent = new UnityEvent ();
			thisEvent.AddListener (listener);
			instance.eventDictionary.Add (eventName, thisEvent);
		}

	}

	public static void DestroyListener (string eventName, UnityAction listener) {
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.RemoveListener (listener);
		}
	}

	public static void TriggerEvent (string eventName) {
		UnityEvent thisEvent;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.Invoke ();
		}
	}
}