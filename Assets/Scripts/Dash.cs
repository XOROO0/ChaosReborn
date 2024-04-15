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
    private bool isDashing = false;
    private bool dashToConsume = false;
    private Vector3 moveDirection;

    private void Update()
    {
        timer -= Time.deltaTime;

        var horizontalMovement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");


       moveDirection =
            FPSController.playerTransform.transform.forward * verticalMovement
            + FPSController.playerTransform.transform.right * horizontalMovement;


        if (Input.GetKeyDown(KeyCode.LeftShift) && timer <= 0 && !isDashing)
        {
            dashToConsume = true;
        }
    }

    private void FixedUpdate()
    {
        if(dashToConsume)
        {
            PerformDash(moveDirection);
            dashToConsume = false;
        }
    }

    private void PerformDash(Vector3 moveDir)
    {
        isDashing = true;
        StartCoroutine(SetFOV(Camera.main.fieldOfView, 100, 0.1f));
        rb = GetComponent<Rigidbody>();

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.useGravity = false;

        rb.AddForce(moveDir.normalized * dashSpeed, ForceMode.Impulse);


        Invoke(nameof(StopDash), dashTime);
    }

    private void StopDash()
    {
        StartCoroutine(SetFOV(Camera.main.fieldOfView, 90, 0.1f));
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        isDashing = false;

        Invoke(nameof(FinishStopDash), 0.1f);
    }

    private void FinishStopDash()
    {
        rb.useGravity = true;
        timer = dashCoolDown;
    }

    private IEnumerator SetFOV(float currentFOV, float targetFOV, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime) 
        {
            Camera.main.fieldOfView = Mathf.Lerp(
                currentFOV, targetFOV, t / duration);

            yield return null;
        }

        Camera.main.fieldOfView = targetFOV;
    }
}
