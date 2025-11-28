using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Observer : MonoBehaviour
{

    public Transform player;
    public GameEnding gameEnding;
    bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }
    void Update()
    {
        if (!m_IsPlayerInRange)
            return;

        Vector3 direction = player.position - transform.position + Vector3.up;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            PlayerMovement pm = hit.collider.transform.root.GetComponent<PlayerMovement>();

            if (pm != null)
            {
                if (pm.hasShield)
                {
                    pm.ConsumeShield();       
                    StartCoroutine(DestroyNextFrame());
                    return;
                }
                else
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }
    IEnumerator DestroyNextFrame()
    {
        yield return null; 
        Destroy(gameObject);
    }
}