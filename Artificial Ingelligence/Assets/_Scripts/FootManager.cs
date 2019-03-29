using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootManager : MonoBehaviour
{
    public Transform foot;

    void Update() {
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);
        RaycastHit hit;
        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 9)) {
            foot.transform.position = hit.point;
        }
        if (Input.GetMouseButtonDown(0)) {
            Vector3 halfExtends = new Vector3(1f, 1f, 0.5f);
            Collider[] colliders = Physics.OverlapBox(foot.transform.position, halfExtends, Quaternion.identity);

            StartCoroutine(CameraShake(0.15f, 0.4f));
            
            foreach (Collider col in colliders) {
                if (col.CompareTag("Ant")) {
                    Destroy(col.gameObject);
                }
                else if (col.CompareTag("Beetle")) {
                    col.GetComponent<Beetle>().stompedOn = true;
                    col.GetComponent<Beetle>().health -= 1;
                }
            }
        }
    }

    public IEnumerator CameraShake(float duration, float magnitude) {
        Vector3 originalPosition = transform.localPosition;
        float elapstedTime  = 0.0f;

        while (elapstedTime < duration) {
            float x = Random.Range(1f, -1f) * magnitude;
            float y = Random.Range(1f, -1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);
            
            elapstedTime += Time.deltaTime;

            yield return null;
        }
    }
}
