using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Image healthBar;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();

        ammoText = GameObject.Find("AmmoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("ClipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)player.health / (float)player.maxHealth;

        if (player.currentWeapon)
        {
            weaponText.text = player.currentWeapon.name;
            ammoText.text = "Ammo: " + player.currentWeapon.ammo + "/" + player.currentWeapon.maxAmmo;
            clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;
        }
        else
        {
            weaponText.text = "";
            ammoText.text = "";
            clipText.text = "";
        }
    }
}
