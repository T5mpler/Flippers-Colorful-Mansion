using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public RaceManagerScript raceManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(raceManager.EndDelay(raceManager.winScreen));
        }
        else if (other.name == "Baldi")
        {
            StartCoroutine(raceManager.EndDelay(raceManager.loseScreen));
        }
    }
}
