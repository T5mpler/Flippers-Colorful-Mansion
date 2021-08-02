using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200003F RID: 63
public class PrincipalScript : MonoBehaviour
{
	// Token: 0x0600013D RID: 317 RVA: 0x0000DAE4 File Offset: 0x0000BCE4
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioQueue = base.GetComponent<AudioQueueScript>();
		this.audioDevice = base.GetComponent<AudioSource>();
		Wander();
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000DB0C File Offset: 0x0000BD0C
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
			this.agent.SetDestination(this.gc.partyPositions[5].position);
		}
		if (this.seesRuleBreak)
		{
			this.timeSeenRuleBreak += 1f * Time.deltaTime;
			if ((double)this.timeSeenRuleBreak >= 0.5 & !this.angry)
			{
				this.angry = true;
				this.seesRuleBreak = false;
				this.timeSeenRuleBreak = 0f;
				this.TargetPlayer();
				this.CorrectPlayer();
			}
		}
		else
		{
			this.timeSeenRuleBreak = 0f;
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (wizardDetentionTime > 0f)
		{
			wizardDetentionTime -= Time.deltaTime;
		}
		else if (wizardInDetention)
		{
			wizardScript.enabled = true;
			wizardScript.agent.enabled = true;
			wizardInDetention = false;
			wizardScript.agent.isStopped = false;
			wizardScript.agent.ResetPath();
			wizardScript.Wander();
		}
		if (wizardSeen)
		{
			agent.SetDestination(wizard.position);
		}
	}

	void LostPlayer()
	{
		this.seesRuleBreak = false;
		if (this.agent.velocity.magnitude <= 0.1f & this.coolDown <= 0f)
		{
			this.Wander();
		}
	}
	private void FixedUpdate()
	{
		if (!this.angry)
		{
			Vector3 direction = (player.position - transform.position).normalized;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) && raycastHit.collider.CompareTag("Player"))
			{
				if (GameControllerScript.i.currentChallenge != ChallengeManagerScript.ChallengeType.Stealthy)
				{
					if (playerScript.guilt > 0f & !inOffice & !angry)
					{
						this.seesRuleBreak = true;
					}
					else
					{
						LostPlayer();
					}
				}
				else
				{
					quietAudioDevice.PlayOneShot(aud_Night);
					angry = true;
					TargetPlayer();
				}
			}
			else
			{
				LostPlayer();
			}
			if (GameControllerScript.i.currentChallenge != ChallengeManagerScript.ChallengeType.Stealthy)
			{
				direction = (bully.position - transform.position).normalized;
				if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.name == "Its a Bully" & this.bullyScript.guilt > 0f & !this.inOffice & !this.angry)
				{
					this.TargetBully();
					return;
				}
				direction = (wizard.position - transform.position).normalized;
				if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.name == "Le Wizard De Heckizos" & this.wizardScript.guilt > 0f & !this.inOffice & !this.angry)
				{
					this.TargetWizard();
					return;
				}
			}
		}
		else
		{
			this.TargetPlayer();
		}
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000DD34 File Offset: 0x0000BF34
	private void Wander()
	{
		this.agent.speed = 20f * am.TotalMultipler;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		if (this.agent.isStopped)
		{
			this.agent.isStopped = false;
		}
		this.coolDown = 1f;
		if (Random.Range(0f, 10f) <= 1f)
		{
			this.quietAudioDevice.PlayOneShot(this.aud_Whistle);
		}
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000DDC0 File Offset: 0x0000BFC0
	private void TargetPlayer()
	{
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 25f * am.TotalMultipler;
		}
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000DE26 File Offset: 0x0000C026
	private void TargetBully()
	{
		if (!this.bullySeen)
		{
			this.agent.SetDestination(this.bully.position);
			this.audioQueue.QueueAudio(this.audNoBullying);
			this.bullySeen = true;
		}
	}
	public void TargetWizard()
	{
		if (!wizardSeen)
		{
			agent.SetDestination(wizard.position);
			audioQueue.QueueAudio(audNoMagic);
			wizardScript.PrincipalFate();
			wizardSeen = true;
		}
	}
	// Token: 0x06000143 RID: 323 RVA: 0x0000DE60 File Offset: 0x0000C060
	private void CorrectPlayer()
	{
		this.audioQueue.ClearAudioQueue();
		if (this.playerScript.guiltType == "faculty")
		{
			this.audioQueue.QueueAudio(this.audNoFaculty);
			return;
		}
		if (this.playerScript.guiltType == "running")
		{
			this.audioQueue.QueueAudio(this.audNoRunning);
			return;
		}
		if (this.playerScript.guiltType == "drink")
		{
			this.audioQueue.QueueAudio(this.audNoDrinking);
			return;
		}
		if (this.playerScript.guiltType == "escape")
		{
			this.audioQueue.QueueAudio(this.audNoEscaping);
		}
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000DF1C File Offset: 0x0000C11C
	private void OnTriggerStay(Collider collider)
	{
		if (collider.name == "Office Trigger")
		{
			this.inOffice = true;
		}
		if (collider.name == "Player" & this.angry & !this.inOffice)
		{
			if (GameControllerScript.i.currentChallenge == ChallengeManagerScript.ChallengeType.Stealthy)
			{
				playerScript.gameOver = true;
			}
			GameControllerScript.i.EndAllGrapplingHooks();
			this.cs.inPool = false;
			this.inOffice = true;
			this.agent.Warp(new Vector3(10f, 0f, 170f));
			this.agent.isStopped = true;
			collider.transform.position = new Vector3(10f, collider.transform.position.y, 160f);
			collider.transform.LookAt(new Vector3(base.transform.position.x, collider.transform.position.y, base.transform.position.z));
			this.audioQueue.QueueAudio(this.aud_Delay);
			this.audioQueue.QueueAudio(this.audTimes[this.detentions]);
			this.audioQueue.QueueAudio(this.audDetention);
			int num = Mathf.RoundToInt(Random.Range(0f, 2f));
			this.audioQueue.QueueAudio(this.audScolds[num]);
			this.officeDoor.LockDoor((float)this.lockTime[this.detentions]);
			if (baldiScript.agent != null)
			{
				this.baldiScript.Hear(base.transform.position, 5f);
			}
			this.coolDown = 5f;
			this.angry = false;
			this.detentions++;
			if (this.detentions > 4)
			{
				if (PlayerPrefs.GetFloat("Detention") == 0f)
				{
					this.aPopUp.QueueAchievement(this.aPopUp.achievements[2]);
					PlayerPrefs.SetFloat("Detention", 1f);
				}
				this.detentions = 4;
			}
		}
		else if (collider.name == "Le Wizard De Heckizos" && !angry && !inOffice && !wizardInDetention && wizardScript.guilt > 0f)
		{
			wizardSeen = false;
			wizardInDetention = true;
			wizardScript.agent.Warp(new Vector3(10f, collider.transform.position.y, 160f));
			wizardScript.agent.isStopped = true;
			wizardScript.agent.enabled = false;
			wizardScript.enabled = false;
			wizardDetentionTime = UnityEngine.Random.Range(25f, 35f);
			wizardScript.audioDevice.PlayOneShot(wizardScript.aud_KnowItAll);
		}
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000E127 File Offset: 0x0000C327
	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			this.inOffice = false;
		}
		if (other.name == "Its a Bully")
		{
			this.bullySeen = false;
		}
	}

	// Token: 0x040002A6 RID: 678
	public bool seesRuleBreak;

	// Token: 0x040002A7 RID: 679
	public Transform player;

	// Token: 0x040002A8 RID: 680
	public Transform bully;

	// Token: 0x040002A9 RID: 681
	public bool bullySeen;

	// Token: 0x040002AA RID: 682
	public PlayerScript playerScript;

	// Token: 0x040002AB RID: 683
	public BullyScript bullyScript;

	// Token: 0x040002AC RID: 684
	public BaldiScript baldiScript;

	// Token: 0x040002AD RID: 685
	public Transform wanderTarget;

	// Token: 0x040002AE RID: 686
	public AILocationSelectorScript wanderer;

	// Token: 0x040002AF RID: 687
	public DoorScript officeDoor;

	// Token: 0x040002B0 RID: 688
	public float coolDown;

	// Token: 0x040002B1 RID: 689
	public float timeSeenRuleBreak;

	// Token: 0x040002B2 RID: 690
	public bool angry;

	// Token: 0x040002B3 RID: 691
	public bool inOffice;

	// Token: 0x040002B4 RID: 692
	private int detentions;

	// Token: 0x040002B5 RID: 693
	private int[] lockTime = new int[]
	{
		20,
		40,
		65,
		80,
		150
	};

	// Token: 0x040002B6 RID: 694
	public AudioClip[] audTimes = new AudioClip[5];

	// Token: 0x040002B7 RID: 695
	public AudioClip[] audScolds = new AudioClip[3];

	// Token: 0x040002B8 RID: 696
	public AudioClip audDetention;

	// Token: 0x040002B9 RID: 697
	public AudioClip audNoDrinking;

	// Token: 0x040002BA RID: 698
	public AudioClip audNoBullying;

	// Token: 0x040002BB RID: 699
	public AudioClip audNoFaculty;

	// Token: 0x040002BC RID: 700
	public AudioClip audNoLockers;

	// Token: 0x040002BD RID: 701
	public AudioClip audNoRunning;

	// Token: 0x040002BE RID: 702
	public AudioClip audNoStabbing;

	// Token: 0x040002BF RID: 703
	public AudioClip audNoEscaping;

	public AudioClip audNoMagic;

	public AudioClip audNoDisappear;

	// Token: 0x040002C0 RID: 704
	public AudioClip aud_Whistle;

	// Token: 0x040002C1 RID: 705
	public AudioClip aud_Delay;

	// Token: 0x040002C2 RID: 706
	private NavMeshAgent agent;

	// Token: 0x040002C3 RID: 707
	private AudioQueueScript audioQueue;

	// Token: 0x040002C4 RID: 708
	private AudioSource audioDevice;

	// Token: 0x040002C5 RID: 709
	public AudioSource quietAudioDevice;

	// Token: 0x040002C6 RID: 710
	public GameControllerScript gc;

	// Token: 0x040002C7 RID: 711
	public AchievementPopUpScript aPopUp;

	// Token: 0x040002C8 RID: 712
	public CameraScript cs;

	public Transform wizard;

	public AudioClip aud_Night;
	public WizardScript wizardScript;
	public bool wizardSeen;
	bool wizardInDetention;
	float wizardDetentionTime;
}
