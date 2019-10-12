using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace TWQ.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent = GameObject.FindGameObjectWithTag("Player").transform)
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}

