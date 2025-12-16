using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasionManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static EvasionManager instance;

    [SerializeField]
    private List<GameObject> evasionPointList;

    public Color disableColor;
    public Color activatedColor;

    public float waitTime;

    [Header("For new enemies")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnTranforms;
    [SerializeField] private float spawnInterval = 2f;



    private void Awake()
    {
        if(instance ==  null)
        {
            instance = this;
        }
    }

    private bool isSpawning = false;
    public void SetRandomEvasionPoint()
    {
        int rand = Random.Range(0, evasionPointList.Count-1);

        if(evasionPointList[rand])
        {
            evasionPointList[rand].GetComponent<EvasionPoint>().ActivateEvasionPoint();
        }
        if (!isSpawning)
        {
            StartCoroutine(SpawnEnemiesRoutine());
            isSpawning = true;
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (enemyPrefab != null && spawnTranforms.Count > 0)
            {
                int randIndex = Random.Range(0, spawnTranforms.Count);
                Transform spawnPoint = spawnTranforms[randIndex];
                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity,null);
            }
        }
    }
}
