using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [Header("Player Input")]
    public InputActionAsset inputActions;
    private InputAction attackAction;



    [Header("Attack Charge")]
    private float currentChargePer = 0f;
    private float attackStartTime = 0f;
    [SerializeField]
    private float maxChargeTime = 2f;
    private float chargedTime = 0f;

    [Header("Player Stats")]
    [SerializeField] private float playerHp = 100f;
    [SerializeField] private float maxHp = 100f;

    [SerializeField] private float playerStamina = 100f;
    [SerializeField] private float maxStamina = 100f;

    [SerializeField]
    private float generalStaminaRegenPerSec = 30f;
    [SerializeField]
    private float attackStaminaRegenPerSec = 10f;

    [HideInInspector]
    public Interactable currentInteractable;

    [Header("Player Weapon")]
    [SerializeField] private List<GameObject> weaponList;
    [SerializeField]
    public GameObject currentWeapon;

    [Header("Player EXP")]
    [SerializeField]
    private float currentEXP;
    [SerializeField]
    private float maxEXP = 100;

    private void Awake()
    {
        // Bind combat action
        var actionMap = inputActions.FindActionMap("Player");
        attackAction = actionMap.FindAction("Attack");

        // Bind to attack input
        attackAction.started += Attack;
        attackAction.canceled += AttackReleased;

        // Bind to Interaction input
        inputActions.FindActionMap("Player").FindAction("Interact").performed += Interact;
        // Bind to Pause input
        inputActions.FindActionMap("Player").FindAction("Pause").performed += PauseGame;

        currentWeapon = weaponList[0];
        // Hide weapon range
        foreach (GameObject g in weaponList)
        {
            g.SetActive(false);
        }
    }


    public void ChangeWeapon(int weaponID)
    {
        currentWeapon = weaponList[weaponID];
        currentChargePer = 0;
        chargedTime = 0;
    }


    private void Update()
    {
        if(playerStamina <= 0)
        {
            currentWeapon.SetActive(false);
            RegenStamina(generalStaminaRegenPerSec);
            return;
        }
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

            CostStamina();
            RegenStamina(attackStaminaRegenPerSec);
            GetComponent<PlayerMovement>().SetPlayerSpeed(currentWeapon.GetComponent<Weapon>().playerSpeedWhenCharging);
        }
        // not attacking
        else
        {
            GetComponent<PlayerMovement>().ResetSpeed();
            RegenStamina(generalStaminaRegenPerSec);
        }
    }

    public void AddExp(float exp)
    {
        currentEXP += exp;
        UIManager.instance.UpdatePlayerEXP(currentEXP / maxEXP);

        //If exp is full
        if(currentEXP >= maxHp)
        {
            EvasionManager.instance.SetRandomEvasionPoint();
            UIManager.instance.ShowEvacuationHint();
        }
        
    }

    private void CostStamina()
    {
        Debug.Log("Cost Stamina");
        playerStamina -= currentWeapon.GetComponent<Weapon>().staminaCostPerSec * Time.deltaTime;
        UIManager.instance.UpdatePlayerStamina(playerStamina / maxStamina);
    }

    private void RegenStamina(float s)
    {
        playerStamina += s * Time.deltaTime;
        if(playerStamina >= maxStamina)
        {
            playerStamina = maxStamina;
        }
        UIManager.instance.UpdatePlayerStamina(playerStamina / maxStamina);
    }

    public void TakeDamage(float d)
    {
        playerHp -= d;
        UIManager.instance.UpdatePlayerHPBar(playerHp / maxHp);
        if(playerHp <= 0)
        {
            OnPlayerDead();
        }
    }

    public void OnPlayerDead()
    {
        UIManager.instance.ShowGameOver();
    }

    public void RecoverHealth(float f)
    {
        playerHp += f;
        if(playerHp > maxHp)
        {
            playerHp = maxHp;
        }
        UIManager.instance.UpdatePlayerHPBar(playerHp / maxHp);
    }

    public void CancelAttack()
    {
        currentWeapon.SetActive(false);
    }
    private void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            currentWeapon.SetActive(true);
            attackStartTime = Time.time;
        }
    }

    public void RemoveEnemyFromWeaponList(Enemy e)
    {
        currentWeapon.GetComponent<Weapon>().enemyInRangeList.Remove(e);
    }
    private void AttackReleased(InputAction.CallbackContext context)
    {
        if (!currentWeapon)
        {
            return;
        }


        List<Enemy> processEnemies = new List<Enemy>(currentWeapon.GetComponent<Weapon>().enemyInRangeList);

        // Cause damage to all the enemies in range
        if (processEnemies.Count > 0)
        {
            foreach (Enemy e in processEnemies)
            {
                if(e)
                {
                    e.TakeDamage(Mathf.Lerp(currentWeapon.GetComponent<Weapon>().minDamage, currentWeapon.GetComponent<Weapon>().maxDamage, currentChargePer));
                }
                
            }
        }

        currentWeapon.GetComponent<Weapon>().ClearEnemey();
        currentWeapon.SetActive(false);
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (currentInteractable)
        {
            currentInteractable.Interact();
        }
    }
    private void PauseGame(InputAction.CallbackContext obj)
    {
        UIManager.instance.ShowPauseMenu();
    }

}
