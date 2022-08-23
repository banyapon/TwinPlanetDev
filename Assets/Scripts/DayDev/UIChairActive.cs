using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChairActive : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.CompareTag("Player"))
        {
            gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.transform.parent.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
