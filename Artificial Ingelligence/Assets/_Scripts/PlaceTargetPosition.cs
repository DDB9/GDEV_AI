using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceTargetPosition : MonoBehaviour
{
    public LayerMask hitLayers;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, hitLayers)){
                this.transform.position = hit.point;
            }
        }
    }
}
