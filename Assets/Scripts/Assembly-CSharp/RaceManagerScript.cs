using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RaceManagerScript : MonoBehaviour
{
	public BaldiScript baldi;
	public PlayerScript player;
	public Transform flag;
	public GameObject winScreen;
	public GameObject loseScreen;
	public List<Transform> someTransforms;
	bool goingToFlag;
	void Start()
	{
		baldi.agent = baldi.GetComponent<NavMeshAgent>();
		baldi.GetAngry(35f);
	}
	private void Update()
	{
		player.walkSpeed = 25f;
		player.runSpeed = 40f;
		if ((baldi.transform.position - baldi.agent.destination).magnitude <= 3f && !goingToFlag)
		{
			Transform[] transformArray = GameObject.Find("Maze").GetComponentsInChildren<Transform>();
			someTransforms = Array.FindAll(transformArray, transform => transform.name == "Floor").ToList();
			baldi.agent.SetDestination(someTransforms[UnityEngine.Random.Range(0, someTransforms.Count)].position);
		}
	}
	private void FixedUpdate()
	{
		Debug.DrawRay(baldi.transform.position, baldi.transform.forward, Color.black);
		if (Physics.Raycast(baldi.transform.position, baldi.transform.forward, out RaycastHit raycast, float.PositiveInfinity) && raycast.collider.name == "Flag")
		{
			goingToFlag = true;
			baldi.agent.SetDestination(raycast.point);
		}
	}
	public IEnumerator EndDelay(GameObject endScreen)
	{
		endScreen.SetActive(true);
		yield return new WaitForSeconds(3f);
		SceneManager.LoadScene("MainMenu");
	}
}