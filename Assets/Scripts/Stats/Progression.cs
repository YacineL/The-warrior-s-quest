using System;
using UnityEngine;

namespace TWQ.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        int size = Enum.GetNames(typeof(CharacterClass)).Length;
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;


        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    if (progressionStat.stat != stat) continue;

                    if (progressionStat.levels.Length < level) continue;

                    return progressionStat.levels[level - 1];
                }
            }
            return 0;
        }

        /*SetHealth() is not usable right now so the health points progression has to be set manually
          in the Progression scriptable object*/


        /*public void SetHealth()
        {
            foreach(ProgressionCharacterClass characterClass in characterClasses)
            {
                for (int i = 0; i < (BaseStats.maxLevel - 1); i++)
                {
                    characterClass.Health[i + 1] = characterClass.Health[i] + (10 / 100) * characterClass.Health[i];
                }
            }
        }*/
        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}
