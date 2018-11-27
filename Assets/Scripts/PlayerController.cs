using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryScreen;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private LineRenderer lr;

    private const float speed = 300f;
    private const float floorDist = .6f;
    private const float jumpForce = 8f;
    private const float deathY = -4f;
    private const float laserTimeRef = 1f;
    private const float laserMax = 1000f;
    private float laserTime;


    private bool hasWon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        laserTime = 0f;

        hasWon = false;
    }

    private void Update()
    {
        laserTime -= Time.deltaTime;
        if (laserTime < 0f)
            lr.enabled = false;
        if (hasWon)
            return;
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, rb.velocity.y);
        if (rb.velocity.x > float.Epsilon)
            sr.flipX = false;
        else if (rb.velocity.x < -float.Epsilon)
            sr.flipX = true;
        if (Mathf.Abs(rb.velocity.x) > float.Epsilon && Mathf.Abs(rb.velocity.y) < float.Epsilon)
            anim.SetBool("IsRunning", true);
        else
            anim.SetBool("IsRunning", false);
        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, floorDist, (1 << 8));
            if (hit.distance > float.Epsilon)
            {
                transform.parent = null;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, ((sr.flipX) ? (-transform.right) : (transform.right)), float.MaxValue, ~(1 << 9));
            lr.enabled = true;
            laserTime = laserTimeRef;
            lr.SetPosition(0, transform.position);
            if (hit.collider != null)
            {
                lr.SetPosition(1, hit.point);
                if (hit.collider.CompareTag("Enemy"))
                    Destroy(hit.collider.gameObject);
            }
            else
                lr.SetPosition(1, new Vector2(((sr.flipX) ? (-1f) : (1f)) * laserMax, transform.position.y));
        }
        if (transform.position.y < deathY)
            Die();
    }

    private void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
            transform.parent = collision.transform;
        else if (collision.collider.CompareTag("Enemy"))
            Die();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
            transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Victory"))
        {
            hasWon = true;
            victoryScreen.SetActive(true);
            rb.velocity = Vector2.zero;
        }
    }
}
