using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        staminaBar = GameObject.Find("StaminaBar").GetComponent<Image>();
        sprintStatus = GameObject.Find("SprintStatus").GetComponent<Image>();

        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("ClipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        fireModeText = GameObject.Find("FireMode").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
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
