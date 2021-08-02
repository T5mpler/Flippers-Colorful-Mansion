using System.Collections;
using UnityEngine;

// Token: 0x0200000E RID: 14
public class BreakRoomScript : MonoBehaviour
{
	// Token: 0x0600003A RID: 58 RVA: 0x00003470 File Offset: 0x00001670
	private void Update()
	{
		for (int i = 0; i < 7; i++)
		{
			if (!this.breakRoomItems[i].activeInHierarchy && breakRoomItems[i] != null)
			{
				this.itemsMissing[i] = true;
			}
		}
		if (this.itemsMissing[0] && this.itemsMissing[1] && this.itemsMissing[2] && this.itemsMissing[3] && this.itemsMissing[4] && this.itemsMissing[5] && this.itemsMissing[6])
		{
			if (!this.noMoreItems)
			{
				this.itemCooldown = 25f;
			}
			this.noMoreItems = true;
		}
		if (this.itemCooldown > 0f)
		{
			this.itemCooldown -= Time.deltaTime;
			return;
		}
		if (this.noMoreItems)
		{
			base.StartCoroutine(this.RefreshBreakRoomItems());
		}
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00003536 File Offset: 0x00001736
	private IEnumerator RefreshBreakRoomItems()
	{
		int count = 3;
		int sameNum = 0;
		while (count > 0)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 6f));
			if (num == sameNum)
			{
				num = Mathf.RoundToInt(Random.Range(0f, 6f));
				sameNum = num;
			}
			this.breakRoomItems[num].SetActive(true);
			int num2 = count;
			count = num2 - 1;
			yield return null;
		}
		for (int i = 0; i < 7; i++)
		{
			this.itemsMissing[i] = false;
		}
		this.noMoreItems = false;
		yield break;
	}

	// Token: 0x04000053 RID: 83
	public GameControllerScript gc;

	// Token: 0x04000054 RID: 84
	public GameObject[] breakRoomItems = new GameObject[7];

	// Token: 0x04000055 RID: 85
	private float itemCooldown;

	// Token: 0x04000056 RID: 86
	private bool noMoreItems;

	// Token: 0x04000057 RID: 87
	private bool[] itemsMissing = new bool[7];
}
