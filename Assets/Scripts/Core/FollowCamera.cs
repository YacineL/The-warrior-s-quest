using UnityEngine;

namespace TWQ.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] float targetHeight = 1.5f;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position + Vector3.up * targetHeight;
        }
    }
}

