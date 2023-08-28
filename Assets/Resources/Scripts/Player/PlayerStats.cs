using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats stats;
    public static string PlayerName;
    public static Sprite PlayerSprite;
    public string Name;


    public GameObject DeathPanel;

    public static int PlayerHealth;
    public static int PlayerMaxHealth;
    public static float PlayerStamina;
    public float NewPlayerStamina;
    public static float PlayerMaxStamina;
    public static int PlayerMana;
    public static int PlayerMaxMana;

    public static int Strength;
    public static int Agility;
    public static int Constitution;
    public static int Intelligence;

    public static int level = 1;
    public static int Exp;
    public static int ExpPoints;
    public static int PlayerProtection;
    public static int PlayerMelDamage;
    public static int PlayerDisDamage;
    public static int PlayerMagDamage;

    public int[] levelChart; //при какой эксп получит новый уровень
    [Header("Start Settings")]
    public int StartStrenght = 1;
    public int StartAgility = 1;
    public int StartConstitution = 1;
    public int StartIntelligence = 1;
    public int StartExpPoint = 0;

    [Header("Bonus Settings")]
    public int strDam = 3;
    public float strWeight = 0.24f;
    public int strStam = 4;
    [Space]
    public int AglDam = 3;
    public float aglSpeed = 0.1f;
    public float aglDash = 0.25f;
    public int aglStam = 4;
    [Space]
    public int conHealth = 10;
    public int conProtect = 1;
    public int requirHealRegen = 3;
    public float TimeHealRegen = 0.1f;
    [Space]
    public int IntMana = 5;
    public int IntMag = 2;
    public int reqireManaRegen = 3;
    public float TimeManaRegen = 0.1f;
    [Space]
    public int lvlHealth = 10;
    public int lvlMana = 5;
    public int lvlStamina = 4;
    [Header("Magic Enemy")]
    public float timeManaRegen = 0.9f; //
    public float timeHealthRegen = 0.9f;
    public float timeStaminaRegen = 0.9f;
    [Header("Effect")]
    [SerializeField] private float staminaWaitModifactor;
    public static float stWait = 1;
    private float staminaPerRegen;
    private float mpWait = 1;
    private float hlWait = 1;
    public static bool Regen = false;

    public AudioSource source;
    public AudioClip clip;
    private void Awake()
    {
        stats = this;
        PlayerSprite = Resources.Load<Sprite>("Sprites/Sprites/PlayerSprite");
        PlayerName = Name;
        source = GetComponent<AudioSource>();
        DeathPanel = GameObject.Find("DeathPanel").gameObject;
    }
    void Start()
    {
        Manager();
        Strength = StartStrenght;
        Agility = StartAgility;
        Constitution = StartConstitution;
        Intelligence = StartIntelligence;

        Exp = 0;
        ExpPoints = StartExpPoint;
        Manager();

        PlayerHealth = PlayerMaxHealth;
        PlayerStamina = PlayerMaxStamina;
        PlayerMana = PlayerMaxMana;
        DeathPanel.SetActive(false);    
    }
    void Update()
    {
       
        HealthRegen();
        ManaRegen();
        StaminaRegen();
    }
    private void FixedUpdate()
    {
        Manager();
    }
    private void Manager()
    {
        SetMaxParemeters();
        SetDamageParameters();
        SetProtectionParameters();
    }
    private void SetMaxParemeters()
    {
        if(PlayerHealth > PlayerMaxHealth) PlayerHealth = PlayerMaxHealth;
        if(PlayerStamina > PlayerMaxStamina) PlayerStamina = PlayerMaxStamina;
        if(PlayerMana > PlayerMaxMana) PlayerMana = PlayerMaxMana;

        staminaPerRegen = PlayerMaxStamina / 5f; //–егулировать скорость восстановлени€ стамины здесь!!

        PlayerMaxHealth = 10 + (lvlHealth * level) + (conHealth * Constitution);
        PlayerMaxStamina = 20 + (lvlStamina * level) + (strDam * Strength) + (aglStam * Agility);
        PlayerMaxMana = 10 + (lvlMana * level) + (IntMana * Intelligence);
    }
    private void SetDamageParameters()
    {
        PlayerMelDamage = (strDam * Strength);
        PlayerDisDamage = (AglDam * Agility);
        PlayerMagDamage = (IntMana * Intelligence);

        if (Inventory.instance.equipment[0]) PlayerMelDamage += Inventory.instance.equipment[0].damage;
        if (Inventory.instance.equipment[1]) PlayerDisDamage += Inventory.instance.equipment[1].damage;
    }
    private void SetProtectionParameters()
    {
        PlayerProtection = level + (conProtect * Constitution);
        if (Inventory.instance.equipment[2]) PlayerProtection += Inventory.instance.equipment[2].protection;
    }
    private void CheckLevel()
    {
        if(Exp >= levelChart[level])
        {
            level++;
            ExpPoints += 2;
            SetMaxParemeters();

            source.PlayOneShot(clip);

            PlayerHealth = PlayerMaxHealth;
            PlayerStamina = PlayerMaxStamina;
            PlayerMana = PlayerMaxMana;
        }
    }
    public void AddExp(int addExp)
    {
        Exp += addExp;
        CheckLevel();
    }
    public void PlayerDamage(int damage)
    {
        Debug.Log(PlayerHealth);
        PlayerHealth -= damage;
        if(PlayerHealth <= 0)
        {
            PlayerHealth = 0;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            DeathPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
    private void HealthRegen()
    {
        if(Constitution >= requirHealRegen)
        {
            if (hlWait < 1)
            {
                hlWait += timeHealthRegen * (1 + Constitution - requirHealRegen) * Time.deltaTime;
            }
            else if (hlWait >= 1 && PlayerHealth < PlayerMaxHealth)
            {
                PlayerHealth += 1;
                hlWait = 0;
            }
        }
    }
    private void ManaRegen()
    {
        if (Intelligence >= reqireManaRegen)
        {
            if (mpWait <= 1)
            {
                mpWait += TimeManaRegen* (Intelligence - reqireManaRegen) * Time.deltaTime;
            }
            else if (mpWait >= 1 && PlayerMana < PlayerMaxMana)
            {
                PlayerMana += 1;
                mpWait = 0;
            }
        }
    }
    private void StaminaRegen()
    {
        if (stWait < 1)
        {
            stWait += Time.deltaTime * staminaWaitModifactor;
        }
        else 
        {
            if (PlayerStamina < PlayerMaxStamina)
            {
                PlayerStamina += staminaPerRegen * Time.deltaTime;
            }        
        }
    }
    public static void CharUp(int index)
    {
        switch (index)
        {
            case 0: Strength++; break;
            case 1: Agility++; break;
            case 2: Constitution++; break;
            case 3: Intelligence++; break;
        }
        ExpPoints--;
        stats.Manager();
    }
    public static bool StaminaLose(float loseCount, float modificator, int parameter)
    {
        loseCount -= modificator * parameter;
        if (loseCount <= 0) loseCount = 1;
        if (PlayerStamina >= loseCount)
        {
            stWait = 0;
            PlayerStamina -= loseCount;
            return true;
        }
        else return false;
    }
    public static bool ManaLose(int lose)
    {
        if (PlayerMana >= lose)
        {
            PlayerMana -= lose;
            return true;
        }
        else return false;
    }
   
}
