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
    private GameObject interactionPopup;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject gameOverScreen;

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
    }

    public void UpdatePlayerHPBar(float d)
    {
        playerHPSlider.value = d;
    }

    public void UpdatePlayerStamina(float d)
    {
        playerStaminaSlider.value = d;
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

}
