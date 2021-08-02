using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200002C RID: 44
public class GameOverScript : MonoBehaviour
{
	// Token: 0x060000CE RID: 206 RVA: 0x000083A8 File Offset: 0x000065A8
	private void Start()
	{
		this.image = base.GetComponent<Image>();
		this.audioDevice = base.GetComponent<AudioSource>();
		this.delay = 5f;
		this.chance = Random.Range(1f, 99f);
		if (this.chance < 98f)
		{
			int num = Mathf.RoundToInt(Random.Range(0f, 4f));
			this.image.sprite = this.images[num];
		}
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00008424 File Offset: 0x00006624
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.M))
		{
			SceneManager.LoadScene("MainMenu");
		}
		else if (Input.GetKeyDown(KeyCode.W))
		{
			SceneManager.LoadScene("Warning");
		}
		if (Input.GetKeyDown(KeyCode.Alpha9) && Input.GetKeyDown(KeyCode.Alpha9))
		{
			this.chosen = true;
			this.image.sprite = this.rare;
			if (PlayerPrefs.GetFloat("99") == 0f)
			{
				this.aPopUp.QueueAchievement(this.aPopUp.achievements[10]);
				PlayerPrefs.SetFloat("99", 1f);
			}
			this.image.transform.localScale = new Vector3(5f, 5f, 1f);
			if (!this.audioDevice.isPlaying)
			{
				this.audioDevice.Play();
			}
			if (this.delay <= -5f)
			{
				Application.Quit();
			}
		}
		if (this.delay > 0f & this.chosen)
		{
			this.delay -= Time.deltaTime;
		}
	}

	// Token: 0x040001A0 RID: 416
	private Image image;

	// Token: 0x040001A1 RID: 417
	private float delay;

	// Token: 0x040001A2 RID: 418
	public Sprite[] images = new Sprite[5];

	// Token: 0x040001A3 RID: 419
	public Sprite rare;

	// Token: 0x040001A4 RID: 420
	private float chance;

	// Token: 0x040001A5 RID: 421
	private AudioSource audioDevice;

	// Token: 0x040001A6 RID: 422
	public AchievementPopUpScript aPopUp;

	// Token: 0x040001A7 RID: 423
	public bool chosen;
}
