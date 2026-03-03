using System.Collections;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioClip[] footStepSFX;

    private Player player;
    private int i = 0;

    void Start()
    {
        player = GetComponent<Player>();
        StartCoroutine(PlayFootsteps());
    }

    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (player.xInput != 0 && player.isGrounded)
            {
                i = Random.Range(0, 9);
                AudioManager.instance.PlaySFX(footStepSFX[i]);
            }

            yield return new WaitForSeconds(0.45f);
        }
    }
}
