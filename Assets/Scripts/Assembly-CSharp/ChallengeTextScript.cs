using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChallengeTextScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(3, 3)]public string buttonText;
    Text challengeText;
    void Awake()
    {
        challengeText = transform.parent.Find("Text").GetComponent<Text>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        challengeText.text = buttonText;
        if (name == "Grapple")
        {
            challengeText.fontSize = 20;
        }
        else
        {
            challengeText.fontSize = 25;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        challengeText.text = string.Empty;
    }
}
