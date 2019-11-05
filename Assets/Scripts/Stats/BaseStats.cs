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
        private void Update()
        {
            if(gameObject.tag == "Player")
            {
                print(GetLevel());
            }
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat,characterClass,GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = GetComponent<Experience>().ExperiencePoints;
            int penultimateLevel = progression.GetLevels(Stat.XPToLevelUp, characterClass);
            for (int level =1 ; level < penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.XPToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }
            return penultimateLevel + 1;
        }
    }
}