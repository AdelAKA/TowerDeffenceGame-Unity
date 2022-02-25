using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int waypointIndex = 0;

    private Enemy enemyStates;
    public float verticalVelocity = 0f;

    private Vector3 distanceLeft;

    public int GetWayPointIndex { get { return waypointIndex; } }
    public float GetDistanceLeft { get { return distanceLeft.magnitude; } }

    public void setWaypointIndex(int _waypointIndex) { waypointIndex = _waypointIndex; }

    void Start()
    {
        enemyStates = GetComponent<Enemy>();

        target = WayPoints.points[waypointIndex];
        verticalVelocity = 0f;
    }

    void Update()
    {
        verticalVelocity += Physics.gravity.y / 16;
        transform.Translate(Vector3.up * verticalVelocity * 1 * Time.deltaTime, Space.World);
        if ((transform.position.y - (transform.localScale.y / 2)) <= 0.5)
        {
            verticalVelocity = 15f;
        }
        distanceLeft = target.position - transform.position;
        transform.Translate(distanceLeft.normalized * enemyStates.speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(target.position, transform.position) <= 0.5f)
        {
            GetNextWaypoint();
        }

        int multiplayer = (PlayerStatus.Rounds - 1) / 10;
        enemyStates.speed = enemyStates.startSpeed + enemyStates.startSpeed * (0.1f * multiplayer);
    }

    void GetNextWaypoint()
    {
        if (waypointIndex >= WayPoints.points.Length - 1)
        {
            ReachEndPath();
            return;
        }

        waypointIndex++;
        target = WayPoints.points[waypointIndex];
    }

    void ReachEndPath()
    {
        PlayerStatus.Lives -= enemyStates.damage;
        PlayerStatus.Money += enemyStates.damagePayback;
        Destroy(gameObject);
    }
}
