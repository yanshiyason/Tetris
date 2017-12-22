using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class FallingBlockGroupTest {

	[Test]
	[Ignore("default example test")]
	public void FallingBlockGroupTestSimplePasses() {
	}


	[UnityTest]
	public IEnumerator FallingBlockGroup_WithRigidBody_WillNotBeAffectedByPhysics()
	{
		var go = new GameObject();
		go.AddComponent<FallingBlockGroup>();
		
		var originalPosition = go.transform.position.y;
		
		yield return new WaitForFixedUpdate();

		Assert.AreEqual(originalPosition, go.transform.position.y);
	}
}
