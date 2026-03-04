using TMPro;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager _instance;

    public TextMeshProUGUI titleComponent;
    public TextMeshProUGUI textComponent;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void SetAndShowtooltip(string title, string message)
    {
        gameObject.SetActive(true);
        titleComponent.text = title;
        textComponent.text = message;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
        titleComponent.text = string.Empty;
        textComponent.text = string.Empty;
    }
}
