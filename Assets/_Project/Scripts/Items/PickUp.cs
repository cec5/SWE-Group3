using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject item;

    private bool collectInput;

    private void Update()
    {
        collectInput = Input.GetKeyUp(KeyCode.E);
        if (collectInput)
        {
            item.SetActive(false);
        }
    }
}
