using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000016 RID: 22
public class ConfettiScript : MonoBehaviour
{
	// Token: 0x0600005A RID: 90 RVA: 0x00004073 File Offset: 0x00002273
	private void Awake()
	{
		this.confettiList = new List<ConfettiScript.Confetti>();
		this.SpawnConfetti();
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00004088 File Offset: 0x00002288
	private void Update()
	{
		foreach (ConfettiScript.Confetti confetti in new List<ConfettiScript.Confetti>(this.confettiList))
		{
			if (confetti.Update())
			{
				this.confettiList.Remove(confetti);
			}
		}
		this.spawnTimer -= Time.deltaTime;
		if (this.spawnTimer <= 0f)
		{
			this.spawnTimer += this.spawnTimerMax;
			int num = Random.Range(1, 4);
			for (int i = 0; i < num; i++)
			{
				this.SpawnConfetti();
			}
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x0000413C File Offset: 0x0000233C
	private void SpawnConfetti()
	{
		float width = base.transform.GetComponent<RectTransform>().rect.width;
		float height = base.transform.GetComponent<RectTransform>().rect.height;
		Vector2 anchoredPosition = new Vector2((float)Random.Range(-380, 380), height / 2f);
		Color color = this.colorArray[Random.Range(0, this.colorArray.Length)];
		ConfettiScript.Confetti item = new ConfettiScript.Confetti(this.confettiPrefab, base.transform, anchoredPosition, color, -419f);
		this.confettiList.Add(item);
	}

	// Token: 0x04000081 RID: 129
	private float spawnTimerMax = 0.033f;

	// Token: 0x04000082 RID: 130
	public Transform confettiPrefab;

	// Token: 0x04000083 RID: 131
	private List<ConfettiScript.Confetti> confettiList;

	// Token: 0x04000084 RID: 132
	private float spawnTimer;

	// Token: 0x04000085 RID: 133
	public Color[] colorArray;

	// Token: 0x02000057 RID: 87
	private class Confetti
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x000101D8 File Offset: 0x0000E3D8
		public Confetti(Transform prefab, Transform container, Vector2 anchoredPosition, Color color, float minimumY)
		{
			this.anchoredPosition = anchoredPosition;
			this.minimumY = minimumY;
			this.transform = Object.Instantiate<Transform>(prefab, container);
			this.rectTransform = this.transform.GetComponent<RectTransform>();
			this.rectTransform.anchoredPosition = anchoredPosition;
			this.rectTransform.sizeDelta *= Random.Range(0.7f, 1.4f);
			this.euler = new Vector3(0f, 0f, Random.Range(0f, 360f));
			this.transform.localEulerAngles = this.euler;
			this.eulerSpeed = Random.Range(100f, 200f);
			this.eulerSpeed *= ((Random.Range(0f, 2f) == 0f) ? 1f : 1f);
			this.moveAmount = new Vector2(0f, Random.Range(-75f, -175f));
			this.transform.GetComponent<Image>().color = color;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x000102F4 File Offset: 0x0000E4F4
		public bool Update()
		{
			this.anchoredPosition += this.moveAmount * Time.deltaTime;
			this.rectTransform.anchoredPosition = this.anchoredPosition;
			this.euler.z = this.euler.z + this.eulerSpeed * Time.deltaTime;
			this.transform.localEulerAngles = this.euler;
			if (this.anchoredPosition.y < this.minimumY)
			{
				Object.Destroy(this.transform.gameObject);
				return true;
			}
			return false;
		}

		// Token: 0x0400035B RID: 859
		private Vector3 euler;

		// Token: 0x0400035C RID: 860
		private float eulerSpeed;

		// Token: 0x0400035D RID: 861
		private Transform transform;

		// Token: 0x0400035E RID: 862
		private RectTransform rectTransform;

		// Token: 0x0400035F RID: 863
		private Vector2 anchoredPosition;

		// Token: 0x04000360 RID: 864
		private Vector2 moveAmount;

		// Token: 0x04000361 RID: 865
		private float minimumY;
	}
}
