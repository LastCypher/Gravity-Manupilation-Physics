using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 10f;
    public bool isGrounded;
    private Rigidbody rb;
    [SerializeField] CameraFollow camFollow;
    public Vector3 jumpDirection;

    // Define the gravity direction
    enum Directions
    {
        left,
        right,
        down,
        up
    }
    Directions currentDirection;

    void Start()
    {
        currentDirection = Directions.right;
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MovePlayer();
        Jump();
        isGrounded = IsGrounded();
        HandleGravityManipulation();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Adjust the move direction based on the current gravity direction
        Vector3 moveDirection = camFollow.transform.forward * vertical + camFollow.transform.right * horizontal;

        Vector3 moveVelocity = moveDirection * moveSpeed;

        rb.MovePosition(transform.position + moveVelocity * Time.fixedDeltaTime);

        // Rotate smoothly towards the movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, camFollow.transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void HandleGravityManipulation()
    {
        Vector3 gravityUp = -Physics.gravity.normalized;
        Vector3 localUp = transform.up;

        Quaternion toRotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            camFollow.currentZ = 90;
            Physics.gravity = new Vector3(9.81f, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, 90);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 90);
            jumpDirection = -Vector3.right;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            camFollow.currentZ = -90;
            jumpDirection = Vector3.right;
            Physics.gravity = new Vector3(-9.81f, 0, 0);
            transform.rotation = Quaternion.Euler(0, 0, -90);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            camFollow.currentZ = 0;
            jumpDirection = Vector3.up;
            Physics.gravity = new Vector3(0, 0, -9.81f);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = Quaternion.Euler(0, 0, 9.81f);
            Camera.main.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            //Vector3.right
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        float raycastLength = 0.6f;
        //Vector3.right
        Debug.DrawRay(transform.position, -jumpDirection, Color.red, 1f);
        return Physics.Raycast(transform.position, -jumpDirection, raycastLength + 0.1f);
    }

}