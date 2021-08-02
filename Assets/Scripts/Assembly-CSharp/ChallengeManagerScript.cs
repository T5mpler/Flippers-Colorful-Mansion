using System;
using UnityEngine;

public class ChallengeManagerScript : MonoBehaviour
{
    public enum ChallengeType
    {
        None,
        Grapple, 
        Race,
        Speedy, 
        Hydration, 
        Stealthy,
        NoItems
    }
    public ChallengeType challengeType;
    public event EventHandler<OnPlayerStartChallenge> OnPlayerInChallenge;
    public void TriggerEvent()
    {
        OnPlayerInChallenge?.Invoke(this, new OnPlayerStartChallenge { challengeType = (ChallengeType)Enum.Parse(typeof(ChallengeType), PlayerPrefs.GetString("ChallengeMode")) });
    }
    public class OnPlayerStartChallenge : EventArgs
    {
        public ChallengeType challengeType;
    }
}
