using UnityEngine;

public class InteractableWeapon : Interactable
{
    public int weaponId;
    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerCombat>().ChangeWeapon(weaponId);
        Destroy(gameObject);
    }
}
