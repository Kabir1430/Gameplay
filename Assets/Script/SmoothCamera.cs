using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target; // Player's transform to follow

   // public float rotationSpeed = 5f; // Rotation speed

    private Vector3 velocity = Vector3.zero;

    [Header("Follow State")]

    public RagdollEnabler enabler;

    [Header("Follow State")]
    public Vector3 offset = new Vector3(-8.8f, 6f, 0); // Offset from the target
    
    public float smoothTime = 0.3f; // Smoothing time for position
    
    [Header("Big View State")]
    public Vector3 Big_offset = new Vector3(-6f, 6f, 8); // Offset from the target
    public Transform Angle_offset;
    public float Big_smoothTime = 0.3f; // Smoothing time for position
  
    [Header("Follow State")]
    public Vector3 Obstacle_offset = new Vector3(-8.8f, 6f, 0); // Offset from the target
    
    public float Obstacle_smoothTime = 0.3f; // Smoothing time for position
    


    public CameraState currentState = CameraState.Follow;

    public enum CameraState
    {
        Follow,
        BigViewFollow,
        ObstacleFollow
    }






    void LateUpdate()
    {
        switch (currentState)
        {
            case CameraState.Follow:
        
              Follow();
                break;
            case CameraState.ObstacleFollow:

                ObstaclegView();
                break;
            case CameraState.BigViewFollow:
               BigView();
                //EnterPlayback();
                break;
            
            
        }
        
    }



    void Follow()

    {

        if (enabler.currentIndex== 1 )    
        {
            Vector3 targetPosition = target.position + offset;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            transform.position = smoothPosition;

            // Smoothly rotate the camera to match player's rotation
            Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            //  transform.LookAt(target);

        }

        else if(enabler.currentIndex == 2 || enabler.currentIndex == 3)
            currentState = CameraState.BigViewFollow;
    }


    void BigView()
    {
        if (enabler.currentIndex==2 || enabler.currentIndex ==3 )

        {
            Vector3 Smooth = Vector3.Lerp(offset, Big_offset, Big_smoothTime);
            Vector3 targetPosition = target.position + Smooth;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Big_smoothTime);
            transform.position = smoothPosition;

            // Smoothly rotate the camera to match player's rotation
            Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            transform.LookAt(Angle_offset);
        }
        else if(enabler.currentIndex == 1)


            currentState = CameraState.Follow;
        else if (enabler.currentIndex == 4)
            currentState = CameraState.ObstacleFollow;
    }
    void ObstaclegView()
    {

        if (enabler.currentIndex == 4 || enabler.currentIndex == 5 || enabler.currentIndex == 6)

        {
            Vector3 Smooth = Vector3.Lerp(Big_offset, Obstacle_offset, Obstacle_smoothTime);
            Vector3 targetPosition = target.position + Smooth;
            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Obstacle_smoothTime);
            transform.position = smoothPosition;

            // Smoothly rotate the camera to match player's rotation
            Quaternion targetRotation = Quaternion.LookRotation(target.forward, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);
            transform.LookAt(Angle_offset);
        }
       

         //   currentState = CameraState.Follow;
    }

}