using System.Collections;
using System.Collections.Generic;
using TWQ.Control;
using UnityEngine;

public class HUDDisabler : MonoBehaviour
{
    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>().enabled == false)
        {
            transform.gameObject.SetActive(false);
        }
        else 
        {
            transform.gameObject.SetActive(true);
        }
    }
}
