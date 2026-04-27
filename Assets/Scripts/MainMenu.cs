using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using LootLocker.Requests;

public class MainMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public GameObject leaderboardPanel;
    public GameObject howToPlayPanel;
    public TextMeshProUGUI[] scoreTexts;
    public GameObject loadingText;
    private Leaderboard leaderboard;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);

        leaderboard = FindObjectOfType<Leaderboard>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
        loadingText.SetActive(true);

        foreach (var text in scoreTexts)
            text.gameObject.SetActive(false);

        leaderboard.GetTopScores((members) =>
        {
            loadingText.SetActive(false);

            foreach (var text in scoreTexts)
                text.gameObject.SetActive(true);

            if (members == null)
            {
                scoreTexts[0].text = "Failed to load scores";
                return;
            }

            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (i < members.Length)
                    scoreTexts[i].text = (i + 1) + ". " + members[i].player.name + " - " + members[i].score;
                else
                    scoreTexts[i].text = (i + 1) + ". ---";
            }
        });
    }

    public void CloseLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}