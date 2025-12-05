using UnityEngine;

public class ExpPickup : Interactable
{
    public float EXP;
    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerCombat>().AddExp(EXP);
        Destroy(gameObject);
    }
}
