using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000031 RID: 49
public class JumpRopeScript : MonoBehaviour
{
	// Token: 0x060000F2 RID: 242 RVA: 0x00009354 File Offset: 0x00007554
	private void OnEnable()
	{
		this.jumpDelay = 1f;
		this.ropeHit = true;
		this.jumpStarted = false;
		this.jumps = 0;
		this.jumpCount.text = "0/15";
		this.playtime.audioDevice.PlayOneShot(this.playtime.aud_ReadyGo);
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x000093AC File Offset: 0x000075AC
	private void Update()
	{
		if (this.jumpDelay > 0f)
		{
			this.jumpDelay -= Time.deltaTime;
		}
		else if (!this.jumpStarted)
		{
			this.jumpStarted = true;
			this.ropePosition = 1f;
			this.rope.SetTrigger("ActivateJumpRope");
			this.ropeHit = false;
		}
		if (this.ropePosition > 0f)
		{
			this.ropePosition -= Time.deltaTime;
			return;
		}
		if (!this.ropeHit)
		{
			this.RopeHit();
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00009439 File Offset: 0x00007639
	private void RopeHit()
	{
		this.ropeHit = true;
		if (this.cs.jumpHeight <= 0.2f)
		{
			this.Fail();
		}
		else
		{
			this.Success();
		}
		this.jumpStarted = false;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000946C File Offset: 0x0000766C
	private void Success()
	{
		this.playtime.audioDevice.Stop();
		this.playtime.audioDevice.PlayOneShot(this.playtime.aud_Numbers[this.jumps]);
		this.jumps++;
		this.jumpCount.text = this.jumps + "/15";
		this.jumpDelay = 0.02f;
		if (this.jumps >= 15)
		{
			this.playtime.audioDevice.Stop();
			this.playtime.audioDevice.PlayOneShot(this.playtime.aud_Congrats);
			this.ps.DeactivateJumpRope();
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00009524 File Offset: 0x00007724
	private void Fail()
	{
		this.jumps = 0;
		this.jumpCount.text = this.jumps + "/15";
		this.jumpDelay = 2f;
		this.playtime.audioDevice.PlayOneShot(this.playtime.aud_Oops);
	}

	// Token: 0x040001EA RID: 490
	public Text jumpCount;

	// Token: 0x040001EB RID: 491
	public Animator rope;

	// Token: 0x040001EC RID: 492
	public CameraScript cs;

	// Token: 0x040001ED RID: 493
	public PlayerScript ps;

	// Token: 0x040001EE RID: 494
	public PlaytimeScript playtime;

	// Token: 0x040001EF RID: 495
	public int jumps;

	// Token: 0x040001F0 RID: 496
	public float jumpDelay;

	// Token: 0x040001F1 RID: 497
	public float ropePosition;

	// Token: 0x040001F2 RID: 498
	public bool ropeHit;

	// Token: 0x040001F3 RID: 499
	public bool jumpStarted;
}
