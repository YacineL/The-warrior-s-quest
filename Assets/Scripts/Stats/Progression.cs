using System;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookUpTable = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];

            if(levels.Length < level)
            {
                return 0;
            }
            return levels[level - 1]; 
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();

            float[] levels = lookUpTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if (lookUpTable != null) return;

            lookUpTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                var statLookUpTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progressionStat in progressionClass.stats)
                {
                    statLookUpTable[progressionStat.stat] = progressionStat.levels;
                }

                lookUpTable[progressionClass.characterClass] = statLookUpTable;
            }
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
