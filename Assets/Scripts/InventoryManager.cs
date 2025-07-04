using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject inventoryPanel;
    public Button openInventoryButton;
    public Button closeInventoryButton;

    [System.Serializable]
    public class FurnitureButton
    {
        public FurnitureData furniture;
        public GameObject buttonObject;    // The button GameObject in the UI
        public Image buttonImage;          // The Image component to show sprite
    }

    public List<FurnitureButton> furnitureButtons;

    private List<FurnitureData> unlockedItems = new List<FurnitureData>();

    void Start()
    {
        inventoryPanel.SetActive(false);
        openInventoryButton.onClick.AddListener(() => inventoryPanel.SetActive(true));
        closeInventoryButton.onClick.AddListener(() => inventoryPanel.SetActive(false));

        
        foreach (var pair in furnitureButtons)
        {
            pair.buttonObject.SetActive(false);
        }
    }

    public void UnlockFurniture(FurnitureData unlocked)
    {
        if (unlockedItems.Contains(unlocked)) return;

        unlockedItems.Add(unlocked);

        foreach (var pair in furnitureButtons)
        {
            if (pair.furniture == unlocked)
            {
                pair.buttonImage.sprite = unlocked.furnitureSprite;
                pair.buttonObject.SetActive(true);
                pair.buttonObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    Debug.Log("Selected: " + unlocked.furnitureName);
                    // Kabs bro place 3D prefab here please
                });
                break;
            }
        }
    }
}
