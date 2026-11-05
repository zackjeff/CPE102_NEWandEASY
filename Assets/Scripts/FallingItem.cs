using UnityEngine;

public class FallingItem : MonoBehaviour
{
    public int points = 1;           // use a negative number for bad items, e.g. -2
    public AudioClip catchSound;

    bool caught;

    void Update()
    {
        if (transform.position.y < -7f) Destroy(gameObject);   // missed, clean up
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (caught || !GameManager.Instance.IsPlaying) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        caught = true;
        GameManager.Instance.AddScore(player.playerNumber, points);
        GameManager.Instance.PlaySfx(catchSound);
        Destroy(gameObject);
    }
}