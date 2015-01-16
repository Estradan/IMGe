using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "Start.tif");
    }
}
