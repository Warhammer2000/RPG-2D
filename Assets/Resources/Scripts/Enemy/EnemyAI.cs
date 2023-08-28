using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemieType
    {
        Simple = 0, Big = 1, Boss = 2
    }
    public EnemieType myType = EnemieType.Simple;
    [Header("Move Settings")]
    public float speed;
    public float forceSpeed;
    public float dashCountDown;
    public float patrolSpeed;
    [Header("Radius Settings")]
    public float chasingRadius;
    public float attackRadius;
    public float retreatRadius;
    public float maxX, minX, minY, maxY;

    [Header("Unity Settings")]
    public bool moveRight = false;
    public bool moveStop = false;
    public bool canMove = true;
    //Other
    private Transform target;
    private Rigidbody2D rb;
    private SpriteRenderer mySprite;
    private bool facingRight;
    private bool isForces;
    private float mySpeed;
    private Vector3 startPos;
    private Vector3 movePos;
    private bool GameStarted;
    private void Start()
    {
        GameStarted = true;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
        mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mySpeed = speed;
        movePos = new Vector2(startPos.x + Random.Range(-minX, maxX), startPos.y + Random.Range(-minY, maxY));
    }
    private void FixedUpdate()
    {
        if (canMove) AiCheacker();
    }

    private void AiCheacker()
    {
        if (myType == EnemieType.Simple) Searching();
    }

    private void Searching()
    {
        if (Vector2.Distance(transform.position, target.position) <= chasingRadius)
        {
            Chashing();
        }
        else Partrol();
    }

    private void Partrol()
    {

        Flip(movePos);

        Vector2 temp = Vector2.MoveTowards(transform.position, movePos, patrolSpeed * Time.deltaTime);
        rb.MovePosition(temp);

        if (Vector2.Distance(transform.position, movePos) <= patrolSpeed * Time.deltaTime)
        {
            movePos = new Vector2(startPos.x + Random.Range(-minX, maxX), startPos.y + Random.Range(-minY, maxY));
        }
    }

    private void Chashing()
    {
        Flip(movePos);

        Vector2 temp = Vector2.MoveTowards(transform.position, target.position, mySpeed * Time.deltaTime);
        rb.MovePosition(temp);
        if (!isForces) StartCoroutine(Force());
    }
    IEnumerator Force()
    {
        isForces = true;
        mySpeed = 0.5f;
        yield return new WaitForSeconds(0.5f);
        mySpeed = forceSpeed;
        yield return new WaitForSeconds(0.2f);
        rb.AddForce(transform.forward * mySpeed, ForceMode2D.Impulse);
        mySpeed = speed; yield return new WaitForSeconds(dashCountDown);
        isForces = false;
    }
    private void Flip(Vector3 flipTarget)
    {
        if(transform.position.x > flipTarget.x && facingRight)
        {
            Flipper();
        }
        if (transform.position.x < flipTarget.x && !facingRight)
        {
            Flipper();
        }


        void Flipper()
        {
            facingRight = !facingRight;
            Vector3 temp = mySprite.transform.localScale;
            temp.x *= -1;
            mySprite.transform.localScale = temp;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!GameStarted) startPos = transform.position;
        Gizmos.color = Color.green;
        float yMod = (maxY - minY) / 2;
        float xMod = (maxX - minX) / 2;

        Vector3 pos = new Vector2(startPos.x + xMod, startPos.y + yMod);
        Vector3 size = new Vector3(maxX + minX, maxY + minY, 0.1f);
        Gizmos.DrawWireCube(pos, size);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(movePos, 0.25f);
    }
}
