using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject loseTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    // Audio Variables
    public AudioSource audioSource;
    public AudioClip collectSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioClip wallCollisionSound;

    // Particle System
    public ParticleSystem collectEffect;
    public ParticleSystem explosionEffect;
    public ScreenShake screenShake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 16)
        {
            winTextObject.SetActive (true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            
            if(audioSource && winSound)
            {
                audioSource.PlayOneShot(winSound);
            }

        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Stop movement and disable collider
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            Instantiate(explosionEffect, transform.position, Quaternion.identity);

            if (screenShake != null)
            {
                StartCoroutine(screenShake.Shake(0.3f, 0.2f));
            }

            // Display Lose message and play audio
            //winTextObject.gameObject.SetActive(true);
            //winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            loseTextObject.SetActive(true);

            if (audioSource && loseSound)
            {
                audioSource.PlayOneShot(loseSound);
                //Destroy(gameObject, loseSound.length);
            }

            GetComponent<MeshRenderer>().enabled = false;
            this.enabled = false;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if(audioSource && wallCollisionSound) {
                audioSource.PlayOneShot(wallCollisionSound);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Spawn particle effect at the pickup's position
            Instantiate(collectEffect, other.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();

            if(audioSource && collectSound)
            {
                audioSource.PlayOneShot(collectSound);
            }
        }
    }
}
