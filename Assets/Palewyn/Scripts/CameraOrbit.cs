using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PaleWyn
{
    public class CameraOrbit : MonoBehaviour
    {
        public bool detatchFromParent = false;
        public Transform target; //traget to orbit around
        public bool hideCursor = true; //is the cursor hidden?
        [Header("Orbit")]
        public Vector3 offset = new Vector3(0, 0, 0); //position offset
        public float xSpeed = 120f; //x orbit speed
        public float ySpeed = 120f; //y orbit soeed
        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;
        public float distanceMin = 0.5f; //min distance to target
        public float distanceMax = 15f; //max distance to target
        [Header("Collision")]
        public bool cameraCollision = true; //is camera collision enabled?
        public float camRadius = 0.3f;//radius of camera collision cast
        public LayerMask ignoreLayers;//layers ignored by collision

        private Vector3 originalOffset;//original offset at start of the game
        private float distance;//current distance
        private float rayDistance = 1000f;//max distance ray can check of collisions
        private float x = 0f; //x degrees of rotation
        private float y = 0f;//y degrees of rotation

        // Use this for initialization
        void Start()
        {
            if (detatchFromParent)
            {
                //Detach camera from parent
                transform.SetParent(null);
            }
            //Set target
            //     target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            //Is the cursor supposed to be hidden?
            if (hideCursor)//if hide cursor is true
            {
                //Lock...
                Cursor.lockState = CursorLockMode.Locked;
                //...hide the Cursor
                Cursor.visible = false;
            }
            //Calculatee original offsetfrom target position
            originalOffset = transform.position - target.position;
            //Set ray distance to current distance magnitude of camera
            rayDistance = originalOffset.magnitude;
            //get camera rotation
            Vector3 angles = transform.eulerAngles;
            //Set X and Y degeres to current camera rotation
            x = angles.y;
            y = angles.x;

        }
        private void FixedUpdate()
        {
            //if a target has been set
            if (target)
            {
                //is camera collision enabled?
                if (cameraCollision)
                {
                    //create a ray staring from targets position and point backwards from camera
                    Ray camRay = new Ray(target.position, -transform.forward);
                    RaycastHit hit;
                    //shoot a sphere in defined ray direction
                    if (Physics.SphereCast(camRay, camRadius, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                    {
                        //set current camera distance to hit objects distance
                        distance = hit.distance;
                        //exit function
                        return;
                    }
                }
                //Set distance to original distance
                distance = originalOffset.magnitude;
            }

        }

        // Update is called once per frame
        void Update()
        {
            //if target has been set
            if (target)
            {
                //rotate the camera baded on Mouse x and Mouse Y inputs
                x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
                y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
                //clamp the angle using the custom "ClampAngle" function
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                //rotate the transform using euler angles (y for x and x for y)
                transform.rotation = Quaternion.Euler(y, x, 0);

            }
        }
        private void LateUpdate()
        {   //if we have a target
            if (target)
            {   //calculate our local offset from offset
                Vector3 localOffset = transform.TransformDirection(offset);
                //reposition camera new position based off distance and offset
                transform.position = (target.position + localOffset) + -transform.forward * distance;
            }
        }
        //Clamps the angle between -360 and +360 degrees and using the
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f)
            {
                angle += 360;
            }
            if (angle > 360f)
            {
                angle -= 360;
            }
            return Mathf.Clamp(angle, min, max);
        }
    }

}