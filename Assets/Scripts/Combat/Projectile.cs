﻿using TWQ.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace TWQ.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;
        [SerializeField] bool isHoming = true;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onProjectileHit;
        Health target = null;
        GameObject instigator = null;
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

        public void SetTarget(GameObject instigator,Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }
        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
            if (targetCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + ( Vector3.up * targetCollider.height  / 1.25f );
        }
        private void OnTriggerEnter(Collider other)
        {
            if (target.IsDead) return;
            if (other.GetComponent<Health>() == null)
            {
                foreach (GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }

                Destroy(gameObject, 0);
            }
            if (other.GetComponent<Health>() != null && other.tag != instigator.tag)
            {
                onProjectileHit.Invoke();
                other.transform.GetComponent<Health>().TakeDamage(instigator,damage);
                transform.GetComponent<Collider>().enabled = false;
                speed = 0f;
                if (hitEffect != null)
                {
                    Instantiate(hitEffect, GetAimLocation(), transform.rotation);
                }

                foreach (GameObject toDestroy in destroyOnHit)
                {
                    Destroy(toDestroy);
                }

                Destroy(gameObject, lifeAfterImpact);
            }
        }
    }
}
