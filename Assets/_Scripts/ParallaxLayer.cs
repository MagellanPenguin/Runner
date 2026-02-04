using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{

    public float speed;
    public float tileWideth = 18f;
    Vector3 startPos;

    void Start() => startPos = transform.position;
    
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < startPos.x - tileWideth)
        {
            transform.position = new Vector3 (transform.position.x + tileWideth * 2f, transform.position.y, transform.position.z);
        }
    }
}
