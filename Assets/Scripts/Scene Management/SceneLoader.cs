using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TWQ.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        SavingWrapper wrapper;
        private void Start()
        {
            wrapper = FindObjectOfType<SavingWrapper>();
        }
        public void ReloadGame()
        {
            wrapper.Delete();
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

