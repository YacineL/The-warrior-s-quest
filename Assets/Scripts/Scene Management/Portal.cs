using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            print("TRIGGERED");
        }
    }
}

