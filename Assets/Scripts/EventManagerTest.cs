using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;

public class EventManagerTest {

	public static GameObject gameManager;

	[SetUp] public void Init () {
		gameManager = new GameObject ();
		gameManager.AddComponent<EventManager> ();
	}

	[TearDown] public void Dispose () { }

	bool moveBlockHandlerCalled;
	[UnityTest] public IEnumerator EventManager_Can_Add_Different_Events () {
		EventManager.Instance.AddListener<MoveTetrominoEvent> (MoveBlockHandler);

		moveBlockHandlerCalled = false;

		EventManager.Instance.TriggerEvent (new MoveTetrominoEvent (gameManager.transform, MoveDirection.Down));

		yield return null;

		Assert.True (moveBlockHandlerCalled);
	}

	[UnityTest] public IEnumerator EventManager_Can_Remove_A_Listener () {

		EventManager.Instance.AddListener<MoveTetrominoEvent> (MoveBlockHandler);

		yield return null;

		EventManager.Instance.RemoveListener<MoveTetrominoEvent> (MoveBlockHandler);

		yield return null;

		moveBlockHandlerCalled = false;

		EventManager.Instance.TriggerEvent (new MoveTetrominoEvent (gameManager.transform, MoveDirection.Down));

		Assert.False (moveBlockHandlerCalled);
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator EventManagerTestWithEnumeratorPasses () {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

	public void MoveBlockHandler (MoveTetrominoEvent e) {
		moveBlockHandlerCalled = true;
	}
}