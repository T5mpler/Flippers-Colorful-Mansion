using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
	public static PostProcessManager instance;
	public PostProcessProfile mainProfile;
	Vignette vignette;
	ChromaticAberration chromaticAberration;
	MotionBlur motionBlur;
	Bloom bloom;
	void Awake()
	{
		instance = this;
		vignette = mainProfile.GetSetting<Vignette>();
		chromaticAberration = mainProfile.GetSetting<ChromaticAberration>();
		motionBlur = mainProfile.GetSetting<MotionBlur>();
		bloom = mainProfile.GetSetting<Bloom>();
	}
	private void Start()
	{
		vignette.enabled.value = bool.Parse(PlayerPrefs.GetString("ToggleVignette"));
		chromaticAberration.enabled.value = bool.Parse(PlayerPrefs.GetString("ToggleChromaticAberration"));
		bloom.enabled.value = bool.Parse(PlayerPrefs.GetString("ToggleBloom"));
		motionBlur.enabled.value = bool.Parse(PlayerPrefs.GetString("ToggleMotionBlur"));
	}
}