using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TetrisManagerTest {
	static GameObject go;

	[SetUp] public void Init () {
		go = new GameObject ();
		go.AddComponent<TetrisManager> ();
	}

	[TearDown] public void Dispose () { }

	[Test] public void TetrisManagerTestSimplePasses () {
		// Use the Assert class to test conditions.
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest] public IEnumerator TetrisManager_ItDestroysDuplicateInstancesUponAwake () {
		var instance2 = new GameObject ();
		instance2.AddComponent<TetrisManager> ();

		yield return null;

		Assert.True (instance2 == null);
	}

	[UnityTest] public IEnumerator TetrisManager_Has_An_EventManagerComponent_Attached () {
		yield return null;
		var component = TetrisManager.Instance.GetComponent<EventManager> ();
		Assert.True (component != null);
	}

	[UnityTest] public IEnumerator TetrisManager_PlayerInputEvents_AreDispatched_WhenFallingBlockIsPresent () {
		yield return null;

	}
}