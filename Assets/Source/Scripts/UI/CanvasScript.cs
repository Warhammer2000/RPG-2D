using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CanvasScript : MonoBehaviour
{

    [SerializeField] private MainPanelSctipt mainPanel;

   

    private static bool mainPanelIsOpen = false;
    
    [SerializeField] private RectTransform hpUi;
    [SerializeField] private RectTransform spUi;
    [SerializeField] private RectTransform mpUi;

    private Image hp;
    private Image sp;
    private Image mp;

    private Text hpText;
    private Text spText;
    private Text mpText;

    [Inject] private PlayerController controller;

    void Awake()
    {
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
            controller.isMelle = true;
            controller.mySword.SetActive(false);

        }
        else Time.timeScale = 1f;
    }
    private void AccessAll()
    {
        mainPanel.Access();
    }
}
