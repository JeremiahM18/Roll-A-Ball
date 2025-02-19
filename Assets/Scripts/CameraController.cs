using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;
    private Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player != null)
        {
            offset = transform.position - player.transform.position;
        }

    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
        }
    }
}
