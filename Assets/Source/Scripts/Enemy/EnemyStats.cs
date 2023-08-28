using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int enemyHealth;
    public int enemyDamage;
    public int hitPulse;
    public BoxCollider2D enemyCollider;

    private EnemyAI ai;

    [Header("Drop")]
    public int exp;
    public Garant[] garantItems;
    public Chance[] chanceItems;
    private GameObject itemPref;

    [Header("Audio Settings")]
    public AudioSource source;
    public AudioClip clip;

    void Start()
    {
        ai = GetComponent<EnemyAI>();
        source = GetComponent<AudioSource>();   
        itemPref = Resources.Load<GameObject>("Prefabs/Item");
        enemyCollider = GetComponent<BoxCollider2D>();  
    }
    public void SetDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            PlayerController.con.killedEnemy++;
            Die();
        }
    }

    private void Die()
    {
        enemyCollider.enabled = false;  
        ai.patrolSpeed = 0f;
        ai.speed = 0f;
        ai.forceSpeed = 0f;
        source.PlayOneShot(clip);
        Destroy(this.gameObject, 3f);
        for (int i = 0; i < garantItems.Length; i++)
        {
            if(!garantItems[i].item || garantItems[i].count <= 0)
            {
                Debug.Log(garantItems[i] + " :Гарантированный объект под индесом " + i + "объекта " + gameObject + "не указан!");
                continue;
            }
            ItemSettings temp = Instantiate(itemPref, UnityEngine.Random.insideUnitSphere * 1.5f + transform.position, Quaternion.identity).GetComponent<ItemSettings>();
            temp.thisItem = garantItems[i].item;
            temp.count = garantItems[i].count;
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, -0.1f);
        }
        for (int i = 0; i < chanceItems.Length; i++)
        {
            if (!chanceItems[i].item || chanceItems[i].count <= 0)
            {
                Debug.Log(chanceItems[i] + " :Случайный объект под индесом " + i + "объекта " + gameObject + "не указан!");
                continue;
            }
            int random = UnityEngine.Random.Range(1, 101);
            if(random <= chanceItems[i].chance)
            {
                ItemSettings temp = Instantiate(itemPref, UnityEngine.Random.insideUnitSphere * 1.5f + transform.position, Quaternion.identity).GetComponent<ItemSettings>();
                temp.thisItem = chanceItems[i].item;
                temp.count = chanceItems[i].count;
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, -0.1f);
            }
        }
        PlayerStats.stats.AddExp(exp);

    }

   
    [System.Serializable]
    public struct Garant
    {
        public Item item;
        public int count;
    }
    [System.Serializable]
    public struct Chance
    {
        public Item item;
        public int count;
        public float chance;
    }

}
