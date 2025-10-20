using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool moveRight = true;

    void Update()
    {
        float moveDirection = moveRight ? 1f : -1f;
        transform.Translate(Vector3.right * moveDirection * speed * Time.deltaTime);
        if (moveRight && transform.position.x > 10f)
        {
            Destroy(gameObject);
        }
        else if (!moveRight && transform.position.x < -10f)
        {
            Destroy(gameObject);
        }
    }
}
