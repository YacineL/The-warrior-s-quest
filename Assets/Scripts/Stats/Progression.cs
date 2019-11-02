using System;
using UnityEngine;

namespace TWQ.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        int size = Enum.GetNames(typeof(CharacterClass)).Length;
        [SerializeField] ProgressionCharacterClass[] characterClasses = new ProgressionCharacterClass[Enum.GetNames(typeof(CharacterClass)).Length];

        public ProgressionCharacterClass[] CharacterClasses { get => characterClasses; set => characterClasses = value; }

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if(progressionClass.CharacterClass == characterClass)
                {
                    return progressionClass.Health[level - 1];
                }
            }
            return 30;
        }

        /*SetHealth() is not usable right now so the health points progression has to be set manually
          in the Progression scriptable object*/
        public void SetHealth()
        {
            foreach(ProgressionCharacterClass characterClass in characterClasses)
            {
                for (int i = 0; i < (BaseStats.maxLevel - 1); i++)
                {
                    characterClass.Health[i + 1] = characterClass.Health[i] + (10 / 100) * characterClass.Health[i];
                }
            }
        }
        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health = new float[BaseStats.maxLevel];

            public CharacterClass CharacterClass { get => characterClass; set => characterClass = value; }
            public float[] Health { get => health; set => health = value; }

            
        }
    }
}
