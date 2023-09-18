using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log(gameObject.name + " Selected");
    }
}