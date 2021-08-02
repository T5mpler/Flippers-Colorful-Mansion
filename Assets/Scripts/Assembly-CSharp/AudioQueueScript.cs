using System;
using UnityEngine;

// Token: 0x02000008 RID: 8
public class AudioQueueScript : MonoBehaviour
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002C3F File Offset: 0x00000E3F
	private void Start()
	{
		this.audioDevice = base.GetComponent<AudioSource>();
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002C4D File Offset: 0x00000E4D
	private void Update()
	{
		if (!this.audioDevice.isPlaying && this.audioInQueue > 0)
		{
			this.PlayQueue();
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C6B File Offset: 0x00000E6B
	public void QueueAudio(AudioClip sound)
	{
		this.audioQueue[this.audioInQueue] = sound;
		this.audioInQueue++;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002C89 File Offset: 0x00000E89
	private void PlayQueue()
	{
		this.audioDevice.PlayOneShot(this.audioQueue[0]);
		this.UnqueueAudio();
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002CA4 File Offset: 0x00000EA4
	private void UnqueueAudio()
	{
		for (int i = 1; i < this.audioInQueue; i++)
		{
			this.audioQueue[i - 1] = this.audioQueue[i];
		}
		this.audioInQueue--;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002CE2 File Offset: 0x00000EE2
	public void ClearAudioQueue()
	{
		this.audioInQueue = 0;
	}

	// Token: 0x0400001F RID: 31
	private AudioSource audioDevice;

	// Token: 0x04000020 RID: 32
	private int audioInQueue;

	// Token: 0x04000021 RID: 33
	private AudioClip[] audioQueue = new AudioClip[100];
}
