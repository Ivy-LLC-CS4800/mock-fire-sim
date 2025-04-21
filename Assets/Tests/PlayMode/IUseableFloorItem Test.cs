using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class IUseableFloorItemTests
{
    private GameObject itemGO;
    private IUseableFloorItem useableItem;
    private GameObject heldItem;

    [SetUp]
    public void SetUp()
    {
        itemGO = new GameObject("UseableFloorItem");
        useableItem = itemGO.AddComponent<IUseableFloorItem>();

        heldItem = new GameObject("HeldItem");
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(itemGO);
        Object.DestroyImmediate(heldItem);
    }

    [UnityTest]
    public IEnumerator Use_IncrementsInteractionCount()
    {
        Assert.AreEqual(0, useableItem.getInteractionCount());

        useableItem.Use(heldItem);
        yield return null;

        Assert.AreEqual(1, useableItem.getInteractionCount());

        useableItem.Use(heldItem);
        yield return null;

        Assert.AreEqual(2, useableItem.getInteractionCount());
    }

    [UnityTest]
    public IEnumerator Use_LogsInteractionMessage()
    {
        LogAssert.Expect(LogType.Log, $"{itemGO.name} interacted with {heldItem.name}, Count: 1");

        useableItem.Use(heldItem);
        yield return null;
    }
}
