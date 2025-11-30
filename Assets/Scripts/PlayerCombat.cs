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
    private GameObject currentWeapon;

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


    private void PauseGame(InputAction.CallbackContext obj)
    {
        UIManager.instance.ShowPauseMenu();
    }

    public void ChangeWeapon(int weaponID)
    {
        currentWeapon = weaponList[weaponID];
        currentChargePer = 0;
        chargedTime = 0;
    }

    private void Interact(InputAction.CallbackContext obj)
    {
        if (currentInteractable)
        {
            currentInteractable.Interact();
        }
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

    private void CostStamina()
    {
        playerStamina -= currentWeapon.GetComponent<Weapon>().staminaCostPerSec * Time.deltaTime;
        UIManager.instance.UpdatePlayerStamina(playerStamina / maxStamina);
    }

    private void RegenStamina(float s)
    {
        playerStamina += s * Time.deltaTime;
        UIManager.instance.UpdatePlayerStamina(playerStamina / maxStamina);
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if(currentWeapon)
        {
            currentWeapon.SetActive(true);
            attackStartTime = Time.time;
        }
        
        
    }

    private void AttackReleased(InputAction.CallbackContext context)
    {
        if(!currentWeapon)
        {
            return;
        }
        // Cause damage to all the enemies in range
        if(currentWeapon.GetComponent<Weapon>().enemyInRangeList.Count > 0)
        {
            foreach(Enemy e in currentWeapon.GetComponent<Weapon>().enemyInRangeList)
            {
                e.TakeDamage(Mathf.Lerp(currentWeapon.GetComponent<Weapon>().minDamage, currentWeapon.GetComponent<Weapon>().maxDamage, currentChargePer));
            }
        }

        currentWeapon.GetComponent<Weapon>().ClearEnemey();
        currentWeapon.SetActive(false);
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


}
