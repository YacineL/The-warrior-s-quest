using TWQ.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace TWQ.UI
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }
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
