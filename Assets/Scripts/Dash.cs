using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashSpeed = 5f;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private float dashCoolDown = 1f;

    Rigidbody rb;
    private float timer;

    private void Update()
    {
        timer -= Time.deltaTime;

        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");


        var moveDirection =
            FPSController.playerTransform.transform.forward * verticalMovement
            + FPSController.playerTransform.transform.right * horizontalMovement;


        if (Input.GetKeyDown(KeyCode.LeftShift) && timer <= 0)
        {

            PerformDash(moveDirection);
        }
    }

    private void PerformDash(Vector3 moveDir)
    {
        rb = GetComponent<Rigidbody>();

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.useGravity = false;

        rb.AddForce(moveDir.normalized * dashSpeed, ForceMode.Impulse);


        Invoke(nameof(StopDash), dashTime);
    }

    private void StopDash()
    {
        rb.useGravity = true;
        timer = dashCoolDown;
    }
}
