using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    [SerializeField] private GameObject palyerRender;
    [SerializeField] private float speed;
    [SerializeField] private float height;
    [SerializeField] private float distance;
    private Vector3 cameraOffset;
    private Vector3 currentVector;
    [SerializeField] Vector2 min;
    [SerializeField] Vector2 max;

    void Start()
    {
        transform.position = new Vector3(
            palyerRender.transform.position.x,
            palyerRender.transform.position.y + height,
            palyerRender.transform.position.z - distance); 
        transform.rotation = Quaternion.LookRotation(palyerRender.transform.position - transform.position);
    }

    public void setOffset(Vector3 offset)
    {
        if(offset.z < 0)
        {
            cameraOffset = offset * 10;
        }
        if (offset.z > 0)
        {
            cameraOffset = offset * 3;
        } else
        {
            cameraOffset = offset * 8;
        }
    }

    private void cameraMove()
    {
        currentVector = new Vector3(
            palyerRender.transform.position.x + cameraOffset.x,
            palyerRender.transform.position.y + height,
            palyerRender.transform.position.z - distance + cameraOffset.z
            );
        currentVector.x = Mathf.Clamp(currentVector.x, min.x, max.x);
        currentVector.z = Mathf.Clamp(currentVector.z, min.y, max.y);
        transform.position = Vector3.Lerp(transform.position, currentVector, speed*Time.deltaTime);
    }

    void Update()
    {
        cameraMove();
    }
}
