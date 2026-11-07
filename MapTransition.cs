using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] Transform teleportTargetPosition;

    enum Direction { up, down, left, right, teleport };

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FadeTransition(collision.gameObject);

            MapController_Manual.Instance?.HighlightArea(mapBoundary.name);
            MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundary.name);
        }
    }

    async void FadeTransition(GameObject player)
    {
        PauseController.SetPause(true);

        await ScreenFader.Instance.FadeOut();

        confiner.BoundingShape2D = mapBoundary;
        updatePlayerPosition(player);

        await ScreenFader.Instance.FadeIn();

        PauseController.SetPause(false);
    }

    private void updatePlayerPosition(GameObject player)
    {
        if (direction == Direction.teleport)
        {
            player.transform.position = teleportTargetPosition.position;
            return;
        }

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
