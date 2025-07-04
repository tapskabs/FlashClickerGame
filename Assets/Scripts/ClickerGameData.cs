using System.Collections.Generic;
using UnityEngine;

public static class ClickerGameData
{
    public static int TotalClicks = 0;

    public static List<FurnitureData> UnlockedFurniture = new List<FurnitureData>();
    public static List<FurnitureData> RemainingFurniture = new List<FurnitureData>();

    public static bool Initialized = false;

    public static void Initialize(FurnitureData[] allFurniture)
    {
        if (Initialized) return;

        RemainingFurniture = new List<FurnitureData>(allFurniture);
        UnlockedFurniture = new List<FurnitureData>();
        TotalClicks = 0;
        Initialized = true;
    }

    public static void UnlockItem(FurnitureData data)
    {
        if (RemainingFurniture.Contains(data))
        {
            RemainingFurniture.Remove(data);
            UnlockedFurniture.Add(data);
        }
    }
}
