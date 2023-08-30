using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Zenject;
using System.Reflection;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed;
    public Joystick joystick;
    
    [Inject] private Inventory inventory;
    [Inject] private PlayerStats stats;
    [Inject] private SpellBook spell;


    private Vector3 cursor;
    public float DashForce; //Сила рывка
    public float DashStaminaLose;
    public float DashTime;
    public float DashCountDown;

    private bool facingRight = true;
    public bool isDashed;
    public bool isHited;
    public bool playerIsStand;
    private Transform mySprite;
    private Rigidbody2D rb;

    [Header("Weapon")]
    public bool ArrowIsReady;
    private bool bowCharched;
    public bool isMelle;

    public GameObject DistantWeapon;
    public SpriteRenderer myBow;
    public GameObject myPoint;
    public GameObject mySword;
    public Animator swordAnim;
    private SpriteRenderer swordRender;
    private BoxCollider2D swordCollider;
    [SerializeField]private float BowReady;

    public Transform arrowPoint;
    private float x, y, XPlus, YPlus;

    public GameObject[] castPoints;
    private bool isCasting = false;

    public int killedEnemy = 0;
    public Text killedEnemyText;
    public AudioClip clip;
    public AudioSource source;
    [SerializeField] private bool isButtonActive = false;
    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        rb = GetComponent<Rigidbody2D>();
        mySprite = transform.GetChild(0);
        mySword = mySprite.GetChild(0).gameObject;
        swordAnim = mySword.GetComponent<Animator>();
        swordRender = mySword.GetComponent<SpriteRenderer>();
        swordCollider = mySword.GetComponent<BoxCollider2D>();

        DistantWeapon = transform.GetChild(1).gameObject;
        myBow = DistantWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>();
        arrowPoint = DistantWeapon.transform.GetChild(1);

        mySword.SetActive(false);
        DistantWeapon.SetActive(false); 

        castPoints = new GameObject[2];
        castPoints[0] = mySprite.GetChild(1).gameObject;
        castPoints[1] = mySprite.GetChild(2).gameObject;

        source = GetComponent<AudioSource>();
    }
    void Update()
    {
        x = joystick.Horizontal;
        y = joystick.Vertical;

        playerIsStand = x == 0 && y == 0;
        cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        DashPc();
        Attack();
    }
    private void FixedUpdate()
    {
        if(!isDashed && !isHited)
        {
            Move(x,y);
        }
        if (killedEnemyText)
        {
            killedEnemyText.text = "EnemyKill : " + killedEnemy;
        }
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void DashPc()
    {
        if (Input.GetKey(KeyCode.Space) && !playerIsStand && !isDashed)
        {
            if (PlayerStats.StaminaLose(DashStaminaLose, stats.aglDash, PlayerStats.Agility))
            {
                isDashed = true;
                DashTime = DashCountDown;
                rb.AddForce(new Vector2(DashForce * x, DashForce * y), ForceMode2D.Impulse);
                source.PlayOneShot(clip);
            }

        }
        if (DashTime > 0) DashTime -= Time.deltaTime;
        else if (isDashed)
        {
            DashTime = 0;
            isDashed = false;
            rb.velocity = Vector3.zero;
        }
    }
    public void DashAndroid()
    {
        if (!playerIsStand && !isDashed)
        {
            if (PlayerStats.StaminaLose(DashStaminaLose, stats.aglDash, PlayerStats.Agility))
            {
                isDashed = true;
                DashTime = DashCountDown;
                rb.AddForce(new Vector2(DashForce * x, DashForce * y), ForceMode2D.Impulse);
                source.PlayOneShot(clip);
            }
        }
        if (DashTime > 0) DashTime -= Time.deltaTime;
        else if (isDashed)
        {
            DashTime = 0;
            isDashed = false;
            rb.velocity = Vector3.zero;
        }
    }
    private void Attack()
    {
        if (!isCasting)
        {
            if (Input.GetMouseButton(1)) DistantPc();
            else if (Input.GetMouseButtonUp(1)) { DistantWeapon.SetActive(false); BowReady = 0f; } 
            else MellePc();
        }
        Magic();
        HitOff();
    }

    private void MellePc()
    {
        if(Input.GetMouseButtonDown(0) && !isMelle)
        {
            if (inventory.equipment[0])
            {
                if(PlayerStats.StaminaLose(inventory.equipment[0].weight, stats.strWeight, PlayerStats.Strength))
                {
                    swordRender.sprite = inventory.equipment[0].sprite;
                    swordCollider.size = new Vector2(0.4f, inventory.equipment[0].length);
                    swordCollider.offset = new Vector2(0.4f, inventory.equipment[0].offset);

                    float sp = inventory.equipment[0].speed;

                    swordAnim.speed = sp;
                    isMelle = true;
                    mySword.SetActive(true);
                }
            }
        }
    }
    public void MelleAndroid()
    {
        if (!isMelle)
        {
            if (inventory.equipment[0])
            {
                if (PlayerStats.StaminaLose(inventory.equipment[0].weight, stats.strWeight, PlayerStats.Strength))
                {
                    swordRender.sprite = inventory.equipment[0].sprite;
                    swordCollider.size = new Vector2(0.4f, inventory.equipment[0].length);
                    swordCollider.offset = new Vector2(0.4f, inventory.equipment[0].offset);

                    float sp = inventory.equipment[0].speed;

                    swordAnim.speed = sp;
                    isMelle = true;
                    mySword.SetActive(true);
                }
            }
        }
    }
    public void MagicAndorid()
    {
        isButtonActive = !isButtonActive;
    }
    private void Magic()
    {
        if (isButtonActive)
        {
            if (isMelle) return;
            isCasting = true;
            if (spell.equipment[0])
            {
                MagicEffect(0);
                MagicCost(0);
            }
            if (spell.equipment[1])
            {
                MagicEffect(1);
                MagicCost(1);
            }
        }
        else
        {
            isCasting = false;
            castPoints[0].SetActive(false);
            castPoints[1].SetActive(false);
        }
    }
    private void MagicCost(int index)
    {
        if (Input.GetMouseButtonDown(index))
        {
            Vector2 castPos = castPoints[index].transform.position;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePos - castPos;
            castPoints[index].transform.right = direction;

            if (PlayerStats.ManaLose(spell.equipment[index].manaCost))
            {
                AttackMagicScript mag = Instantiate(spell.equipment[index].spellPref, castPoints[index].transform.position, castPoints[index].transform.rotation).GetComponent<AttackMagicScript>();
                mag.Initialize(spell.equipment[index]);
            }
        }
    }
    private void MagicEffect(int index)
    {
        castPoints[index].SetActive(true);
        ParticleSystem.MainModule castMain = castPoints[index].GetComponent<ParticleSystem>().main;
        castMain.startColor = new ParticleSystem.MinMaxGradient(spell.equipment[index].SpellColor);
    }
    private void HitOff()
    {
        if (isMelle)
        {
            if (!swordAnim.GetCurrentAnimatorStateInfo(0).IsName("SwordAnim"))
            {
                isMelle=false;
                mySword.SetActive(false);
            }
        }
    }

    private void DistantPc()
    {
        if (isMelle || !inventory.equipment[1]) return;
        DistantWeapon.SetActive(true);

        Vector2 bowPos = DistantWeapon.transform.position;
        Vector2 direction = (Vector2)cursor - bowPos;
        DistantWeapon.transform.right = direction;
        PlayerStats.stWait = 0;

        BowReady += inventory.equipment[1].speed * 0.2f * Time.deltaTime;
        if(BowReady <= 2)
        {
            
            if (myBow.sprite != inventory.equipment[1].sprite) myBow.sprite = inventory.equipment[1].sprite;
           if(BowReady < 1f)arrowPoint.gameObject.SetActive(false);
           
            
                if (inventory.ArrowCheaker(inventory.equipment[1].ArrowId))
                {
                    Debug.Log("1");
                    ArrowIsReady = true;
                    arrowPoint.gameObject.SetActive(true);


                    arrowPoint.localPosition = new Vector3(1.25f, 0, 0);
                    arrowPoint.GetComponent<SpriteRenderer>().sprite = inventory.equipment[1].arrowSprite;
                }
                else arrowPoint.gameObject.SetActive(false);    
            
        }
        else if (ArrowIsReady)
        {
            if (myBow.sprite != inventory.equipment[1].sprite) myBow.sprite = inventory.equipment[1].sprite;
            arrowPoint.localPosition = new Vector3(0.625f,0,0);

            bowCharched = true;
            if (bowCharched)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(PlayerStats.StaminaLose(inventory.equipment[1].weight, stats.strWeight, PlayerStats.Strength))
                    {
                        bowCharched = false;
                        ArrowIsReady = false;
                        BowReady = 0f;
                        inventory.ArrowUse();
                        Instantiate(inventory.equipment[1].myArrow, arrowPoint.position, arrowPoint.rotation);
                    }
                }
            }
        }
    }
    public void DistantAndroid()
    {
        if (isMelle || !inventory.equipment[1]) return;
        DistantWeapon.SetActive(true);

        Vector2 bowPos = DistantWeapon.transform.position;
        Vector2 direction = (Vector2)cursor - bowPos;
        DistantWeapon.transform.right = direction;
        PlayerStats.stWait = 0;

        BowReady += inventory.equipment[1].speed * 0.2f * Time.deltaTime;
        if (BowReady <= 2)
        {

            if (myBow.sprite != inventory.equipment[1].sprite) myBow.sprite = inventory.equipment[1].sprite;
            if (BowReady < 1f) arrowPoint.gameObject.SetActive(false);


            if (inventory.ArrowCheaker(inventory.equipment[1].ArrowId))
            {
                ArrowIsReady = true;
                arrowPoint.gameObject.SetActive(true);


                arrowPoint.localPosition = new Vector3(1.25f, 0, 0);
                arrowPoint.GetComponent<SpriteRenderer>().sprite = inventory.equipment[1].arrowSprite;
            }
            else arrowPoint.gameObject.SetActive(false);

        }
        else if (ArrowIsReady)
        {
            if (myBow.sprite != inventory.equipment[1].sprite) myBow.sprite = inventory.equipment[1].sprite;
            arrowPoint.localPosition = new Vector3(0.625f, 0, 0);

            bowCharched = true;
            if (bowCharched)
            {
               if (PlayerStats.StaminaLose(inventory.equipment[1].weight, stats.strWeight, PlayerStats.Strength))
               {
                   bowCharched = false;
                   ArrowIsReady = false;
                   BowReady = 0f;
                   inventory.ArrowUse();
                   Instantiate(inventory.equipment[1].myArrow, arrowPoint.position, arrowPoint.rotation);
               }
            }
        }
    }
    private void Move(float x, float y)
    {
        mySprite.eulerAngles = new Vector3(0, 0, 15 * -x);
        rb.velocity = new Vector3(x, y) * speed;
        //if(cursor.x < transform.position.x && facingRight)
        //{
        //    Flip();
        //}
        //else if(cursor.x > transform.position.x && !facingRight)
        //{
        //    Flip();
        //}
        if (x < 0 && facingRight)
        {
            Flip();
        }
        else if (x > 0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = mySprite.localScale;
        scale.x *= -1;
        mySprite.localScale = scale;
    }
}
