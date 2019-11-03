using UnityEngine;

namespace TWQ.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public const int maxLevel = 5;
        [Range(1,maxLevel)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;

        public float GetHealth()
        {
            return progression.GetHealth(characterClass,startingLevel);
        }

        public float GetXPReward()
        {
            return 10;
        }
    }
}