using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWQ.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;
            if (currentAction != null)
            {
                currentAction.Cancel();
                print("Ahl echaqour wel B14 !!");
            }           
            currentAction = action;
        }
    }

}