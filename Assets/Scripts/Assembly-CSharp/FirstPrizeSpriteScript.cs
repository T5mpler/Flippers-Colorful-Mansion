using System;
using UnityEngine;

// Token: 0x02000027 RID: 39
public class FirstPrizeSpriteScript : MonoBehaviour
{
	// Token: 0x0600009F RID: 159 RVA: 0x00005710 File Offset: 0x00003910
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00005720 File Offset: 0x00003920
	private void Update()
	{
		this.angleF = Mathf.Atan2(this.cam.position.z - this.body.position.z, this.cam.position.x - this.body.position.x) * 57.29578f;
		if (this.angleF < 0f)
		{
			this.angleF += 360f;
		}
		this.debug = this.body.eulerAngles.y;
		this.angleF += this.body.eulerAngles.y;
		this.angle = Mathf.RoundToInt(this.angleF / 22.5f);
		while (this.angle < 0 || this.angle >= 16)
		{
			this.angle += (int)(-16f * Mathf.Sign((float)this.angle));
		}
		this.sprite.sprite = this.sprites[this.angle];
	}

	// Token: 0x040000F4 RID: 244
	public float debug;

	// Token: 0x040000F5 RID: 245
	public int angle;

	// Token: 0x040000F6 RID: 246
	public float angleF;

	// Token: 0x040000F7 RID: 247
	private SpriteRenderer sprite;

	// Token: 0x040000F8 RID: 248
	public Transform cam;

	// Token: 0x040000F9 RID: 249
	public Transform body;

	// Token: 0x040000FA RID: 250
	public Sprite[] sprites = new Sprite[16];
}
