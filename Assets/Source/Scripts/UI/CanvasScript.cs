using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    public MainPanelSctipt mainPanel;

    public static CanvasScript canvas;

    public static bool mainPanelIsOpen = false;
    public RectTransform hpUi;
    public RectTransform spUi;
    public RectTransform mpUi;

    Image hp;
    Image sp;
    Image mp;

    Text hpText;
    Text spText;
    Text mpText;
    void Awake()
    {
        canvas = this;
        hp = hpUi.GetChild(0).GetComponent<Image>();
        sp = spUi.GetChild(0).GetComponent<Image>();
        mp = mpUi.GetChild(0).GetComponent<Image>();

        hpText = hpUi.GetChild(1).GetComponent<Text>();
        spText = spUi.GetChild(1).GetComponent<Text>();
        mpText = mpUi.GetChild(1).GetComponent<Text>();

        mainPanel.gameObject.SetActive(false);
        mainPanelIsOpen = false;
        AccessAll();
    }
    private void FixedUpdate()
    {
        DataBase();
        SizeBase();
    }
    private void DataBase()
    {
        float perHealth = PlayerStats.PlayerMaxHealth / 100f;
        hp.fillAmount = (PlayerStats.PlayerHealth / perHealth) / 100f;

        float perStamina = PlayerStats.PlayerMaxStamina / 100f;
        sp.fillAmount = (PlayerStats.PlayerStamina / perStamina) / 100f;

        float perMana = PlayerStats.PlayerMaxMana / 100f;
        mp.fillAmount = (PlayerStats.PlayerMana / perMana) / 100f;

        hpText.text = PlayerStats.PlayerHealth + " / " + PlayerStats.PlayerMaxHealth;
        spText.text = PlayerStats.PlayerStamina.ToString("0") + " / " + PlayerStats.PlayerMaxStamina.ToString("0");
        mpText.text = PlayerStats.PlayerMana + " / " + PlayerStats.PlayerMaxMana;
    }
    private void SizeBase()
    {
        hpUi.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxHealth * 3, hpUi.sizeDelta.y);
        spUi.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxStamina * 3, spUi.sizeDelta.y);
        mpUi.sizeDelta = new Vector2(100 + PlayerStats.PlayerMaxMana * 3, mpUi.sizeDelta.y);
    }
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            mainPanelIsOpen = !mainPanelIsOpen;
            mainPanel.gameObject.SetActive(mainPanelIsOpen);
        }
        if (mainPanelIsOpen)
        {
            Time.timeScale = 0f;
            PlayerController.con.isMelle = true;
            PlayerController.con.mySword.SetActive(false);

        }
        else Time.timeScale = 1f;
    }
    private void AccessAll()
    {
        mainPanel.Access();
    }
}
