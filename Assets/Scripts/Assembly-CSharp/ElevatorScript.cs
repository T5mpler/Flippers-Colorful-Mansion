using System;
using UnityEngine;

// Token: 0x0200001D RID: 29
public class ElevatorScript : MonoBehaviour
{
	// Token: 0x0600007B RID: 123 RVA: 0x00004964 File Offset: 0x00002B64
	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
		this.elevator.SetActive(true);
		this.swingDoorWall.SetActive(false);
		this.elevatorAnimator.SetTrigger("elevatorOpen");
		this.audioDevice.PlayOneShot(this.aud_elevatorOpen);
		this.elevatorCollider.isTrigger = true;
	}

	// Token: 0x0600007C RID: 124 RVA: 0x000049C4 File Offset: 0x00002BC4
	private void Update()
	{
		if (this.timer > 0f)
		{
			this.timer -= Time.deltaTime;
		}
		else if (!this.isDone & this.elevating)
		{
			this.elevating = false;
			this.isDone = true;
			this.elevator.SetActive(false);
			this.swingDoorWall.SetActive(true);
		}
		if (Vector3.Distance(this.player.position, this.elevatorSensor.transform.position) <= 20f)
		{
			this.CloseElevator();
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x00004A58 File Offset: 0x00002C58
	public void CloseElevator()
	{
		if (!this.elevatorClosed)
		{
			this.elevatorClosed = true;
			this.elevatorAnimator.SetTrigger("elevatorClose");
			this.audioDevice.PlayOneShot(this.aud_elevatorClose);
			this.elevatorCollider.isTrigger = false;
			this.elevating = true;
			this.timer = 3f;
		}
	}

	// Token: 0x040000B1 RID: 177
	public Animator elevatorAnimator;

	// Token: 0x040000B2 RID: 178
	public BoxCollider elevatorCollider;

	// Token: 0x040000B3 RID: 179
	public GameObject elevator;

	// Token: 0x040000B4 RID: 180
	public GameObject elevatorSensor;

	// Token: 0x040000B5 RID: 181
	public Transform player;

	// Token: 0x040000B6 RID: 182
	public bool elevatorClosed;

	// Token: 0x040000B7 RID: 183
	public float timer;

	// Token: 0x040000B8 RID: 184
	public bool isDone;

	// Token: 0x040000B9 RID: 185
	public GameObject swingDoorWall;

	// Token: 0x040000BA RID: 186
	public bool elevating;

	// Token: 0x040000BB RID: 187
	public AudioClip aud_elevatorClose;

	// Token: 0x040000BC RID: 188
	public AudioClip aud_elevatorOpen;

	// Token: 0x040000BD RID: 189
	public AudioSource audioDevice;
}
