using System.Collections;
using System.Collections.Generic;
using TWQ.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace TWQ.UI
{
    public class LevelDisplay : MonoBehaviour
    {
        BaseStats stats;
        private void Awake()
        {
            stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }
        private void Update()
        {
            if (GetComponent<Text>() != null)
            {
                GetComponent<Text>().text = stats.GetLevel().ToString();
            }
        }
    }
}

