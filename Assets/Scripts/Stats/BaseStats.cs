using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public const int maxLevel = 99;
        [Range(1,maxLevel)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
    }
}