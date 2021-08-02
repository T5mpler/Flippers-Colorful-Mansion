using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000026 RID: 38
public class FirstPrizeScript : MonoBehaviour
{
	// Token: 0x06000096 RID: 150 RVA: 0x0000512C File Offset: 0x0000332C
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>();
		this.coolDown = 1f;
		this.Wander();
	}

	// Token: 0x06000097 RID: 151 RVA: 0x0000514C File Offset: 0x0000334C
	public ActivityModifierScript am
	{
		get
		{
			return GetComponent<ActivityModifierScript>();
		}
	}
	private void Update()
	{
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
		if (this.autoBrakeCool > 0f)
		{
			this.autoBrakeCool -= 1f * Time.deltaTime;
		}
		else
		{
			this.agent.autoBraking = true;
		}
		this.angleDiff = Mathf.DeltaAngle(base.transform.eulerAngles.y, Mathf.Atan2(this.agent.steeringTarget.x - base.transform.position.x, this.agent.steeringTarget.z - base.transform.position.z) * Mathf.Rad2Deg);
		if (this.crazyTime <= 0f)
		{
			if (Mathf.Abs(this.angleDiff) < 5f)
			{
				base.transform.LookAt(new Vector3(this.agent.steeringTarget.x, base.transform.position.y, this.agent.steeringTarget.z));
				if (this.gc.stunnedCharacter.transform.name != base.transform.name)
				{
					this.agent.speed = currentSpeed * am.TotalMultipler;
				}
			}
			else
			{
				base.transform.Rotate(new Vector3(0f, this.turnSpeed * Mathf.Sign(this.angleDiff) * Time.deltaTime, 0f));
				if (this.gc.stunnedCharacter.transform.name != base.transform.name)
				{
					this.agent.speed = 0f;
				}
			}
		}
		else
		{
			this.agent.speed = 0f;
			base.transform.Rotate(new Vector3(0f, 180f * Time.deltaTime, 0f));
			this.crazyTime -= Time.deltaTime;
		}
		this.motorAudio.pitch = (this.agent.velocity.magnitude + 1f) * Time.timeScale;
	}

	// Token: 0x06000098 RID: 152 RVA: 0x0000539C File Offset: 0x0000359C
	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player")
		{
			if (!this.playerSeen && !this.audioDevice.isPlaying)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Found[num]);
			}
			this.playerSeen = true;
			this.TargetPlayer();
			if (this.gc.stunnedCharacter.transform.name != base.transform.name)
			{
				this.currentSpeed = this.runSpeed;
				return;
			}
		}
		else
		{
			if (this.gc.stunnedCharacter.transform.name != base.transform.name)
			{
				this.currentSpeed = this.normSpeed;
			}
			if (this.playerSeen & this.coolDown <= 0f)
			{
				if (!this.audioDevice.isPlaying)
				{
					int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
					this.audioDevice.PlayOneShot(this.aud_Lost[num2]);
				}
				this.playerSeen = false;
				this.Wander();
				return;
			}
			if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f & (base.transform.position - this.agent.destination).magnitude < 5f)
			{
				this.Wander();
			}
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00005570 File Offset: 0x00003770
	private void Wander()
	{
		this.wanderer.GetNewTargetHallway();
		this.agent.SetDestination(this.wanderTarget.position);
		this.hugAnnounced = false;
		int num = Mathf.RoundToInt(Random.Range(0f, 9f));
		if (!this.audioDevice.isPlaying & num == 0 & this.coolDown <= 0f)
		{
			int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
			this.audioDevice.PlayOneShot(this.aud_Random[num2]);
		}
		this.coolDown = 1f;
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00005614 File Offset: 0x00003814
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 0.5f;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00005638 File Offset: 0x00003838
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			if (!this.audioDevice.isPlaying & !this.hugAnnounced)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Hug[num]);
				this.hugAnnounced = true;
			}
			this.agent.autoBraking = false;
		}
	}

	// Token: 0x0600009C RID: 156 RVA: 0x000056AC File Offset: 0x000038AC
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			this.autoBrakeCool = 1f;
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x000056CB File Offset: 0x000038CB
	public void GoCrazy()
	{
		this.crazyTime = 15f;
	}

	// Token: 0x040000DA RID: 218
	public float debug;

	// Token: 0x040000DB RID: 219
	public float turnSpeed;

	// Token: 0x040000DC RID: 220
	public float str;

	// Token: 0x040000DD RID: 221
	public float angleDiff;

	// Token: 0x040000DE RID: 222
	public float normSpeed;

	// Token: 0x040000DF RID: 223
	public float runSpeed;

	// Token: 0x040000E0 RID: 224
	public float currentSpeed;

	// Token: 0x040000E1 RID: 225
	public float acceleration;

	// Token: 0x040000E2 RID: 226
	public float speed;

	// Token: 0x040000E3 RID: 227
	public float autoBrakeCool;

	// Token: 0x040000E4 RID: 228
	public float crazyTime;

	// Token: 0x040000E5 RID: 229
	public Quaternion targetRotation;

	// Token: 0x040000E6 RID: 230
	public float coolDown;

	// Token: 0x040000E7 RID: 231
	public bool playerSeen;

	// Token: 0x040000E8 RID: 232
	public bool hugAnnounced;

	// Token: 0x040000E9 RID: 233
	public AILocationSelectorScript wanderer;

	// Token: 0x040000EA RID: 234
	public Transform player;

	// Token: 0x040000EB RID: 235
	public Transform wanderTarget;

	// Token: 0x040000EC RID: 236
	public AudioClip[] aud_Found = new AudioClip[2];

	// Token: 0x040000ED RID: 237
	public AudioClip[] aud_Lost = new AudioClip[2];

	// Token: 0x040000EE RID: 238
	public AudioClip[] aud_Hug = new AudioClip[2];

	// Token: 0x040000EF RID: 239
	public AudioClip[] aud_Random = new AudioClip[2];

	// Token: 0x040000F0 RID: 240
	public AudioSource audioDevice;

	// Token: 0x040000F1 RID: 241
	public AudioSource motorAudio;

	// Token: 0x040000F2 RID: 242
	private NavMeshAgent agent;

	// Token: 0x040000F3 RID: 243
	public GameControllerScript gc;
}
