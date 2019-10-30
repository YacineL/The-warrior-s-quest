﻿using System;
using System.Collections;
using System.Collections.Generic;
using TWQ.Core;
using UnityEngine;

namespace TWQ.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        Health target = null;
        float damage = 0f;

        void Start()
        {
            transform.LookAt(GetAimLocation());
        }
        void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead)
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
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
        private void OnTriggerEnter(Collider other)
        {
            if (target.IsDead) return; 
            if (other.GetComponent<Health>() != null)
            {
                other.transform.GetComponent<Health>().TakeDamage(damage);
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }
                Destroy(gameObject);
            }
        }
    }
}
