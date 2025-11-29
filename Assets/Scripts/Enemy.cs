using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 3f;     
    public float detectionRange = 5f; 
    public float rotationSpeed = 500f;
    public float stopDistance = 0.7f;

    private Transform player;
    private Rigidbody2D rb;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

        UpdateHpBar();
    }

    private void FixedUpdate()
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
                dir.Normalize();
                rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
            }
            // Rotate enemy
            float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
        }

        // Keep Slider not rotated
        hpSlider.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        hpSlider.transform.rotation = Quaternion.identity;

        //If player is inside, calculate damage when it meets the damage interval
        if (isPlayerInside)
        {
            damageTimer += Time.fixedDeltaTime;
            if (damageTimer >= damageInterval)
            {
                player.GetComponent<PlayerCombat>().TakeDamage(enemyDamage);
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
    }

    public void UpdateHpBar()
    {
        hpSlider.value = hp / maxHp;
    }
}
