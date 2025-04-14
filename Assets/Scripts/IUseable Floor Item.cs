using System.Diagnostics;
using System.Globalization;
using UnityEngine;

public class IUseableFloorItem : MonoBehaviour, IUseableFloor
{
    private int interactionCount = 0;


    public void Use(GameObject source)
    {
        interactionCount++;
        UnityEngine.Debug.Log($"{gameObject.name} interacted with {source.name}, Count: {interactionCount}");
    }

    public int getInteractionCount(){
        return interactionCount;
    }
}
