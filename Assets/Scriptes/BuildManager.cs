using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
    {
        instance = this;
    }

    public GameObject buildEffect;
    public GameObject sellEffect;
    private TurretBluePrint turretToBuild;
    private Node selectedNode;

    public NodeUI nodeUI;

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HaveMoney { get { return PlayerStatus.Money >= turretToBuild.cost; } }
    public TurretBluePrint GetTurretToBuild() { return turretToBuild; }

    public void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
        nodeUI.ring.Play();
    }

    public void DeselectNode()
    {
        selectedNode = null;

        nodeUI.Hide();
        nodeUI.ring.Stop();
    }

    public void SelectTurretToBuild(TurretBluePrint turret)
    {
        turretToBuild = turret;

        DeselectNode();
    }
}
