using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002E RID: 46
public class ImageAnimator : MonoBehaviour
{
	// Token: 0x14000001 RID: 1
	// (add) Token: 0x060000D7 RID: 215 RVA: 0x0000860C File Offset: 0x0000680C
	// (remove) Token: 0x060000D8 RID: 216 RVA: 0x00008644 File Offset: 0x00006844
	public event EventHandler OnAnimationLoopedFirstTime;

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x060000D9 RID: 217 RVA: 0x0000867C File Offset: 0x0000687C
	// (remove) Token: 0x060000DA RID: 218 RVA: 0x000086B4 File Offset: 0x000068B4
	public event EventHandler OnAnimationLooped;

	// Token: 0x060000DB RID: 219 RVA: 0x000086E9 File Offset: 0x000068E9
	private void Awake()
	{
		this.image = base.gameObject.GetComponent<Image>();
		if (this.frameArray != null)
		{
			this.PlayAnimation(this.frameArray, this.framerate, true);
			return;
		}
		this.isPlaying = false;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00008720 File Offset: 0x00006920
	private void Update()
	{
		if (!this.isPlaying)
		{
			return;
		}
		this.timer += Time.deltaTime;
		if (this.timer >= this.framerate)
		{
			this.timer -= this.framerate;
			this.currentFrame = (this.currentFrame + 1) % this.frameArray.Length;
			if (!this.loop && this.currentFrame == 0)
			{
				this.StopPlaying();
			}
			else
			{
				this.image.sprite = this.frameArray[this.currentFrame];
			}
			if (this.currentFrame == 0)
			{
				this.loopCounter++;
				if (this.loopCounter == 1 && this.OnAnimationLoopedFirstTime != null)
				{
					this.OnAnimationLoopedFirstTime(this, EventArgs.Empty);
				}
				if (this.OnAnimationLooped != null)
				{
					this.OnAnimationLooped(this, EventArgs.Empty);
				}
			}
		}
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00008804 File Offset: 0x00006A04
	public void StopPlaying()
	{
		this.isPlaying = false;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00008810 File Offset: 0x00006A10
	public void PlayAnimation(Sprite[] frameArray, float framerate, bool loop = true)
	{
		this.frameArray = frameArray;
		this.framerate = framerate;
		this.isPlaying = true;
		this.currentFrame = 0;
		this.timer = 0f;
		this.loopCounter = 0;
		this.image.sprite = frameArray[this.currentFrame];
	}

	// Token: 0x040001AE RID: 430
	[SerializeField]
	private Sprite[] frameArray;

	// Token: 0x040001AF RID: 431
	[SerializeField]
	private float framerate = 0.1f;

	// Token: 0x040001B0 RID: 432
	[SerializeField]
	private bool loop;

	// Token: 0x040001B1 RID: 433
	private int currentFrame;

	// Token: 0x040001B2 RID: 434
	private float timer;

	// Token: 0x040001B3 RID: 435
	private Image image;

	// Token: 0x040001B4 RID: 436
	private bool isPlaying;

	// Token: 0x040001B5 RID: 437
	private int loopCounter;
}
