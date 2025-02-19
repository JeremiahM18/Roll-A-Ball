using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
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
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;

            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";

            if (audioSource && loseSound)
            {
                audioSource.PlayOneShot(loseSound);
                Destroy(gameObject, loseSound.length);
            }
            else
            {
                Destroy(gameObject);

            }

        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            if (audioSource && wallCollisionSound)
            {
                audioSource.PlayOneShot(wallCollisionSound);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText() ;

            if(audioSource && collectSound)
            {
                audioSource.PlayOneShot(collectSound);
            }
        }
    }
}
