using UnityEngine;

public class RainbowTrail : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    private float timeElapsed;

    // Speed of color transition
    public float scrollSpeed = 1.0f;

    public float colorOffset = 0.5f;


    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        timeElapsed = 0f;

        // Define the rainbow color gradient
        Gradient gradient = new Gradient();
        gradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.red, 0f),
            new GradientColorKey(new Color(1f, 0.5f, 0f), 0.16f), //Orange
            new GradientColorKey(Color.yellow, 0.33f),
            new GradientColorKey(Color.green, 0.5f),
            new GradientColorKey(Color.cyan, 0.66f),
            new GradientColorKey(Color.blue, 0.83f),
            new GradientColorKey(new Color(0.5f, 0f, 1f), 1f) //Purple
        };

        // Set the gradient on the TrailRenderer
        trailRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timeElapsed based on the frame time
        timeElapsed += Time.deltaTime * scrollSpeed;

        float normalizedTime = Mathf.Repeat(timeElapsed, 1f); ;

        Color currentColor = trailRenderer.colorGradient.Evaluate(normalizedTime);
        Color endColor = trailRenderer.colorGradient.Evaluate(Mathf.Repeat(normalizedTime + colorOffset, 1f));

        trailRenderer.startColor = currentColor;
        trailRenderer.endColor = endColor;
    }
}
