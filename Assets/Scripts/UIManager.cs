using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Elements")]
    [SerializeField] 
    private Slider playerHPSlider;
    [SerializeField]
    private Slider playerStaminaSlider;
    [SerializeField]
    private Slider playerEXPSlider;
    [SerializeField]
    private GameObject interactionPopup;
    [SerializeField]
    private GameObject evacuationHint;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject victoryScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        InitUI();
    }

    public void InitUI()
    {
        interactionPopup.SetActive(false);
        pauseScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        victoryScreen.SetActive(false);
        evacuationHint.SetActive(false);

    }

    public void UpdatePlayerHPBar(float d)
    {
        playerHPSlider.value = d;
    }

    public void UpdatePlayerStamina(float d)
    {
        playerStaminaSlider.value = d;
    }

    public void UpdatePlayerEXP(float d)
    {
        playerEXPSlider.value = d;
    }

    public void ShowEvacuationHint()
    {
        evacuationHint.SetActive(true);
        //StartCoroutine(HideEvcuationHint());
    }

    IEnumerator HideEvcuationHint()
    {
        yield return new WaitForSeconds(2f);
        evacuationHint.SetActive(false);
    }

    public void ShowInteractionHint()
    {
        interactionPopup.SetActive(true);
    }

    public void HideInteractionHint()
    {
        interactionPopup.SetActive(false);
    }

    public void ShowPauseMenu()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void HidePauseMenu()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void ShowGameVictory()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;
    }

}
