using UnityEngine;

// Token: 0x02000006 RID: 6
public class AlarmClockScript : MonoBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002AE1 File Offset: 0x00000CE1
	private void Start()
	{
		this.timeLeft = 30f;
		this.lifeSpan = 35f;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002AFC File Offset: 0x00000CFC
	private void Update()
	{
		if (this.timeLeft >= 0f)
		{
			this.timeLeft -= Time.deltaTime;
		}
		else if (!this.rang)
		{
			this.Alarm();
		}
		if (this.lifeSpan >= 0f)
		{
			this.lifeSpan -= Time.deltaTime;
			return;
		}
		Destroy(base.gameObject, 0f);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002B68 File Offset: 0x00000D68
	private void Alarm()
	{
		this.rang = true;
		this.baldi.Hear(base.transform.position, 10f);
		this.audioDevice.clip = this.ring;
		this.audioDevice.loop = false;
		this.audioDevice.Play();
	}

	// Token: 0x04000016 RID: 22
	public float timeLeft;

	// Token: 0x04000017 RID: 23
	private float lifeSpan;

	// Token: 0x04000018 RID: 24
	private bool rang;

	// Token: 0x04000019 RID: 25
	public BaldiScript baldi;

	// Token: 0x0400001A RID: 26
	public AudioClip ring;

	// Token: 0x0400001B RID: 27
	public AudioSource audioDevice;
}
