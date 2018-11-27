using UnityEngine;

public class PlatformMouvement : MonoBehaviour
{
    [SerializeField]
    private NextNode currNode;

    private const float speed = 3f;
    private const float distChange = .2f;

    private void Update()
    {
        int x = 0, y = 0;

        Vector2 dist = transform.position - currNode.transform.position;

        if (dist.x < -distChange)
            x = 1;
        else if (dist.x > distChange)
            x = -1;

        if (dist.y < -distChange)
            y = 1;
        else if (dist.y > distChange)
            y = -1;

        if (x == 0 && y == 0)
            currNode = currNode.GetNextNode();
        else
            transform.Translate(new Vector2(x, y) * speed * Time.deltaTime);
    }
}
