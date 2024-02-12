/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAppleController : MonoBehaviour
{
    Animator anim;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    EnemyController enemyController;

    bool isShooting;
    bool isMelee;

    [SerializeField] bool enableAI;
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform antBulletShootPos;
    [SerializeField] GameObject antBulletPrefab;

    publci enum BossAppState { Shooting, Melee };
    public BossAppState bossAppState = BossAppState.Shooting;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        anim = enemyController.GetComponent<Animator>();
        box2d = enemyController.GetComponent<BoxCollider2D>();
        rb2d = enemyController.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isShooting = true;
        isMelee = false;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float playerDistance = Vector2.Distance(player.transform.position, transform.position);

        if (enableAI)
        {
            switch (bossAppState)
            {
                case BossAppState.Shooting:
                    anim.Play("Shooting");
                    if (playerDistance > viewDistance)
                    {
                        isShooting = true;
                        isMelee = false;
                    }
                    break;

                case BossAppState.Melee:
                    anim.Play("Melee");

                    isMelee = true;

            }
        }
    }

    private void ShootBullet()
    {
        GameObject antBullet = Instantiate(antBulletPrefab, antBulletShootPos.position, Quaternion.identity);
        antBullet.name = antBulletPrefab.name;
        antBullet.GetComponent<AntBullet>().SetDamageValue(bulletDamage);
        antBullet.GetComponent<AntBullet>().SetBulletSpeed(bulletSpeed);
        antBullet.GetComponent<AntBullet>().SetBulletDirection((isFacingRight) ? Vector2.right : Vector2.left);
        antBullet.GetComponent<AntBullet>().Shoot();
    }

}
*/