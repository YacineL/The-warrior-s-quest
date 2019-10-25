using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquppingWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
