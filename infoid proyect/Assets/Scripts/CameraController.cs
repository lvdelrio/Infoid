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
    public float shakeMagnitude = 0.5f;

    void Start()
    {
        cam = GetComponent<Camera>();
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

                if (player.transform.position.y < cameraBottom + speedUp || playerSpeed < -downwardSpeed)
                {
                    float adjustedSpeed = Mathf.Max(downwardSpeed, Mathf.Abs(playerSpeed) / catchUpMultiplier);
                    transform.position += Vector3.down * adjustedSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.down * downwardSpeed * Time.deltaTime;
                }

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

    public void ResetCameraPosition()
    {
        transform.position = new Vector3(0, 0, -10);
        cam.orthographicSize = 12f;
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}