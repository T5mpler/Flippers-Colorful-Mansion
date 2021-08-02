using UnityEngine;

// Token: 0x0200003B RID: 59
public class PickupScript : MonoBehaviour
{
	private void Update()
	{
		RaycastHit raycastHit;
		if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 10f))
		{
			void PickupItem(int item)
			{
				raycastHit.collider.gameObject.SetActive(false);
				if (!breakRoomItem)
				{
					Destroy(raycastHit.collider.gameObject);
				}
				this.gc.CollectItem(item);
			}
			void CollectMoney(float moneyAmount)
			{
				raycastHit.collider.gameObject.SetActive(false);
				if (!breakRoomItem)
				{
					Destroy(raycastHit.transform.gameObject);
				}
				this.gc.money += moneyAmount;
				this.gc.UpdateMoneyCount();
			}
			if (canBePickedUp)
			{
				switch (raycastHit.transform.name)
				{
					case "Pickup_EnergyFlavoredZestyBar": PickupItem(1); break;
					case "Pickup_YellowDoorLock": PickupItem(2); break;
					case "Pickup_Key": PickupItem(3); break;
					case "Pickup_BSODA": PickupItem(4); break;
					case "Pickup_Flashlight": PickupItem(5); break;
					case "Pickup_Tape": PickupItem(6); break;
					case "Pickup_AlarmClock": PickupItem(7); break;
					case "Pickup_WD-3D": PickupItem(8); break;
					case "Pickup_SafetyScissors": PickupItem(9); break;
					case "Pickup_MagicWand": PickupItem(10); break;
					case "Pickup_ZipZapper": PickupItem(11); break;
					case "Pickup_KnifeSteamer": PickupItem(12); break;
					case "Pickup_FalseAlarm": PickupItem(13); break;
					case "Pickup_SafetyFlameThrower": PickupItem(14); break;
					case "Pickup_GrapplingHook": PickupItem(15); break;
					case "Pickup_MemeCD": PickupItem(16); break;
					case "Pickup_SongCD": PickupItem(17); break;
					case "Pickup_Radar": PickupItem(18); break;
					case "Pickup_ColorfulSaw": PickupItem(19); gc.chainSawUses = UnityEngine.Random.Range(3, 5); break;
					case "Pickup_BoomBox": PickupItem(20); break;
					case "Pickup_WoodWand": PickupItem(21); gc.woodWantUses = UnityEngine.Random.Range(3, 5); break;
					case "Pickup_Amp": PickupItem(22); break;
					case "Pickup_Penny": CollectMoney(0.01f); break;
					case "Pickup_Nickel": CollectMoney(0.05f); break;
					case "Pickup_Dime": CollectMoney(0.1f); break;
					case "Pickup_Quarter": CollectMoney(0.25f); break;
					case "Pickup_1Dollar": CollectMoney(1f); break;
					case "Pickup_5Dollar": CollectMoney(5f); break;
					case "Pickup_Present":
						raycastHit.transform.gameObject.SetActive(false);
						Destroy(raycastHit.collider.gameObject);
						int item_ID = Mathf.RoundToInt(Random.Range(0f, 18f));
						this.gc.CollectItem(item_ID);
						break;
				}
			}
		}
	}

	public GameControllerScript gc;

	public Transform player;

	public bool canBePickedUp;

	public bool breakRoomItem;

}
