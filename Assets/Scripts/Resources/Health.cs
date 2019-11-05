using UnityEngine;
using TWQ.Saving;
using TWQ.Stats;
using TWQ.Core;
using System;

namespace TWQ.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;
        bool isDead = false;

        public bool IsDead { get => isDead; set => isDead = value; }

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }
        public void TakeDamage(GameObject instigator,float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            if (healthPoints == 0)
            {
                Die();
                AwardXP(instigator);
            }
        }

        public float GetPercentage()
        {
            return (healthPoints / GetComponent<BaseStats>().GetStat(Stat.Health)) * 100;
        }

        public float GetHealthPoints()
        {
            return healthPoints;
        }
        private void Die()
        {
            if (IsDead) return;
            GetComponent<Animator>().SetTrigger("death");
            isDead = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AwardXP(GameObject instigator)
        {
            Experience xp = instigator.GetComponent<Experience>();
            if (xp != null)
            { 
                xp.GainXP(GetComponent<BaseStats>().GetStat(Stat.XPReward));
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }

            if (isDead && healthPoints!= 0)
            {
                GetComponent<Animator>().SetTrigger("loadAfterDeath");
                isDead = false;
            }
        }
    }
}

