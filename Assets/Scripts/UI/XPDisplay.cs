using TWQ.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace TWQ.UI
{
    public class XPDisplay : MonoBehaviour
    {
        Experience xp;
        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }
        private void Update()
        {
            if (GetComponent<Text>() != null)
            {
                GetComponent<Text>().text = xp.ExperiencePoints.ToString();
            }
        }
    }
}
