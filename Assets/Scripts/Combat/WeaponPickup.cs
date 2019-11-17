using System.Collections;
using TWQ.Attributes;
using TWQ.Control;
using TWQ.Inventory;
using UnityEngine;

namespace TWQ.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float respawnTime = 4f;
        [SerializeField] float healthPercentageToRestore = 0;

        WeaponInventory weaponInventory = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PickUp(other.gameObject);
            }
        }

        private void PickUp(GameObject subject)
        {
            if(weapon != null)
            {
                subject.GetComponent<Fighter>().EquppingWeapon(weapon);
                weaponInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<WeaponInventory>();
                if (!weaponInventory.IsAlreadyInInventory(weapon))
                {
                    weaponInventory.StoredWeapons.Add(weapon);
                }
            }
            
            if(healthPercentageToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthPercentageToRestore);
            }
            
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerControler callingControler)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PickUp(callingControler.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
