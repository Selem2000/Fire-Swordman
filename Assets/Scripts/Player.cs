using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 10f;

    [SerializeField]
    private float rollForce = 40f;

    public float m_rollDuration = .008f;
    public float m_rollCurrentTime;
    public bool m_rolling = false;

    [SerializeField]
    private float jumpForce = 10f;

    private float movementX;

    private int currentAttack = 0;
    private float timeSinceAttack = 0.0f;

    private Rigidbody2D mybody;

    private SpriteRenderer sr;

    private Animator anim;
    private string RUN_ANIMATION = "Run";
    private string JUMP_ANIMATION = "jump";
    private string ROLL_ANIMATION = "roll";
    private string HIT_ANIMATION = "take_hit";
    private string DEATH_ANIMATION = "death";
    private string SPICIAL_ATTACK_ANIMATION = "sp_attack";
    private string ATTACK_1_ANIMATION = "attack_1";
    private string ATTACK_2_ANIMATION = "attack_2";
    private string ATTACK_3_ANIMATION = "attack_3";
    private string AIR_ATTACK_ANIMATION = "air_attack";
    private string DEFEND_ANIMATION = "defend";

    private bool isGrounded = true;
    private string GROUND_TAG = "Ground";

    private void Awake()
    {
        mybody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
            anim.SetBool(ROLL_ANIMATION,false);
            m_rolling = false;

        playerAnimate();
        playerMove();
        playerJump();
        playerRoll();
        Attack();

    }

    void playerMove()
    {
        movementX = Input.GetAxisRaw("Horizontal");
        
        transform.position += new Vector3(movementX, 0f, 0f) * moveForce * Time.deltaTime;

    }

    
    void playerRoll()
    {
        if (Input.GetKeyDown(KeyCode.C) && !m_rolling && movementX!=0)
        {
            m_rolling = true;
            anim.SetBool(ROLL_ANIMATION,true);
           // mybody.velocity = new Vector2(movementX * rollForce, mybody.velocity.y);
        }
    }


void playerJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            mybody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
            isGrounded = false;
        }
    }


    void playerAnimate()
    {
        if (movementX > 0)
        {
            anim.SetBool(RUN_ANIMATION, true);
            sr.flipX = false;
        }
        else if (movementX < 0)
        {
            anim.SetBool(RUN_ANIMATION, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(RUN_ANIMATION, false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !m_rolling)
        {
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 3)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            anim.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }
        else if(Input.GetMouseButtonDown(1)&& !m_rolling)
        {
            anim.SetTrigger("spAttack" );
        }
    }
         


private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GROUND_TAG))
        {
            isGrounded = true;
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }
}
