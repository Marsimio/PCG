using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 100;
    public float sprintSpeed = 200;

    private GameObject playerCamera;
    private Rigidbody rb;
    public float mouseSensitivity = 100.0f;
    public Vector3 cameraOffset = new Vector3(0, 1, -3);

    void Start()
    {
        CreateAndSetupCamera();

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
            rb.useGravity = true;
        }

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.height = 4.0f;
            capsuleCollider.radius = 1f;
            capsuleCollider.center = new Vector3(0, 1, 0);
        }
    }

    void Update()
    {
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;
        Vector3 newPosition = rb.position + moveDirection * speed * Time.deltaTime;
        rb.MovePosition(newPosition);

        playerCamera.transform.position = rb.position + cameraOffset;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        playerCamera.transform.rotation *= Quaternion.Euler(mouseY, mouseX, 0);
        playerCamera.transform.eulerAngles = new Vector3(playerCamera.transform.eulerAngles.x, playerCamera.transform.eulerAngles.y, 0);
    }

    private void CreateAndSetupCamera()
    {
        if (Camera.main == null)
        {
            playerCamera = new GameObject("PlayerCamera");
            Camera camera = playerCamera.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }
        else
        {
            playerCamera = Camera.main.gameObject;
        }

        playerCamera.transform.position = this.transform.position + cameraOffset;
        playerCamera.transform.LookAt(this.transform);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}