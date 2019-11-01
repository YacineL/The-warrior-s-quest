using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 4f;
        WeaponInventory weaponInventory = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquppingWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
                weaponInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<WeaponInventory>();
                if (!weaponInventory.IsAlreadyInInventory(weapon))
                {
                    weaponInventory.StoredWeapons.Add(weapon);
                }
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = false;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
       
    }
}
