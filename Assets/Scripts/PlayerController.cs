using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed = 1;

    [FormerlySerializedAs("turnSpeed")] [SerializeField]
    private float horizontalTurnSpeed = 1;

    [SerializeField] private float verticalTurnSpeed = 1;

    [SerializeField] private float cameraSensitivity = 5;
    [SerializeField] private float cameraOffset = 5;

    private float _yaw;
    private float _pitch;

    private Rigidbody _rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateCamera();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateCamera()
    {
        if (!IsOwner) return;
        
        var camera = GetComponentInChildren<Camera>();
        if (camera == null) return;

        var xMouseDelta = Input.GetAxisRaw("Mouse X");
        _yaw += xMouseDelta * cameraSensitivity;

        var yMouseDelta = -Input.GetAxisRaw("Mouse Y");
        _pitch += yMouseDelta * cameraSensitivity;
        _pitch = Mathf.Clamp(_pitch, -90, 90);

        var rot = Quaternion.Euler(new(_pitch, _yaw, 0));
        camera.transform.rotation = rot;

        var offset = rot * new Vector3(0, 0, -cameraOffset);
        camera.transform.position = transform.position + offset;
    }

    private void UpdateMovement()
    {
        if (!IsOwner) return;
        
        // Forward and backward
        _rb.velocity += transform.forward * (CreateForwardBackwardFloat(KeyCode.W, KeyCode.S) * speed);

        // Left and right
        var yawDelta = 0f;
        yawDelta += CreateForwardBackwardFloat(KeyCode.D, KeyCode.A) * horizontalTurnSpeed;

        var yawDeltaRotation = Vector3.up * yawDelta;
        _rb.angularVelocity += yawDeltaRotation;

        // Up and down
        var pitchDelta = CreateForwardBackwardFloat(KeyCode.UpArrow, KeyCode.DownArrow) * verticalTurnSpeed;

        var pitchDeltaRotation = Vector3.right * pitchDelta;
        _rb.angularVelocity += pitchDeltaRotation;
    }

    private static float CreateForwardBackwardFloat(KeyCode one, KeyCode zero) =>
        (Input.GetKey(one) ? 1 : 0) - (Input.GetKey(zero) ? 1 : 0);
}