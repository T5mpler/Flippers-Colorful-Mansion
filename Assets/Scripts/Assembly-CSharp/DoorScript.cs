using UnityEngine;

// Token: 0x0200001C RID: 28
public class DoorScript : MonoBehaviour
{
	// Token: 0x06000073 RID: 115 RVA: 0x000046E3 File Offset: 0x000028E3
	private void Start()
	{
		this.myAudio = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000074 RID: 116 RVA: 0x000046F4 File Offset: 0x000028F4
	private void Update()
	{
		if (this.lockTime > 0f)
		{
			this.lockTime -= 1f * Time.deltaTime;
		}
		else if (this.bDoorLocked)
		{
			this.UnlockDoor();
		}
		if (this.openTime > 0f)
		{
			this.openTime -= 1f * Time.deltaTime;
		}
		if (this.openTime <= 0f & this.bDoorOpen)
		{
			this.barrier.enabled = true;
			this.invisibleBarrier.enabled = true;
			this.bDoorOpen = false;
			this.inside.sharedMaterial = this.closed;
			this.outside.sharedMaterial = this.closed;
			if (this.silentOpens <= 0)
			{
				this.myAudio.PlayOneShot(this.doorClose, 1f);
			}
		}
		RaycastHit raycastHit;
		if ((Input.GetMouseButtonDown(0) & Time.timeScale != 0f) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit) && (raycastHit.collider == this.trigger & Vector3.Distance(this.player.position, base.transform.position) < this.openingDistance & !this.bDoorLocked))
		{
			if (this.baldi.isActiveAndEnabled & this.silentOpens <= 0)
			{
				this.baldi.Hear(base.transform.position, 1f);
			}
			this.OpenDoor();
			if (this.silentOpens > 0)
			{
				this.silentOpens--;
			}
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000048A0 File Offset: 0x00002AA0
	public void OpenDoor()
	{
		if (this.silentOpens <= 0 && !this.bDoorOpen)
		{
			this.myAudio.PlayOneShot(this.doorOpen, 1f);
		}
		this.barrier.enabled = false;
		this.invisibleBarrier.enabled = false;
		this.bDoorOpen = true;
		this.inside.sharedMaterial = this.open;
		this.outside.sharedMaterial = this.open;
		this.openTime = 3f;
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00004920 File Offset: 0x00002B20
	private void OnTriggerStay(Collider other)
	{
		if (!this.bDoorLocked & other.CompareTag("NPC"))
		{
			this.OpenDoor();
		}
	}
	// Token: 0x06000077 RID: 119 RVA: 0x0000493F File Offset: 0x00002B3F
	public void LockDoor(float time)
	{
		this.bDoorLocked = true;
		this.lockTime = time;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000494F File Offset: 0x00002B4F
	public void UnlockDoor()
	{
		this.bDoorLocked = false;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00004958 File Offset: 0x00002B58
	public void SilenceDoor()
	{
		this.silentOpens = 4;
	}

	// Token: 0x0400009F RID: 159
	public float openingDistance;

	// Token: 0x040000A0 RID: 160
	public Transform player;

	// Token: 0x040000A1 RID: 161
	public BaldiScript baldi;

	// Token: 0x040000A2 RID: 162
	public MeshCollider barrier;

	// Token: 0x040000A3 RID: 163
	public MeshCollider trigger;

	// Token: 0x040000A4 RID: 164
	public MeshCollider invisibleBarrier;

	// Token: 0x040000A5 RID: 165
	public MeshRenderer inside;

	// Token: 0x040000A6 RID: 166
	public MeshRenderer outside;

	// Token: 0x040000A7 RID: 167
	public AudioClip doorOpen;

	// Token: 0x040000A8 RID: 168
	public AudioClip doorClose;

	// Token: 0x040000A9 RID: 169
	public Material closed;

	// Token: 0x040000AA RID: 170
	public Material open;

	// Token: 0x040000AB RID: 171
	public bool bDoorOpen;

	// Token: 0x040000AC RID: 172
	private bool bDoorLocked;

	// Token: 0x040000AD RID: 173
	public int silentOpens;

	// Token: 0x040000AE RID: 174
	private float openTime;

	// Token: 0x040000AF RID: 175
	public float lockTime;

	// Token: 0x040000B0 RID: 176
	private AudioSource myAudio;
}
