using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Wall Running")]
    public float wallRunSpeed = 0.5f;
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minimumJumpHeight = 1.5f;
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallJumpForce;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    [Header("FX")]
    [SerializeField] private Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunfov;
    [SerializeField] float wallRunfovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt {  get; private set; }
    public static bool isWallRunning { get; private set; }

    public static Vector3 wallDir;

    Rigidbody rb;

    bool wallLeft = false;
    bool wallRight = false;

    RaycastHit leftWallHit;
    RaycastHit rightWallHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight, groundLayer);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(groundCheck.position, -orientation.right, out leftWallHit, wallDistance, wallLayer);
        wallRight = Physics.Raycast(groundCheck.position, orientation.right, out rightWallHit, wallDistance, wallLayer);


        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        wallDir = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallDir).magnitude > (orientation.forward - -wallDir).magnitude)
            wallDir = -wallDir;
    }

    private void Update()
    {
        CheckWall();

        
        if(CanWallRun())
        {
            if(wallLeft)
            {
                //Debug.Log("Wall Run Left");
                StartWallRun();
            }
            else if(wallRight)
            {
                //Debug.Log("Wall Run Right");
                StartWallRun();
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            
            if(isWallRunning)
            {
                rb.useGravity = true;
            }

            StopWallRun();
        }
    }

    void StartWallRun()
    {
        isWallRunning = true;
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);
        rb.AddForce(wallRight ? orientation.right : -orientation.right * 5f, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunfov, wallRunfovTime * Time.deltaTime);

        if(wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if(wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDirection = transform.up + leftWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * 100f, ForceMode.Force);
            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDirection = transform.up + rightWallHit.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDirection * wallJumpForce * 100f, ForceMode.Force);
            }
        }
    }

    void StopWallRun()
    {
        //Debug.Log("Stop Wall Run");

        isWallRunning = false;

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunfovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }

    
}
