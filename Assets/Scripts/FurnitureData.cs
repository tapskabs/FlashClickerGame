using UnityEngine;

[CreateAssetMenu(menuName = "Furniture Data")]
public class FurnitureData : ScriptableObject
{
    public string furnitureName;
    public Sprite furnitureSprite;
    public GameObject furniturePrefab; // For future use brodigy

}
