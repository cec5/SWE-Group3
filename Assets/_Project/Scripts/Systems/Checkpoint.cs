using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.UpdateCheckpoint(transform.position);
                activated = true;

                // Debug for now
                Debug.Log("Checkpoint activated");
            }
        }
    }
}