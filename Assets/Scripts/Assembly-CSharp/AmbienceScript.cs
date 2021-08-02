using UnityEngine;

// Token: 0x02000007 RID: 7
public class AmbienceScript : MonoBehaviour
{
	// Token: 0x06000017 RID: 23 RVA: 0x00002BBF File Offset: 0x00000DBF
	private void Start()
	{
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002BC4 File Offset: 0x00000DC4
	public void PlayAudio()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 49f));
		if (!this.audioDevice.isPlaying & num == 0)
		{
			base.transform.position = this.aiLocation.position;
			int num2 = Mathf.RoundToInt(Random.Range(0f, (float)(this.sounds.Length - 1)));
			this.audioDevice.PlayOneShot(this.sounds[num2]);
		}
	}

	// Token: 0x0400001C RID: 28
	public Transform aiLocation;

	// Token: 0x0400001D RID: 29
	public AudioClip[] sounds;

	// Token: 0x0400001E RID: 30
	public AudioSource audioDevice;
}
