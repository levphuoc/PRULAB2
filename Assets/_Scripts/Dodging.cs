using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodging : MonoBehaviour
{
    public float speed;
    public Boundary boundary;
    private Rigidbody2D rb;
    private float target;
    public Vector2 startWait;
    public float dodgeValue;
    public Vector2 dodgeTime;
    public Vector2 dodgeWait;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Dodge());
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float dodgingbehavior = Mathf.MoveTowards(rb.velocity.x, target, speed);
        rb.velocity = new Vector2(dodgingbehavior, rb.velocity.y);
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                                    Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax));

    }
    IEnumerator Dodge()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));
        while (true)
        {
            target = Random.Range(1, dodgeValue) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(dodgeTime.x, dodgeTime.y));
            target = 0;
            yield return new WaitForSeconds(Random.Range(dodgeWait.x, dodgeWait.y));
        }

    }

}
