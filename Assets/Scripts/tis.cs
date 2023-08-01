using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tis : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Translate(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            transform.localScale += new Vector3(.05f, .01f, 0f);
        }
    }
}
