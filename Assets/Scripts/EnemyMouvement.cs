using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMouvement : MonoBehaviour
{
    private const float speed = 100f;
    private const float distMin = .6f;
    private Rigidbody2D rb;

    [SerializeField]
    private bool goLeft = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(((goLeft) ? (-1f) : (1f)) * speed * Time.deltaTime, rb.velocity.y);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, ((goLeft) ? (-transform.right) : (transform.right)), distMin, (1 << 8));
        if (hit.distance > float.Epsilon)
            goLeft = !goLeft;
    }
}
