using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuButtonScript : MonoBehaviour
{
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
    }
}
