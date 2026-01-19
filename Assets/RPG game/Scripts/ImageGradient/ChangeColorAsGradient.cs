using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeColorAsGradient : MonoBehaviour
{
    public Image bgImage;
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public int transitionTime = 2; // Duration in seconds

    private void Start()
    {
        if (bgImage != null)
        {
            StartCoroutine(ChangeColorRoutine());
        }
    }

    private IEnumerator ChangeColorRoutine()
    {
        float elapsed = 0f;

        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            
            // Calculate progress (0.0 to 1.0)
            float t = elapsed / transitionTime;

            // Apply linear interpolation between A and B
            bgImage.color = Color.Lerp(colorA, colorB, t);

            yield return null; // Wait for the next frame
        }

        // Ensure the final color is exactly colorB
        bgImage.color = colorB;
    }
}