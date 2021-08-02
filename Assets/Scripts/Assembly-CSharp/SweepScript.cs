using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000048 RID: 72
public class SweepScript : MonoBehaviour
{
	// Token: 0x06000169 RID: 361 RVA: 0x0000EA6E File Offset: 0x0000CC6E
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.origin = base.transform.position;
		this.waitTime = Random.Range(120f, 180f);
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000EAB0 File Offset: 0x0000CCB0
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
		agent.speed = 50f * am.TotalMultipler;
		if (this.gc.officeParty)
		{
			this.agent.SetDestination(this.gc.partyPositions[4].position);
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.waitTime > 0f)
		{
			this.waitTime -= Time.deltaTime;
			return;
		}
		if (!this.active)
		{
			this.active = true;
			this.wanders = 0;
			this.Wander();
			this.audioDevice.PlayOneShot(this.aud_Intro);
		}
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000EB5C File Offset: 0x0000CD5C
	private void FixedUpdate()
	{
		if ((double)this.agent.velocity.magnitude <= 0.1 & this.coolDown <= 0f & this.wanders < 5 & this.active)
		{
			this.Wander();
			return;
		}
		if (this.wanders >= 5)
		{
			this.GoHome();
		}
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
	private void Wander()
	{
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.wanderer.GetNewTargetHallway();
			this.agent.SetDestination(this.wanderTarget.position);
			this.coolDown = 1f;
			this.wanders++;
		}
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000EC38 File Offset: 0x0000CE38
	public void GoHome()
	{
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.SetDestination(this.origin);
			this.waitTime = Random.Range(120f, 180f);
			this.wanders = 0;
		}
		this.active = false;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000ECA1 File Offset: 0x0000CEA1
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC" || other.tag == "Player" && !gc.officeParty)
		{
			this.audioDevice.PlayOneShot(this.aud_Sweep);
		}
	}

	// Token: 0x040002EA RID: 746
	public Transform wanderTarget;

	// Token: 0x040002EB RID: 747
	public AILocationSelectorScript wanderer;

	// Token: 0x040002EC RID: 748
	public float coolDown;

	// Token: 0x040002ED RID: 749
	public float waitTime;

	// Token: 0x040002EE RID: 750
	public int wanders;

	// Token: 0x040002EF RID: 751
	public bool active;

	// Token: 0x040002F0 RID: 752
	private Vector3 origin;

	// Token: 0x040002F1 RID: 753
	public AudioClip aud_Sweep;

	// Token: 0x040002F2 RID: 754
	public AudioClip aud_Intro;

	// Token: 0x040002F3 RID: 755
	private NavMeshAgent agent;

	// Token: 0x040002F4 RID: 756
	private AudioSource audioDevice;

	// Token: 0x040002F5 RID: 757
	public GameControllerScript gc;
}
