using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000004 RID: 4
public class InverjaScript : MonoBehaviour
{
	// Token: 0x06000009 RID: 9 RVA: 0x000028B2 File Offset: 0x00000AB2
	public ActivityModifierScript am
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000028C6 File Offset: 0x00000AC6
	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (letterCooldown > 0f)
		{
			letterCooldown -= Time.deltaTime;
		}
		if (am.TotalAdder.magnitude > 0f)
		{
			agent.velocity += am.TotalAdder;
		}
		if (hummingCooldown > 0f)
		{
			hummingCooldown -= Time.deltaTime;
		}
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000028F0 File Offset: 0x00000AF0
	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.CompareTag("Player") && letterCooldown <= 0f && !throwingLetter)
		{
			this.db = true;
			this.TargetPlayer();
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
		agent.speed = 20f * am.TotalMultipler;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		if (hummingCooldown <= 0f)
		{
			if (new System.Random().Next(8) > 3)
			{
				audioDevice.PlayOneShot(aud_Hum);
			}
			hummingCooldown = UnityEngine.Random.Range(20f, 35f);
		}
		this.coolDown = 1f;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x000029C6 File Offset: 0x00000BC6
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
		if (!targetingPlayer && !throwingLetter)
		{
			agent.speed = 25f * am.TotalMultipler;
			audioDevice.PlayOneShot(aud_Subject);
			targetingPlayer = true;
		}
	}
	IEnumerator Sequence()
	{
		float maxHeight = 3f;
		float minHeight = 1.5f;
		float riseSpeed = 2f;
		Transform child = transform.GetChild(0);
		animator.enabled = false;
		sr.sprite = riseSprite;
		agent.isStopped = true;
		while (child.localPosition.y < maxHeight)
		{
			child.position += Vector3.up * riseSpeed * Time.deltaTime;
			yield return null;
		}
		System.Random random = new System.Random();
		float maxAmount = 4;
		float minAmount = 2;
		int amount = (int)((maxAmount + minAmount) * random.NextDouble() + minAmount);
		yield return new WaitForSeconds(2f);
		sr.sprite = throwALetter;
		for (int i = 0; i < amount; i++)
		{
			audioDevice.PlayOneShot(aud_Throw);
			sr.sprite = throwALetter;
			GameObject letter = Instantiate(letterPrefab, transform.position, Quaternion.identity);
			Letter letterScript = letter.GetComponent<Letter>();
			letterScript.player = player;
			string randomLetter = potentialKeys[UnityEngine.Random.Range(0, potentialKeys.Length)];
			letterScript.Initalize(randomLetter, UnityEngine.Random.Range(10f, 25f), this);
			yield return new WaitForSeconds(1.5f);
		}
		sr.sprite = riseSprite;
		StartCoroutine(CheckHits());
		while (child.localPosition.y > minHeight)
		{
			child.position -= Vector3.up * riseSpeed * Time.deltaTime;
			yield return null;
		}
		letterCooldown = UnityEngine.Random.Range(30f, 60f);
		throwingLetter = false;
		agent.isStopped = false;
		Wander();
		animator.enabled = true;
		animator.SetBool("Walking", true);
		yield break;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (letterCooldown <= 0f && other.CompareTag("Player") && targetingPlayer && !throwingLetter)
		{
			throwingLetter = true;
			animator.SetBool("Walking", false);
			targetingPlayer = false;
			StartCoroutine(Sequence());
		}
	}
	IEnumerator CheckHits()
	{
		if (totalHits >= 5)
		{
			audioDevice.PlayOneShot(aud_Ha);
			yield return new WaitForSeconds(1f);
			PlayerScript.instance.gameOver = true;
		}
		totalHits = 0;
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

	bool targetingPlayer;

	// Token: 0x04000012 RID: 18
	private NavMeshAgent agent;

	float letterCooldown;

	public GameObject letterPrefab;
	public string[] potentialKeys;
	public Animator animator;
	public SpriteRenderer sr;
	public Sprite throwALetter;
	public Sprite throwCool;
	public Sprite riseSprite;
	public AudioClip aud_Ha, aud_Throw, aud_Hum, aud_Subject;
	public AudioSource audioDevice;
	public int totalHits;
	float hummingCooldown;
	bool throwingLetter;
}