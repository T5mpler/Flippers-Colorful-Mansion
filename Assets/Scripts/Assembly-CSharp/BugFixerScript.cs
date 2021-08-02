using UnityEngine;

public class BugFixerScript : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
    }
}