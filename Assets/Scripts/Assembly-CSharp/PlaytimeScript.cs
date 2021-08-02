using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200003D RID: 61
public class PlaytimeScript : MonoBehaviour
{
	// Token: 0x06000132 RID: 306 RVA: 0x0000D6A5 File Offset: 0x0000B8A5
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.Wander();
	}

	public ActivityModifierScript am
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	private void Update()
	{
		if (this.gc.officeParty)
		{
			this.agent.SetDestination(this.gc.partyPositions[0].position);
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.playCool >= 0f)
		{
			this.playCool -= Time.deltaTime;
			return;
		}
		if (this.animator.GetBool("disappointed"))
		{
			this.playCool = 0f;
			this.animator.SetBool("disappointed", false);
		}
		if (am.TotalAdder.magnitude > 0f) agent.velocity += am.TotalAdder;
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000D774 File Offset: 0x0000B974
	private void FixedUpdate()
	{
		if (!this.ps.jumpRope)
		{
			Vector3 direction = this.player.position - base.transform.position;
			RaycastHit raycastHit;
			if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" & (base.transform.position - this.player.position).magnitude <= 80f & this.playCool <= 0f)
			{
				this.playerSeen = true;
				this.TargetPlayer();
			}
			else if (this.playerSeen & this.coolDown <= 0f)
			{
				this.playerSeen = false;
				this.Wander();
			}
			else if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
			{
				this.Wander();
			}
			this.jumpRopeStarted = false;
			return;
		}
		if (!this.jumpRopeStarted)
		{
			this.agent.Warp(base.transform.position - base.transform.forward * 10f);
		}
		this.jumpRopeStarted = true;
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 0f;
		}
		this.playCool = 30f;
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000D918 File Offset: 0x0000BB18
	private void Wander()
	{
		this.wanderer.GetNewTargetHallway();
		this.agent.SetDestination(this.wanderTarget.position);
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 15f * am.TotalMultipler;
		}
		this.playerSpotted = false;
		this.audVal = Mathf.RoundToInt(Random.Range(0f, 1f));
		if (!this.audioDevice.isPlaying)
		{
			this.audioDevice.PlayOneShot(this.aud_Random[this.audVal]);
		}
		this.coolDown = 1f;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000D9D0 File Offset: 0x0000BBD0
	private void TargetPlayer()
	{
		this.animator.SetBool("disappointed", false);
		this.agent.SetDestination(this.player.position);
		if (this.gc.stunnedCharacter.transform.name != base.transform.name)
		{
			this.agent.speed = 20f * am.TotalMultipler;
		}
		this.coolDown = 0.2f;
		if (!this.playerSpotted)
		{
			this.playerSpotted = true;
			this.audioDevice.PlayOneShot(this.aud_LetsPlay);
		}
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000DA67 File Offset: 0x0000BC67
	public void Disappoint()
	{
		this.animator.SetBool("disappointed", true);
		this.audioDevice.Stop();
		this.audioDevice.PlayOneShot(this.aud_Sad);
	}
	// Token: 0x0400028D RID: 653
	public bool db;

	// Token: 0x0400028E RID: 654
	public bool playerSeen;

	// Token: 0x0400028F RID: 655
	public bool disappointed;

	// Token: 0x04000290 RID: 656
	public int audVal;

	// Token: 0x04000291 RID: 657
	public Animator animator;

	// Token: 0x04000292 RID: 658
	public Transform player;

	// Token: 0x04000293 RID: 659
	public PlayerScript ps;

	// Token: 0x04000294 RID: 660
	public Transform wanderTarget;

	// Token: 0x04000295 RID: 661
	public AILocationSelectorScript wanderer;

	// Token: 0x04000296 RID: 662
	public float coolDown;

	// Token: 0x04000297 RID: 663
	public float playCool;

	// Token: 0x04000298 RID: 664
	public bool playerSpotted;

	// Token: 0x04000299 RID: 665
	public bool jumpRopeStarted;

	// Token: 0x0400029A RID: 666
	private NavMeshAgent agent;

	// Token: 0x0400029B RID: 667
	public AudioClip[] aud_Numbers = new AudioClip[10];

	// Token: 0x0400029C RID: 668
	public AudioClip[] aud_Random = new AudioClip[2];

	// Token: 0x0400029D RID: 669
	public AudioClip aud_Instrcutions;

	// Token: 0x0400029E RID: 670
	public AudioClip aud_Oops;

	// Token: 0x0400029F RID: 671
	public AudioClip aud_LetsPlay;

	// Token: 0x040002A0 RID: 672
	public AudioClip aud_Congrats;

	// Token: 0x040002A1 RID: 673
	public AudioClip aud_ReadyGo;

	// Token: 0x040002A2 RID: 674
	public AudioClip aud_Sad;

	// Token: 0x040002A3 RID: 675
	public AudioSource audioDevice;

	// Token: 0x040002A4 RID: 676
	public GameControllerScript gc;

	// Token: 0x040002A5 RID: 677
	public bool goingToSurface;
}