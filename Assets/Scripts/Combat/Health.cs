using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TWQ.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public bool IsDead { get => isDead; set => isDead = value; }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0 && !(isDead))
            {
                GetComponent<Animator>().SetTrigger("death");
                isDead = true;
            }
        }
    }
}

