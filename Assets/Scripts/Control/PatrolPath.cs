using UnityEngine;

namespace TWQ.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float WAYPOINT_RADIUS = 0.3f;
            for(int i =0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, WAYPOINT_RADIUS);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild((i+1) % transform.childCount).position);
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}

