using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000034 RID: 52
public class LoadingScreenScript : MonoBehaviour
{
	// Token: 0x060000FE RID: 254 RVA: 0x00009788 File Offset: 0x00007988
	public void LoadScene(int sceneIndex)
	{
		this.tipText.text = string.Empty;
		this.loadingScreen.SetActive(true);
		this.imageRect.localScale = new Vector3(0f, 1.5f, 1f);
		base.StartCoroutine(this.LoadAsynchronously(sceneIndex));
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000097DE File Offset: 0x000079DE
	private IEnumerator LoadAsynchronously(int sceneIndex)
	{
		this.transitionTime = 2f;
		int chance = Mathf.RoundToInt(Random.Range(0f, 15f));
		float increaseFactor = 15f;
		this.rawImage.texture = this.textures[chance];
		while (this.imageRect.localScale.x < 20f)
		{
			this.imageRect.localScale += new Vector3(Time.deltaTime * increaseFactor, 0f, 0f);
			yield return null;
		}
		this.tipText.text = this.tipTexts[chance];
		this.imageRect.localScale = new Vector3(20f, 1.5f, 1f);
		this.tipText.fontSize = 30;
		this.tipText.color = Color.white;
		while (transitionTime > 0f)
		{
			transitionTime -= Time.deltaTime;
		}
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		if (operation != null)
		{
			while (!operation.isDone)
			{
				float progress = Mathf.Clamp01(operation.progress / 0.9f);
				float interPercentageProgress = Mathf.RoundToInt(progress * 100f);
				this.progressSlider.value = interPercentageProgress;
				this.progressText.text = interPercentageProgress + "%";
				if (operation.progress >= 50f & !this.transitionStarted)
				{
					this.transitionStarted = true;
					this.transitionAnimator.SetTrigger("Start");
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x040001FD RID: 509
	public GameObject loadingScreen;

	// Token: 0x040001FE RID: 510
	public Slider progressSlider;

	// Token: 0x040001FF RID: 511
	public Text progressText;

	// Token: 0x04000200 RID: 512
	public RawImage rawImage;

	// Token: 0x04000201 RID: 513
	public Texture[] textures = new Texture[16];

	// Token: 0x04000202 RID: 514
	public string[] tipTexts = new string[]
	{
		"The wizard will break his wand if he does too many spells",
		"Make sure to remember the schedule for the library",
		"Make sure to check out the tips in the MathGame",
		"Joe Wonderer isn't as bad as you may think",
		"The Swimming pool will fill your stamina bar but don't count on it all the time",
		"If all items in the Breakroom are gone, then some of them will refresh and come back",
		"Think strategically about going for some desktop applications",
		"You won't get detention if you run and you're outside",
		"The Grappling Hook might be buggy sometimes...",
		"Use the flashlight to see better in the Dark Event",
		"Once you get in, There's no getting out",
		"You can break Flippers knife for an amount of time with a special item",
		"If you see Doodletime, RUN, she's much harder than usual",
		"Choose the settings that you think work for you!",
		"Follow the rules, well at times. Or you'll face detention",
		"The Technology room is more interesting than you think"
	};

	// Token: 0x04000203 RID: 515
	public Text tipText;

	// Token: 0x04000204 RID: 516
	public RectTransform imageRect;

	// Token: 0x04000205 RID: 517
	public Animator transitionAnimator;

	// Token: 0x04000206 RID: 518
	public float transitionTime;

	// Token: 0x04000207 RID: 519
	public bool transitionStarted;
}
