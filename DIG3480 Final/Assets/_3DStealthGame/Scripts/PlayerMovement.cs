using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Animator m_Animator;

    public InputAction MoveAction;
    public InputAction UnfreezeAction;
    public TMP_Text FrozenStatusText;
    public GameObject shieldVisual; 
    public bool hasShield = false;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;
   

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    bool isFrozen = false;
    int freezeButtonPresses = 0;
    int requiredPressesToUnfreeze = 3;
    float freezeTimer = 0f;
    float freezeInterval = 5f;
    float freezeChance = 0.3f;
    float pressCooldown = 0.2f;
    float lastPressTime = 0f;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        UnfreezeAction.Enable();
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }
    void FixedUpdate()
    {
        freezeTimer += Time.deltaTime;
        if (!isFrozen && freezeTimer >= freezeInterval)
        {
            if (Random.value < freezeChance)
            {
                isFrozen = true;
                freezeButtonPresses = 0;
                m_Animator.SetBool("IsWalking", false);
                if (FrozenStatusText != null)
                    FrozenStatusText.text = "Player is Frozen! Press Right Mouse Button 3 times to unfreeze!";
            }
            freezeTimer = 0f;
        }
        if (isFrozen)
        {
            if (UnfreezeAction.ReadValue<float>() > 0 && Time.time - lastPressTime > pressCooldown)
            {
                freezeButtonPresses++;
                lastPressTime = Time.time;

                if (freezeButtonPresses >= requiredPressesToUnfreeze)
                {
                    isFrozen = false;
                    if (FrozenStatusText != null)
                        FrozenStatusText.text = "";
                }
            }

            return;
        }

        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);
    }
    public void ActivateShield()
    {
        hasShield = true;
        if (shieldVisual != null)
            shieldVisual.SetActive(true);
    }

    public void ConsumeShield()
    {
        hasShield = false;
        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

}