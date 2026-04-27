using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public GameObject upgradePanel;
    public TextMeshProUGUI[] buttonTexts;
    public TextMeshProUGUI upgradeTitleText;

    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    private int rapidFireCount = 0;
    private const int maxRapidFire = 4;
    private const float minFireRate = 0.05f;
    private bool isBossUpgrade = false;
    private const float maxSpeed = 15f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    public void ShowUpgrades(bool bossUpgrade = false)
    {
        isBossUpgrade = bossUpgrade;
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;

        if (bossUpgrade)
        {
            upgradeTitleText.text = "Boss Reward!";
            upgradeTitleText.color = Color.red;

            if (rapidFireCount < maxRapidFire)
            {
                buttonTexts[0].text = "Rapid Fire";
            }
            else if (!playerMovement.doubleShot)
            {
                buttonTexts[0].text = "Double Shot";
            }
            else if (!playerMovement.tripleShot)
            {
                buttonTexts[0].text = "Triple Shot";
            }
            else
            {
                buttonTexts[0].text = "Explosive Bullets";
            }
            buttonTexts[1].text = "Damage Reduction";
            buttonTexts[2].text = playerMovement.moveSpeed >= maxSpeed ? "Max Speed Reached!" : "Double Speed Boost";
        }
        else
        {
            upgradeTitleText.text = "Choose Upgrade!";
            upgradeTitleText.color = Color.white;

            buttonTexts[0].text = "Heal";
            buttonTexts[1].text = playerMovement.moveSpeed >= maxSpeed ? "Max Speed Reached!" : "Speed Boost";
            buttonTexts[2].text = "Damage Reduction";
        }
        
    }

    public void SelectUpgrade(int index)
    {
        if (isBossUpgrade)
        {
            switch (index)
            {
                case 0:
                    if (rapidFireCount < maxRapidFire)
                    {
                        playerMovement.fireRate = Mathf.Max(playerMovement.fireRate - 0.05f, minFireRate);
                        rapidFireCount++;
                    }
                    else if (!playerMovement.doubleShot)
                    {
                        playerMovement.doubleShot = true;
                    }
                    else if (!playerMovement.tripleShot)
                    {
                        playerMovement.tripleShot = true;
                    }
                    else
                    {
                        playerMovement.explosiveBullets = true;
                    }
                    break;
                case 1:
                    playerHealth.damageReduction = true;
                    break;
                case 2:
                    if (playerMovement.moveSpeed < maxSpeed)
                        playerMovement.moveSpeed = Mathf.Min(playerMovement.moveSpeed + 2f, maxSpeed);
                    break;
            }
        }
        else
        {
            switch (index)
            {
                case 0:
                    playerHealth.Heal(2);
                    break;
                case 1:
                    if (playerMovement.moveSpeed < maxSpeed)
                        playerMovement.moveSpeed = Mathf.Min(playerMovement.moveSpeed + 1f, maxSpeed);
                    break;
                case 2:
                    playerHealth.damageReduction = true;
                    break;
            }
        }

        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
        FindObjectOfType<EnemySpawner>().SetWaitingForUpgrade(false);
    }
}