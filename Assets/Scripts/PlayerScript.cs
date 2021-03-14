using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public Text score;
    public Text livesText;
    private int scoreValue = 0;
    private int lives;
    public Text winText;
    Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform GroundCheck;
    public float checkRadius;
    public LayerMask AllGround;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives = 3;
        SetLivesText ();
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (Input.GetKeyDown(KeyCode.D))
            {
                anim.SetInteger("State", 1); 
            }
        if (Input.GetKeyUp(KeyCode.D))
            {
                anim.SetInteger("State", 0);
            }
        if (Input.GetKeyDown(KeyCode.A))
            {
                anim.SetInteger("State", 1); 
            }
        if (Input.GetKeyUp(KeyCode.A))
            {
                anim.SetInteger("State", 0);
            }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 3);
        }
        if (isOnGround == false)
            {
                anim.SetInteger("State", 3);               
            }
        if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
        else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        else if (hozMovement == 0 && isOnGround == true)
            {
                anim.SetInteger("State", 0); 
            }
         isOnGround = Physics2D.OverlapCircle(GroundCheck.position, checkRadius, AllGround);
    }
        private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
       {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if (scoreValue >= 8)
                {
                    winText.text = "You Win! Game by Phoenix Rogers.";
                    musicSource.clip = musicClipTwo;
                    musicSource.Play();
                    musicSource.loop = false;
                }
            if (scoreValue == 4)
                {
                    transform.position = new Vector3(50.0f, 0.5f, 0.0f);
                    lives = 3; 
                    SetLivesText ();                    
                }

            }
        else if (collision.collider.tag == "enemy")
     {
          Destroy(collision.collider.gameObject);
          lives = lives - 1;  
          SetLivesText();
     }
    
    }
        void SetLivesText()
            {
                livesText.text = "Lives: " + lives.ToString ();
                    if (lives == 0)
                {
                        winText.text = "You Lose! Game by Phoenix Rogers";
                        Destroy(this);
                }
                }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
                {
                    rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                }
        }
    }
    void Flip()
   {
     facingRight = !facingRight;
     Vector2 Scaler = transform.localScale;
     Scaler.x = Scaler.x * -1;
     transform.localScale = Scaler;
   }
}