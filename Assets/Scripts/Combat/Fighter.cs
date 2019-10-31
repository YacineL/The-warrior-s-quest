using UnityEngine;
using TWQ.Movement;
using TWQ.Core;
using System;
using TWQ.Saving;
using System.Collections.Generic;

namespace TWQ.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        Weapon currentWeapon = null;
        string weaponInventoryNames = null;
        WeaponInventory weaponInventory = null;
        int currentIndex;

        private void Start()
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag("Inventory");
            weaponInventory = gameObject.GetComponent<WeaponInventory>();
            if (currentWeapon == null)
            {
                EquppingWeapon(defaultWeapon);
            }
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.C)) SwitchWeapons();

            if (target == null) return;

            if (target.IsDead) return;

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public bool IsAlreadyInInventory(Weapon pickedWeapon)
        {
            foreach(Weapon weapon in weaponInventory.StoredWeapons)
            {
                if (weapon.Equals(pickedWeapon)) return true;
            }
            return false;
        }
        public void EquppingWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
            currentIndex = GetCurrentWeaponIndex();
            if(!IsAlreadyInInventory(weapon))
            {
                weaponInventory.StoredWeapons.Add(weapon);
                weaponInventoryNames += (weapon.name + "/");
            }
        }

        public int GetCurrentWeaponIndex()
        {
            int iterator = 0;
            foreach (Weapon weapon in weaponInventory.StoredWeapons)
            {
                if (weapon.Equals(currentWeapon))
                {
                    return iterator;
                }
                iterator++;
            }
            return 0;
        }

        public void LoadWeaponInventory(string weaponInventoryString)
        {
            int iterator = 0;
            string weaponName = null;
            int currentWeaponIndex;
            foreach (char c in  (weaponInventoryString))
            {
                iterator++;
                if (c.Equals("|"))
                {
                    currentWeaponIndex = int.Parse(weaponInventoryString.Substring(iterator));
                    EquppingWeapon(weaponInventory.StoredWeapons[currentWeaponIndex]);
                    return;
                }
                else if (c.Equals("/"))
                {
                    weaponInventory.StoredWeapons.Add(Resources.Load<Weapon>(weaponName));
                    weaponName = null;
                }
                else 
                {
                    weaponName += c;
                }
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;

            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().SetTrigger("attack");
            GetComponent<Animator>().ResetTrigger("stopAttack");
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();

        }

        private void StopAttack()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            GetComponent<Animator>().ResetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null && !targetToTest.IsDead);
        }

        //Called by the animator
        void Hit()
        {
            if (target == null) { return; }
            
            if(currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            { 
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        public void SwitchWeapons()
        {
            int switchIndex = (GetCurrentWeaponIndex() + 1) % weaponInventory.StoredWeapons.Count;
            EquppingWeapon(weaponInventory.StoredWeapons[switchIndex]);
        }/**/

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag("Inventory");
            weaponInventory = gameObject.GetComponent<WeaponInventory>();
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquppingWeapon(weapon);
        }
    }
}

