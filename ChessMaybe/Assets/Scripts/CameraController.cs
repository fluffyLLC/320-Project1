using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform controller;
    public Transform cam;
    float boardLength;
    float moveMod = 30;
    public bool pauseCamRig  = false;
    Vector3 mousePositionPrev;
    Vector3 localRotaion = new Vector3(30,0,0);
    Vector3 localPosition;
    Vector3 localCamPostition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        boardLength = 8 * 1.2f;
        localPosition = new Vector3(boardLength / 2, 0, boardLength / 2);
        controller.position = localPosition;
        localCamPostition = new Vector3(0,0, -14);
        cam.localPosition = localCamPostition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Input.
        if (!pauseCamRig)
        {
            //zoom in and out
            

            if (Input.GetMouseButtonDown(2)|| Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                //print("down");

            }
            else if (Input.GetMouseButtonUp(2) && !Input.GetMouseButton(1) || Input.GetMouseButtonUp(1) && !Input.GetMouseButton(2)) {
                Cursor.lockState = CursorLockMode.Confined;
                //print("up");

            }


            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                localCamPostition.z = localCamPostition.z + Input.mouseScrollDelta.y;//Input.GetAxis("Mouse ScrollWheel");
                localCamPostition.z = Mathf.Clamp(localCamPostition.z, -boardLength * 2, -1);
                //localCamPostition.x = 0;
                //localCamPostition.y = 0;

                cam.localPosition = localCamPostition;

            }else if (Input.GetMouseButton(2))
            {

                localRotaion.x = Mathf.Clamp((Input.GetAxis("Mouse Y") * -1) + localRotaion.x, -100, 100);
                //localRotaion.y = Mathf.Clamp(Input.GetAxis("Mouse Y") + localRotaion.y, -180, 180);


                localRotaion.y += Input.GetAxis("Mouse X");
                //localRotaion.x += Input.GetAxis("Mouse Y");


                controller.rotation = Quaternion.Euler(localRotaion);


            }
            else if (Input.GetMouseButton(1)) {
                localPosition.x = Mathf.Clamp((Input.GetAxis("Mouse X")*moveMod*Time.deltaTime) + localPosition.x, 0, boardLength);
                localPosition.z = Mathf.Clamp(((Input.GetAxis("Mouse Y")*-1)*moveMod*Time.deltaTime) + localPosition.z, 0, boardLength);

                controller.position = localPosition;
            
            }


            //


           // mousePositionPrev = Input.mousePosition;
            //transform.position = new Vector3(target.position.x, target.position.y + 1, target.position.z);
        }
    }
}
