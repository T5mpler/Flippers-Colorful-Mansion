using System.Collections;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class CameraScript : MonoBehaviour
{
	public static CameraScript i;
	// Token: 0x06000051 RID: 81 RVA: 0x00003B9F File Offset: 0x00001D9F
	private void Awake()
	{
		i = this;
	}
	private void Start()
	{
		this.offset = base.transform.position - this.player.transform.position;
	}
	private void Update()
	{
		if (this.ps.jumpRope)
		{
			this.velocity -= this.gravity * Time.deltaTime;
			this.jumpHeight += this.velocity * Time.deltaTime;
			if (this.jumpHeight <= 0f)
			{
				this.jumpHeight = 0f;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					this.velocity = this.initVelocity;
				}
			}
			this.jumpHeightV3 = new Vector3(0f, this.jumpHeight, 0f);
		}
		else if (Input.GetButton("Look Behind"))
		{
			this.lookBehind = 180;
		}
		else
		{
			this.lookBehind = 0;
		}
		if (Input.GetKeyDown(KeyCode.Space) & Vector3.Distance(this.player.transform.position, this.spSide.position) <= 8f & !this.inPool)
		{
			this.poolRides++;
			this.inPool = true;
			if (this.poolRides == 4)
			{
				this.poolDirty = true;
			}
			else
			{
				this.poolDirty = false;
			}
			this.player.transform.position = this.waterTransform.position;
			return;
		}
		if (Input.GetKeyDown(KeyCode.Space) & Vector3.Distance(this.player.transform.position, this.spSide.position) <= 8f & this.inPool)
		{
			this.inPool = false;
			if (this.poolDirty)
			{
				this.poolRides = 0;
				this.poolDirty = false;
			}
			this.player.transform.position = new Vector3(-85f, player.transform.position.y, 215f);
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00003D84 File Offset: 0x00001F84
	private void LateUpdate()
	{
		base.transform.position = this.player.transform.position + this.offset;
		if (!this.ps.gameOver & !this.ps.jumpRope)
		{
			base.transform.position = this.player.transform.position + this.offset;
			base.transform.rotation = this.player.transform.rotation * Quaternion.Euler(0f, (float)this.lookBehind, 0f);
			return;
		}
		if (this.ps.gameOver)
		{
			this.player.GetComponent<MeshRenderer>().enabled = false;
			base.transform.position = this.baldi.transform.position + this.baldi.transform.forward * 2f + new Vector3(0f, 5f, 0f);
			base.transform.LookAt(new Vector3(this.baldi.position.x, this.baldi.position.y + 5f, this.baldi.position.z));
			return;
		}
		if (this.ps.jumpRope)
		{
			base.transform.position = this.player.transform.position + this.offset + this.jumpHeightV3;
			base.transform.rotation = this.player.transform.rotation;
		}
	}
	// Token: 0x04000070 RID: 112
	public GameObject player;

	// Token: 0x04000071 RID: 113
	public PlayerScript ps;

	// Token: 0x04000072 RID: 114
	public Transform baldi;

	// Token: 0x04000073 RID: 115
	public float initVelocity;

	// Token: 0x04000074 RID: 116
	public float velocity;

	// Token: 0x04000075 RID: 117
	public float gravity;

	// Token: 0x04000076 RID: 118
	private int lookBehind;

	// Token: 0x04000077 RID: 119
	private Vector3 offset;

	// Token: 0x04000078 RID: 120
	public float jumpHeight;

	// Token: 0x04000079 RID: 121
	private Vector3 jumpHeightV3;

	// Token: 0x0400007A RID: 122
	public Transform spSide;

	// Token: 0x0400007B RID: 123
	public Transform waterTransform;

	// Token: 0x0400007C RID: 124
	public bool inPool;

	// Token: 0x0400007D RID: 125
	public int poolRides;

	// Token: 0x0400007E RID: 126
	public bool poolDirty;

	Vector3 cameraAdder;
}
