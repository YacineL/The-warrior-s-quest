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
        [SerializeField] List<Weapon> weaponInventory = null;
        string weaponInventoryNames = null;

        private void Start()
        {
            defaultWeapon.WeaponRange = 15;
            if (currentWeapon == null)
            {
                EquppingWeapon(defaultWeapon);
            }
        }

        public bool IsAlreadyInInventory(Weapon pickedWeapon)
        {
            foreach(Weapon weapon in weaponInventory)
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
            if(!IsAlreadyInInventory(weapon))
            {
                weaponInventory.Add(weapon);
                weaponInventoryNames += (weapon.name + "/");
            }
        }

        public int GetCurrentWeaponIndex()
        {
            int iterator = 0;
            foreach (Weapon weapon in weaponInventory)
            {
                if (weapon.Equals(currentWeapon))
                {
                    print(iterator);
                    return iterator;
                }
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
                    EquppingWeapon(weaponInventory[currentWeaponIndex]);
                    return;
                }
                else if (c.Equals("/"))
                {
                    weaponInventory.Add(Resources.Load<Weapon>(weaponName));
                    weaponName = null;
                }
                else 
                {
                    weaponName += c;
                }
            }
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            LoadWeaponInventory((string)state);
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquppingWeapon(weapon);
        }
    }
}

