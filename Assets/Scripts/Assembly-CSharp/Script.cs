using System;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class Script : MonoBehaviour
{
	// Token: 0x06000155 RID: 341 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Start()
	{
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000E410 File Offset: 0x0000C610
	private void Update()
	{
		if (!this.audioDevice.isPlaying & this.played)
		{
			Application.Quit();
		}
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000E42E File Offset: 0x0000C62E
	private void OnTriggerEnter(Collider other)
	{
		if (other.name == "Player" & !this.played)
		{
			this.audioDevice.Play();
			this.played = true;
		}
	}

	// Token: 0x040002D7 RID: 727
	public AudioSource audioDevice;

	// Token: 0x040002D8 RID: 728
	private bool played;
}
