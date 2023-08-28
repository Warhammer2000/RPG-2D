using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float speed;

    public static PlayerController con;


    public PlayerStats stats;
    private Vector3 cursor;
    public float DashForce; //���� �����
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
    //public GameObject Arrow;
    private float x, y, XPlus, YPlus;

    public GameObject[] castPoints;
    private bool isCasting = false;

    public int killedEnemy = 0;
    public Text killedEnemyText;
    public AudioClip clip;
    public AudioSource source;
    void Awake()
    {
        con = this;
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
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        playerIsStand = x == 0 && y == 0;
        cursor = Camera.main.ScreenToWorldPoint(Input.mousePosition);


        Dash();
        Attack();
    }
    private void FixedUpdate()
    {
        if(!isDashed && !isHited)
        {
            Move();
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
    private void Dash()
    {
        if(Input.GetKey(KeyCode.Space) && !playerIsStand && !isDashed)
        {
            if(PlayerStats.StaminaLose(DashStaminaLose, PlayerStats.stats.aglDash, PlayerStats.Agility))
            {
                isDashed = true;
                DashTime = DashCountDown;
                rb.AddForce(new Vector2(DashForce * x, DashForce * y), ForceMode2D.Impulse);
                source.PlayOneShot(clip);
            }
           
        }
        if(DashTime > 0) DashTime -= Time.deltaTime;    
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
            if (Input.GetMouseButton(1)) Distant();
            else if (Input.GetMouseButtonUp(1)) { DistantWeapon.SetActive(false); BowReady = 0f; } 
            else Melle();
        }
        Magic();
        HitOff();
    }

    private void Melle()
    {
        if(Input.GetMouseButtonDown(0) && !isMelle)
        {
            if (Inventory.instance.equipment[0])
            {
                if(PlayerStats.StaminaLose(Inventory.instance.equipment[0].weight, PlayerStats.stats.strWeight, PlayerStats.Strength))
                {
                    swordRender.sprite = Inventory.instance.equipment[0].sprite;
                    swordCollider.size = new Vector2(0.4f, Inventory.instance.equipment[0].length);
                    swordCollider.offset = new Vector2(0.4f, Inventory.instance.equipment[0].offset);

                    float sp = Inventory.instance.equipment[0].speed;

                    swordAnim.speed = sp;
                    isMelle = true;
                    mySword.SetActive(true);
                }
            }
        }
    }
  

    private void Magic()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isMelle) return;
            isCasting = true;
            if (SpellBook.instance.equipment[0])
            {
                MagicEffect(0);
                MagicCost(0);
            }
            if (SpellBook.instance.equipment[1])
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

            if (PlayerStats.ManaLose(SpellBook.instance.equipment[index].manaCost))
            {
                AttackMagicScript mag = Instantiate(SpellBook.instance.equipment[index].spellPref, castPoints[index].transform.position, castPoints[index].transform.rotation).GetComponent<AttackMagicScript>();
                mag.Initialize(SpellBook.instance.equipment[index]);
            }
        }
    }
    private void MagicEffect(int index)
    {
        castPoints[index].SetActive(true);

        ParticleSystem.MainModule castMain = castPoints[index].GetComponent<ParticleSystem>().main;
        castMain.startColor = new ParticleSystem.MinMaxGradient(SpellBook.instance.equipment[index].SpellColor);
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

    private void Distant()
    {
        if (isMelle || !Inventory.instance.equipment[1]) return;
        DistantWeapon.SetActive(true);

        Vector2 bowPos = DistantWeapon.transform.position;
        Vector2 direction = (Vector2)cursor - bowPos;
        DistantWeapon.transform.right = direction;
        PlayerStats.stWait = 0;

        BowReady += Inventory.instance.equipment[1].speed * 0.2f * Time.deltaTime;
        if(BowReady <= 2)
        {
            Debug.Log("0");
            if (myBow.sprite != Inventory.instance.equipment[1].sprite) myBow.sprite = Inventory.instance.equipment[1].sprite;
           if(BowReady < 1f)arrowPoint.gameObject.SetActive(false);
           
            
                if (Inventory.instance.ArrowCheaker(Inventory.instance.equipment[1].ArrowId))
                {
                    Debug.Log("1");
                    ArrowIsReady = true;
                    arrowPoint.gameObject.SetActive(true);


                    arrowPoint.localPosition = new Vector3(1.25f, 0, 0);
                    arrowPoint.GetComponent<SpriteRenderer>().sprite = Inventory.instance.equipment[1].arrowSprite;
                }
                else arrowPoint.gameObject.SetActive(false);    
            
        }
        else if (ArrowIsReady)
        {
            if (myBow.sprite != Inventory.instance.equipment[1].sprite) myBow.sprite = Inventory.instance.equipment[1].sprite;
            arrowPoint.localPosition = new Vector3(0.625f,0,0);

            bowCharched = true;
            if (bowCharched)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if(PlayerStats.StaminaLose(Inventory.instance.equipment[1].weight, PlayerStats.stats.strWeight, PlayerStats.Strength))
                    {
                        bowCharched = false;
                        ArrowIsReady = false;
                        BowReady = 0f;
                        Inventory.instance.ArrowUse();
                        Instantiate(Inventory.instance.equipment[1].myArrow, arrowPoint.position, arrowPoint.rotation);
                    }
                }
            }
        }
    }

    private void Move()
    {
        mySprite.eulerAngles = new Vector3(0, 0, 15 * -x);
        rb.velocity = new Vector3(x, y) * speed;
        if(cursor.x < transform.position.x && facingRight)
        {
            Flip();
        }
        else if(cursor.x > transform.position.x && !facingRight)
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
