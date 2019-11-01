using System.Collections.Generic;
using TWQ.Combat;
using UnityEngine;

namespace TWQ.Inventory
{
    public class WeaponInventory : MonoBehaviour
    {
        [SerializeField] string weaponInventoryString = null;
        [SerializeField] List<Weapon> storedWeapons = null;

        public string WeaponInventoryString { get => weaponInventoryString; set => weaponInventoryString = value; }
        public List<Weapon> StoredWeapons { get => storedWeapons; set => storedWeapons = value; }
        public bool IsAlreadyInInventory(Weapon pickedWeapon)
        {
            foreach (Weapon weapon in StoredWeapons)
            {
                if (weapon.Equals(pickedWeapon)) return true;
            }
            return false;
        }
    }
}
