using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPos;

    public static Transform CamHolder;

    private void Start()
    {
        CamHolder = transform;
    }

    void Update()
    {
        transform.position = cameraPos.position;    
    }
}
