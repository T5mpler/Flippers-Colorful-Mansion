using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
	public Rigidbody rb;
	public float speed = 10f;
	public MoveModifier moveMod;
	Vector3 moveVelocity;
	[HideInInspector] public LarryScript larry;
	Vector3 originalScale;
	Transform subject;
	public enum FireballType
	{
		Orange,
		Blue
	}
	public FireballType fireballType;
	float timeSinceSpawned;
	public void Setup(Vector3 velocity, float timer)
	{
		moveMod.timer = timer;
		moveVelocity = velocity;
		moveMod.OnMoveModRemoved += Removed;
	}
	private void Removed(object sender, System.EventArgs e)
	{
		ActivityModifierScript activityModifier = larry.hitCollider.GetComponent<ActivityModifierScript>();
		if (activityModifier != null && activityModifier.movementModList.Contains(moveMod))
		{
			activityModifier.movementModList.Remove(moveMod);
		}
		if (fireballType == FireballType.Blue)
		{
			subject.localScale = originalScale;
		}
		larry.fireballWorld.SetActive(false);
		larry.fireballCanvas.SetActive(false);
		larry.hitCollider = null;
		Destroy(gameObject);
	}

	private void Update()
	{
		timeSinceSpawned += Time.deltaTime;
		if (timeSinceSpawned > 30f && GetComponent<Collider>().enabled)
		{
			Destroy(gameObject);
		}
	}
	private void FixedUpdate()
	{
		rb.velocity = moveVelocity * speed;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("NPC") || other.CompareTag("Player") && other.name != "Its a Bully" && other.name != "Larry")
		{
			ActivityModifierScript activityModifier = other.GetComponent<ActivityModifierScript>();
			switch (fireballType)
			{
				case FireballType.Orange:
					moveMod.multipler = 0.2f;
					break;
				case FireballType.Blue:
					moveMod.multipler = 0.4f;
					StartCoroutine(Shrink(other.transform, other.CompareTag("Player")));
					break;
			}
			activityModifier.movementModList.Add(moveMod);
			if (other.CompareTag("Player"))
			{
				larry.audioDevice.PlayOneShot(larry.aud_Gotcha);
				larry.fireballCanvas.SetActive(true);
			}
			larry.hitCollider = other.transform;
			larry.fireballWorld.SetActive(true);
		}
		OnHitObject(other);
	}
	void OnHitObject(Collider other)
	{
		if (other.CompareTag("NPC") || other.CompareTag("Player") && other.name != "Its a Bully" && other.name != "Larry")
		{
			moveMod.tick = true;
			SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
			sr.sprite = null;
			moveVelocity = Vector3.zero;
			Collider collider = GetComponent<Collider>();
			collider.enabled = false;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	IEnumerator Shrink(Transform subject, bool isPlayer)
	{
		this.subject = subject;
		originalScale = subject.localScale;
		subject.localScale = new Vector3(subject.localScale.x / 1.5f, subject.localScale.y / 1.5f, subject.localScale.z / 1.5f);
		if (isPlayer) subject.position -= Vector3.up * 2f;
		yield break;
	}
}