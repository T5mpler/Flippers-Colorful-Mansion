using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopButtonScript : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button.onClick.AddListener(() => SceneManager.LoadScene("Shop"));
    }

}
