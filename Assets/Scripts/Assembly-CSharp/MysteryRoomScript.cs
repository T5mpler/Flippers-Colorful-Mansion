using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MysteryRoomScript : MonoBehaviour
{
	public MysteryRoomPoint[] roomPoints;
	MysteryRoomPoint currentRoomPoint;
	public Transform[] itemPositions;
	List<GameObject> itemsInRoom = new List<GameObject>();
	Transform player;
	public Transform endPoint;
	bool playerInRoom;
	private void Awake()
	{
		player = PlayerScript.instance.transform;
	}
	private void OnEnable()
	{
		itemsInRoom = new List<GameObject>();
		ResetRoom();
	}
	void ResetRoom()
	{
		MysteryRoomPoint roomPoint = roomPoints[UnityEngine.Random.Range(0, roomPoints.Length)];
		transform.position = roomPoint.position;
		transform.rotation = roomPoint.rotation;
		foreach (GameObject blockingObject in roomPoint.blockingObjects)
		{
			blockingObject.SetActive(false);
		}
		NewItems();
		currentRoomPoint = roomPoint;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRoom = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			playerInRoom = false;
		}
	}
	public void StopEvent()
	{
		using (List<GameObject>.Enumerator enumerator1 = currentRoomPoint.blockingObjects.ToList().GetEnumerator())
		{
			while (enumerator1.MoveNext())
			{
				GameObject blockingObject = enumerator1.Current;
				blockingObject.SetActive(true);
			}
		}
		if (playerInRoom)
		{
			player.position = new Vector3(endPoint.position.x, player.position.y, endPoint.position.z);
		}
		using (List<GameObject>.Enumerator enumerator = itemsInRoom.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject item = enumerator.Current;
				Destroy(item);
			}
		}
		itemsInRoom.Clear();
		gameObject.SetActive(false);
	}
	void NewItems()
	{
		for (int i = 0; i < itemPositions.Length; i++)
		{
			Sprite randomItemSprite = GameControllerScript.i.itemSprites[UnityEngine.Random.Range(0, GameControllerScript.i.itemSprites.Length)];
			GameObject item = GameControllerScript.i.MakeNewItem(itemPositions[i].position, transform.Find("ItemParent"), randomItemSprite);
			item.GetComponent<PickupScript>().canBePickedUp = true;
			itemsInRoom.Add(item);
		}
	}
}
