using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "Linear Velocity: 0\n\nAngular Velocity: 0";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = $"Linear Velocity: {rb.linearVelocity.magnitude}\n\nAngular Velocity: {rb.angularVelocity.magnitude}";
    }
}
