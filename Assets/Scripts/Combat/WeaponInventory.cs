using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Combat
{
    [CreateAssetMenu(fileName = "Weapon Inventory", menuName = "Weapon Inventories/Make new weapon inventory", order = 0)]
    public class WeaponInventory : ScriptableObject
    {
        List<Weapon> weapons = null;
        int lastUsedWeaponIndex;

        public List<Weapon> Weapons { get => weapons; set => weapons = value; }
        public int LastUsedWeaponIndex { get => lastUsedWeaponIndex; set => lastUsedWeaponIndex = value; }
    }
}
