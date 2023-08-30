using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMagicScript : MonoBehaviour
{
    private Spell mySpell;
    private Rigidbody2D rb;
    private bool isHited = false;

    public void Initialize(Spell newSpell)
    {
        rb = GetComponent<Rigidbody2D>();
        mySpell = newSpell;
        Destroy(gameObject, 3f);
        Debug.Log(mySpell);
    }
    private  void Update()
    {
        if(!isHited || mySpell.isPiercing)
        {
            rb.velocity = transform.right * mySpell.speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Enemy") && !isHited)
        {
            isHited = true;
            if (mySpell.explosionRadius > 0f) Explosion();
            else Hit(collision.GetComponent<EnemyCanDie>());

            if (!mySpell.isPiercing)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                Destroy(gameObject, 0.4f);
            }
        }
    }
 
    private void Explosion()
    {
        Debug.Log("Explosion");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, mySpell.explosionRadius);
        foreach(Collider2D collider in colliders)
        {
            if (collider.tag.Contains("Enemy"))
            {
                Debug.Log("Explosion12");
                Hit(collider.GetComponent<EnemyCanDie>());
            }
        }
    }
    private void Hit(EnemyCanDie candie)
    {
        candie.MagicHit(transform, mySpell);
    }    
}
