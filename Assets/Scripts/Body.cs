using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.gameObject.tag == "Wall") { transform.position -= new Vector3(-0.75f, 0f, 0f); }
    }
}
