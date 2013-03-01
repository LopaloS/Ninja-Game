using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WayPoint : MonoBehaviour {
    static public  List<Vector3> waypoints;

	// Use this for initialization
	void Start () 
    {
        RebuildWaypointList();
    }

    public static Vector3 FindClosest( Vector3 pos)
    {
        Vector3 closest = Vector3.zero;
        float closestDistance = 10000;

        foreach (Vector3 wp in waypoints)
        {
            float distance = Vector3.Distance(pos, wp);
            if (distance < closestDistance)
            {
                closest = wp;
                closestDistance = distance;
            }
        }
        return closest;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Waypoint.tif");
    }

    void RebuildWaypointList()
    {
        WayPoint[] foundWaypoints = FindObjectsOfType(typeof(WayPoint)) as WayPoint[];
        waypoints = new List<Vector3>();
        foreach (WayPoint w in foundWaypoints)
        {
            waypoints.Add(w.transform.position);
        }
    }
}
