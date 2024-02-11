using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetPotController : MonoBehaviour
{
    Animator anim;
    BoxCollider2D box2d;
    Rigidbody2D rb2d;
    EnemyController enemyController;

    bool isFacingRight;
    bool doAttack;
    bool isShooting;

    float openTimer;
    float closedTimer;
    float shootTimer;
    float sleepTimer;

    int shootSequenceNum;
    int shootSequenceCount;

    [SerializeField] bool enableAI;
    [SerializeField] int bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] Transform antBulletShootPos;
    [SerializeField] GameObject antBulletPrefab;

    public float openDelay = 0.5f;
    public float closedDelay = 0.5f;
    public float shootDelay = 0.25f;
    public float sleepDelay = 2f;
    public float viewDistance = 2f;
    public int shootSequenceMax = 5;

    public enum MetPotState { Closed, Open };
    public MetPotState metPotState = MetPotState.Closed;

    void Awake()
    {
        enemyController = GetComponent<EnemyController>();
        anim = enemyController.GetComponent<Animator>();
        box2d = enemyController.GetComponent<BoxCollider2D>();
        rb2d = enemyController.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        isFacingRight = false;
        isShooting = false;

        openTimer = openDelay;
        closedTimer = closedDelay;
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        float playerDistance = Vector2.Distance(player.transform.position, transform.position);

        if (enableAI)
        {
            switch (metPotState)
            {
                case MetPotState.Closed:
                    anim.Play("Look");
                    if (playerDistance < viewDistance)
                    {
                        if (!doAttack)
                        {
                            shootSequenceCount = 0;
                            shootSequenceNum = Random.Range(1, shootSequenceMax + 1);
                            closedTimer = closedDelay;
                            doAttack = true;
                        }
                        else
                        {
                            if (shootSequenceCount < shootSequenceNum)
                            {
                                closedTimer -= Time.deltaTime;
                                if (closedTimer < 0)
                                {
                                    isShooting = false;
                                    openTimer = openDelay;
                                    shootTimer = shootDelay;
                                    metPotState = MetPotState.Open;
                                }
                            }
                            else
                            {
                                sleepTimer -= Time.deltaTime;
                                if (sleepTimer < 0)
                                {
                                    doAttack = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        doAttack = false;
                    }
                    break;
                case MetPotState.Open:
                    
                    anim.Play("Shoot");

                    if (shootSequenceCount < shootSequenceNum)
                    {
                        openTimer -= Time.deltaTime;
                        shootTimer -= Time.deltaTime;

                        if (shootTimer < 0 && !isShooting)
                        {
                            ShootBullet();
                            isShooting = true;
                        }

                        if (openTimer < 0)
                        {
                            isShooting = false;
                            shootSequenceCount++;
                            closedTimer = closedDelay;
                            metPotState = MetPotState.Closed;
                            float variance = Random.Range(0, 5) / 10f;
                            if (variance > 0f)
                            {
                                closedTimer -= variance;
                            }

                            if (shootSequenceCount == shootSequenceNum)
                            {
                                sleepTimer = sleepDelay;
                            }
                        }
                    }
                    break;
            }
        }
    }

    public void EnableAI(bool enable)
    {
        this.enableAI = enable;
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

    private void StartInvincibleAnimation()
    {
        enemyController.Invincible(true);
    }

    private void StopInvincibleAnimation()
    {
        enemyController.Invincible(false);
    }

}
