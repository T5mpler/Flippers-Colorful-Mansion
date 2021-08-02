using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000030 RID: 48
public class JoeWondererScript : MonoBehaviour
{
	// Token: 0x060000E6 RID: 230 RVA: 0x00008DC4 File Offset: 0x00006FC4
	private void Start()
	{
		if (!this.gc.stunned)
		{
			this.agent.speed = (float)Mathf.RoundToInt(Random.Range(15f, 20f)) * am.TotalMultipler;
		}
		this.agent = base.GetComponent<NavMeshAgent>();
		this.Wander();
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00008E10 File Offset: 0x00007010
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
		if (this.gc.officeParty)
		{
			this.agent.SetDestination(this.gc.partyPositions[2].position);
		}
		if (!this.audioDevice.isPlaying && this.audioInQueue > 0)
		{
			this.PlayQueue();
		}
		if (this.playCooldown > 0f)
		{
			this.playCooldown -= Time.deltaTime;
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.beatingUp & this.beatUps <= 6)
		{
			this.agent.SetDestination(this.player.position);
		}
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00008ED4 File Offset: 0x000070D4
	private void FixedUpdate()
	{
		if (!this.gc.inJoeGame)
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" & this.playCooldown <= 0f)
			{
				if (Vector3.Distance(base.transform.position, this.player.position) <= 20f)
				{
					this.db = true;
					this.TargetPlayer();
					return;
				}
			}
			else
			{
				this.db = false;
				if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
				{
					this.Wander();
					return;
				}
			}
		}
		else
		{
			this.gc.inJoeGame = true;
			if (this.gc.stunnedCharacter.transform.name != base.transform.name)
			{
				this.agent.speed = 0f;
			}
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00009008 File Offset: 0x00007208
	private void Wander()
	{
		agent.speed = 15f * am.TotalMultipler;
		this.wanders++;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
		if (this.wanders >= 5)
		{
			this.wanders = 0;
			int num = Mathf.RoundToInt(Random.Range(0f, 1f));
			this.QueueAudio(this.aud_Wanders[num]);
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00009084 File Offset: 0x00007284
	private void TargetPlayer()
	{
		this.playerSpotted = true;
		this.joeSprite.sprite = this.joe_Eh;
		if (!this.audioDevice.isPlaying)
		{
			this.audioDevice.PlayOneShot(this.aud_Found);
		}
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 20f * am.TotalMultipler;
		}
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00009120 File Offset: 0x00007320
	public void DecideEmotion()
	{
		if (this.wrongAnswers == 3)
		{
			this.joeSprite.sprite = this.joe_Mad;
			this.audioDevice.PlayOneShot(this.aud_redoQuestions);
			this.gc.ActivateJoeGame();
		}
		if (this.wrongAnswers == 1 || this.wrongAnswers == 2)
		{
			this.beatingUp = true;
			this.joeSprite.sprite = this.joe_Mad;
			this.audioDevice.PlayOneShot(this.aud_Incorrect);
		}
		else if (this.wrongAnswers == 0)
		{
			this.audioDevice.PlayOneShot(this.aud_Correct);
			this.joeSprite.sprite = this.joe_Happy;
			if (Mathf.RoundToInt(Random.Range(0f, 1f)) == 1 && gc.currentChallenge != ChallengeManagerScript.ChallengeType.NoItems)
			{
				this.QueueAudio(this.aud_Item);
				int item_ID = Mathf.RoundToInt(Random.Range(0f, 9f));
				this.gc.CollectItem(item_ID);
			}
		}
		this.playCooldown = 30f;
		if (this.gc.started)
		{
			this.gc.started = false;
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00009232 File Offset: 0x00007432
	private void QueueAudio(AudioClip sound)
	{
		this.audioQueue[this.audioInQueue] = sound;
		this.audioInQueue++;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00009250 File Offset: 0x00007450
	private void PlayQueue()
	{
		this.audioDevice.PlayOneShot(this.audioQueue[0]);
		this.UnqueueAudio();
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000926C File Offset: 0x0000746C
	private void UnqueueAudio()
	{
		for (int i = 1; i < this.audioInQueue; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue--;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000092AA File Offset: 0x000074AA
	public void BeatUp()
	{
		base.transform.position = base.transform.forward * 15f;
		this.beatUps++;
		int pushBackTimes = Mathf.RoundToInt(Random.Range(200f, 300f));
		StartCoroutine(Pushback(pushBackTimes));
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000092BC File Offset: 0x000074BC
	public void GetSad()
	{
		this.joeSprite.sprite = this.joe_sad;
		this.gc.DeactivateJoeGame(GameObject.Find("JoeGame(Clone)"));
		this.agent.speed = (float)Mathf.RoundToInt(Random.Range(15f, 20f)) * am.TotalMultipler;
		this.QueueAudio(this.joe_Cry);
		this.QueueAudio(this.aud_JustWant);
		this.playCooldown = 30f;
	}
	IEnumerator Pushback(int maxPushback)
	{
		Rigidbody rb = player.GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * maxPushback, ForceMode.Impulse);
		transform.position -= transform.forward * 10f;
		yield break;
	}
	// Token: 0x040001C8 RID: 456
	public bool db;

	// Token: 0x040001C9 RID: 457
	public Transform player;

	// Token: 0x040001CA RID: 458
	public Transform wanderTarget;

	// Token: 0x040001CB RID: 459
	public AILocationSelectorScript wanderer;

	// Token: 0x040001CC RID: 460
	public float coolDown;

	// Token: 0x040001CD RID: 461
	public NavMeshAgent agent;

	// Token: 0x040001CE RID: 462
	public JoeGameScript jg;

	// Token: 0x040001CF RID: 463
	public GameControllerScript gc;

	// Token: 0x040001D0 RID: 464
	public float playCooldown;

	// Token: 0x040001D1 RID: 465
	private int wanders;

	// Token: 0x040001D2 RID: 466
	public AudioSource audioDevice;

	// Token: 0x040001D3 RID: 467
	public AudioClip aud_Item;

	// Token: 0x040001D4 RID: 468
	public AudioClip[] aud_Wanders = new AudioClip[2];

	// Token: 0x040001D5 RID: 469
	public AudioClip aud_Found;

	// Token: 0x040001D6 RID: 470
	public AudioClip aud_Correct;

	// Token: 0x040001D7 RID: 471
	public AudioClip aud_Incorrect;

	// Token: 0x040001D8 RID: 472
	private bool talking;

	// Token: 0x040001D9 RID: 473
	private int audioInQueue;

	// Token: 0x040001DA RID: 474
	private AudioClip[] audioQueue = new AudioClip[20];

	// Token: 0x040001DB RID: 475
	public Sprite joe_Happy;

	// Token: 0x040001DC RID: 476
	public Sprite joe_Mad;

	// Token: 0x040001DD RID: 477
	public Sprite joe_Eh;

	// Token: 0x040001DE RID: 478
	public SpriteRenderer joeSprite;

	// Token: 0x040001DF RID: 479
	public AudioClip joe_Cry;

	// Token: 0x040001E0 RID: 480
	public AudioClip aud_JustWant;

	// Token: 0x040001E1 RID: 481
	public int wrongAnswers;

	// Token: 0x040001E2 RID: 482
	public bool beatingUp;

	// Token: 0x040001E3 RID: 483
	public int beatUps;

	// Token: 0x040001E4 RID: 484
	public PlayerScript ps;

	// Token: 0x040001E5 RID: 485
	public float lostCapability;

	// Token: 0x040001E6 RID: 486
	public float timer;

	// Token: 0x040001E7 RID: 487
	public bool playerSpotted;

	// Token: 0x040001E8 RID: 488
	public Sprite joe_sad;

	// Token: 0x040001E9 RID: 489
	public AudioClip aud_redoQuestions;
}
