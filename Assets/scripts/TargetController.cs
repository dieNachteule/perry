using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float moveSpeed = 5f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector2 movement = new(moveX, moveY);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
