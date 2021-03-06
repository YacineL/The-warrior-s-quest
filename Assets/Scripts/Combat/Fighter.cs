﻿using UnityEngine;
using TWQ.Movement;
using TWQ.Core;
using TWQ.Saving;
using TWQ.Inventory;
using TWQ.Attributes;
using TWQ.Stats;
using System.Collections.Generic;

namespace TWQ.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeapon = null;

        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        public WeaponConfig currentWeaponConfig = null;
        Weapon currentWeapon;

        private void Start()
        {
            WeaponInventory weaponInventory;
            GameObject gameObject = GameObject.FindGameObjectWithTag("Inventory");
            weaponInventory = gameObject.GetComponent<WeaponInventory>();
            if (currentWeaponConfig == null)
            {
                EquppingWeapon(defaultWeapon);
                if (!weaponInventory.IsAlreadyInInventory(defaultWeapon)  && transform.tag =="Player")
                {
                    weaponInventory.StoredWeapons.Add(defaultWeapon);
                }
            }
            else if (!weaponInventory.IsAlreadyInInventory(currentWeaponConfig) && transform.tag == "Player")
            {
                weaponInventory.StoredWeapons.Add(currentWeaponConfig);
            }
            currentWeapon = currentWeaponConfig.EquippedPrefab;
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead) return;

            if (!GetIsInRange(target.transform))
            {
                Vector3 targetPosition = Vector3.Scale(target.transform.position, (Vector3.right + Vector3.forward)) + Vector3.up * transform.position.y;
                print(target.transform.position);
                print(transform.position);
                print(targetPosition);
                GetComponent<Mover>().MoveTo(targetPosition, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public void EquppingWeapon(WeaponConfig weapon)
        {
            currentWeaponConfig = weapon;
            currentWeapon = weapon.EquippedPrefab;
            Animator animator = GetComponent<Animator>();
            currentWeapon = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.PercentageBonus;
            }
        }

        private bool GetIsInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.WeaponRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
               !GetIsInRange(combatTarget.transform))
            {
                return false;
            };
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null && !targetToTest.IsDead);
        }

        //Called by the animator
        void Hit()
        {
            if (target == null) { return; }

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            
            if(currentWeaponConfig.EquippedPrefab != null)
            {
                currentWeapon.OnHit();
            }

            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target , transform.gameObject, damage);
            }
            else
            {
                target.TakeDamage(gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }

        

        public object CaptureState()
        {
            return currentWeaponConfig.name;
        }

        public void RestoreState(object state)
        {

            string weaponName = (string)state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquppingWeapon(weapon);
        }
    }
}

