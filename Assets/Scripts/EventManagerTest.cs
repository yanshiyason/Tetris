using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class EventManagerTest {

	public static GameObject gameManager;	

	[SetUp] public void Init()
    {
		gameManager = new GameObject();
		gameManager.AddComponent<EventManager>();
	}

    [TearDown] public void Dispose()
	{
	}

	bool dummyFunctionCalled;
	[UnityTest] public IEnumerator EventManager_Can_Add_Different_Events() {
		var listener = new UnityAction(DummyFunction);
		EventManager.RegisterListener("MyNewEvent", listener);

		dummyFunctionCalled = false;

		EventManager.TriggerEvent("MyNewEvent");

		yield return null;

		Assert.True(dummyFunctionCalled);
	}

	[UnityTest] public IEnumerator EventManager_Can_Remove_A_Listener() {
		var listener = new UnityAction(DummyFunction);
		EventManager.RegisterListener("MyNewEvent", listener);

		yield return null;

		EventManager.DestroyListener("MyNewEvent", listener);

		yield return null;

		dummyFunctionCalled = false;

		EventManager.TriggerEvent("MyNewEvent");

		Assert.False(dummyFunctionCalled);
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator EventManagerTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}

	public void DummyFunction() {
		dummyFunctionCalled = true;
	}
}
