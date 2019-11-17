using UnityEngine;
using TWQ.Saving;
using TWQ.Stats;
using TWQ.Core;
using UnityEngine.Events;

namespace TWQ.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 70;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        float healthPoints = -1f;
        float currentHealthPercentage = 100;
        bool isDead = false;

        public bool IsDead { get => isDead; set => isDead = value; }

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
            if(healthPoints<0)
            {
                healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            healthPoints = (Mathf.Max(currentHealthPercentage, regenerationPercentage) / 100) * regenHealthPoints;
            currentHealthPercentage = GetPercentage();
            if (isDead && healthPoints > 0)
            {
                GetComponent<Animator>().SetTrigger("loadAfterDeath");
                isDead = false;
                transform.GetComponent<Collider>().enabled = true;
            }

        }

        public void Heal(float healthToRestore)
        {
            float percentageToRestore = GetMaxHealthPoints() * (healthToRestore / 100);
            healthPoints = Mathf.Min(healthPoints + percentageToRestore, GetMaxHealthPoints());
        }

        public void TakeDamage(GameObject instigator,float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);
            currentHealthPercentage = GetPercentage();
            if (healthPoints == 0)
            {
                onDie.Invoke();
                Die();
                AwardXP(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
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

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
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
            currentHealthPercentage = GetPercentage();

            if (healthPoints == 0)
            {
                Die();
            }

            if (isDead && healthPoints > 0)
            {
                GetComponent<Animator>().SetTrigger("loadAfterDeath");
                isDead = false;
                transform.GetComponent<Collider>().enabled = true;
            }
        }
    }
}

