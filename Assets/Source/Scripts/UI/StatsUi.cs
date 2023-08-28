using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class StatsUi : MonoBehaviour
{
    [Header("Charcter Panel")]
    [SerializeField] private Image CharImage;
    [SerializeField] private Text charText;

    [SerializeField] private Text StrText;
    [SerializeField] private Text AglText;
    [SerializeField] private Text ConText;
    [SerializeField] private Text IntText;

    [SerializeField] private GameObject[] buttons;
    [Header("Info Panel")]
    [SerializeField] private Text lvlText;
    [SerializeField] private Text expText;
    [SerializeField] private Text nextLvl;
    [SerializeField] private Text pointText;

    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;

    [SerializeField] private Text melWeaponText;
    [SerializeField] private Text armorText;
    [SerializeField] private Text DisWeaponText;
    [SerializeField] private Text protectionText;

    [SerializeField] private Text melDamage;
    [SerializeField] private Text disDamage;
    [SerializeField] private Text magDamage;

    [Header("DeathPanel")]
    [SerializeField] private Text lvlDText;
    [SerializeField] private Text expDText;

    [Inject]
    private Inventory inventory;

    public AudioSource source;
    public AudioClip clip;
    public void Access()
    {
        if (buttons[0]) buttons[0].GetComponent<Button>().onClick.AddListener(SrtBut);
        if (buttons[1]) buttons[1].GetComponent<Button>().onClick.AddListener(AglBut);
        if (buttons[2]) buttons[2].GetComponent<Button>().onClick.AddListener(ConBut);
        if (buttons[3]) buttons[3].GetComponent<Button>().onClick.AddListener(IntBut);
    }
    private void OnEnable()
    {
        source = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();    
        CharRefresh();
        InfoRefresh();
    }
    private void FixedUpdate()
    {
        if (lvlDText) lvlDText.text = "Level : " + PlayerStats.level.ToString();
        if (expDText) expDText.text = "ExpPoints " + PlayerStats.Exp.ToString();
    }
    private void CharRefresh()
    {
        if (CharImage) CharImage.sprite = PlayerStats.PlayerSprite;
        if (charText) charText.text = PlayerStats.PlayerName;

        if(StrText) StrText.text = PlayerStats.Strength.ToString();
        if(AglText) AglText.text = PlayerStats.Agility.ToString();
        if(ConText) ConText.text = PlayerStats.Constitution.ToString();
        if(IntText) IntText.text = PlayerStats.Intelligence.ToString();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(PlayerStats.ExpPoints > 0);
        }
    }
    private void InfoRefresh()
    {
        if(lvlText) lvlText.text = PlayerStats.level.ToString();
        if(expText) expText.text = PlayerStats.Exp.ToString();
 //       if(nextLvl) nextLvl.text = PlayerStats.stats.levelChart[PlayerStats.level].ToString();
        if(pointText) pointText.text = PlayerStats.ExpPoints.ToString();


        if(hpText) hpText.text = PlayerStats.PlayerMaxHealth.ToString();
        if(mpText) mpText.text = PlayerStats.PlayerMaxMana.ToString();

        if (melWeaponText)
        {
            if (inventory.equipment[0]) melWeaponText.text = inventory.equipment[0].name;
            else melWeaponText.text = "no";
        }
        if (DisWeaponText)
        {
            if (inventory.equipment[1]) DisWeaponText.text = inventory.equipment[1].name;
            else DisWeaponText.text = "no";
        }
        if (armorText)
        {
            if (inventory.equipment[2]) armorText.text = inventory.equipment[2].name;
            else armorText.text = "no"; 
        }
        if(protectionText) protectionText.text = PlayerStats.PlayerProtection.ToString();
        if(melDamage) melDamage.text = PlayerStats.PlayerMelDamage.ToString();
        if(disDamage) disDamage.text = PlayerStats.PlayerDisDamage.ToString();
        if(magDamage) magDamage.text = PlayerStats.PlayerMagDamage.ToString();
    }
    private void SrtBut()
    {
        source.PlayOneShot(clip);
        PlayerStats.CharUp(0);
        CharRefresh();
        InfoRefresh();
    }
    private void AglBut()
    {
        source.PlayOneShot(clip);
        PlayerStats.CharUp(1);
        CharRefresh();
        InfoRefresh();
    }
    private void ConBut()
    {
        source.PlayOneShot(clip);
        PlayerStats.CharUp(2);
        CharRefresh();
        InfoRefresh();
    }
    private void IntBut()
    {
        source.PlayOneShot(clip);
        PlayerStats.CharUp(3);
        CharRefresh();
        InfoRefresh();
    }
}
