using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public PlayerController playerController;
    private CollisionController collisionController;
    public List<SimpleEnemyController> enemies = new List<SimpleEnemyController>();
    public float colliderRadius;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask parryLayer;



    

    void Start()
    {
<<<<<<< HEAD
        attackPoint = playerController.attackPoint;
        attackRange = playerController.attackRange/2;
    }

=======
        collisionController = playerController.GetComponent<CollisionController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            collisionController.skipTrigger = true;

            SimpleEnemyController enemy = other.GetComponent<SimpleEnemyController>();

            if (enemy != null)
            {
                enemies.Add(enemy);
            }
        }
    }
>>>>>>> 442af4a0d64c42b3a7d1494681ba98eb8a683fbb

    public void Parry()
    {
        Debug.Log("Parrying");
        attackPoint.GetChild(2).gameObject.SetActive(true);
        playerController.ShowParry();
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, parryLayer);
        foreach (Collider2D entity in hitEntities)
        {
            Debug.Log("We hit " + entity.name);
            if (entity.tag == "Enemy")
            {
                entity.GetComponent<SimpleEnemyController>().Die();
                StartCoroutine(playerController.parryBoost());
                if (playerController.inDeathDoor)
                {
                    playerController.RegisterEnemyKill();
                }

            }
            if (entity.tag == "Boss")
            {
                Debug.Log("Boss hit");
                entity.GetComponent<BossController>().TakeDamage(playerController.damage);
                StartCoroutine(playerController.parryBoost());
                if (playerController.inDeathDoor)
                {
                    playerController.RegisterEnemyKill();
                }
            }
            if (entity.tag == "Proyectile")
            {
                GameObject proyectileCreator = entity.GetComponent<Projectile>().projectileCreator;
                if (proyectileCreator.tag == "Enemy")
                {
                    proyectileCreator.GetComponent<SimpleEnemyController>().Die();
                    Destroy(entity.gameObject);
                }
                if (proyectileCreator.tag == "Boss")
                {
                    proyectileCreator.GetComponent<BossController>().TakeDamage(playerController.damage);
                    Destroy(entity.gameObject);
                }
                Destroy(entity.gameObject);
            }
        }
        attackPoint.GetChild(2).gameObject.SetActive(false);
    }

}
