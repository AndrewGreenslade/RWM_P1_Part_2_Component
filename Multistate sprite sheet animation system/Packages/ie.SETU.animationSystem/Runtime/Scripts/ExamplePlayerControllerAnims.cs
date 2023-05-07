using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(AnimationManager))]
public class ExamplePlayerControllerAnims : MonoBehaviour
{
    private AnimationManager manager;
    private Rigidbody2D rb2D;
    private SpriteRenderer mySprite;
    private float raycstOffset = 0.025f;
    private float raycstDist = 0.05f;
    private bool jumping = false;

    public float jumpHeight = 2.0f;

    public float speed = 5.0f;
    public Transform raycastPosition;

    private void Start()
    {
        manager = GetComponent<AnimationManager>();
        rb2D = GetComponent<Rigidbody2D>();
        mySprite = GetComponent<SpriteRenderer>();

        GameObject raycasstObj = new GameObject("Raycasy Pos");
        raycastPosition = raycasstObj.transform;
        raycasstObj.transform.parent = transform;
        raycasstObj.transform.position = mySprite.bounds.center;
        raycasstObj.transform.position += new Vector3(0, (-mySprite.bounds.size.y / 2) - raycstOffset, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded())
        {
            jumping = false;
        }

            // if user presses space anytime
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded())
            {
                //if hes not currently playing walking animation
                if (manager.currentAnimation.frameName != "Jumping")
                {
                    manager.ChangeAnimEvent.Invoke("Jumping");
                }

                jumping = true;
                rb2D.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            }
        }

        //if user presses right arrow
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //move character on unitys horizontal axis, so right in this case
            Vector3 direction = transform.right * Input.GetAxis("Horizontal");
            direction.x *= speed;
            direction.y = rb2D.velocity.y;
            rb2D.velocity = direction;
            
            //Unflip the players sprite - just incase he is flipped from moving right
            mySprite.flipX = false;

            //if hes not currently playign walking anim
            if (manager.currentAnimation.frameName != "Walk") 
            {
                manager.ChangeAnimEvent.Invoke("Walking");
            }
        }
        else
        //or if user presses Left arrow
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //move character on unitys horizontal axis, so left in this case
            Vector3 direction = transform.right * Input.GetAxis("Horizontal");
            direction.x *= speed;
            direction.y = rb2D.velocity.y;
            rb2D.velocity = direction;

            //Flip the players sprite
            mySprite.flipX = true;

            //if hes not currently playign walking anim
            if (manager.currentAnimation.frameName != "Walk")
            {
                manager.ChangeAnimEvent.Invoke("Walking");
            }
        }
        else
        {
            //play idle animation if grounded
            if (manager.currentAnimation.frameName != "Idle")
            {
                if (!jumping)
                {
                    manager.ChangeAnimEvent.Invoke("Idle");
                }
            }
        } 
    }

    private bool isGrounded()
    {
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition.position, -Vector2.up, raycstDist);

        // If it hits something...
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
}
