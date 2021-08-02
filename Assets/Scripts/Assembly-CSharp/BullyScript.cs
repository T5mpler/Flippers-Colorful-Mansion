using UnityEngine;

// Token: 0x02000011 RID: 17
public class BullyScript : MonoBehaviour
{
	// Token: 0x06000046 RID: 70 RVA: 0x0000370D File Offset: 0x0000190D
	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
		this.waitTime = Random.Range(60f, 120f);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003730 File Offset: 0x00001930
	private void Update()
	{
		if (this.waitTime > 0f)
		{
			this.waitTime -= Time.deltaTime;
		}
		else if (!this.active)
		{
			this.Activate();
		}
		if (this.active)
		{
			this.activeTime += Time.deltaTime;
			if (this.activeTime >= 180f & (base.transform.position - this.player.position).magnitude >= 120f)
			{
				this.Reset();
			}
		}
		if (this.guilt > 0f)
		{
			this.guilt -= Time.deltaTime;
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000037EC File Offset: 0x000019EC
	private void FixedUpdate()
	{
		Vector3 direction = this.player.position - base.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position + Vector3.up * 4f, direction, out raycastHit, float.PositiveInfinity) & raycastHit.transform.tag == "Player" & this.bullyRenderer.isVisible & (base.transform.position - this.player.position).magnitude <= 30f & this.active)
		{
			if (!this.spoken)
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Taunts[num]);
				this.spoken = true;
			}
			this.guilt = 10f;
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000038E4 File Offset: 0x00001AE4
	private void Activate()
	{
		this.wanderer.GetNewTargetHallway();
		base.transform.position = this.wanderTarget.position + new Vector3(0f, 5f, 0f);
		while ((base.transform.position - this.player.position).magnitude < 20f)
		{
			this.wanderer.GetNewTargetHallway();
			base.transform.position = this.wanderTarget.position + new Vector3(0f, 5f, 0f);
		}
		this.active = true;
	}

	// Token: 0x0600004A RID: 74 RVA: 0x00003998 File Offset: 0x00001B98
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag == "Player")
		{
			if (this.gc.item[0] == 0 & this.gc.item[1] == 0 & this.gc.item[2] == 0)
			{
				this.audioDevice.PlayOneShot(this.aud_Denied);
			}
			else
			{
				int num = Mathf.RoundToInt(Random.Range(0f, 2f));
				while (this.gc.item[num] == 0)
				{
					num = Mathf.RoundToInt(Random.Range(0f, 2f));
				}
				this.gc.LoseItem(num);
				int num2 = Mathf.RoundToInt(Random.Range(0f, 1f));
				this.audioDevice.PlayOneShot(this.aud_Thanks[num2]);
				this.Reset();
			}
		}
		if (other.transform.name == "Principal of the Thing" & this.guilt > 0f)
		{
			this.Reset();
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003AA8 File Offset: 0x00001CA8
	private void Reset()
	{
		base.transform.position = base.transform.position - new Vector3(0f, -50, 0f);
		this.waitTime = Random.Range(60f, 120f);
		this.active = false;
		this.activeTime = 0f;
		this.spoken = false;
	}

	// Token: 0x0400005F RID: 95
	public Transform player;

	// Token: 0x04000060 RID: 96
	public GameControllerScript gc;

	// Token: 0x04000061 RID: 97
	public Renderer bullyRenderer;

	// Token: 0x04000062 RID: 98
	public Transform wanderTarget;

	// Token: 0x04000063 RID: 99
	public AILocationSelectorScript wanderer;

	// Token: 0x04000064 RID: 100
	public float waitTime;

	// Token: 0x04000065 RID: 101
	public float activeTime;

	// Token: 0x04000066 RID: 102
	public float guilt;

	// Token: 0x04000067 RID: 103
	public bool active;

	// Token: 0x04000068 RID: 104
	public bool spoken;

	// Token: 0x04000069 RID: 105
	private AudioSource audioDevice;

	// Token: 0x0400006A RID: 106
	public AudioClip[] aud_Taunts = new AudioClip[2];

	// Token: 0x0400006B RID: 107
	public AudioClip[] aud_Thanks = new AudioClip[2];

	// Token: 0x0400006C RID: 108
	public AudioClip aud_Denied;
}
