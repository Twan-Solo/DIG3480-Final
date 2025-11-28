using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject pickupVisual;

    void Start()
    {
        if (pickupVisual != null)
            pickupVisual.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();
        if (player != null)
        {
            player.ActivateShield();

            if (pickupVisual != null)
                pickupVisual.SetActive(false);

            Destroy(gameObject);
        }
    }
}
