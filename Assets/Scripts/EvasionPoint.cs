using UnityEngine;

public class EvasionPoint : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    private bool isActivated;
    private bool isInRange;

    private float startedTime;

    private void Awake()
    {
        if(GetComponent<SpriteRenderer>())
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        } 
    }
    private void Start()
    {
        DisableEvasionPoint();
    }


    // Active the evasion point
    public void ActivateEvasionPoint()
    {
        spriteRenderer.color = EvasionManager.instance.activatedColor;
        isActivated = true;
    }

    public void DisableEvasionPoint()
    {
        spriteRenderer.color = EvasionManager.instance.disableColor;
        isActivated = false;
    }

    private void Update()
    {
        if(isActivated && isInRange)
        {
            SpawnEnemies();
            if(Time.time - startedTime >= EvasionManager.instance.waitTime)
            {
                UIManager.instance.ShowGameVictory();
            }           
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (isActivated)
            {
                startedTime = Time.time;
            }
            isInRange = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
        }
    }


    private float spawnTimer;
    private float spawnInterval = 2f;

    public Transform enemySpawnPoint;
    public GameObject enemyPrefab;
    private void SpawnEnemies()
    {
        spawnTimer += Time.deltaTime;

        if(spawnTimer >= spawnInterval)
        {
            GameObject e = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity,null);
            spawnInterval = Random.Range(1f, 3f);
            spawnTimer = 0;
        }
    }
}
