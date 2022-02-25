using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBluePrint standardTurret;
    public TurretBluePrint missileLauncher;
    public TurretBluePrint laserBeamer;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
        standardTurret.turretType = TurretType.Standard;
        missileLauncher.turretType = TurretType.MissileLauncher;
        laserBeamer.turretType = TurretType.LaserBeamer;
    }

    public void SelectStandadTurret()
    {
        Debug.Log("Standard Turret Selected");
        buildManager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        buildManager.SelectTurretToBuild(missileLauncher);
    }

    public void SelectLaserBeamer()
    {
        Debug.Log("Laser Beamer Selected");
        buildManager.SelectTurretToBuild(laserBeamer);
    }
}
