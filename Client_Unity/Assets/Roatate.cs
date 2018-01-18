using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roatate : MonoBehaviour
{
    private bool flag = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            flag = !flag;
        }
        if (flag)
        {
            transform.Rotate(new Vector3(0, 0, Time.deltaTime * 100f), Space.Self);
        }
    }
}