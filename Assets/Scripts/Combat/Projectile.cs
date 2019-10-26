using System;
using System.Collections;
using System.Collections.Generic;
using TWQ.Core;
using UnityEngine;

namespace TWQ.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;

        Health target = null;
        // Update is called once per frame
        void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target)
        {
            this.target = target;
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + ( Vector3.up * targetCollider.height  / 1.5f );
        }
    }
}
