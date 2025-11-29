using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float MinDamage;
    public float MaxDamage;

    [SerializeField]
    private float chargeMaxTime;

    [SerializeField]
    private float staminaCost;

    // Store all the enemies in the weapon range
    public List<Enemy> enemyInRangeList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && !enemyInRangeList.Contains(collision.GetComponent<Enemy>()))
        {
            enemyInRangeList.Add(collision.GetComponent<Enemy>());
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.GetComponent<Enemy>();
            if (enemyInRangeList.Contains(e))
            {
                enemyInRangeList.Remove(e);
            }
        }
    }

    public void ClearEnemey()
    {
        enemyInRangeList.Clear();
    }



}
