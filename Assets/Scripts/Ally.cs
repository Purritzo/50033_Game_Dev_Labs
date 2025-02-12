using UnityEngine;

public class Ally : Entity
{
    public int attackPower = 10;
    public float attackInterval = 0.5f;
    private float timer = 0f;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;
    private bool isInMeleeRange = false;
    public Boss targetBoss;
    public Vector3 startPosition;
    public Animator allyAnimator;
    //public AudioSource allyAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        maxHealth = 100;
        health = maxHealth;
        hpBar.setMaxHealth(maxHealth);
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            if (timer >= attackInterval)
            {
                if (isInMeleeRange)
                {
                    AttackBoss();
                    timer = 0;
                }
            } else 
            {
                timer += Time.deltaTime;
            }
        }
    }

    void FixedUpdate()
    {
        if (!isInMeleeRange)
        {
            Vector3 direction = (targetBoss.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x, direction.y) * moveSpeed;
        }
    }

    void AttackBoss()
    {
        allyAnimator.SetTrigger("Attack");
        targetBoss.TakeDamage(attackPower);
        //Debug.Log(gameObject.name + " attacking Boss with " + attackPower);
    }

    // void playHitSound()
    // {
    //     allyAudio.PlayOneShot(allyAudio.clip);
    // }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            isInMeleeRange = true; // Ensures melee state remains active
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            isInMeleeRange = false;
        }
    }

    
}
