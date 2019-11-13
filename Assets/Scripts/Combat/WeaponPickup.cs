using System.Collections;
using TWQ.Control;
using TWQ.Inventory;
using UnityEngine;

namespace TWQ.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 4f;
        WeaponInventory weaponInventory = null;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PickUp(other.GetComponent<Fighter>());
            }
        }

        private void PickUp(Fighter fighter)
        {
            fighter.EquppingWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
            weaponInventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<WeaponInventory>();
            if (!weaponInventory.IsAlreadyInInventory(weapon))
            {
                weaponInventory.StoredWeapons.Add(weapon);
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

        public bool HandleRaycast(PlayerControler callingControler)
        {
            if(Input.GetMouseButtonDown(0))
            {
                PickUp(callingControler.GetComponent<Fighter>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
