using UnityEngine;
namespace TWQ.UI
{
    public class CameraFacing : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}

