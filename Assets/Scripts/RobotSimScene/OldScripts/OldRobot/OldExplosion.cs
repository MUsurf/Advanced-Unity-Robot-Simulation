using UnityEngine;

public class OldExplosion : MonoBehaviour
{
    public Rigidbody rb;

    private Vector3 position;

    public float explosionForce = 1000f;
    public GameObject explosionPoint;
    private float timeSinceLastExplode = 0f;

    void Update()
    {
        timeSinceLastExplode += Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.B) && timeSinceLastExplode > 0.1f)
        {
            Explode();
            timeSinceLastExplode = 0f;
        }
    }

    public void Explode()
    {
        position = new Vector3(transform.position.x + RandomFloat(2f, 5f), transform.position.y + RandomFloat(1f, 4f), transform.position.z + RandomFloat(1f, 4f));
        explosionPoint.transform.position = position;
        Invoke("happenExplosion", 0.5f);
    }

    public void happenExplosion()
    {
        rb.AddExplosionForce(explosionForce, position, 0, 0f, ForceMode.Force);
    }

    float RandomFloat(float min, float max)
    {
        float returnedValue = Random.Range(min, max);

        if(Random.Range(0, 2) == 1)
        {
            returnedValue *= -1;
        }

        return returnedValue;
    }
}
