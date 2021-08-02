using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

// Token: 0x0200000B RID: 11
public class BaldiScript : MonoBehaviour
{
	// Token: 0x06000026 RID: 38 RVA: 0x00002D6F File Offset: 0x00000F6F
	private void Start()
	{
		this.baldiAudio = base.GetComponent<AudioSource>();
		this.agent = base.GetComponent<NavMeshAgent>();
		this.timeToMove = this.baseTime;
		this.Wander();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002D9C File Offset: 0x00000F9C
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
		if (this.surfaceCooldown > 0f)
		{
			this.surfaceCooldown -= Time.deltaTime;
		}
		if (this.undergroundCooldown > 0f)
		{
			this.undergroundCooldown -= Time.deltaTime;
		}
		if (!this.ps.isUnderground && isUnderground || ps.isUnderground && !isUnderground)
		{
			this.agent.SetDestination(new Vector3(undergroundEntrance.transform.position.x, transform.position.y, undergroundEntrance.transform.position.z));
		}
		if (this.timeToMove > 0f)
		{
			this.timeToMove -= 1f * Time.deltaTime;
		}
		else
		{
			this.Move();
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.baldiTempAnger > 0f)
		{
			this.baldiTempAnger -= 0.02f * Time.deltaTime;
		}
		else
		{
			this.baldiTempAnger = 0f;
		}
		if (this.antiHearingTime > 0f)
		{
			this.antiHearingTime -= Time.deltaTime;
		}
		else
		{
			this.antiHearing = false;
		}
		if (this.endless)
		{
			if (this.timeToAnger > 0f)
			{
				this.timeToAnger -= 1f * Time.deltaTime;
				return;
			}
			this.timeToAnger = this.angerFrequency;
			this.GetAngry(this.angerRate);
			this.angerRate += this.angerRateRate;
		}
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00003070 File Offset: 0x00001270
	private void FixedUpdate()
	{
		if (this.moveFrames > 0f)
		{
			this.moveFrames -= 1f;
			this.agent.speed = speed * am.TotalMultipler;
		}
		else
		{
			this.agent.speed = 0f;
		}
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up * 2f, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.CompareTag("Player") && SceneManager.GetActiveScene().name != "Race" && direction.magnitude >= 1f)
		{
			this.db = true;
			this.TargetPlayer();
			return;
		}
		this.db = false;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00003137 File Offset: 0x00001337
	private void Wander()
	{
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		this.coolDown = 1f;
		this.currentPriority = 0f;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003171 File Offset: 0x00001371
	public void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
		this.currentPriority = 0f;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.name == "UndergrondEntrance")
		{
			if (!this.isUnderground & this.undergroundCooldown <= 0f & !this.goingToSurface)
		        {
				this.isUnderground = true;
				base.StartCoroutine(this.GoUnderGround());
			}
			else if (this.isUnderground & this.surfaceCooldown <= 0f & !this.goingToUnderground)
		    {
				this.isUnderground = false;
				base.StartCoroutine(this.GoBackToSurface());
			}
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x000031A0 File Offset: 0x000013A0
	private void Move()
	{
		if (!this.gc.noKnife)
		{
			if (base.transform.position == this.previous & this.coolDown < 0f)
			{
				this.Wander();
			}
			this.moveFrames = 10f;
			if (this.gc.mode != "farmMode")
			{
				float finalTempAnger = baldiTempAnger;
				if (gc.notebooks == 1)
				{
					finalTempAnger = -finalTempAnger;
				}
				this.timeToMove = this.baldiWait - finalTempAnger;
			}
			else if (flipperHappy)
			{
				timeToMove = 0.1f;
			}
			else
			{
				this.timeToMove = 0.6f;
			}
			this.previous = base.transform.position;
			string slapState;
			slapState = this.gc.hardSlapEvent == false ? "slap" : "brokenSlap";
			if (!this.gc.hardSlapEvent)
			{
				this.baldiAudio.PlayOneShot(this.slap);
			}
			this.baldiAnimator.SetTrigger(slapState);
		}
	}

	// Token: 0x0600002C RID: 44 RVA: 0x0000326C File Offset: 0x0000146C
	public void GetAngry(float value)
	{
		this.baldiAnger += value;
		if (this.baldiAnger < 0.5f)
		{
			this.baldiAnger = 0.5f;
		}
		this.baldiWait = -2.6f * this.baldiAnger / (this.baldiAnger + 2f / this.baldiSpeedScale) + 3f;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000032CB File Offset: 0x000014CB
	public void GetTempAngry(float value)
	{
		this.baldiTempAnger += value;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000032DB File Offset: 0x000014DB
	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!this.antiHearing && priority >= this.currentPriority)
		{
			indicatorAnimator.SetTrigger("heard");
			this.agent.SetDestination(soundLocation);
			this.currentPriority = priority;
		}
		else
		{
			indicatorAnimator.SetTrigger("notHeard");
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003302 File Offset: 0x00001502
	public void ActivateAntiHearing(float t)
	{
		this.Wander();
		this.antiHearing = true;
		this.antiHearingTime = t;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003318 File Offset: 0x00001518
	public void RemoveKnife(float time)
	{
		this.gc.noKnife = true;
		this.gc.noKnifeTime = time;
		this.gc.baldi.SetActive(false);
		this.clonedNoKnifeBaldi = Object.Instantiate<GameObject>(this.noKnifeBaldi, this.gc.baldi.transform.position, this.gc.cameraTransform.rotation);
		this.clonedNoKnifeBaldi.SetActive(true);
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003390 File Offset: 0x00001590
	public IEnumerator GoUnderGround()
	{
		this.goingToUnderground = true;
		base.transform.position = new Vector3(-75f, base.transform.position.y, 95f);
		this.isUnderground = true;
		base.GetComponent<NavMeshAgent>().enabled = false;
		base.GetComponent<BaldiScript>().enabled = false;
		this.goingUnderground = false;
		while (base.transform.position.y > -9.36f)
		{
			base.transform.position -= new Vector3(0f, Time.deltaTime * 5f, 0f);
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, -9.36f, base.transform.position.z);
		this.surfaceCooldown = 3f;
		this.goingToUnderground = false;
		base.GetComponent<BaldiScript>().enabled = true;
		base.GetComponent<NavMeshAgent>().enabled = true;
		yield break;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x0000339F File Offset: 0x0000159F
	public IEnumerator GoBackToSurface()
	{
		this.goingToSurface = true;
		base.transform.position = new Vector3(-75f, base.transform.position.y, 95f);
		this.isUnderground = false;
		base.GetComponent<NavMeshAgent>().enabled = false;
		base.GetComponent<BaldiScript>().enabled = false;
		while (base.transform.position.y < 1.64f)
		{
			base.transform.position += new Vector3(0f, Time.deltaTime * 5f, 0f);
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, 1.64f, base.transform.position.z);
		base.GetComponent<BaldiScript>().enabled = true;
		base.GetComponent<NavMeshAgent>().enabled = true;
		this.undergroundCooldown = 3f;
		this.goingToSurface = false;
		yield break;
	}

	// Token: 0x04000024 RID: 36
	public bool db;

	// Token: 0x04000025 RID: 37
	public float baseTime;

	// Token: 0x04000026 RID: 38
	public float speed;

	// Token: 0x04000027 RID: 39
	public float timeToMove;

	// Token: 0x04000028 RID: 40
	public float baldiAnger;

	// Token: 0x04000029 RID: 41
	public float baldiTempAnger;

	// Token: 0x0400002A RID: 42
	public float baldiWait;

	// Token: 0x0400002B RID: 43
	public float baldiSpeedScale;

	// Token: 0x0400002C RID: 44
	private float moveFrames;

	// Token: 0x0400002D RID: 45
	private float currentPriority;

	// Token: 0x0400002E RID: 46
	public bool antiHearing;

	// Token: 0x0400002F RID: 47
	public float antiHearingTime;

	// Token: 0x04000030 RID: 48
	public float angerRate;

	// Token: 0x04000031 RID: 49
	public float angerRateRate;

	// Token: 0x04000032 RID: 50
	public float angerFrequency;

	// Token: 0x04000033 RID: 51
	public float timeToAnger;

	// Token: 0x04000034 RID: 52
	public bool endless;

	// Token: 0x04000035 RID: 53
	public Transform player;

	// Token: 0x04000036 RID: 54
	public Transform wanderTarget;

	// Token: 0x04000037 RID: 55
	public AILocationSelectorScript wanderer;

	// Token: 0x04000038 RID: 56
	private AudioSource baldiAudio;

	// Token: 0x04000039 RID: 57
	public AudioClip slap;

	// Token: 0x0400003A RID: 58
	public AudioClip[] speech = new AudioClip[3];

	// Token: 0x0400003B RID: 59
	public Animator baldiAnimator;

	// Token: 0x0400003C RID: 60
	public float coolDown;

	// Token: 0x0400003D RID: 61
	private Vector3 previous;

	// Token: 0x0400003E RID: 62
	public NavMeshAgent agent;

	// Token: 0x0400003F RID: 63
	public TapePlayerScript tp;

	// Token: 0x04000040 RID: 64
	public AudioClip aud_NoKnife;

	// Token: 0x04000041 RID: 65
	public GameControllerScript gc;

	// Token: 0x04000042 RID: 66
	public Sprite noKnifeSprite;

	// Token: 0x04000043 RID: 67
	public SpriteRenderer baldiSprite;

	// Token: 0x04000044 RID: 68
	public GameObject noKnifeBaldi;

	// Token: 0x04000045 RID: 69
	public GameObject clonedNoKnifeBaldi;

	// Token: 0x04000046 RID: 70
	public GameObject undergroundEntrance;

	// Token: 0x04000047 RID: 71
	public bool goingUnderground;

	// Token: 0x04000048 RID: 72
	public bool isUnderground;

	// Token: 0x04000049 RID: 73
	public PlayerScript ps;

	// Token: 0x0400004A RID: 74
	public float undergroundCooldown;

	// Token: 0x0400004B RID: 75
	public float surfaceCooldown;

	// Token: 0x0400004C RID: 76
	public bool goingToUnderground;

	// Token: 0x0400004D RID: 77
	public bool goingToSurface;

	// Token: 0x0400004E RID: 78
	public bool flipperHappy;

	// Token: 0x0400004F RID: 79
	public bool upSideDown;

	public Animator indicatorAnimator;
}
