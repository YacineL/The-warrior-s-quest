﻿using UnityEngine;

namespace TWQ.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}

