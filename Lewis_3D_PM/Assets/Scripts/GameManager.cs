using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Image healthBar;
    public Image staminaBar;
    public Image sprintStatus;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI fireModeText;

    public GameObject pauseMenu;

    public bool paused = false;
    public bool enemiesGone = false;

    public int enemyCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            pauseMenu = GameObject.FindGameObjectWithTag("Pause");
            pauseMenu.SetActive(false);

            healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
            staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
            sprintStatus = GameObject.Find("SprintStatus").GetComponent<Image>();

            ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
            clipText = GameObject.Find("ClipText").GetComponent<TextMeshProUGUI>();
            weaponText = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
            fireModeText = GameObject.Find("FireMode").GetComponent<TextMeshProUGUI>();

            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if(enemyCount <= 0)
            {
                enemiesGone = true;
            }

            healthBar.fillAmount = (float)player.health / (float)player.maxHealth;
            staminaBar.fillAmount = player.stamina / player.maxStamina;

            sprintStatus.enabled = player.canSprint;

            if (player.currentWeapon)
            {
                weaponText.text = player.currentWeapon.name;
                ammoText.text = "Ammo: " + player.currentWeapon.ammo + "/" + player.currentWeapon.maxAmmo;
                clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;

                if (player.currentWeapon.fireModes >= 2)
                {
                    fireModeText.text = "Current Fire Mode: ";

                    if (player.currentWeapon.weaponID == 1)
                    {
                        if (player.currentWeapon.currentFireMode == 0)
                            fireModeText.text += "Single Fire";

                        else if (player.currentWeapon.currentFireMode == 1)
                            fireModeText.text += "Full Auto";
                    }
                }
                else
                    fireModeText.text = "";

            }
            else
            {
                weaponText.text = "";
                fireModeText.text = "";
                ammoText.text = "";
                clipText.text = "";
            }
        }
    }

    public void Pause()
    {
        paused = !paused;

        pauseMenu.SetActive(paused);

        Cursor.visible = paused;

        if(paused)
        {
            Cursor.lockState = CursorLockMode.None;

            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            Time.timeScale = 1;
        }
    }

    public void LoadLevel(int levelID)
    {
        if (levelID >= SceneManager.sceneCountInBuildSettings)
            Debug.Log("Level ID is too high: " + levelID);
        else
            SceneManager.LoadScene(levelID);
    }

    public void LoadNextNevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
