using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public GameObject thisEnemy;
    [Header("Enemy Settings")]   
    public float detectionRange = 5f; 
    public float stopDistance = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private NavMeshAgent agent;

    [Header("Enemy Combat Stats")]
    [SerializeField]
    private float hp = 100;
    [SerializeField]
    private float maxHp = 100;
    [SerializeField]
    private Slider hpSlider;

    [Header("Enemy Combat Settings")]
    [SerializeField]
    private float enemyDamage = 20f;
    [SerializeField]
    private float damageInterval = 2f;
    private float damageTimer = 0f;
    private bool isPlayerInside = false;

    [Header("Enemy EXP")]
    public float gainedEXP = 100;

    public bool isWarningEnemy = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        UpdateHpBar();
    }

    private void FixedUpdate()
    {
        if(isWarningEnemy)
        {
            agent.SetDestination(player.position);
            hpSlider.transform.position = transform.position + new Vector3(0, 1.5f, 0);
            hpSlider.transform.rotation = Quaternion.identity;
        }
        else
        {
            ChasePlayer();
        }
        CheckDamage();
    }

    private void ChasePlayer()
    {
        // Get player distance
        Vector2 dir = player.position - transform.position;
        float distance = dir.magnitude;

        // Detect Player
        if (distance <= detectionRange)
        {
            // Avoid character shaking (when its too closed)
            if (distance > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            // Rotate enemy
            /*            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                        rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);*/
        }

        // Keep Slider not rotated
        hpSlider.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        hpSlider.transform.rotation = Quaternion.identity;
    }

    private void CheckDamage()
    {
        //If player is inside, calculate damage when it meets the damage interval
        if (isPlayerInside)
        {
            damageTimer += Time.fixedDeltaTime;
            if (damageTimer >= damageInterval)
            {
                player.GetComponentInParent<PlayerCombat>().TakeDamage(enemyDamage);
                damageTimer = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInParent<PlayerCombat>().TakeDamage(enemyDamage);
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInside = false;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        UpdateHpBar();

        //If enemy dies
        if(hp <= maxHp)
        {
            if(player.gameObject.GetComponentInParent<PlayerCombat>())
            {
                player.gameObject.GetComponentInParent<PlayerCombat>().AddExp(gainedEXP);
                player.gameObject.GetComponentInParent<PlayerCombat>().CancelAttack();
                player.gameObject.GetComponentInParent<PlayerCombat>().RemoveEnemyFromWeaponList(this);
                Destroy(thisEnemy);
            }

        }
    }

    public void UpdateHpBar()
    {
        hpSlider.value = hp / maxHp;
    }
}
