using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanDie : MonoBehaviour
{
    [SerializeField]private Color hitColor;
    private Rigidbody2D rb;
    private EnemyStats stats;
    private EnemyAI ai;

    private bool isHited;


    private SpriteRenderer mySprite;
    private Color myColor;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
        stats = GetComponent<EnemyStats>();
        ai = GetComponent<EnemyAI>();

        mySprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        myColor = mySprite.color;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Sword")
        {
            SwordHit();
        }
    }
    private void SwordHit()
    {
        if (!isHited)
        {
            StartCoroutine(HitVisual());
            stats.SetDamage(PlayerStats.PlayerMelDamage);
            Transform player = PlayerController.con.transform;
            rb.velocity = Vector2.zero;

            if(player.position.x > transform.position.x)
            {
                rb.AddForce(Vector2.left * (Inventory.instance.equipment[0].pulse));
            }
            else
            {
                rb.AddForce(Vector2.right * (Inventory.instance.equipment[0].pulse));
            }
        }
    }
    public void MagicHit(Transform spellPos, Spell spell)
    {
        if (!isHited)
        {
            Debug.Log("MagDamage");
            StartCoroutine(HitVisual());
            stats.SetDamage(PlayerStats.PlayerMagDamage + spell.damage);
            rb.velocity = Vector2.zero;

            if (spellPos.position.x > transform.position.x)
            {
                rb.AddForce(Vector2.left * (spell.pulse));
            }
            else
            {
                rb.AddForce(Vector2.right * (spell.pulse));
            }
        }
    }
    public void DisHit(Vector3 arrowPos)
    {
        if (!isHited)
        {
            StartCoroutine(HitVisual());
            stats.SetDamage(PlayerStats.PlayerDisDamage);
            Transform player = PlayerController.con.transform;
            rb.velocity = Vector2.zero;

            if(arrowPos.x > transform.position.x)
            {
                rb.AddForce(Vector2.left * (Inventory.instance.equipment[1].pulse));
            }
            else
            {
                rb.AddForce(Vector2.right * (Inventory.instance.equipment[1].pulse));
            }
        }
    }
    private IEnumerator HitVisual()
    {
        isHited = true;
        ai.canMove = false;
        mySprite.color = hitColor;

        yield return new WaitForSeconds(0.3f);

        isHited=false;
        ai.canMove = true;
        mySprite.color = myColor;
    }
}
