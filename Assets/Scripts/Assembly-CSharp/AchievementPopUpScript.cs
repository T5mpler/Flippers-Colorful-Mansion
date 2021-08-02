using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
public class AchievementPopUpScript : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	public void Start()
	{
		this.achievementInQueue = 0;
	}

	// Token: 0x06000002 RID: 2 RVA: 0x0000205C File Offset: 0x0000025C
	public void Update()
	{
		if (this.achievementInQueue > 0 & !this.inAchievement & !this.toAchievement)
		{
			this.toAchievement = true;
			this.delayToAchievement = 1f;
		}
		if (this.delayToAchievement > 0f)
		{
			this.delayToAchievement -= Time.deltaTime;
			return;
		}
		if (this.achievementInQueue > 0 & !this.inAchievement)
		{
			base.StartCoroutine(this.PopUpAchievemnts());
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x000020DC File Offset: 0x000002DC
	public IEnumerator PopUpAchievemnts()
	{
		this.achievementImage.enabled = true;
		this.inAchievement = true;
		this.audioDevice.PlayOneShot(this.achievement);
		this.achievementImage.sprite = this.achievementQueue[0];
		this.animator.SetTrigger("Down");
		float timer = 3f;
		while (timer > 0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		this.animator.SetTrigger("Up");
		this.UnQueueAchievement();
		base.StopAllCoroutines();
		yield break;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000020EB File Offset: 0x000002EB
	public void QueueAchievement(Sprite achievement)
	{
		this.achievementQueue[this.achievementInQueue] = achievement;
		this.achievementInQueue++;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000210C File Offset: 0x0000030C
	private void UnQueueAchievement()
	{
		for (int i = 1; i < this.achievementQueue.Length; i++)
		{
			this.achievementQueue[i - 1] = this.achievementQueue[i];
		}
		this.achievementInQueue--;
		this.inAchievement = false;
		this.toAchievement = false;
	}

	// Token: 0x04000001 RID: 1
	public Image achievementImage;

	// Token: 0x04000002 RID: 2
	public Animator animator;

	// Token: 0x04000003 RID: 3
	public bool inAchievement;

	// Token: 0x04000004 RID: 4
	public Sprite[] achievementQueue = new Sprite[10];

	// Token: 0x04000005 RID: 5
	public int achievementInQueue;

	// Token: 0x04000006 RID: 6
	public Sprite[] achievements = new Sprite[17];

	// Token: 0x04000007 RID: 7
	public float delayToAchievement;

	// Token: 0x04000008 RID: 8
	public bool toAchievement;

	// Token: 0x04000009 RID: 9
	public AudioClip achievement;

	// Token: 0x0400000A RID: 10
	public AudioSource audioDevice;
}
