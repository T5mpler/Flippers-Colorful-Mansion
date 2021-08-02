using System;
using System.Collections;
using UnityEngine;
public class GrapplingHookScript : MonoBehaviour
{
	void Start()
	{
		transform.rotation = Camera.main.transform.rotation;
		mainAudioSource.PlayOneShot(start);
		GameControllerScript.i.grapplingHooks.Add(this);
		motorSound.pitch = 0f;
	}

	void Update()
	{
		if (!hookLocked)
		{
			rb.velocity = transform.forward * speed;
			totalTime += Time.deltaTime;
			if (totalTime > 15f || transform.position.y < -1f && !PlayerScript.instance.isUnderground || (transform.position - player.position).magnitude > maxBreakDistance)
			{
				EndGrapplingHook();
			}
		}
		else
		{
			Vector3 direction = (player.position - transform.position);
			direction.y = 0f;
			if (direction.magnitude <= stopDistance)
			{
				EndGrapplingHook();
			}
			moveMod.adder = (transform.position - player.position).normalized * force;
			if (!snapped)
			{
				float pitchMultipler = 1.25f;
				float newPitch = (Mathf.InverseLerp(startDistance, transform.position.magnitude, player.position.magnitude) + 1f) * pitchMultipler;
				this.motorSound.pitch = newPitch;
			}
			force += forceIncreaser * Time.deltaTime;
			pressure = (transform.position - player.position).magnitude - (startDistance - force);
			if (pressure > maxPressure && !snapped)
			{
				mainAudioSource.Stop();
				SnapGrapplingHook();
			}
		}
		linePositions[0] = transform.position;
		linePositions[1] = player.position;
		lineRenderer.SetPositions(linePositions);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("NPC") && !other.CompareTag("Player") && !other.isTrigger && other.gameObject.layer != 10 && other.gameObject.layer != 9 && !hookLocked && Vector3.Distance(player.position, transform.position) > 1f)
		{
			StartMovingPlayer();
		}
	}
	void StartMovingPlayer()
	{
		hookLocked = true;
		rb.velocity = Vector3.zero;
		mainAudioSource.PlayOneShot(clang);
		PlayerScript.instance.ActivityModifier.movementModList.Add(moveMod);
		motorSound.Play();
		startDistance = (transform.position - player.position).magnitude;
	}
	public void EndGrapplingHook()
	{
		if (PlayerScript.instance.ActivityModifier.movementModList.Contains(moveMod))
		{
			moveMod.adder = Vector3.zero;
			PlayerScript.instance.ActivityModifier.movementModList.Remove(moveMod);
		}
		motorSound.Stop();
		StartCoroutine(DelayByAudio());
		GameControllerScript.i.grapplingHooks.Remove(this);
		Destroy(gameObject);
	}
	void SnapGrapplingHook()
	{
		snapped = true;
		moveMod.adder = Vector3.zero;
		PlayerScript.instance.ActivityModifier.movementModList.Remove(moveMod);
		lineRenderer.enabled = false;
		motorSound.Stop();
		mainAudioSource.PlayOneShot(snap);
		StartCoroutine(DelayByAudio());
	}
	IEnumerator DelayByAudio()
	{
		yield return new WaitUntil(() => !mainAudioSource.isPlaying);
		EndGrapplingHook();
		yield break;
	}
	public Rigidbody rb;
	[HideInInspector] public Transform player;
	public LineRenderer lineRenderer;
	public MoveModifier moveMod;
	public AudioSource motorSound;
	public AudioSource mainAudioSource;
	public AudioClip start;
	public AudioClip clang;
	public AudioClip snap;
	readonly Vector3[] linePositions = new Vector3[2];
	const float speed = 100f;
	const float stopDistance = 6.5f;
	const float forceIncreaser = 30f;
	const float maxPressure = 125f;
	const float maxBreakDistance = 400f;
	float force;
	float pressure;
	float totalTime;
	float startDistance;
	bool hookLocked;
	bool snapped;
	public LayerMask layerMask;
}