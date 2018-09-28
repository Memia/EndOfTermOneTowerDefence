using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Utilities;
using TowerDefense.Towers.Placement;

namespace PaleWyn
{
    public class TowerPlacement : MonoBehaviour
    {
        public LayerMask hitLayers;
        //public Tower selectedTower;
        // Use this for initialization
        void Start()
        {

        }

        private void OnDrawGizmos()
        {
            //Perform raycast
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(camRay.origin, camRay.origin + camRay.direction * 1000f);
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void FixedUpdate()
        {   //If left moues button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                //Perform raycase
                Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(camRay, out hit, 1000f, hitLayers))
                {
                    //Check if hitting grid
                    IPlacementArea placement = hit.collider.GetComponent<IPlacementArea>();
                    if (placement != null)
                    {
                        //Snap position tower to grid element
                        transform.position = placement.Snap(hit.point, new IntVector2(1, 1));
                    }
                }
            }
        }
    }
}