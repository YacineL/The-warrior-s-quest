using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace TWQ.Movement
{
    public class Mover : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Transform target;
        Ray lastRay;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            UpdateAnimator();
        }

        public void MoveTo(Vector3 destination)
        {
            GetComponent<NavMeshAgent>().destination = destination;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("ForwardSpeed", speed);
        }
    }
}

