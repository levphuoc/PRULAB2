using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{
    public int health;
    public int moneyToGive;
    public GameObject explosion;
    public Color damageColor;
    public bool isDead = false;
    private SpriteRenderer spriteRenderer;
    public GameObject[] dropItem;
    public GameObject starItem;
    private static int chanceToDropItem = 0;
    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public virtual void TakeDamage(int damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                AudioManager.instance.PlaySFX(11);
                if (explosion != null)
                {
                    Instantiate(explosion, transform.position, transform.rotation);
                }

                if (this.GetComponent<PlayerController>() != null)
                {
                    GetComponent<PlayerController>().Respawn();
                }
                else
                {
                    chanceToDropItem += 3;
                    int random = Random.Range(0, 100);

                    // Luôn rơi item star
                    if (starItem != null)
                    {
                        Instantiate(starItem, transform.position, Quaternion.identity);
                    }

                    // Xác suất rơi thêm item khác
                    if (random < chanceToDropItem && dropItem.Length > 0)
                    {
                        GameObject itemToDrop = dropItem[Random.Range(0, dropItem.Length)];
                        if (itemToDrop != null)
                        {
                            Instantiate(itemToDrop, transform.position, Quaternion.identity);
                        }

                        chanceToDropItem = 0;  // Reset lại tỉ lệ rơi item
                    }

                    isDead = true;
                    GameManager.instance.money += moneyToGive;
                    PlayerPrefs.SetInt("Money", GameManager.instance.money);
                    Destroy(gameObject);
                }
            }
            else
            {
                StartCoroutine(TakingDamage());
            }
        }
    }


    IEnumerator TakingDamage()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
