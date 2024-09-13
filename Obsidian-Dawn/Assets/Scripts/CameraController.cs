using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BoxCollider2D BoundsBox;

    private PlayerController playerController;

    private float halfHeight;
    private float halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        AudioManager.Instance.PlayLevelMusic();

    }

    // Update is called once per frame
    void Update()
    {
        if(playerController != null)
        {
            transform.position = new Vector3(
                Mathf.Clamp(playerController.transform.position.x, BoundsBox.bounds.min.x + halfWidth, BoundsBox.bounds.max.x - halfWidth),
                Mathf.Clamp(playerController.transform.position.y, BoundsBox.bounds.min.y + halfHeight, BoundsBox.bounds.max.y - halfHeight), 
                transform.position.z);
        }
    }
}
