using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    float playerHeight = 2f;

    [Header("Movement")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerOBJ;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravityMultiplier = 6f;
    [SerializeField] private float airMultiplier = 0.4f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private WallRun wallRun;
    [SerializeField] private float maxSlideTime;
    [SerializeField] private float slideForce;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideYScale;

    private float startYScale;
    public static bool isSliding;


    private float moveMultiplier = 10f;

    private float groundDrag = 6f;
    private float airDrag = 2f;

    private float horizontalMovement;
    private float verticalMovement;

    private bool groundBool;
    private bool isGrounded;
    private float groundDistance = 0.4f;

    private Vector3 moveDirection;
    private Vector3 slopeMoveDirection;

    public static bool IsMoving;
    public static Transform playerTransform;

    public static Vector3 currentPos;
    public static bool canMove = true;

    Rigidbody rb;

    RaycastHit slopeHit;

    bool doubleJump;
    int allowedJumps = 2;

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if(slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYScale = playerOBJ.localScale.y;

        playerTransform = orientation.transform;
    }

    private void Update()
    {
        groundBool = 
            Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!isGrounded && groundBool)
        {
            isGrounded = true;
            allowedJumps = 2;
        }
        else if(isGrounded && !groundBool)
        {
            isGrounded = false;
        }

        HandleInput();
        HandleDrag();

        if(Input.GetKeyDown(KeyCode.Space) && allowedJumps > 0 && !WallRun.isWallRunning)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        currentPos = transform.position;

        if (WallRun.isWallRunning)
            allowedJumps = 1;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleGravity();

        if (isSliding)
            SlidingMovement();
    }

    private void HandleGravity()
    {
        if(rb.useGravity)
            rb.AddForce(Physics.gravity * gravityMultiplier);
    }

    private void HandleInput()
    {
        if(!canMove)
        {
            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 20 * Time.deltaTime);
            horizontalMovement = verticalMovement = 0;
        }

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        IsMoving = horizontalMovement != 0 || verticalMovement != 0;

        if(Input.GetKeyDown(KeyCode.LeftShift) && IsMoving)
        {
            StartSlide();
        }

        if(Input.GetKeyUp(KeyCode.LeftShift) && isSliding)
        {

        }


        moveDirection =
            orientation.transform.forward * verticalMovement + orientation.transform.right * horizontalMovement;
        
    }

    private void HandleMovement()
    {
        if(isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(isGrounded && OnSlope())
        {
            rb.AddForce(slopeMoveDirection.normalized * moveSpeed * moveMultiplier, ForceMode.Acceleration);
        }
        else if(!isGrounded)
        {
            rb.AddForce((WallRun.isWallRunning ? WallRun.wallDir : moveDirection.normalized)
                * moveSpeed * moveMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }

    private void HandleDrag()
    {
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void Jump()
    {
        allowedJumps--;

        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void StartSlide()
    {
        isSliding = true;

        playerOBJ.localScale = new Vector3(playerOBJ.localScale.x,
            slideYScale, playerOBJ.localScale.z);

        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        isSliding = false;

        playerOBJ.localScale = new Vector3(playerOBJ.localScale.x,
            startYScale, playerOBJ.localScale.z);
    }

}
