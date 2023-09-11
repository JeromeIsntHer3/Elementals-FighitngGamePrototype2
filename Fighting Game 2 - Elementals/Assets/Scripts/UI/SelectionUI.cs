using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject go;

    public void OnSelect(BaseEventData eventData)
    {
        go.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        go.SetActive(false);
    }
}