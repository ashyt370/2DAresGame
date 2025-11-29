using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Input")]
    public InputActionAsset inputActions;
    private InputAction attackAction;

    [SerializeField]
    private GameObject currentWeapon;

    [Header("Attack Charge")]
    private float currentChargePer = 0f;
    private float attackStartTime = 0f;
    [SerializeField]
    private float maxChargeTime = 2f;
    private float chargedTime = 0f;

    [Header("Player Stats")]
    [SerializeField] private float playerHp = 100f;
    [SerializeField] private float MaxHp = 100f;

    [SerializeField]
    private Slider HPSlider;


    private void Awake()
    {
        // Bind combat action
        var actionMap = inputActions.FindActionMap("Player");
        attackAction = actionMap.FindAction("Attack");

        attackAction.started += Attack;
        attackAction.canceled += AttackReleased;

        // Hide weapon range
        currentWeapon.SetActive(false);
    }

    private void Update()
    {
        // If player is attacking, calculate charging
        if(currentWeapon.activeSelf)
        {
            if (Time.time - attackStartTime > maxChargeTime)
            {
                chargedTime = maxChargeTime;
            }
            else
            {
                chargedTime = Time.time - attackStartTime;
            }
            currentChargePer = chargedTime / maxChargeTime;
            
            // Change charging color
            Color color = currentWeapon.GetComponent<SpriteRenderer>().color;
            color.a = Mathf.Lerp(0, 0.8f, currentChargePer);
            currentWeapon.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        currentWeapon.SetActive(true);
        attackStartTime = Time.time;
    }

    private void AttackReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Charge Per:  " + currentChargePer);

        // Cause damage to all the enemies in range
        if(currentWeapon.GetComponent<Weapon>().enemyInRangeList.Count > 0)
        {
            foreach(Enemy e in currentWeapon.GetComponent<Weapon>().enemyInRangeList)
            {
                e.TakeDamage(Mathf.Lerp(currentWeapon.GetComponent<Weapon>().MinDamage, currentWeapon.GetComponent<Weapon>().MaxDamage, currentChargePer));
            }
        }

        currentWeapon.GetComponent<Weapon>().ClearEnemey();
        currentWeapon.SetActive(false);
    }

    public void TakeDamage(float d)
    {
        playerHp -= d;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        HPSlider.value = playerHp / MaxHp;
    }
}
