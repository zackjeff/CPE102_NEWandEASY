using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] goodItems;
    public GameObject[] badItems;
    [Range(0f, 1f)] public float badChance = 0.3f;   // 30% of drops are bad

    public float xRange = 7f;           // spawn between -7 and 7 on X
    public float spawnY = 6f;           // just above the top of the screen

    public float startInterval = 1f;    // seconds between drops at the start...
    public float endInterval = 0.35f;   // ...and at the end
    public float startGravity = 0.25f;  // fall speed at the start...
    public float endGravity = 0.8f;     // ...and at the end

    float timer;

    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            float t = GameManager.Instance.Progress;
            Spawn(Mathf.Lerp(startGravity, endGravity, t));
            timer = Mathf.Lerp(startInterval, endInterval, t);
        }
    }

    void Spawn(float gravity)
    {
        GameObject[] list = Random.value < badChance ? badItems : goodItems;
        GameObject prefab = list[Random.Range(0, list.Length)];

        Vector3 pos = new Vector3(Random.Range(-xRange, xRange), spawnY, 0f);
        GameObject item = Instantiate(prefab, pos, Quaternion.identity);
        item.GetComponent<Rigidbody2D>().gravityScale = gravity;
    }
}