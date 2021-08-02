using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000024 RID: 36
public class FarmControllerScript : MonoBehaviour
{
	// Token: 0x06000090 RID: 144 RVA: 0x00004CED File Offset: 0x00002EED
	private void Start()
	{
		this.gc = GameObject.Find("GameController").GetComponent<GameControllerScript>();
		this.finished = false;
		this.time = 0f;
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00004D18 File Offset: 0x00002F18
	private void Update()
	{
		if (this.delay > 0f)
		{
			if (this.goToMainMenu)
			{
				this.delay -= this.finalDeltaTime;
			}
		}
		else if (this.goToMainMenu)
		{
			this.goToMainMenu = false;
			SceneManager.LoadScene("MainMenu");
		}
		if (!this.finished)
		{
			this.time += Time.deltaTime;
			this.timeText.text = "Time: " + Mathf.RoundToInt(this.time) + " seconds";
			if (this.time >= 200f & !this.appeared)
			{
				this.appeared = true;
				this.baldi.GetComponent<BaldiScript>().timeToMove = 0.6f;
				this.baldi.SetActive(true);
				this.baldiScript.agent.SetDestination(this.playerPosition.position);
			}
			else if (!this.appeared)
			{
				this.baldi.SetActive(false);
			}
			if (Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt))
			{
				if (!this.cheater.activeSelf)
				{
					this.cheater.SetActive(true);
					return;
				}
				this.cheater.SetActive(false);
				return;
			}
		}
		else
		{
			this.finished = false;
			this.finishedText.SetActive(true);
			if ((float)Mathf.RoundToInt(this.time) < PlayerPrefs.GetFloat("FinishedTime"))
			{
				PlayerPrefs.SetFloat("FinishedTime", (float)Mathf.RoundToInt(this.time));
			}
			this.score.text = "Your final time is: " + Mathf.RoundToInt(this.time) + " seconds!";
			if ((float)Mathf.RoundToInt(this.time) < PlayerPrefs.GetFloat("FinishedTime"))
			{
				Text text = this.score;
				text.text += "\n WOWIEZZZZ DUDE YOU GOT A NEW HIGHSCORE!!!!!!!!!";
			}
			this.finalDeltaTime = Time.deltaTime;
			Time.timeScale = 0f;
			this.delay = 15f;
			this.goToMainMenu = true;
			this.AchievementCheck();
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00004F34 File Offset: 0x00003134
	public void AchievementCheck()
	{
		if (PlayerPrefs.GetFloat("FarmCompleted") != 1f)
		{
			PlayerPrefs.SetFloat("FarmCompleted", 1f);
		}
		if ((float)Mathf.RoundToInt(this.time) < 200f & PlayerPrefs.GetFloat("LessThan200") != 1f)
		{
			this.gc.aPopUp.QueueAchievement(this.gc.aPopUp.achievements[13]);
			PlayerPrefs.SetFloat("LessThan200", 1f);
		}
		if ((float)Mathf.RoundToInt(this.time) < 175f & PlayerPrefs.GetFloat("LessThan175") != 1f)
		{
			this.gc.aPopUp.QueueAchievement(this.gc.aPopUp.achievements[14]);
			PlayerPrefs.SetFloat("LessThan175", 1f);
		}
		if ((float)Mathf.RoundToInt(this.time) < 150f & PlayerPrefs.GetFloat("LessThan150") != 1f)
		{
			this.gc.aPopUp.QueueAchievement(this.gc.aPopUp.achievements[15]);
			PlayerPrefs.SetFloat("LessThan150", 1f);
		}
		if (this.baldi.activeSelf & PlayerPrefs.GetFloat("FlipperActive") != 1f)
		{
			this.gc.aPopUp.QueueAchievement(this.gc.aPopUp.achievements[12]);
			PlayerPrefs.SetFloat("FlipperActive", 1f);
		}
	}

	// Token: 0x040000CB RID: 203
	public Text timeText;

	// Token: 0x040000CC RID: 204
	public float time;

	// Token: 0x040000CD RID: 205
	public bool finished;

	// Token: 0x040000CE RID: 206
	public GameObject baldi;

	// Token: 0x040000CF RID: 207
	public BaldiScript baldiScript;

	// Token: 0x040000D0 RID: 208
	public Transform playerPosition;

	// Token: 0x040000D1 RID: 209
	public GameObject cheater;

	// Token: 0x040000D2 RID: 210
	public GameObject finishedText;

	// Token: 0x040000D3 RID: 211
	public float delay;

	// Token: 0x040000D4 RID: 212
	public float finalDeltaTime;

	// Token: 0x040000D5 RID: 213
	public bool goToMainMenu;

	// Token: 0x040000D6 RID: 214
	public Text score;

	// Token: 0x040000D7 RID: 215
	public GameControllerScript gc;

	// Token: 0x040000D8 RID: 216
	public bool appeared;
}
