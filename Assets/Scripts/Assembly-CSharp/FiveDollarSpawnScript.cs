using UnityEngine;

// Token: 0x02000028 RID: 40
public class FiveDollarSpawnScript : MonoBehaviour
{
	// Token: 0x060000A2 RID: 162 RVA: 0x0000584C File Offset: 0x00003A4C
	private void Start()
	{
		if (Mathf.RoundToInt(Random.Range(0f, 10f)) == 0)
		{
			base.gameObject.SetActive(true);
			int num = Mathf.RoundToInt(Random.Range(0f, 32f));
			base.transform.position = this.wanderer.newLocation[num].position;
			this.cooldown = (float)Mathf.RoundToInt(Random.Range(15f, 50f));
			return;
		}
		base.gameObject.SetActive(false);
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x000058D8 File Offset: 0x00003AD8
	public void Update()
	{
		if (this.cooldown > 0f)
		{
			this.cooldown -= Time.deltaTime;
			return;
		}
		int num = Mathf.RoundToInt(Random.Range(0f, 32f));
		base.transform.position = this.wanderer.newLocation[num].position;
		this.cooldown = (float)Mathf.RoundToInt(Random.Range(15f, 50f));
	}

	// Token: 0x040000FB RID: 251
	public AILocationSelectorScript wanderer;

	// Token: 0x040000FC RID: 252
	public float cooldown;
}
