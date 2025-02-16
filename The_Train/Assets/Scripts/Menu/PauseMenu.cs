using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
	public GameObject settingsUI;
    public AudioMixerGroup mixer;

	internal bool isOpened;
	bool canUseMenu = true;

	private void Start()
	{
		foreach(Slider slider in settingsUI.GetComponentsInChildren<Slider>())
		{
			slider.value = PlayerPrefs.GetFloat(slider.name,1);
		}
        Resume();
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canUseMenu)
        {
			if (GameIsPaused)
			{
				Resume();
			}
			else
			{
				Pause();
			}
        }
    }
    public void Resume()
	{
		isOpened = false;
		pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
	}
    public void Pause()
	{
		Inventory.instance.CloseInventory();
		isOpened = true;
		pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
	}
    public void QuitGame()
    {
		Death.instance.BlockAll();
        SceneLoader.SwitchToScene("Menu");
    }

    public void Settings()
    {
        settingsUI.SetActive(true);
		pauseMenuUI.SetActive(false);
    }

    public void Back()
    {
        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

	private float CalculateVolume(float volume)
	{
		if (volume > 0.5f)
			return Mathf.Lerp(-40, 0, volume);
		else if (volume > 0.25f)
			return Mathf.Lerp(-60, 20, volume);
		else
			return Mathf.Lerp(-80, 80, volume);
	}

	public void ChangeMasterVolume(float volume)
	{
		mixer.audioMixer.SetFloat("MasterVolume", CalculateVolume(volume));
		PlayerPrefs.SetFloat("MasterVolume", volume);
	}

	public void ChangeMusicVolume(float volume)
	{
		mixer.audioMixer.SetFloat("MusicVolume", CalculateVolume(volume));
		PlayerPrefs.SetFloat("MusicVolume", volume);
	}

	public void ChangeEffectsVolume(float volume)
	{
		mixer.audioMixer.SetFloat("EffectsVolume", CalculateVolume(volume));
		PlayerPrefs.SetFloat("EffectsVolume", volume);
	}

	public void BlockPauseMenu()
	{
		canUseMenu = false;
		Resume();
	}
}
