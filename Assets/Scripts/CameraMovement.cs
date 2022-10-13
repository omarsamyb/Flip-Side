using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 cameraPosition = player.position + offset;
        cameraPosition.y = 2;

        if (CompareTag("MainCamera"))
        {
            cameraPosition.x = 0;
            transform.position = cameraPosition;

            //if (GameManager.game.isFlipped)
            //    transform.rotation = Quaternion.Euler(-10, 0, 0);
            //else
            //    transform.rotation = Quaternion.Euler(10, 0, 0);
        }
        else
        {
            cameraPosition.x = -4.99f;
            transform.position = cameraPosition;
        }
    }
}
