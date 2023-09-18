using UnityEngine;
using UnityEngine.UI;

public class SelectionUI : MonoBehaviour
{
    [SerializeField] GameObject go;

    Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public virtual void OnSelect()
    {
        go.SetActive(true);
        button.colors = new ColorBlock
        {
            normalColor = Color.white,
            colorMultiplier = 1f,
            disabledColor = button.colors.disabledColor,
            selectedColor = button.colors.selectedColor,
            fadeDuration = button.colors.fadeDuration
        };
    }

    public virtual void OnDeselect()
    {
        go.SetActive(false);
        button.colors = new ColorBlock
        {
            normalColor = button.colors.disabledColor,
            colorMultiplier = 1f,
            disabledColor = button.colors.disabledColor,
            selectedColor = button.colors.selectedColor,
            fadeDuration = button.colors.fadeDuration
        };
    }
}