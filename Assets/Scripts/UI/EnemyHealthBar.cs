using TWQ.Resources;
using UnityEngine;

namespace TWQ.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] Canvas canvas;
        void Update()
        {
            if (Mathf.Approximately(health.GetPercentage(), 0) ||
                Mathf.Approximately(health.GetPercentage(), 100))
            {
                canvas.enabled = false;
                return;
            }
            canvas.enabled = true;
        }
    }
}
