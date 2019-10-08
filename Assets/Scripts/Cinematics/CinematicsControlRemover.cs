using UnityEngine;
using UnityEngine.Playables;

namespace TWQ.Cinematics
{
    public class CinematicsControlRemover : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        public void DisableControl(PlayableDirector playableDirector)
        {
            print("Disabled");
        }

        public void EnableControl(PlayableDirector playableDirector)
        {
            print("Enabled ");
        }
    }
}
