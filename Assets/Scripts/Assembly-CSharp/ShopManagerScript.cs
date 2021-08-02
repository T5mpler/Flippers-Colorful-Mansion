using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
	public static ShopManagerScript Instance { get; private set; }
	[SerializeField] Text pointsText;
	public Text itemReplaceText;
	string itemReplaceConfirmationText;
	bool confirming;
	[SerializeField] GameObject buyItemComfirmation;
	[SerializeField] AudioClip aud_ShopGreet;
	[SerializeField] AudioClip aud_SelectItem;
	[SerializeField] AudioClip aud_Confirmation;
	[SerializeField] AudioClip aud_NotEnough;
	[SerializeField] AudioClip aud_Hope;
	[SerializeField] AudioClip aud_CouldBuyItem;
	[SerializeField] AudioSource shopMusic;
	[SerializeField] AudioSource audioDevice;
	public AudioQueueScript audioQueue;
	delegate void OnChoicePicked();
	OnChoicePicked choicePicked;
	RaycastHit raycastHitItem;
	public Button yesBuyItemButton;
	public Button noBuyItemButton;
	public Button yesReplaceItemButton;
	public Button noReplaceItemButton;
	public GameObject itemReplaceConfirmation;
	int[] startingItems = new int[3];
	void Start()
	{
		Instance = this;
		shopMusic.Play();
		yesBuyItemButton.onClick.AddListener((TryBuyItem));
		noBuyItemButton.onClick.AddListener(() => OnBoughtItem("noBuyItem"));
		choicePicked = () => OnBoughtItem("choicePicked");
		noReplaceItemButton.onClick.AddListener(() =>
		{
			itemReplaceConfirmation.SetActive(false);
			UnlockPlayerThings();
		});
		SetPointsText("Points: " + PlayerPrefs.GetInt("ShopPoints"));
		audioQueue.QueueAudio(aud_ShopGreet);
		audioQueue.QueueAudio(aud_SelectItem);
		void OnBoughtItem(string name)
		{
			buyItemComfirmation.SetActive(false);
			if (name == "noBuyItem")
			{
				UnlockPlayerThings();
			}
			else if (name == "choicePicked")
			{
				return;
			}
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0) && !confirming)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit raycastHit, 10f) && raycastHit.collider.CompareTag("ShopInteractable") && !confirming)
			{
				buyItemComfirmation.SetActive(true);
				audioDevice.Stop();
				audioDevice.PlayOneShot(aud_Confirmation);
				raycastHitItem = raycastHit;
				LockPlayerThings();
			}
		}
	}
	void LockPlayerThings()
	{
		PlayerScript.instance.LockMouseMove();
		GameControllerScript.i.UnlockMouse();
		PlayerScript.instance.FreezePlayer();
		confirming = true;
	}
	void UnlockPlayerThings()
	{
		PlayerScript.instance.UnlockMouseMove();
		GameControllerScript.i.LockMouse();
		PlayerScript.instance.UnFreezePlayer();
		confirming = false;
	}
	void SetPointsText(string value)
	{
		pointsText.text = value;
	}
	public void TryBuyItem()
	{
		int itemID = GetItemID(raycastHitItem.collider.name);
		int selectedItem = GameControllerScript.i.itemSelected;
		int itemCost = GetItemCost(raycastHitItem.collider.name);
		int currentShopPoints = PlayerPrefs.GetInt("ShopPoints");
		if (currentShopPoints >= itemCost)
		{
			AddItem(itemID, selectedItem);
		}
		else
		{
			audioDevice.PlayOneShot(aud_NotEnough);
			UnlockPlayerThings();
		}
		choicePicked();
	}
	int GetItemCost(string name)
	{
		int itemCost = 0;
		switch (name)
		{
			case "ZestyBar": itemCost = 40; break;
			case "YellowDoorLock": itemCost = 50; break;
			case "Key": itemCost = 25; break;
			case "BSODA": itemCost = 100; break;
			case "Flashlight": itemCost = 35; break;
			case "Tape": itemCost = 60; break;
			case "AlarmClock": itemCost = 45; break;
			case "WD - 3D": itemCost = 30; break;
			case "SafetyScissors": itemCost = 25; break;
			case "MagicWand": itemCost = 40; break;
			case "ZipZapper": itemCost = 75; break;
			case "KnifeSteamer": itemCost = 70; break;
			case "FalseAlarm": itemCost = 55; break;
			case "SafetyFlamethrower": itemCost = 40; break;
			case "GrapplingHook": itemCost = 80; break;
			case "Radar": itemCost = 25; break;
			case "ColorfulSaw": itemCost = 55; break;
			case "BoomBox": itemCost = 65; break;
			case "WoodWand": itemCost = 80; break;
		}
		return itemCost;
	}
	int GetItemID(string name)
	{
		int itemID = 0;
		switch (name)
		{
			case "ZestyBar": itemID = 1; break;
			case "YellowDoorLock": itemID = 2; break;
			case "Key": itemID = 3; break;
			case "BSODA": itemID = 4; break;
			case "Flashlight": itemID = 5; break;
			case "Tape": itemID = 6; break;
			case "AlarmClock": itemID = 7; break;
			case "WD - 3D": itemID = 8; break;
			case "SafetyScissors": itemID = 9; break;
			case "MagicWand": itemID = 10; break;
			case "ZipZapper": itemID = 11; break;
			case "KnifeSteamer": itemID = 12; break;
			case "FalseAlarm": itemID = 13; break;
			case "SafetyFlamethrower": itemID = 14; break;
			case "GrapplingHook": itemID = 15; break;
			case "Radar": itemID = 18; break;
			case "ColorfulSaw": itemID = 19; break;
			case "BoomBox": itemID = 20; break;
			case "WoodWand": itemID = 21; break;
		}
		return itemID;
	}
	void AddItem(int itemID, int selectedItem)
	{
		int itemCost = GetItemCost(raycastHitItem.collider.name);
		int currentShopPoints = PlayerPrefs.GetInt("ShopPoints");
		if (startingItems[selectedItem] == 0)
		{
			AddOrReplaceItem();
			UnlockPlayerThings();
		}
		else
		{
			itemReplaceConfirmation.SetActive(true);
			buyItemComfirmation.SetActive(false);
			itemReplaceConfirmationText = "The item you're trying to buy (" + raycastHitItem.collider.name + ") replaces the item at item slot " + selectedItem +
				"\n Are you sure you want to make this action?";
			itemReplaceText.text = itemReplaceConfirmationText;
			yesReplaceItemButton.onClick.AddListener(AddOrReplaceItem);
		}
		void AddOrReplaceItem()
		{
			startingItems[selectedItem] = itemID;
			GameControllerScript.i.itemSlot[selectedItem].texture = GameControllerScript.i.itemTextures[itemID];
			audioDevice.PlayOneShot(aud_CouldBuyItem);
			currentShopPoints -= itemCost;
			PlayerPrefs.SetInt("ShopPoints", currentShopPoints);
			SetPointsText("Points: " + PlayerPrefs.GetInt("ShopPoints"));
			itemReplaceConfirmation.SetActive(false);
			buyItemComfirmation.SetActive(false);
			UnlockPlayerThings();
			SerializeBoughtItems();
		}
	}
	public void SerializeBoughtItems()
	{
		for (int i = 0; i < startingItems.Length; i++)
		{
			PlayerPrefs.SetInt("StartingItem" + (i + 1), startingItems[i]);
			print("Starting item slot" + i + "has an item ID of" + startingItems[i]);
		}
	}
}