using UnityEngine;

public class LavaTrigger : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private RisingLava lavaObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Lava triggered");
            if (lavaObject != null)
            {
                lavaObject.StartRising();
            }
            else
            {
                Debug.LogWarning("LavaTrigger.cs requires an lavaObject reference!");
            }
        }
    }
}