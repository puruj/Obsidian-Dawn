using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{

    private CameraController cameraController;
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private float camSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        cameraController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, camPosition.position, camSpeed * Time.deltaTime);
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
}
