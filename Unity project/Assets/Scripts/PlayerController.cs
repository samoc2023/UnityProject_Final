using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   
    private Rigidbody playerRb;
    public float gravityModifier;
    public float jumpGravity;
    public float jumpForce;
    public float speed;
    private Animator playerAnim;
    //private CharacterController characterController;
    //private BoxCollider boxCollider;
    private Vector3 moveDirection;
    public bool gameOver;
    public bool isOnGround;
    public bool hasPowerup;

    private float fireRate = 1.0f;
    private float nextFire = 0.0f;






    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityModifier;
        Physics.gravity *= jumpGravity;
        playerAnim = GetComponent<Animator>();
    //characterController = GetComponent<CharacterController>();
    //boxCollider = GetComponent<BoxCollider>();



}

    // Update is called once per frame
    void Update()
    {

        /*if (transform.position.z > -20)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -21);
        }
        */

        //get input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementDirection = new Vector3(horizontalInput * speed, 0);
        //float magnitude = Mathf.Clamp01(moveDirection.magnitude) * speed;
        //movementDirection.Normalize();
        playerRb.MovePosition(transform.position + movementDirection);


        //moveDirection = new Vector3(horizontal, 0, vertical);

        //Animations




        //forwardInput = Input.GetAxis("Vertical");


        //move player forward
        if (movementDirection != Vector3.zero)
        {
            //movementDirection = Vector3.forward * Time.deltaTime * speed;
            playerAnim.SetBool("Static_b", true);

        }

        else if (movementDirection == Vector3.zero)
        {
            playerAnim.SetBool("Static_b", false);
        }




        // While space is pressed - Jump
        if (Input.GetKey(KeyCode.Space) && isOnGround)
        {
            moveDirection = Vector3.zero;
            isOnGround = false;
            playerRb.AddForce(Vector3.up * jumpForce * jumpGravity, ForceMode.Impulse);


            playerAnim.SetTrigger("Jump_trig");
            playerAnim.SetBool("Static_b", false);
        }


        // While shoot button is pressed - reload
        if (Input.GetKey(KeyCode.J) && Time.time > nextFire)
        {

            playerAnim.SetTrigger("Shoot_trig");
            //nextFire = Time.time + fireRate;
            //Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);

        }

    }

   



    private void OnCollisionEnter(Collision collision)
    {
        //DETECTS IF PLAYER IS ON GROUND
        if (collision.gameObject.tag == "Ground")
        {
            isOnGround = true;



        }
        else
            isOnGround= false;

        /*
                if (collision.gameObject.tag != "Ground")
                {
                    isOnGround = false;
                    playerRb.AddForce(Vector3.down  * jumpGravity, ForceMode.Impulse);

                }
        */

        if (!isOnGround)
        {
            playerAnim.SetBool("Static_b", false);

        }


        // if player hits Ground
        else if (collision.gameObject.CompareTag("Death"))
        {
            playerAnim.SetBool("Death_b", true);
            gameOver = true;
            Debug.Log("Game Over!");
            print("gameover"); 
            transform.position = new Vector3(transform.position.x, 40, transform.position.z);
        }

        


        if (collision.gameObject.CompareTag("endIsland")) { OnEndIsland(); }

    }
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.CompareTag("powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            playerRb.AddForce(Vector3.up * 90, ForceMode.Impulse);

        }

        if (other.CompareTag("Enemy"))
        {
            InteractWithEnemy(other.gameObject);
        }
    




        if (other.gameObject.CompareTag("falling"))
        {
            playerRb.AddForce(Vector3.down *3, ForceMode.Impulse);
            print("falling");
        }
    }




    private static void OnEndIsland()
    {
        SceneManager.LoadScene(2);
    }

    void InteractWithEnemy(GameObject enemy)
    {
        // Interact with the enemy (you can define this method in your EnemyController script)
        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.TakeDamage(10); // Example: reduce enemy health by 10
        }
    }
}

