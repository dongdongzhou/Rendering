using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //
    // VARIABLES
    //

    public float turnSpeed = 4.0f; // Speed of camera turning when mouse moves in along an axis
  ///  public float panSpeed = 4.0f;  // Speed of the camera when being panned
  //  public float zoomSpeed = 4.0f; // Speed of the camera going back and forth

    private Vector3 mouseOrigin; // Position of cursor when mouse dragging starts
//    private bool isPanning;      // Is the camera being panned?
    private bool isRotating;     // Is the camera being rotated?
 //   private bool isZooming;      // Is the camera zooming?

    //
    // UPDATE
    //

    private void Update()
    {
        // Get the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isRotating = true;
        }

        // Get the right mouse button
//        if (Input.GetMouseButtonDown(1))
//        {
//            // Get mouse origin
//            mouseOrigin = Input.mousePosition;
//            isPanning = true;
//        }

        // Get the middle mouse button
//        if (Input.GetMouseButtonDown(2))
//        {
//            // Get mouse origin
//            mouseOrigin = Input.mousePosition;
//            isZooming = true;
//        }

        // Disable movements on button release
        if (!Input.GetMouseButton(0)) isRotating = false;
   //     if (!Input.GetMouseButton(1)) isPanning = false;
   //     if (!Input.GetMouseButton(2)) isZooming = false;

        // Rotate camera along X and Y axis
        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(Vector3.zero, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(Vector3.zero, Vector3.up, pos.x * turnSpeed);
        }

        // Move the camera on it's XY plane
//        if (isPanning)
//        {
//            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//
//            Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
//            transform.Translate(move, Space.Self);
//        }

        // Move the camera linearly along Z axis
//        if (isZooming)
//        {
//            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);
//
//            Vector3 move = pos.y * zoomSpeed * transform.forward;
//            transform.Translate(move, Space.World);
//        }


        float ScrollWheelChange = Input.GetAxis("Mouse ScrollWheel"); 
        if (ScrollWheelChange != 0)
        {                                                                               
            float R = ScrollWheelChange * 15;                                           
            float PosX = Camera.main.transform.eulerAngles.x + 90;                      
            float PosY = -1 * (Camera.main.transform.eulerAngles.y - 90);               
            PosX = PosX / 180 * Mathf.PI;                                              
            PosY = PosY / 180 * Mathf.PI;                                               
            float X = R * Mathf.Sin(PosX) * Mathf.Cos(PosY);                            
            float Z = R * Mathf.Sin(PosX) * Mathf.Sin(PosY);                            
            float Y = R * Mathf.Cos(PosX);                                              
            float CamX = Camera.main.transform.position.x;                            
            float CamY = Camera.main.transform.position.y;                             
            float CamZ = Camera.main.transform.position.z;                              
            Camera.main.transform.position = new Vector3(CamX + X, CamY + Y, CamZ + Z); 
        }
    }
}