using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            UIManager.instance.ShowInteractionHint();
            GameObject.Find("Player").GetComponent<PlayerCombat>().currentInteractable = this;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.instance.HideInteractionHint();
            GameObject.Find("Player").GetComponent<PlayerCombat>().currentInteractable = null;
        }
            
    }

}
