using System.Collections;
using UnityEngine;

public class MachimaticsScript : MonoBehaviour
{
    private void Start()
    {
        Initalize();
    }
    void Initalize()
    {
        int sign = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
        string operatorSign = string.Empty;
        switch (sign)
        {
            case 0: //Addition Question
                operatorSign = "+";
                num1 = UnityEngine.Random.Range(0, 9);
                int clampValue = 9 - num1;
                num2 = UnityEngine.Random.Range(0, clampValue);
                solution = num1 + num2;
                break;
            case 1: //Subtraction Question
                operatorSign = "-";
                num1 = UnityEngine.Random.Range(0, 9);
                num2 = UnityEngine.Random.Range(0, num1);
                solution = num1 - num2;
                break;
        }
        questionText.text = num1 + operatorSign + num2 + "=";
    }
    public void CheckAnswer(int value)
    {
        if (value == solution)
        {
            audioDevice.PlayOneShot(wow);
            questionText.text += value + "!";
            rewardedNotebook.SetActive(true);
            StartCoroutine(PopAllBallons());
            StartCoroutine(ShopText());
        }
        else
        {
            audioDevice.PlayOneShot(incorrect);
        }
        PlayerScript.instance.UnpickupNumberBallon();
    }
    IEnumerator PopAllBallons()
    {
        for (int i = 0; i < numberBallons.Length; i++)
        {
            yield return new WaitForSeconds(0.1f);
            PopBallon(i);
        }
        yield break;
    }
    void PopBallon(int index)
    {
        audioDevice.PlayOneShot(pop);
        Destroy(numberBallons[index].gameObject);
    }
    IEnumerator ShopText()
    {
        int currentPoints = PlayerPrefs.GetInt("ShopPoints");        
        int pointsToAdd = UnityEngine.Random.Range(10, 20);
        int currentPointsAdded = 0;
        GameControllerScript.i.pointsCurrentlyAdded += pointsToAdd;
        PlayerPrefs.SetInt("ShopPoints", currentPoints + pointsToAdd);
        GameControllerScript.i.shopPoints.gameObject.SetActive(true);
        GameControllerScript.i.shopPointsAdder.gameObject.SetActive(true);
        GameControllerScript.i.shopPoints.text = "Shop Points:\n" + currentPoints;
        GameControllerScript.i.shopPointsAdder.text = "0";
        while (pointsToAdd > 0)
        {
            currentPointsAdded += 1;
            pointsToAdd -= 1;
            GameControllerScript.i.shopPointsAdder.text = currentPointsAdded.ToString();
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        RectTransform rectTransform = GameControllerScript.i.shopPointsAdder.rectTransform;
        Vector2 direction = GameControllerScript.i.shopPoints.rectTransform.anchoredPosition - rectTransform.anchoredPosition;
        float originalY = rectTransform.anchoredPosition.y;
        while (direction.magnitude > 12f)
        {
            float speed = 75f;
            rectTransform.anchoredPosition += direction.normalized * Time.deltaTime * speed;
            direction = GameControllerScript.i.shopPoints.rectTransform.anchoredPosition - rectTransform.anchoredPosition;
            yield return null;
        }
        int newPoints = currentPoints + currentPointsAdded;
        GameControllerScript.i.shopPoints.text = "Shop Points:\n" + newPoints;
        GameControllerScript.i.shopPointsAdder.text = string.Empty;
        yield return new WaitForSeconds(1f);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, originalY);
        GameControllerScript.i.shopPoints.gameObject.SetActive(false);
        GameControllerScript.i.shopPointsAdder.gameObject.SetActive(false);
        yield break;

    }
    public TextMesh questionText;
    public NumberBallonScript[] numberBallons;
    int num1, num2, solution;
    public GameObject rewardedNotebook;
    public AudioClip wow;
    public AudioClip pop;
    public AudioClip incorrect;
    public AudioSource audioDevice;
}
