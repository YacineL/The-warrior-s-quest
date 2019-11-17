using TWQ.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace TWQ.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] Health health;
        private void Update()
        {
            if(GetComponent<Text>() != null)
            {
                GetComponent<Text>().text = health.GetHealthPoints().ToString();
            }
            if (GetComponent<Image>() != null)
            {
                GetComponent<Image>().fillAmount = health.GetPercentage() / 100;
            }
        }
    }
}
