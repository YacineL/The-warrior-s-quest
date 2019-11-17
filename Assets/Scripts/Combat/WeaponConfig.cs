using TWQ.Attributes;
using UnityEngine;

namespace TWQ.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] float weaponRange = 1.5f;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";
        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float WeaponRange { get => weaponRange; set => weaponRange = value; }
        public float PercentageBonus { get => percentageBonus; set => percentageBonus = value; }
        public Weapon EquippedPrefab { get => equippedPrefab; set => equippedPrefab = value; }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (EquippedPrefab != null)
            {
                Transform hand = GetHandTransform(rightHand, leftHand);
                weapon = Instantiate(EquippedPrefab, hand);
                weapon.gameObject.name = weaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if(animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;  
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private static void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform hand;
            if (isRightHanded)
            {
                hand = rightHand;
            }
            else
            {
                hand = leftHand;
            }

            return hand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target , GameObject instigator , float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget( instigator, target , calculatedDamage);
        }
    }
}
