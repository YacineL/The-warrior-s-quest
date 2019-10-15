using UnityEngine;
using UnityEngine.Playables;
using TWQ.Core;
using TWQ.Control;

namespace TWQ.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector pd)
        {
            print("DisableControl");
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerControler>().enabled = false;
        }

        void EnableControl(PlayableDirector pd)
        {
            print("EnableControl");
            player.GetComponent<PlayerControler>().enabled = true;
        }
    }
}