using UnityEngine;
namespace TWQ.Combat
{    
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make new weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] AnimatorOverrideController weaponOverride = null;
        [SerializeField] GameObject weaponPrefab = null;

        public void Spawn(Transform handTransform, Animator animator)
        {
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = weaponOverride;
        }
    }
}
