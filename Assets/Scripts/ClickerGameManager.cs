using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGameManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject clickerPanel;
    public Button startClickerButton;
    public Button clickerButton;
    public Button backButton;
    public TMP_Text objectNameText;
    public Image objectSpriteImage;
    public TMP_Text clickCountText;

    [Header("Clicker Settings")]
    public int clicksToUnlock = 10;
    private int currentClicks = 0;
    private bool unlocked = false;

    [Header("Furniture List")]
    public FurnitureData[] furniturePool;
    private FurnitureData currentFurniture;

    private void Start()
    {
        clickerPanel.SetActive(false);
        startClickerButton.onClick.AddListener(OpenClickerGame);
        clickerButton.onClick.AddListener(OnClickerPressed);
        backButton.onClick.AddListener(CloseClickerGame);
    }

    void OpenClickerGame()
    {
        clickerPanel.SetActive(true);
        ResetClicker();
        SelectRandomFurniture();
    }

    void CloseClickerGame()
    {
        clickerPanel.SetActive(false);
    }

    void OnClickerPressed()
    {
        if (unlocked) return;

        currentClicks++;
        clickCountText.text = $"Clicks: {currentClicks}/{clicksToUnlock}";

        if (currentClicks >= clicksToUnlock)
        {
            UnlockFurniture();
        }
    }

    void ResetClicker()
    {
        currentClicks = 0;
        unlocked = false;
        clickCountText.text = $"Clicks: {currentClicks}";
        objectNameText.text = "";
        objectSpriteImage.sprite = null;
        objectSpriteImage.enabled = false; 
    }

    void SelectRandomFurniture()
    {
        currentFurniture = furniturePool[Random.Range(0, furniturePool.Length)];
    }

    void UnlockFurniture()
    {
        unlocked = true;
        objectNameText.text = currentFurniture.furnitureName;
        objectSpriteImage.sprite = currentFurniture.furnitureSprite;
        objectSpriteImage.enabled = true; 

        
        Debug.Log($"Unlocked: {currentFurniture.furnitureName}");
    }
}
