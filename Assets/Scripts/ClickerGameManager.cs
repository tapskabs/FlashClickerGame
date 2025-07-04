using System.Collections.Generic;
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
    public InventoryManager inventoryManager;
    [Header("Furniture List")]
    public FurnitureData[] allFurnitureItems;
    public FurnitureData[] allFurniture;
    private List<FurnitureData> remainingItems = new List<FurnitureData>();
    private int nextUnlockThreshold = 0;
    
    private List<FurnitureData> remainingFurniture = new List<FurnitureData>();
    private void Start()
    {
        clickerPanel.SetActive(false);
        startClickerButton.onClick.AddListener(OpenClickerGame);
        clickerButton.onClick.AddListener(OnClickerPressed);
        backButton.onClick.AddListener(CloseClickerGame);
        remainingItems = new List<FurnitureData>(allFurniture);
        
        remainingFurniture = new List<FurnitureData>(allFurniture);
        ClickerGameData.Initialize(allFurnitureItems);
    }
    public void UnlockRandomFurniture()
    {
        if (remainingFurniture.Count == 0)
        {
            Debug.Log("All furniture unlocked!");
            return;
        }

        int rand = Random.Range(0, remainingFurniture.Count);
        FurnitureData item = remainingFurniture[rand];
        remainingFurniture.RemoveAt(rand);

        inventoryManager.UnlockFurniture(item);
        Debug.Log("Unlocked: " + item.furnitureName);
    }

    void OpenClickerGame()
    {
        clickerPanel.SetActive(true);
        objectNameText.text = "";
        objectSpriteImage.sprite = null;
        objectSpriteImage.enabled = false;

        if (ClickerGameData.RemainingFurniture.Count > 0)
        {
            GenerateNewThreshold();
            UpdateClickText();
        }
        else
        {
            clickCountText.text = "All items unlocked!";
        }
    }

    void CloseClickerGame()
    {
        clickerPanel.SetActive(false);
    }

    void OnClickerPressed()
    {
        if (ClickerGameData.RemainingFurniture.Count == 0)
        {
            clickCountText.text = "All items unlocked!";
            return;
        }

        ClickerGameData.TotalClicks++;
        UpdateClickText();

        if (ClickerGameData.TotalClicks >= nextUnlockThreshold)
        {
            UnlockFurniture();
            if (ClickerGameData.RemainingFurniture.Count > 0)
                GenerateNewThreshold();
            else
                clickCountText.text = "All items unlocked!";
        }
    }

    void GenerateNewThreshold()
    {
        nextUnlockThreshold = ClickerGameData.TotalClicks + Random.Range(5, 16);
    }

    void UpdateClickText()
    {
        clickCountText.text = $"Clicks: {ClickerGameData.TotalClicks}";
    }

    void UnlockFurniture()
    {
        var pool = ClickerGameData.RemainingFurniture;
        if (pool.Count == 0) return;

        int rand = Random.Range(0, pool.Count);
        var unlocked = pool[rand];

        ClickerGameData.UnlockItem(unlocked);

        objectNameText.text = unlocked.furnitureName;
        objectSpriteImage.sprite = unlocked.furnitureSprite;
        objectSpriteImage.enabled = true;

        Debug.Log($"Unlocked: {unlocked.furnitureName}");

       
        inventoryManager.UnlockFurniture(unlocked);
    }

}
