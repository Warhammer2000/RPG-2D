using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowSpeed = 200f;
    private Rigidbody2D rb;
    private bool isHited = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 8f);
    }
    private void FixedUpdate()
    {
        if (!isHited) rb.velocity = transform.right * arrowSpeed * Time.deltaTime;
        else if (rb) rb.velocity *= 0;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag != "Player" && !isHited)
        {
            isHited = true;
            if(collision.transform.tag == "Enemy")
            {
                Debug.Log("hit to enemy");
                collision.transform.GetComponent<EnemyCanDie>().DisHit(transform.position);
                Destroy(rb);
                transform.SetParent(collision.transform);
            }
        }
    }
}
