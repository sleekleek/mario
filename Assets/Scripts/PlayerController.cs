using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 70;
    public float upSpeed = 50;
    public float maxSpeed = 18;
    private float initalGravityScale;
    public float jumpFallGravityMultiplier = 12;
    private float originalX;

    public TextMeshProUGUI scoreText;
    private int score = 0;

    private bool onGroundState = true;
    private bool onEnemyState = false;
    private bool faceRightState = true;
    private bool countScoreState = false;

    public Transform enemyLocation;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;

    // Start is called before the first frame update
    void  Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        originalX = transform.position.x;

        initalGravityScale = marioBody.gravityScale;
    }

    void HandleGravity()
    {
        if (onGroundState)
        {
            marioBody.gravityScale = initalGravityScale;
        } else 
        {
            marioBody.gravityScale = initalGravityScale * jumpFallGravityMultiplier;
        }
    }

  // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
      // Dynamic rigidbody
      float moveHorizontal = Input.GetAxis("Horizontal");
      if (Mathf.Abs(moveHorizontal) > 0){
          Vector2 movement = new Vector2(moveHorizontal, 0);
          if (marioBody.velocity.magnitude < maxSpeed)
                  marioBody.AddForce(movement * speed);
      }

      if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
          // Stop
          marioBody.velocity = Vector2.zero;
      }

      if (Input.GetKeyDown("space") && (onGroundState || onEnemyState)) {
          marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
          onGroundState = false;
          countScoreState = true;
      }

      HandleGravity();
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }

        // When jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
                {
                    countScoreState = false;
                    score++;
                    Debug.Log(score);
                }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) 
        {
            Debug.Log("On the Ground!");
            onGroundState = true;
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        }

        if (col.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with Gomba!");
            Time.timeScale = 0;
            score = 0;
            scoreText.text = "Score: " + score.ToString();
            marioBody.MovePosition(new Vector3(originalX, 0, 0));
        }
    }
}
