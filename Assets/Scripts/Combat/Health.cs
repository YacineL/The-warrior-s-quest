using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TWQ.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
        }
    }
}

