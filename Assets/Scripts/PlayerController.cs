using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

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

    // Health Variables
    public int lifeCount = 3;
    public GameObject[] lifeIcon;
    private bool isRecovering = false;
    private float recoveryDuration = 1.5f;
    private Animator animator;
    private MeshRenderer meshRenderer;
    private Color originalColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

        if(meshRenderer != null)
        {
            originalColor = meshRenderer.material.color;
        }

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
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));

            if (audioSource && winSound)
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
            loseHealth();

        } else if (collision.gameObject.CompareTag("Wall"))
        {
            if (audioSource && wallCollisionSound) {
                audioSource.PlayOneShot(wallCollisionSound);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            // Spawn particle effect at the pickup's position
            ParticleSystem effect = Instantiate(collectEffect, other.transform.position, Quaternion.identity);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);

            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();

            if (audioSource && collectSound)
            {
                audioSource.PlayOneShot(collectSound);
            }
        } else if (other.gameObject.CompareTag("Heart"))
        {
            other.gameObject.SetActive(false);
            gainHealth();

            if(collectEffect != null)
            {
                ParticleSystem healEffect = Instantiate(collectEffect, transform.position, Quaternion.identity);
                healEffect.Play();
                Destroy(healEffect.gameObject, healEffect.main.duration + healEffect.main.startLifetime.constantMax);
            }
        }

    }

    #region Health & Recover
    private void loseHealth()
    {
        if (isRecovering) return;

        lifeCount--;

        if (lifeCount >= 0 && lifeCount < lifeIcon.Length)
        {
            lifeIcon[lifeCount].SetActive(false);
        }

        if (lifeCount == 0)
        {
            //animator.SetTrigger("Death");
            StartCoroutine(Death());
            return;
        }
        else
        {
            PlaySound(2);
            StartCoroutine(RecoveryFlash());
        }
    }

    private void gainHealth()
    {
        if (lifeCount < lifeIcon.Length)
        {
            PlaySound(2);
            lifeIcon[lifeCount].SetActive(true);
            lifeCount++;
        }
    }

    private IEnumerator RecoveryFlash()
    {
        isRecovering = true;
        float elapsed = 0f;

        while (elapsed < recoveryDuration)
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.color = Color.red;
            }

            yield return new WaitForSeconds(0.1f);

            if (meshRenderer != null)
            {
                meshRenderer.material.color = originalColor;
            }

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.2f;
        }

        if (meshRenderer != null)
        {
            meshRenderer.material.color = originalColor;
        }

        isRecovering = false;
    }
    

    private IEnumerator Death()
    { 
        // Stop movement and disable collider
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        ParticleSystem dEffect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        dEffect.Play();

        Destroy(dEffect.gameObject, dEffect.main.duration + dEffect.main.startLifetime.constantMax);

        if (screenShake != null)
        {
            StartCoroutine(screenShake.Shake(0.3f, 0.2f));
        }

        //yield return new WaitForSeconds(0.5f);

        if(meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        if(loseTextObject != null)
        {
            loseTextObject.SetActive(true);
        }

        if (audioSource && loseSound)
        {
            audioSource.PlayOneShot(loseSound);
        }

        yield return null;
    }

    private void PlaySound(int soundType)
    {
        if (audioSource)
        {
            if (soundType == 1 && loseSound)
            {
                audioSource.PlayOneShot(loseSound);
            }
            else if (soundType == 2 && collectSound)
            {
                audioSource.PlayOneShot(collectSound);
            }
        }
    }
    
    #endregion
}

