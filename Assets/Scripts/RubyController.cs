using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    [SerializeField] private int HP;   //other cannot access or modify the HP
    public int currentHP { get => HP; }   //get HP
    public int maxHP;

    public float timeInvincible; // make player won't die in the damage zone immediately
    bool isInvincible;
    float invincibleTimer;

    float hoz;
    float ver;
    [Range(0,20f)]
    public float movementSpeed;
    public Rigidbody2D rb;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public GameObject projectilePrefab;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        hoz = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(hoz, ver);

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        //transform.position += (new Vector3(hoz, ver, 0))*movementSpeed;
        //transform.position = transform.position + new 

        //print("x Input: "+xInput);
        //print("y Input");

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X)) 
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position + Vector2.up*0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
        Vector2 positionToMove = new Vector2(hoz, ver) * movementSpeed * Time.fixedDeltaTime;
        Vector2 newPos = (Vector2)transform.position + positionToMove;
        rb.MovePosition(newPos);
    }

    public void ChangeHP(int value)
    {
        if (value < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        HP += value;
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        if (HP < 0)
        {
            HP = 0;
        }

        HP = Mathf.Clamp(HP, 0, maxHP);
        Debug.Log("player hp is now " + HP);
        UIHealthBar.instance.SetValue(HP / (float)maxHP);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        animator.SetTrigger("Launch");

    }

    public void PlaySound (AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
