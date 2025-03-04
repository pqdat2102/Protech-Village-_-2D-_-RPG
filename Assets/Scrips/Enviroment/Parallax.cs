using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float prallaxOffset = -0.15f;

    private Camera cam;
    private Vector2 startCamPos;
    private Vector2 travel => (Vector2)cam.transform.position - startCamPos;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        startCamPos = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = startCamPos + travel * prallaxOffset;
    }



}
