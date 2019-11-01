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

        [System.Serializable]
        public class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health = new float[BaseStats.maxLevel];
            public void SetHealth()
            {
                for (int i=0; i<(BaseStats.maxLevel-1); i++)
                {
                    health[i + 1] = health[i] + (10 / 100) * health[i];
                }
            }
        }
    }
}
