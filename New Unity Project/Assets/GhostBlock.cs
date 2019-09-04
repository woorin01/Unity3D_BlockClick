using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBlock : MonoBehaviour
{
    private bool inPlayer;
    public bool InPlayer
    {
        get
        {
            return inPlayer;
        }
        set
        {
            inPlayer = value;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            Debug.Log("응기잇");
            inPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("하읏");
            inPlayer = false;
        }
    }
}
