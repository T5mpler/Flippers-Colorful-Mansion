using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class TapePlayerScript : MonoBehaviour
{
	// Token: 0x06000177 RID: 375 RVA: 0x0000EF52 File Offset: 0x0000D152
	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000EF60 File Offset: 0x0000D160
	private void Update()
	{
		if (this.audioDevice.isPlaying & Time.timeScale == 0f)
		{
			this.audioDevice.Pause();
			return;
		}
		if (Time.timeScale > 0f & this.baldi.antiHearingTime > 0f)
		{
			this.audioDevice.UnPause();
		}
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000EFBF File Offset: 0x0000D1BF
	public void Play()
	{
		this.sprite.sprite = this.closedSprite;
		this.audioDevice.Play();
		this.baldi.ActivateAntiHearing(30f);
	}

	// Token: 0x04000308 RID: 776
	public Sprite closedSprite;

	// Token: 0x04000309 RID: 777
	public SpriteRenderer sprite;

	// Token: 0x0400030A RID: 778
	public BaldiScript baldi;

	// Token: 0x0400030B RID: 779
	private AudioSource audioDevice;
}
