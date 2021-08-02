using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000049 RID: 73
public class SwingingDoorScript : MonoBehaviour
{
	// Token: 0x06000170 RID: 368 RVA: 0x0000ECD8 File Offset: 0x0000CED8
	private void Start()
	{
		this.myAudio = base.GetComponent<AudioSource>();
		this.bDoorLocked = true;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
	private void Update()
	{
		if (!this.requirementMet & this.gc.notebooks >= 2 && bDoorLocked)
		{
			this.requirementMet = true;
			this.UnlockDoor();
		}
		if (this.openTime > 0f)
		{
			this.openTime -= 1f * Time.deltaTime;
		}
		if (this.lockTime > 0f)
		{
			this.lockTime -= Time.deltaTime;
		}
		else if (this.bDoorLocked & this.requirementMet)
		{
			this.UnlockDoor();
		}
		if (this.openTime <= 0f & this.bDoorOpen & !this.bDoorLocked)
		{
			this.bDoorOpen = false;
			this.inside.sharedMaterial = this.closed;
			this.outside.sharedMaterial = this.closed;
		}
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000EDCE File Offset: 0x0000CFCE
	private void OnTriggerStay(Collider other)
	{
		if (!this.bDoorLocked)
		{
			this.bDoorOpen = true;
			this.inside.sharedMaterial = this.open;
			this.outside.sharedMaterial = this.open;
			this.openTime = 2f;
		}
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000EE0C File Offset: 0x0000D00C
	private void OnTriggerEnter(Collider other)
	{
		if (this.gc.notebooks < 2 & other.tag == "Player")
		{
			this.myAudio.PlayOneShot(this.baldiDoor, 1f);
			return;
		}
		if (!this.bDoorLocked)
		{
			this.myAudio.PlayOneShot(this.doorOpen, 1f);
			if (other.tag == "Player" && this.baldi.isActiveAndEnabled)
			{
				this.baldi.Hear(base.transform.position, 1f);
			}
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000EEAC File Offset: 0x0000D0AC
	public void LockDoor(float time)
	{
		this.barrier.enabled = true;
		this.obstacle.SetActive(true);
		this.bDoorLocked = true;
		this.lockTime = time;
		this.inside.sharedMaterial = this.locked;
		this.outside.sharedMaterial = this.locked;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000EF04 File Offset: 0x0000D104
	public void UnlockDoor()
	{
		this.barrier.enabled = false;
		this.obstacle.SetActive(false);
		this.bDoorLocked = false;
		this.inside.sharedMaterial = this.closed;
		this.outside.sharedMaterial = this.closed;
	}

	// Token: 0x040002F6 RID: 758
	public GameControllerScript gc;

	// Token: 0x040002F7 RID: 759
	public BaldiScript baldi;

	// Token: 0x040002F8 RID: 760
	public MeshCollider barrier;

	// Token: 0x040002F9 RID: 761
	public GameObject obstacle;

	// Token: 0x040002FA RID: 762
	public MeshCollider trigger;

	// Token: 0x040002FB RID: 763
	public MeshRenderer inside;

	// Token: 0x040002FC RID: 764
	public MeshRenderer outside;

	// Token: 0x040002FD RID: 765
	public Material closed;

	// Token: 0x040002FE RID: 766
	public Material open;

	// Token: 0x040002FF RID: 767
	public Material locked;

	// Token: 0x04000300 RID: 768
	public AudioClip doorOpen;

	// Token: 0x04000301 RID: 769
	public AudioClip baldiDoor;

	// Token: 0x04000302 RID: 770
	private float openTime;

	// Token: 0x04000303 RID: 771
	private float lockTime;

	// Token: 0x04000304 RID: 772
	private bool bDoorOpen;

	// Token: 0x04000305 RID: 773
	private bool bDoorLocked;

	// Token: 0x04000306 RID: 774
	private bool requirementMet;

	// Token: 0x04000307 RID: 775
	private AudioSource myAudio;
}
