using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopExitTriggerScript : MonoBehaviour
{
	public Button yesButton;
	public Button noButton;
	public GameObject exitConfirmation;
	private void Start()
	{
		yesButton.onClick.AddListener(() => 
		{
			ShopManagerScript.Instance.SerializeBoughtItems();
			SceneManager.LoadScene("MainMenu");
		});
		noButton.onClick.AddListener(() =>
		{
			exitConfirmation.SetActive(false);
			PlayerScript.instance.UnlockMouseMove();
			PlayerScript.instance.UnFreezePlayer();
			GameControllerScript.i.LockMouse();
		});
	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag("Player"))
		{
			exitConfirmation.SetActive(true);
			PlayerScript.instance.LockMouseMove();
			PlayerScript.instance.FreezePlayer();
			GameControllerScript.i.UnlockMouse();
		}
	}
}