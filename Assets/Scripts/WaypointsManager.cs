using UnityEngine;

public class WaypointsManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    public Transform[] Waypoints => waypoints;
}
