using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TriggerController : MonoBehaviour
{

    public Animator animator;
    public GameObject player;
    private PolygonCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = player.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("s"))
        {
            animator.SetBool("bool_falling_fast", true);

            collider.SetPath(0, new Vector2[48] {
                new Vector2(0.04501605f, 0.4304198f),
                new Vector2(0.0350709856f, 0.4304095f),
                new Vector2(0.0352517739f, 0.440264463f),
                new Vector2(-0.0155765852f, 0.440489531f),
                new Vector2(-0.0156325456f, 0.430524439f),
                new Vector2(-0.0252667312f, 0.430209845f),
                new Vector2(-0.0251096468f, 0.390011847f),
                new Vector2(-0.0353089757f, 0.390214026f),
                new Vector2(-0.0355594866f, 0.370298f),
                new Vector2(-0.04562933f, 0.370028377f),
                new Vector2(-0.0459680781f, 0.300880343f),
                new Vector2(-0.0650284737f, 0.300813854f),
                new Vector2(-0.06484717f, 0.2901169f),
                new Vector2(-0.075f, 0.290479839f),
                new Vector2(-0.07519002f, 0.1796596f),
                new Vector2(-0.06594088f, 0.179716289f),
                new Vector2(-0.06605233f, 0.149479911f),
                new Vector2(-0.0554408543f, 0.150232479f),
                new Vector2(-0.05449934f, 0.139488667f),
                new Vector2(-0.0449355841f, 0.139978588f),
                new Vector2(-0.04497959f, 0.119332336f),
                new Vector2(-0.0349022858f, 0.1194386f),
                new Vector2(-0.0354613066f, 0.0597396865f),
                new Vector2(-0.0255038086f, 0.05971571f),
                new Vector2(-0.0251709744f, 0.0495566241f),
                new Vector2(-0.0150750019f, 0.0499315523f),
                new Vector2(-0.0147262961f, 0.0394490473f),
                new Vector2(0.0254230853f, 0.0397326872f),
                new Vector2(0.0248759147f, 0.0501271933f),
                new Vector2(0.03556827f, 0.0493806f),
                new Vector2(0.0356750637f, 0.0594789349f),
                new Vector2(0.0453844629f, 0.06f),
                new Vector2(0.04547525f, 0.119821176f),
                new Vector2(0.0554044768f, 0.1199961f),
                new Vector2(0.0552937873f, 0.139304474f),
                new Vector2(0.07532905f, 0.139761686f),
                new Vector2(0.07615817f, 0.149825931f),
                new Vector2(0.0855568f, 0.15016982f),
                new Vector2(0.08546641f, 0.179492861f),
                new Vector2(0.09523292f, 0.179681391f),
                new Vector2(0.0955181345f, 0.2906711f),
                new Vector2(0.08512911f, 0.2910264f),
                new Vector2(0.08472448f, 0.3004961f),
                new Vector2(0.06580321f, 0.300858f),
                new Vector2(0.06538833f, 0.3706697f),
                new Vector2(0.0551835634f, 0.370644063f),
                new Vector2(0.0551967472f, 0.390079945f),
                new Vector2(0.0456862934f, 0.390323937f)
            });
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("bool_falling_fast", false);

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
        }
    }
}
