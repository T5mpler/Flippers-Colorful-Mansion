using UnityEngine;

// Token: 0x02000010 RID: 16
public class BsodaSparyScript : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x00003674 File Offset: 0x00001874
	private void Start()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.rb.velocity = base.transform.forward * this.speed;
		this.lifeSpan = 30f;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000036B0 File Offset: 0x000018B0
	private void Update()
	{
		this.rb.velocity = base.transform.forward * this.speed;
		this.lifeSpan -= Time.deltaTime;
		if (this.lifeSpan < 0f)
		{
			Object.Destroy(base.gameObject, 0f);
		}
	}

	// Token: 0x0400005C RID: 92
	public float speed;

	// Token: 0x0400005D RID: 93
	private float lifeSpan;

	// Token: 0x0400005E RID: 94
	private Rigidbody rb;
}
