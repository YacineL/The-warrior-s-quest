using UnityEngine;
namespace TWQ.Combat
{    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponRange = 1.5f;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] float weaponDamage = 10f;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] bool isRightHanded = true;

        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float WeaponRange { get => weaponRange; set => weaponRange = value; }

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
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
                Instantiate(equippedPrefab, hand);
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }
    }
}
