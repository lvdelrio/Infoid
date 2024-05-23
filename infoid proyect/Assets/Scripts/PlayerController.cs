using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    public GameController gameController;
    public Rigidbody2D rb;
    private PolygonCollider2D collider;
    public Camera camera;
    public Animator animator;
    public GameObject grapplingHookPrefab;


    [Header("Player Movement")]
    public float moveSpeed = 7f;
    public float distance;
    public float FallingConst = 5f;

    private float lastYPosition;
    private float wallDirection = 0f;
    public float grappleSpeed = 20f;
    public float grappleLifetime = 1f;

    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int damage = 10;
    public int luck = 1;

    [Header("Wall Jumping System")]
    bool isTouchingWall = false;
    bool isWallSliding = false;
    private bool canMove = true;
    public float wallSlidingSpeed;
    public float wallJumpDuration;
    public float jumpAngle = 45f;
    public float wallJumpForce;
    bool wallJumping;
    private bool wantsToJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();
        lastYPosition = transform.position.y;
    }

    void Update()
    {
        if (isTouchingWall && Input.GetKeyDown(KeyCode.Space))
        {
            wantsToJump = true;
        }
    }

    void FixedUpdate()
    {
        float moveInputx = Input.GetAxis("Horizontal");
        float moveInputy = Input.GetAxis("Vertical");

        if (canMove)
        {
            if (isTouchingWall)
            {
                isWallSliding = true;

            }
            else
            {
                isWallSliding = false;
            }

            if (wallJumping)
            {
                float jumpAngleRadians = jumpAngle * Mathf.Deg2Rad;
                Vector2 jumpDirection = new Vector2(Mathf.Cos(jumpAngleRadians), Mathf.Sin(jumpAngleRadians));

                jumpDirection.x = Mathf.Abs(jumpDirection.x) * wallDirection;
                rb.velocity = new Vector2(jumpDirection.x * wallJumpForce, jumpDirection.y * wallJumpForce);
            }
            else if (isWallSliding)
            {
                rb.velocity = new Vector2(0f, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));

                if (wantsToJump)
                {
                    Jump();
                }
            }
            else
            {
                rb.velocity = new Vector2(moveInputx * moveSpeed, moveInputy != 0 ? moveInputy * moveSpeed : -FallingConst);

                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.localScale = new Vector3(7, 7, 1);
            }

            float currentYPosition = transform.position.y;

            if (currentYPosition < lastYPosition)
            {
                distance += lastYPosition - currentYPosition;
            }

            lastYPosition = currentYPosition;

            float verticalExtent = Camera.main.orthographicSize;
            float minY = Camera.main.transform.position.y - (verticalExtent - 5);
            float maxY = Camera.main.transform.position.y + (verticalExtent - 2);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);

            if (Input.GetKeyDown(KeyCode.J))
            {
                FireGrapplingHook();
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }


    void Jump()
    {
        wallJumping = true;
        isTouchingWall = false;
        wantsToJump = false;

        Invoke("StopWallJump", wallJumpDuration);




        // rb.AddForce(jumpDirection.normalized * jumpForce, ForceMode2D.Impulse);
        // transform.rotation = Quaternion.identity;
    }

    void StopWallJump()
    {
        wallJumping = false;

        collider.SetPath(0, new Vector2[58] {
            new Vector2(0.0552864634f, 0.2803506f),
            new Vector2(0.0550761372f, 0.299890578f),
            new Vector2(0.04470165f, 0.299836069f),
            new Vector2(0.04524698f, 0.330095351f),
            new Vector2(0.0361101851f, 0.329888254f),
            new Vector2(0.0355839431f, 0.339785784f),
            new Vector2(0.0148250069f, 0.340248972f),
            new Vector2(0.0156925954f, 0.3302632f),
            new Vector2(0.00488289958f, 0.33017382f),
            new Vector2(-0.00490501756f, 0.320395142f),
            new Vector2(-0.0143141048f, 0.3201608f),
            new Vector2(-0.0141140707f, 0.3300757f),
            new Vector2(-0.0353403278f, 0.330305725f),
            new Vector2(-0.0347424559f, 0.320226222f),
            new Vector2(-0.045871377f, 0.320232719f),
            new Vector2(-0.04521325f, 0.290394753f),
            new Vector2(-0.05517704f, 0.2905533f),
            new Vector2(-0.05551745f, 0.270272166f),
            new Vector2(-0.06504053f, 0.270251453f),
            new Vector2(-0.06528434f, 0.229420245f),
            new Vector2(-0.0549932644f, 0.229712173f),
            new Vector2(-0.05553746f, 0.180211529f),
            new Vector2(-0.175243586f, 0.1808208f),
            new Vector2(-0.17521663f, 0.149605229f),
            new Vector2(-0.16562295f, 0.1496776f),
            new Vector2(-0.165343389f, 0.13973105f),
            new Vector2(-0.155070588f, 0.139949039f),
            new Vector2(-0.1548189f, 0.129677236f),
            new Vector2(-0.144407183f, 0.129680961f),
            new Vector2(-0.144315124f, 0.139540315f),
            new Vector2(-0.0857621f, 0.139176279f),
            new Vector2(-0.08557672f, 0.1296526f),
            new Vector2(-0.03556893f, 0.129227549f),
            new Vector2(-0.0352877043f, 0.06971218f),
            new Vector2(-0.0250399448f, 0.06945452f),
            new Vector2(-0.0250795223f, 0.0598905236f),
            new Vector2(-0.0156539157f, 0.0599504672f),
            new Vector2(-0.0153169436f, 0.0494862124f),
            new Vector2(0.025364574f, 0.0497826673f),
            new Vector2(0.0251459926f, 0.0595907979f),
            new Vector2(0.0353552848f, 0.059666004f),
            new Vector2(0.0354882665f, 0.0691755f),
            new Vector2(0.0456979163f, 0.06934117f),
            new Vector2(0.04552823f, 0.129455358f),
            new Vector2(0.08533574f, 0.129193753f),
            new Vector2(0.08570391f, 0.139756113f),
            new Vector2(0.154444963f, 0.139598086f),
            new Vector2(0.154488578f, 0.129637361f),
            new Vector2(0.165537953f, 0.129384488f),
            new Vector2(0.165323287f, 0.139413878f),
            new Vector2(0.175042644f, 0.1397692f),
            new Vector2(0.175366551f, 0.149668232f),
            new Vector2(0.185f, 0.149641246f),
            new Vector2(0.185000017f, 0.170215935f),
            new Vector2(0.175468817f, 0.169782147f),
            new Vector2(0.175483733f, 0.180244222f),
            new Vector2(0.06477666f, 0.180611655f),
            new Vector2(0.06510662f, 0.2802139f),
        });

        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = new Vector3(7, 7, 1);
        animator.SetBool("onWall", false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && !wallJumping)
        {
            collider.SetPath(0, new Vector2[97] {
                new Vector2(0.0564007f, 0.339855939f),
                new Vector2(0.0553022027f, 0.349401683f),
                new Vector2(0.0449876748f, 0.350634217f),
                new Vector2(0.0459006131f, 0.36087805f),
                new Vector2(-0.03339358f, 0.36f),
                new Vector2(-0.07583374f, 0.360851735f),
                new Vector2(-0.07591887f, 0.329206139f),
                new Vector2(-0.0162781328f, 0.329100132f),
                new Vector2(-0.01614993f, 0.29208833f),
                new Vector2(-0.0445675179f, 0.290742964f),
                new Vector2(-0.0460564755f, 0.280605f),
                new Vector2(-0.0557907633f, 0.280521184f),
                new Vector2(-0.0552877672f, 0.2708075f),
                new Vector2(-0.07411793f, 0.270668954f),
                new Vector2(-0.07495103f, 0.2613131f),
                new Vector2(-0.0857018f, 0.2603367f),
                new Vector2(-0.0848519355f, 0.251195848f),
                new Vector2(-0.09539549f, 0.2507543f),
                new Vector2(-0.09525122f, 0.240141273f),
                new Vector2(-0.106041223f, 0.239648238f),
                new Vector2(-0.105643407f, 0.2310732f),
                new Vector2(-0.11448396f, 0.231138736f),
                new Vector2(-0.1155197f, 0.220935479f),
                new Vector2(-0.134435385f, 0.220196426f),
                new Vector2(-0.135425553f, 0.210898638f),
                new Vector2(-0.145993233f, 0.2099195f),
                new Vector2(-0.146236271f, 0.191360041f),
                new Vector2(-0.156063616f, 0.190833345f),
                new Vector2(-0.15578106f, 0.179940477f),
                new Vector2(-0.164765209f, 0.180402189f),
                new Vector2(-0.164779514f, 0.169425532f),
                new Vector2(-0.155648217f, 0.169425547f),
                new Vector2(-0.154964164f, 0.15921931f),
                new Vector2(-0.1074167f, 0.159627065f),
                new Vector2(-0.114974305f, 0.1012394f),
                new Vector2(-0.116859585f, 0.0814063f),
                new Vector2(-0.124711722f, 0.0807307661f),
                new Vector2(-0.126023442f, 0.06060682f),
                new Vector2(-0.13529411f, 0.0597277321f),
                new Vector2(-0.135773391f, 0.04114996f),
                new Vector2(-0.14712064f, 0.0399169177f),
                new Vector2(-0.14642784f, 0.008715294f),
                new Vector2(-0.13610433f, 0.008289752f),
                new Vector2(-0.135286763f, -0.0007843226f),
                new Vector2(-0.104472868f, -0.000216889312f),
                new Vector2(-0.104478173f, 0.008539951f),
                new Vector2(-0.09286447f, 0.009576048f),
                new Vector2(-0.09277877f, 0.0302087888f),
                new Vector2(-0.0836921558f, 0.03880221f),
                new Vector2(-0.034270864f, 0.03858068f),
                new Vector2(-0.0335046574f, 0.04950631f),
                new Vector2(-0.0252570044f, 0.04889664f),
                new Vector2(-0.0244455617f, 0.0595269762f),
                new Vector2(-0.0051728976f, 0.05861839f),
                new Vector2(-0.00358775351f, 0.0708586648f),
                new Vector2(0.00592337456f, 0.06880894f),
                new Vector2(0.00531316f, 0.0793762654f),
                new Vector2(0.0249108933f, 0.0786645561f),
                new Vector2(0.0254389569f, 0.08955327f),
                new Vector2(0.0363098122f, 0.08898141f),
                new Vector2(0.0359355174f, 0.0990436748f),
                new Vector2(0.0460128449f, 0.0980737954f),
                new Vector2(0.0461545326f, 0.10973689f),
                new Vector2(0.05517629f, 0.10926605f),
                new Vector2(0.0560607575f, 0.119761772f),
                new Vector2(0.0647693f, 0.119087867f),
                new Vector2(0.06574804f, 0.1302592f),
                new Vector2(0.0559471324f, 0.129900634f),
                new Vector2(0.05595613f, 0.139418975f),
                new Vector2(0.0461129844f, 0.139886692f),
                new Vector2(0.04542484f, 0.151231185f),
                new Vector2(0.0360477678f, 0.149709389f),
                new Vector2(0.03627597f, 0.160019428f),
                new Vector2(0.0264493152f, 0.161110565f),
                new Vector2(0.026045911f, 0.1702187f),
                new Vector2(0.005019577f, 0.170078784f),
                new Vector2(0.00536194677f, 0.179267168f),
                new Vector2(0.0165553559f, 0.178370163f),
                new Vector2(0.01605577f, 0.189008668f),
                new Vector2(0.0259126723f, 0.188594639f),
                new Vector2(0.0264457781f, 0.199137241f),
                new Vector2(0.0366195738f, 0.19882156f),
                new Vector2(0.0374342836f, 0.209162936f),
                new Vector2(0.04633288f, 0.208695367f),
                new Vector2(0.0458354726f, 0.21930553f),
                new Vector2(0.0559912845f, 0.217965066f),
                new Vector2(0.05571622f, 0.229502216f),
                new Vector2(0.0670980439f, 0.227884367f),
                new Vector2(0.06690023f, 0.239128053f),
                new Vector2(0.07687429f, 0.238420978f),
                new Vector2(0.07706013f, 0.2479306f),
                new Vector2(0.0857664943f, 0.247482121f),
                new Vector2(0.0866632f, 0.282640219f),
                new Vector2(0.07607581f, 0.280352235f),
                new Vector2(0.075931944f, 0.2910067f),
                new Vector2(0.06620569f, 0.2914203f),
                new Vector2(0.06626815f, 0.3400416f),
            });

            isTouchingWall = true;
            animator.SetBool("onWall", true);
            wallDirection = collision.GetContact(0).normal.x;

            if (transform.localPosition.x < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, 7, 1);
                transform.localPosition = new Vector3(-9.48f, transform.localPosition.y, transform.localPosition.z);
            }
            else if (transform.localPosition.x > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, -90);
                transform.localScale = new Vector3(7, -7, 1);
                transform.localPosition = new Vector3(9.48f, transform.localPosition.y, transform.localPosition.z);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "obstacle":
                Debug.Log("Player hit an obstacle!");
                break;
            case "EdgeCollider":
                Debug.Log("Player hit the edge!");
                //canMove = false;
                gameController.EndLevel();
                //transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                //camera.GetComponent<CameraController>().ResetCameraPosition();
                //canMove = true;
                break;
            case "Enemy":
                Debug.Log("PLayer hit enemy!");
                break;
            case "PowerUp":
                Debug.Log("Player hit power up!");
                int statToUpgrade = UnityEngine.Random.Range(0, 4);
                switch (statToUpgrade)
                {
                    case 0:
                        health += 10;
                        maxHealth += 10;
                        break;
                    case 1:
                        damage += 5;
                        break;
                    case 2:
                        luck += 1;
                        break;
                    case 3:
                        moveSpeed += 1;
                        break;
                    default:
                        break;
                }

                break;
            default:
                break;
        }

    }

    public void ResetPlayerPosition()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool CanMove()
    {
        return canMove;
    }

    public void MoveToEnemy(Vector3 enemyPosition)
    {
        StartCoroutine(MoveTowardsPosition(enemyPosition));
    }

    IEnumerator MoveTowardsPosition(Vector3 position)
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, position);
        float journeyTime = journeyLength / grappleSpeed;

        while (Time.time - startTime < journeyTime)
        {
            float fracJourney = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Lerp(startPosition, position, fracJourney);
            yield return null;
        }
    }

    void FireGrapplingHook()
    {
        float offsetDistance = 5.0f;
        Vector3 spawnPosition = transform.position + Vector3.down * offsetDistance;
        GameObject grapple = Instantiate(grapplingHookPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D rb = grapple.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, grappleSpeed);
        Debug.Log("disparo y destruyendo");
        Destroy(grapple, 2f);
        Debug.Log("se destruyo en teoria");
    }
}
