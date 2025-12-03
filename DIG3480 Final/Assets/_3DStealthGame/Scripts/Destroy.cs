using StealthGame;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
        if (player != null && player.hasShield)
        {
            player.ConsumeShield();
            Destroy(gameObject);
        }
    }
}
