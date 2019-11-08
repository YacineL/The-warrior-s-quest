using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TWQ.Saving;
using System;

namespace TWQ.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;

        public float ExperiencePoints { get => experiencePoints; set => experiencePoints = value; }

        public event Action onXPGained;

        public void GainXP(float experience)
        {
            experiencePoints += experience;
            onXPGained();
        }

        public object CaptureState()
        {
            return ExperiencePoints;
        }


        public void RestoreState(object state)
        {
            ExperiencePoints = (float)state;
        }
    }
}

