using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static int Money;
    public int startMoney = 400;

    public static int Lives;
    public int startLives = 20;

    public static int missileLauncherBuildOwn = 0;
    public static int laserBeamerBuildOwn = 0;

    public static int Rounds;

    void Start()
    {
        Money = startMoney;
        Lives = startLives;

        Rounds = 0;

        missileLauncherBuildOwn = 0;
        laserBeamerBuildOwn = 0;

    }
}
