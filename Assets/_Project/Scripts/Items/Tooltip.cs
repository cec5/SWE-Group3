using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public string title;
    public string message;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) TooltipManager._instance.SetAndShowtooltip(title, message);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) TooltipManager._instance.HideToolTip();
    }
}
