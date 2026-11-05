using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerNumber = 1;     // 1 or 2
    public Key leftKey = Key.A;
    public Key rightKey = Key.D;
    public Key jumpKey = Key.W;
    public float speed = 8f;
    public float xLimit = 7.4f;      // how far left/right the player can go

    [Header("Jump")]
    public float jumpSpeed = 12f;    // higher = jumps higher
    public float gravity = 30f;      // higher = falls back down faster
    public AudioClip jumpSound;

    float groundY;                   // floor height (where the player starts)
    float velocityY;                 // current up/down speed
    bool grounded = true;

    void Start()
    {
        groundY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        bool canControl = GameManager.Instance.IsPlaying && Keyboard.current != null;

        if (canControl)
        {
            // Move left / right
            float move = 0f;
            if (Keyboard.current[leftKey].isPressed) move -= 1f;
            if (Keyboard.current[rightKey].isPressed) move += 1f;
            pos.x = Mathf.Clamp(pos.x + move * speed * Time.deltaTime, -xLimit, xLimit);

            // Jump - only allowed while standing on the floor
            if (grounded && Keyboard.current[jumpKey].wasPressedThisFrame)
            {
                velocityY = jumpSpeed;
                grounded = false;
                GameManager.Instance.PlaySfx(jumpSound);
            }
        }

        // Gravity - keeps working after time runs out, so nobody gets stuck in the air
        if (!grounded)
        {
            velocityY -= gravity * Time.deltaTime;
            pos.y += velocityY * Time.deltaTime;

            if (pos.y <= groundY)    // landed
            {
                pos.y = groundY;
                velocityY = 0f;
                grounded = true;
            }
        }

        transform.position = pos;
    }
}