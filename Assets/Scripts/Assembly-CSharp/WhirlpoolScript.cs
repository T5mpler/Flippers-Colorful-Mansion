using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WhirlpoolScript : MonoBehaviour
{
	public LayerMask colliderLayerMask;
	public Transform centerPoint;
	bool formed;
	public bool teleporting;
	bool checkSubject = true;
	List<Collider> hitColliders = new List<Collider>();
	Transform teleportingSubject;
	bool isPlayerTeleporting;
	bool deforming;
	float subjectOriginPosition;
	private void Start()
	{
		StartCoroutine(Form());
	}
	private void Update()
	{
		float rotateSpeed = 200f;
		transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
		if (GameControllerScript.i.notebooks >= 2)
		{
			foreach (Collider collider in hitColliders)
			{
				if (collider != null && formed && !teleporting && checkSubject && !deforming)
				{
					Vector3 direction = (transform.position - collider.transform.position).normalized;
					direction.y = 0f;
					collider.transform.position += direction * Time.deltaTime * 5f;
				}
			}
		}
	}
	private void FixedUpdate()
	{
		if (!teleporting)
		{
			Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, colliderLayerMask, QueryTriggerInteraction.Ignore);
			hitColliders = colliders.ToList();
		}
	}
	void Despawn()
	{
		FloodEventScript.instance.whirlpoolList.Remove(gameObject);
		Destroy(gameObject);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("NPC") && !teleporting && GameControllerScript.i.notebooks >= 2)
		{
			StartCoroutine(Teleport(other.transform, other.CompareTag("Player")));
		}
	}
	IEnumerator Form()
	{
		GetComponent<Collider>().enabled = false;
		transform.localScale = Vector3.zero;
		Vector3 scale = transform.localScale;
		while (scale.x < 5f)
		{
			scale += Vector3.one * Time.deltaTime * 3f;
			transform.localScale = scale;
			yield return null;
		}
		transform.localScale = Vector3.one * 5f;
		GetComponent<Collider>().enabled = true;
		formed = true;
		yield break;
	}
	public IEnumerator Deform(float deformSpeed = 1f)
	{
		deforming = true;
		GetComponent<Collider>().enabled = false;
		Vector3 scale = transform.localScale;
		while (scale.x > 0f)
		{
			scale -= Vector3.one * Time.deltaTime * deformSpeed;
			transform.localScale = scale;
			yield return null;
		}
		transform.localScale = Vector3.zero;
		Despawn();
		yield break;
	}
	IEnumerator Teleport(Transform subject, bool isPlayer)
	{
		isPlayerTeleporting = isPlayer;
		teleportingSubject = subject;
		subjectOriginPosition = subject.position.y;
		float originalSubjectY = subject.position.y;
		float sinkSpeed = 3f;
		int loseItemIndex = UnityEngine.Random.Range(0, GameControllerScript.i.item.Length);
		teleporting = true;
		if (PlayerScript.instance.currentHeldNumberBallon != null && isPlayer)
		{
			PlayerScript.instance.UnpickupNumberBallon();
		}
		subject.position = new Vector3(transform.position.x, subject.position.y, transform.position.z);
		FloodEventScript flood = FloodEventScript.instance;
		subject.GetComponent<Collider>().enabled = false;
		if (isPlayer)
		{
			GameControllerScript.i.LoseItem(loseItemIndex);
			PlayerScript.instance.FreezePlayer();
		}
		else
		{
			subject.GetComponent<NavMeshAgent>().isStopped = true;
			subject.GetComponent<NavMeshAgent>().enabled = false;
		}
		float targetY = isPlayer ? 0.5f : originalSubjectY - 3f;
		while (subject.position.y > targetY)
		{
			subject.position -= Vector3.up * Time.deltaTime * sinkSpeed;
			yield return null;
		}
		GameControllerScript gc = GameControllerScript.i;
		Color color = gc.blackScreen.color;
		WhirlpoolScript newWhirlpool = flood.SpawnWhirlpool();
		newWhirlpool.transform.localScale = new Vector3(1f, 0f, 1f);
		Vector3 newPosition = new Vector3(newWhirlpool.transform.position.x, 0.5f, newWhirlpool.transform.position.z);
		if (isPlayer)
		{
			while (color.a < 1f)
			{
				color.a += Time.deltaTime * 2f;
				gc.blackScreen.color = color;
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			subject.position = newPosition;
			while (color.a > 0f)
			{
				color.a -= Time.deltaTime * 1.5f;
				gc.blackScreen.color = color;
				yield return null;
			}
		}
		else
		{
			subject.position = newPosition;
		}
		while (subject.position.y < originalSubjectY)
		{
			subject.position += Vector3.up * Time.deltaTime * sinkSpeed;
			yield return null;
		}
		subject.position = new Vector3(newWhirlpool.transform.position.x, originalSubjectY, newWhirlpool.transform.position.z);
		subject.GetComponent<Collider>().enabled = true;
		if (isPlayer)
		{
			PlayerScript.instance.UnFreezePlayer();
		}
		else
		{
			subject.GetComponent<NavMeshAgent>().isStopped = false;
			subject.GetComponent<NavMeshAgent>().enabled = true;
		}
		teleporting = false;
		StartCoroutine(WaitForPlayerOutOfReach(newWhirlpool, subject));
		StartCoroutine(Deform());
		yield break;
	}
	private void OnDestroy()
	{
		GameControllerScript.i.blackScreen.color = Color.clear;
		if (teleporting)
		{
			if (isPlayerTeleporting && teleportingSubject != null)
			{
				PlayerScript.instance.UnFreezePlayer();
				teleportingSubject.position = new Vector3(teleportingSubject.position.x, subjectOriginPosition, teleportingSubject.position.z);
			}
			else
			{
				teleportingSubject.GetComponent<NavMeshAgent>().enabled = true;
				teleportingSubject.GetComponent<NavMeshAgent>().isStopped = false;
			}
			teleportingSubject.GetComponent<Collider>().enabled = true;
		}
	}

	IEnumerator WaitForPlayerOutOfReach(WhirlpoolScript whirlpool, Transform subject)
	{
		whirlpool.GetComponent<Collider>().enabled = false;
		whirlpool.checkSubject = false;
		if (whirlpool != null)
		{
			while (Vector3.Distance(whirlpool.transform.position, subject.transform.position) <= 50f)
			{
				yield return null;
			}
			whirlpool.GetComponent<Collider>().enabled = true;
			whirlpool.checkSubject = true;
		}
		yield break;
	}
}