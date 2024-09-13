using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public BoxCollider2D BoundsBox; // Defines the boundaries within which the camera can move

    private PlayerController playerController; // Reference to the player's controller

    private float halfHeight; // Half of the camera's height, used to calculate boundaries
    private float halfWidth;  // Half of the camera's width, derived from height and aspect ratio

    // Initialize camera settings and player reference at the start of the game
    void Start()
    {
        // Find the player's controller to track its position
        playerController = FindObjectOfType<PlayerController>();

        // Calculate half of the camera's dimensions based on the current aspect ratio and orthographic size
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        // Play the background level music when the scene starts
        AudioManager.Instance.PlayLevelMusic();
    }

    // Camera movement logic, executed every frame
    void Update()
    {
        // Ensure the camera follows the player's position, while keeping it within defined boundaries
        if (playerController != null)
        {
            transform.position = new Vector3(
                // Clamp the camera's X position within the horizontal bounds of the scene
                Mathf.Clamp(playerController.transform.position.x, BoundsBox.bounds.min.x + halfWidth, BoundsBox.bounds.max.x - halfWidth),

                // Clamp the camera's Y position within the vertical bounds of the scene
                Mathf.Clamp(playerController.transform.position.y, BoundsBox.bounds.min.y + halfHeight, BoundsBox.bounds.max.y - halfHeight),

                // Maintain the camera's Z position (typically constant for 2D games)
                transform.position.z);
        }
    }
}
