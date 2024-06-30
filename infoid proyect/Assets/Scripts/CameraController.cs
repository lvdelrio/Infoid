using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float downwardSpeed = 5f;
    public float catchUpMultiplier = 1.2f;
    public float speedUp = 1.0f;
    public float zoomOutSpeed = 15f;
    public float zoomInSpeed = 10f;
    private Camera cam;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.25f;
    private Vector3 shakeOffset;
    private float shakeTimeRemaining;
    public float followSpeed = 0.5f;
    private Vector3 velocity = Vector3.zero;
    private float initialX;
    private float initialZ;
    private BoxCollider2D boundaryCollider; // Reference to the BoxCollider2D component

    void Start()
    {
        cam = GetComponent<Camera>();
        initialX = transform.position.x;
        initialZ = transform.position.z;
    }

    void FixedUpdate()
    {
        if (player != null && player.GetComponent<PlayerController>().CanMove())
        {
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                float playerSpeed = playerRigidbody.velocity.y;
                float cameraBottom = transform.position.y - Camera.main.orthographicSize;
                float moveInputy = Input.GetAxis("Vertical");

                Vector3 targetPosition = new Vector3(initialX, player.transform.position.y, initialZ);

                if (player.transform.position.y < cameraBottom + speedUp || playerSpeed < -downwardSpeed)
                {
                    float adjustedSpeed = Mathf.Max(downwardSpeed, Mathf.Abs(playerSpeed) / catchUpMultiplier);
                    targetPosition.y += Vector3.down.y * adjustedSpeed * Time.deltaTime;
                }
                else
                {
                    targetPosition.y += Vector3.down.y * downwardSpeed * Time.deltaTime;
                }

                Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, followSpeed);

                smoothedPosition += shakeOffset;

                // Constrain the camera to the boundary if the boundaryCollider exists
                if (boundaryCollider != null)
                {
                    smoothedPosition = ConstrainCamera(smoothedPosition);
                }

                transform.position = smoothedPosition;

                if (moveInputy < 0)
                {
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 20f, zoomInSpeed * Time.deltaTime);
                }
                else
                {
                    cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 12f, zoomOutSpeed * Time.deltaTime);
                }
            }
        }
    }

    private Vector3 ConstrainCamera(Vector3 position)
    {
        if (boundaryCollider != null)
        {
            Bounds bounds = boundaryCollider.bounds;
            float halfHeight = cam.orthographicSize;
            float buffer = 100f;

            float cameraBottom = position.y - halfHeight;

            if (cameraBottom < bounds.min.y - buffer)
            {
                position.y = bounds.min.y + halfHeight + buffer;
            }

            position.x = initialX;
        }

        return position;
    }

    public void ResetCameraPosition()
    {
        transform.position = new Vector3(initialX, 0, initialZ);
        cam.orthographicSize = 12f;
    }

    public void ShakeCamera()
    {
        shakeTimeRemaining = shakeDuration;
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        while (shakeTimeRemaining > 0)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            shakeOffset = new Vector3(x, y, 0);
            shakeTimeRemaining -= Time.deltaTime;

            yield return null;
        }

        shakeOffset = Vector3.zero;
    }

    // Method to be called when the BoxCollider2D is instantiated
    public void SetBoundaryCollider(GameObject newColliderObject)
    {
        boundaryCollider = newColliderObject.GetComponent<BoxCollider2D>();
    }
}
