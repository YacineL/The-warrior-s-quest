using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] float experiencePoints =0f;

    public void GainXP(float experience)
    {
        experiencePoints += experience;
    }
}
