using UnityEngine;

[System.Serializable]
public class TurretBluePrint
{
    public GameObject prefab;
    public GameObject upgradePrefab;
    public TurretType turretType;

    public int cost;
    private int upgradeCost;
    private int totalCost;

    public int GetUpgradeCost() { return upgradeCost; }
    public int GetTotalCost() { return totalCost; }
    public void IncreaseTotalCost(int amount) { totalCost += amount; }

    public void StartCalculation()
    {
        upgradeCost = (int)(cost * 0.8f);
        totalCost = cost / 2;
    }

    public int GetSellAmount()
    {
        return totalCost;
    }
}
