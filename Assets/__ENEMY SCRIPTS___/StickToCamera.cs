using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToCamera : MonoBehaviour
{
    Camera Cam;

    [SerializeField]
    private Vector3 Offset;

    [SerializeField]
    private Quaternion OffsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 t = Cam.transform.position;

        Vector3 local = new Vector3();
        local += Offset;
        transform.localPosition = local;
        transform.localRotation = OffsetRotation;
        
    
    }
}
