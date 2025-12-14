using UnityEngine;

public class InteractableWeapon : Interactable
{
    public int weaponId;
    public override void Interact()
    {
        GameObject g = Instantiate(GameObject.Find("Player").GetComponent<PlayerCombat>().currentWeapon.GetComponent<Weapon>().weaponPrefab, GameObject.Find("Player").transform.position, Quaternion.identity, null);
        
        GameObject.Find("Player").GetComponent<PlayerCombat>().ChangeWeapon(weaponId);
        
        Destroy(gameObject);
    }
}
