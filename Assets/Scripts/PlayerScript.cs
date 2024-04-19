// Encargado de mover al jugador en la pantalla
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    Vector2 screenBounds;
    private Animator animator;
    private PolygonCollider2D polyCollider;
    private bool isTouchingWall = false;
    private bool rotated = false;
    private int collisionCount = 0;

    void Start()
    {
        Camera mainCamera = Camera.main;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        polyCollider = GetComponent<PolygonCollider2D>();

        // Conseguimos las coordenadas del punto mas alto y mas bajo de la pantalla
        float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        // Tomamos en consideración el alto del jugador
        float objectHeight = transform.localScale.y / 2.0f;

        // Seteamos la información
        screenBounds = new Vector2(screenTop - objectHeight, screenBottom + objectHeight);
    }



    void FixedUpdate()
    {
        if (!GameManagerScript.Instance.lost)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

            if (!isTouchingWall)
            {
                // Si me muevo horizontalmente cambio la rotación para mirar hacia abajo, esto lo hacemos así para que el cambio se vea más smooth
                if (moveHorizontal != 0)
                {
                    rotated = false;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                }

                // Si estamos apretando la tecla para abajo cambiamos la animación, la velocidad de caida y el collider
                if (moveVertical < 0)
                {
                    animator.Play("Falling Fast");
                    Config.FALL_SPEED = 10f;
                    polyCollider.SetPath(0, Config.playerFallingFastColliderPoints);
                }
                else
                {
                    animator.Play("Falling");
                    Config.FALL_SPEED = 5f;
                    polyCollider.pathCount = 1;
                    polyCollider.SetPath(0, Config.playerFallingColliderPoints);

                }
            }


            // Nos aseguramos de que el jugador no este tratando de salir de la pantalla(en vertical nomas ya que en horizontal choca con las murallas)
            newPosition.y = Mathf.Clamp(newPosition.y, screenBounds.y, screenBounds.x);

            rb.MovePosition(newPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Finish"))
        {
            GameManagerScript.Instance.lost = true;
            animator.speed = 0;
        }

        if (collision.gameObject.CompareTag("Left Wall"))
        {
            // Tenemos que hacer esto ya que como toca varias murallas da falso positivos
            collisionCount++;

            animator.Play("Running");
            polyCollider.SetPath(0, Config.playerRunningColliderPoints);
            isTouchingWall = true;

            if (!rotated)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                rotated = true;
            }

        }

        if (collision.gameObject.CompareTag("Right Wall"))
        {
            // Tenemos que hacer esto ya que como toca varias murallas da falso positivos
            collisionCount++;

            animator.Play("Running");
            polyCollider.SetPath(0, Config.playerRunningColliderPoints);
            isTouchingWall = true;

            if (!rotated)
            {
                transform.localRotation = Quaternion.Euler(0, 180, -90);
                rotated = true;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Left Wall"))
        {
            collisionCount--;

            if (collisionCount == 0)
            {
                isTouchingWall = false;
            }
        }

        if (collision.collider.CompareTag("Right Wall"))
        {
            collisionCount--;

            if (collisionCount == 0)
            {
                isTouchingWall = false;
            }
        }
    }
}
