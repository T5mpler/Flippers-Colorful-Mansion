using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

// Token: 0x02000004 RID: 4
public class LarryScript : MonoBehaviour
{
	// Token: 0x06000009 RID: 9 RVA: 0x000028B2 File Offset: 0x00000AB2
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
		sprintCooldown = UnityEngine.Random.Range(5f, 10f);
		sprintTick = true;
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000028C6 File Offset: 0x00000AC6
	public ActivityModifierScript am
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	private void Update()
	{
		if (am.TotalAdder.magnitude > 0f) agent.velocity += am.TotalAdder;
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (spellCooldown > 0f && sprintTick) spellCooldown -= Time.deltaTime;
		if (sprintCooldown > 0f && sprintTick)
		{
			sprintCooldown -= Time.deltaTime;
		}
		else if (!sprinting && sprintTick)
		{
			StartCoroutine(Sprint());
		}
		if (hitCollider != null && fireballWorld.activeSelf)
		{
			fireballWorld.transform.position = new Vector3(hitCollider.position.x, fireballWorld.transform.position.y, hitCollider.position.z);
		}
		if (heCooldown > 0f)
		{
			heCooldown -= Time.deltaTime;
		}
		else
		{
			heCooldown = UnityEngine.Random.Range(20f, 35f);
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000028F0 File Offset: 0x00000AF0
	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" && spellCooldown <= 0f)
		{
			StartCoroutine(FireballSequence());
			this.db = true;
			return;
		}
		this.db = false;
		if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
		{
			this.Wander();
		}
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002997 File Offset: 0x00000B97
	private void Wander()
	{
		if (!sprinting)
		{
			animator.SetBool("Sprint", false);
			animator.SetBool("Walking", true);
		}
		agent.speed = 15f * am.TotalMultipler;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		if (heCooldown <= 0)
		{
			if (new System.Random().Next(8) > 7)
			{
				audioDevice.PlayOneShot(aud_Hehe);
			}
			heCooldown = UnityEngine.Random.Range(20f, 35f);
		}
		this.coolDown = 1f;
	}
	IEnumerator Sprint()
	{
		audioDevice.PlayOneShot(aud_Haha);
		sprinting = true;
		animator.SetBool("Walking", false);
		animator.SetBool("Sprint", true);
		agent.speed = 35f * am.TotalMultipler;
		yield return new WaitForSeconds(1.5f);
		StopSprinting();
		yield break;
	}
	void StopSprinting()
	{
		sprinting = false;
		Wander();
		sprintCooldown = UnityEngine.Random.Range(5f, 10f);
	}
	IEnumerator FireballSequence()
	{
		spellCooldown = 20f;
		StopSprinting();
		sprintTick = false;
		agent.isStopped = true;
		animator.enabled = false;
		animator.enabled = true;
		animator.SetTrigger("CastingFireball");
		yield return new WaitForSeconds(3f);
		yield return StartCoroutine(Fireball());
		yield break;
	}
	IEnumerator Fireball()
	{
		audioDevice.PlayOneShot(aud_Waa);
		animator.enabled = false;
		animator.enabled = true;
		animator.SetTrigger("CastedFireball");
		Vector3 direction = (player.position - transform.position).normalized;
		direction.y = 0f;
		Fireball fireball = Instantiate(fireballPrefab, Vector3.Lerp(player.position, transform.position, 0.5f), Quaternion.identity).GetComponent<Fireball>();
		int chance = Mathf.RoundToInt(UnityEngine.Random.value);
		fireball.fireballType = (Fireball.FireballType)chance;
		fireball.GetComponentInChildren<SpriteRenderer>().sprite = fireballs[chance];
		fireballWorld.GetComponent<SpriteRenderer>().sprite = fireballWorlds[chance];
		fireballCanvas.GetComponent<Image>().sprite = fireballsCanvas[chance];
		fireball.Setup(direction, UnityEngine.Random.Range(10f, 20f));
		fireball.larry = this;
		yield return new WaitForSeconds(1f);
		sprintTick = true;
		agent.isStopped = false;
		Wander();
		yield break;
	}
	// Token: 0x0400000D RID: 13
	public bool db;

	// Token: 0x0400000E RID: 14
	public Transform player;

	// Token: 0x0400000F RID: 15
	public Transform wanderTarget;

	// Token: 0x04000010 RID: 16
	public AILocationSelectorScript wanderer;

	// Token: 0x04000011 RID: 17
	public float coolDown;

	// Token: 0x04000012 RID: 18
	private NavMeshAgent agent;

	public Animator animator;

	[HideInInspector] public Transform hitCollider;

	public float spellCooldown;

	float sprintCooldown;
	bool sprinting;
	bool sprintTick = true;
	float heCooldown;
	public GameObject fireballPrefab;
	public GameObject fireballWorld;
	public GameObject fireballCanvas;
	public Sprite[] fireballs;
	public Sprite[] fireballWorlds;
	public Sprite[] fireballsCanvas;
	public AudioClip aud_Gotcha, aud_Waa, aud_Hehe, aud_Haha;
	public AudioSource audioDevice;
}