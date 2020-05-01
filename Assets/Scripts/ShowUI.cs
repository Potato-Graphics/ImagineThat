﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUI : MonoBehaviour
{

    public GameObject uiObject;
    void Start()
    {
        uiObject.SetActive(false);
    }
    // Update is called once per frame
 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
            StartCoroutine(WaitForSec());
        }
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(4);
        Destroy(uiObject);
        Destroy(gameObject);
    }

}
