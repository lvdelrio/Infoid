using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryController : MonoBehaviour
{
    public PlayerController playerController;
    public List<SimpleEnemyController> enemies = new List<SimpleEnemyController>();
    public float colliderRadius;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask parryLayer;

    void Start()
    {
        attackPoint = playerController.attackPoint;
        attackRange = playerController.attackRange/2;
    }

    public void Parry()
    {
        Debug.Log("Parrying");
        attackPoint.GetChild(2).gameObject.SetActive(true);
        playerController.ShowParry();
        Collider2D[] hitEntities = Physics2D.OverlapCircleAll(attackPoint.position, attackRange*2, parryLayer);
        foreach (Collider2D entity in hitEntities)
        {
            Debug.Log("We hit " + entity.name);
            if (entity.tag == "Enemy")
            {
                entity.GetComponent<SimpleEnemyController>().Die();
                playerController.TriggerParryBoost();
                if (playerController.inDeathDoor)
                {
                    playerController.RegisterEnemyKill();
                }

            }
            if (entity.tag == "Boss")
            {
                Debug.Log("Boss hit");
                entity.GetComponent<BossController>().TakeDamage(playerController.damage, transform.position);
                playerController.TriggerParryBoost();
                if (playerController.inDeathDoor)
                {
                    playerController.RegisterEnemyKill();
                }
            }
            if (entity.tag == "Proyectile")
            {
                GameObject proyectileCreator = entity.GetComponent<Projectile>().projectileCreator;
                playerController.TriggerParryBoost();
                if (proyectileCreator.tag == "Enemy")
                {
                    proyectileCreator.GetComponent<SimpleEnemyController>().Die();
                    Destroy(entity.gameObject);
                }
                if (proyectileCreator.tag == "Boss")
                {
                    //proyectileCreator.GetComponent<BossController>().TakeDamage(playerController.damage, transform.position);
                    Destroy(entity.gameObject);
                }
                Destroy(entity.gameObject);
            }
        }
        attackPoint.GetChild(2).gameObject.SetActive(false);
    }

}
