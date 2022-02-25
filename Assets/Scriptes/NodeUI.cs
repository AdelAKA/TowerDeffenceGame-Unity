using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public GameObject ui;
    public ParticleSystem ring;

    public Button upgradeDamageButton;
    public Button upgradeRangeButton;
    public Button upgradeFireRateButton;
    public Button upgradeSlowDownButton;

    public Text upgradeDamageCostText;
    public Text upgradeRangeCostText;
    public Text upgradeFireRateCostText;
    public Text upgradeSlowDownCostText;
    public Text sellAmountText;

    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = target.GetBuildPosition();

        // When you select a laserBeamer it has no fire rate but has a slowdown effect
        if (target.turretBluePrint.turretType == TurretType.LaserBeamer)
        {
            upgradeFireRateButton.gameObject.SetActive(false);
            upgradeSlowDownButton.gameObject.SetActive(true);
        }
        // Other turrets vice versa
        else
        {
            upgradeFireRateButton.gameObject.SetActive(true);
            upgradeSlowDownButton.gameObject.SetActive(false);
        }

        UpdateTextValues();

        // if (!target.isUpgraded)
        // {
        //     upgradeCostText.text = "$" + target.turretBluePrint.upgradeCost;
        //     upgradeButton.interactable = true;
        // }
        // else
        // {
        //     upgradeCostText.text = "MAXED!";
        //     upgradeButton.interactable = false;
        // }
        // sellAmountText.text = "$" + target.turretBluePrint.GetSellAmount();

        ui.SetActive(true);
    }

    private void UpdateTextValues()
    {
        Turret tempTurret = target.turret.GetComponent<Turret>();
        int tempUpgradeCost = target.turretBluePrint.GetUpgradeCost();

        // Update the range radius of the Turret
        float newRange = tempTurret.range / 10f;
        ring.transform.localScale = new Vector3(newRange, newRange, 1);
        // Debug.Log(ring.transform.localScale + " " + newRange);

        // Every Upgrade button price should say MAXed if the turret reached the max level of this upgrade
        // Damage Upgrade Cost
        string sMAX = "MAXed!";
        // upgradeDamageCostText.text = tempTurret.GetDamageLevel() == 5 ? s2 : s1;
        if (tempTurret.GetDamageLevel() == 5)
        {
            upgradeDamageCostText.text = sMAX;
            upgradeDamageButton.interactable = false;
        }
        else
        {
            upgradeDamageCostText.text = "$" + (tempUpgradeCost / (5 - tempTurret.GetDamageLevel())); ;
            upgradeDamageButton.interactable = true;
        }

        // Rnage Upgrade Cost
        // upgradeRangeCostText.text = tempTurret.GetRangeLevel() == 5 ? s2 : s1;
        if (tempTurret.GetRangeLevel() == 5)
        {
            upgradeRangeCostText.text = sMAX;
            upgradeRangeButton.interactable = false;
        }
        else
        {
            upgradeRangeCostText.text = "$" + (tempUpgradeCost / (5 - tempTurret.GetRangeLevel())); ;
            upgradeRangeButton.interactable = true;
        }
        // FireRate Upgrade Cost
        // upgradeFireRateCostText.text = tempTurret.GetFireRateLevel() == 5 ? s2 : s1;
        if (tempTurret.GetFireRateLevel() == 5)
        {
            upgradeFireRateCostText.text = sMAX;
            upgradeFireRateButton.interactable = false;
        }
        else
        {
            upgradeFireRateCostText.text = "$" + (tempUpgradeCost / (5 - tempTurret.GetFireRateLevel())); ;
            upgradeFireRateButton.interactable = true;
        }
        // Slowdown Upgrade Cost
        // upgradeSlowDownCostText.text = tempTurret.GetSlowDownLevel() == 5 ? s2 : s1;
        if (tempTurret.GetSlowDownLevel() == 5)
        {
            upgradeSlowDownCostText.text = sMAX;
            upgradeSlowDownButton.interactable = false;
        }
        else
        {
            upgradeSlowDownCostText.text = "$" + (tempUpgradeCost / (5 - tempTurret.GetSlowDownLevel())); ;
            upgradeSlowDownButton.interactable = true;
        }
        sellAmountText.text = "$" + (target.turretBluePrint.GetSellAmount() + target.GetMoneyLeftFromUpgrades());
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void UpgradeDamage()
    {
        target.UpgradeDamage();
        UpdateTextValues();
        // BuildManager.instance.DeselectNode();
    }

    public void UpgradeRange()
    {
        target.UpgradeRange();
        UpdateTextValues();
    }

    public void UpgradeFireRate()
    {
        target.UpgradeFireRate();
        UpdateTextValues();
    }

    public void UpgradeSlowDown()
    {
        target.UpgradeSlowDown();
        UpdateTextValues();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
