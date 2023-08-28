using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController controller;
    private SpriteRenderer render;
    private Color myCol;
    [SerializeField]
    private Color hitColor;


    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        myCol = render.color;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log(collision);
        if (!controller.isHited)
        {
            if(collision.transform.tag == "Enemy")
            {
                Hit(collision.transform.GetComponent<EnemyStats>());
            }
        }
    }
    private void Hit(EnemyStats stats)
    {
        controller.isHited = true;
        int damage = stats.enemyDamage - PlayerStats.PlayerProtection / 2;
        if (damage <= 0) damage = 1;
        PlayerStats.stats.PlayerDamage(damage);
        StartCoroutine(Stun());
        rb.velocity = Vector2.zero;
        if(stats.transform.position.x > transform.position.x)
        {
            rb.AddForce(Vector2.left * stats.hitPulse, ForceMode2D.Impulse);
        }
        else rb.AddForce(Vector2.right * stats.hitPulse, ForceMode2D.Impulse);
    }
    private IEnumerator Stun()
    {
        render.color = hitColor;
        yield return new WaitForSeconds(0.5f);
        render.color = myCol;
        controller.isHited = false;
    }
}
