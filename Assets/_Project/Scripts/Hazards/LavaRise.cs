using UnityEngine;

public class RisingLava : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float riseSpeed = 0.4f; // Might be adjusted for the final level
    [SerializeField] private float maxHeight = 0f; // Likewise, adjust for final 

    private float currentResetHeight;
    private bool isRising = false;

    private void Start()
    {
        currentResetHeight = transform.position.y;
    }

    // Listens for the respawn event that triggers when the player dies (by colliding with an object tagged "Hazard"); see player script for reference
    private void OnEnable()
    {
        Player.OnPlayerRespawn += ResetLava;
    }

    private void OnDisable()
    {
        Player.OnPlayerRespawn -= ResetLava;
    }

    private void Update()
    {
        if (isRising)
        {
            transform.Translate(Vector2.up * riseSpeed * Time.deltaTime);
            if (transform.position.y >= maxHeight)
            {
                Vector3 pos = transform.position;
                pos.y = maxHeight;
                transform.position = pos;
                isRising = false;
            }
        }
    }

    // Called by trigger script
    public void StartRising()
    {
        if (transform.position.y < maxHeight)
        {
            isRising = true;
        }
    }

    // Would be called by a checkpoint script; however, based on current level design, this probably won't be used at all
    public void SetResetHeight(float yHeight)
    {
        currentResetHeight = yHeight;
    }

    private void ResetLava()
    {
        Vector3 newPos = transform.position;
        newPos.y = currentResetHeight;
        transform.position = newPos;
        isRising = false;
    }
}