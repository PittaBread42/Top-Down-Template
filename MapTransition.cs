using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;

    enum  Direction { up, down, left, right };

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            updatePlayerPosition(collision.gameObject);
        }
    }

    private void updatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.up:
                newPos.y += 2;
                break;

            case Direction.down:
                newPos.y -= 2;
                break;

            case Direction.left:
                newPos.x += 2;
                break;

            case Direction.right:
                newPos.x -= 2;
                break;

        }

       player.transform.position = newPos;
    }
}
