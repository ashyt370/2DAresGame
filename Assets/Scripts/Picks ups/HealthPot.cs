using UnityEngine;

public class HealthPot : Interactable
{
    public float recoverAmount = 50;
    public override void Interact()
    {
        GameObject.Find("Player").GetComponent<PlayerCombat>().RecoverHealth(recoverAmount);
        Destroy(gameObject);
    }
}
