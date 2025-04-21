using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PickableItemTests
{
    private GameObject pickableGO;
    private PickableItem pickableItem;

    [SetUp]
    public void SetUp()
    {
        pickableGO = new GameObject("PickableItem");
        pickableItem = pickableGO.AddComponent<PickableItem>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(pickableGO);
    }

    [UnityTest]
    public IEnumerator PickUp_WithRigidbody_SetsKinematicAndTransforms()
    {
        var rb = pickableGO.AddComponent<Rigidbody>();

        // Call Awake() manually in tests
        pickableItem.Invoke("Awake", 0f);
        yield return null;

        var returnedGO = pickableItem.PickUp();

        Assert.AreEqual(pickableGO, returnedGO);
        Assert.IsTrue(rb.isKinematic);
        Assert.AreEqual(Vector3.zero, pickableGO.transform.localPosition);
        Assert.AreEqual(Quaternion.Euler(270f, 0f, 340f), pickableGO.transform.localRotation);
        Assert.AreEqual(Vector3.one, pickableGO.transform.localScale);
    }

    [UnityTest]
    public IEnumerator PickUp_WithoutRigidbody_SkipsKinematicAndSetsTransforms()
    {
        // Ensure no Rigidbody
        var rb = pickableGO.GetComponent<Rigidbody>();
        if (rb != null) Object.DestroyImmediate(rb);

        pickableItem.Invoke("Awake", 0f);
        yield return null;

        var returnedGO = pickableItem.PickUp();

        Assert.AreEqual(pickableGO, returnedGO);
        Assert.AreEqual(Vector3.zero, pickableGO.transform.localPosition);
        Assert.AreEqual(Quaternion.Euler(270f, 0f, 340f), pickableGO.transform.localRotation);
        Assert.AreEqual(Vector3.one, pickableGO.transform.localScale);
    }
}
