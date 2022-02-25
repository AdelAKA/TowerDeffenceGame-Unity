using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffset;

    [HideInInspector] public GameObject turret;
    [HideInInspector] public TurretBluePrint turretBluePrint;
    // private float turretRange = 0;
    [HideInInspector] public bool isUpgraded = false;
    private int moneyLeftFromUpgrades = 0;

    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;

    public int GetMoneyLeftFromUpgrades() { return moneyLeftFromUpgrades; }

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
        buildManager.SelectTurretToBuild(null);
        // turretRange = turretBluePrint.prefab.GetComponent<Turret>().range;
    }

    void BuildTurret(TurretBluePrint blueprint)
    {
        if (PlayerStatus.Money < blueprint.cost)
        {
            Debug.Log("Insufficient gold!");
            return;
        }
        if (blueprint.turretType == TurretType.MissileLauncher)
        {
            if (PlayerStatus.missileLauncherBuildOwn <= 0)
            {
                Debug.Log("You have 0 MissileLaunchers");
                return;
            }
            PlayerStatus.missileLauncherBuildOwn--;
        }
        else if (blueprint.turretType == TurretType.LaserBeamer)
        {
            if (PlayerStatus.laserBeamerBuildOwn <= 0)
            {
                Debug.Log("You have 0 LaserBeamers");
                return;
            }
            PlayerStatus.laserBeamerBuildOwn--;
        }
        PlayerStatus.Money -= blueprint.cost;

        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBluePrint = blueprint;
        turretBluePrint.StartCalculation();

        isUpgraded = false;

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret build!");

    }

    // Upgrade the turret into the special type of turret
    public void UpgradeTurretSpecial()
    {
        //Get rid of the old turret
        Destroy(turret);

        //Build a new turret
        GameObject _turret = (GameObject)Instantiate(turretBluePrint.upgradePrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turret.GetComponent<Turret>().UpdateValues();

        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgraded = true;
        Debug.Log("Turret Upgraded!");
    }

    public void UpgradeDamage()
    {
        int tempdamagelevel = turret.GetComponent<Turret>().GetDamageLevel();
        int tempCost = turretBluePrint.GetUpgradeCost() / (5 - tempdamagelevel);
        if (PlayerStatus.Money < tempCost)
        {
            Debug.Log("Insufficient gold!");
            return;
        }
        PlayerStatus.Money -= tempCost;
        // turretBluePrint.GetTotalCost() += tempCost / 2;
        moneyLeftFromUpgrades += tempCost / 2;

        turret.GetComponent<Turret>().damageLevel++;

        AfterUpgradeEffect();
        if (turret.GetComponent<Turret>().damageLevel == 5) CheckIfMaxed();
    }

    public void UpgradeRange()
    {
        int tempRangeLevel = turret.GetComponent<Turret>().GetRangeLevel();
        int tempCost = turretBluePrint.GetUpgradeCost() / (5 - tempRangeLevel);
        if (PlayerStatus.Money < tempCost)
        {
            Debug.Log("Insufficient gold!");
            return;
        }
        PlayerStatus.Money -= tempCost;
        moneyLeftFromUpgrades += tempCost / 2;

        turret.GetComponent<Turret>().rangeLevel++;

        AfterUpgradeEffect();
        if (turret.GetComponent<Turret>().rangeLevel == 5) CheckIfMaxed();
    }

    public void UpgradeFireRate()
    {
        int tempFireRateLevel = turret.GetComponent<Turret>().GetFireRateLevel();
        int tempCost = turretBluePrint.GetUpgradeCost() / (5 - tempFireRateLevel);
        if (PlayerStatus.Money < tempCost)
        {
            Debug.Log("Insufficient gold!");
            return;
        }
        PlayerStatus.Money -= tempCost;
        moneyLeftFromUpgrades += tempCost / 2;

        turret.GetComponent<Turret>().fireRateLevel++;

        AfterUpgradeEffect();
        if (turret.GetComponent<Turret>().fireRateLevel == 5) CheckIfMaxed();
    }

    public void UpgradeSlowDown()
    {
        int tempSlowDownLevel = turretBluePrint.prefab.GetComponent<Turret>().GetSlowDownLevel();
        int tempCost = turretBluePrint.GetUpgradeCost() / (5 - tempSlowDownLevel);
        if (PlayerStatus.Money < tempCost)
        {
            Debug.Log("Insufficient gold!");
            return;
        }
        PlayerStatus.Money -= tempCost;
        moneyLeftFromUpgrades += tempCost / 2;

        turret.GetComponent<Turret>().slowDownLevel++;

        AfterUpgradeEffect();
        if (turret.GetComponent<Turret>().slowDownLevel == 5) CheckIfMaxed();
    }

    private void AfterUpgradeEffect()
    {
        turret.GetComponent<Turret>().UpdateValues();
        GameObject effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    // Check if a turret is fully upgraded
    private void CheckIfMaxed()
    {
        Turret tempTurret = turret.GetComponent<Turret>();
        if (turretBluePrint.turretType == TurretType.LaserBeamer)
        {
            if (tempTurret.rangeLevel == 5
            && tempTurret.damageLevel == 5
            && tempTurret.slowDownLevel == 5)
            {
                UpgradeTurretSpecial();
            }
        }
        else
        {
            if (tempTurret.rangeLevel == 5
            && tempTurret.damageLevel == 5
            && tempTurret.fireRateLevel == 5)
            {
                UpgradeTurretSpecial();
            }
        }
    }

    public void SellTurret()
    {
        PlayerStatus.Money += turretBluePrint.GetSellAmount() + moneyLeftFromUpgrades;
        moneyLeftFromUpgrades = 0;

        if (turretBluePrint.turretType == TurretType.MissileLauncher)
            PlayerStatus.missileLauncherBuildOwn++;
        else if (turretBluePrint.turretType == TurretType.LaserBeamer)
            PlayerStatus.laserBeamerBuildOwn++;

        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBluePrint = null;
    }

    void OnMouseEnter()
    {

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (buildManager.HaveMoney)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}
