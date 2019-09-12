using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TWQ.Movement;
using TWQ.Core;

namespace TWQ.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;

        private void Update()
        {
            if (target == null) return;


            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);  
            }
            else
            {
                GetComponent<Mover>().Cancel();
                Cancel();
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
            print("WAAAAAA3");
        }

        public void Cancel()
        {
            target = null;
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }
    }
}

