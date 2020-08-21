using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAround : MonoBehaviour
{
    public float floatTime;
    public AnimationCurve moveCurve;
    private float floatingTime;
    public Transform currentStar;
    private Vector3 lastPos;
    private Quaternion lastRotation, lookDirection;

    public float maxDist, minDist;
    private Camera cam;

    private void Start() {
        cam = FindObjectOfType<Camera>();
        cam.transform.localPosition = new Vector3(minDist, 0f);
    }

    public void GoToStar(GameObject star) {
        lastPos = transform.position;
        lastRotation = cam.transform.rotation;
        currentStar = star.transform;
        lookDirection = Quaternion.FromToRotation(cam.transform.forward, currentStar.position - lastPos);
        floatingTime = floatTime;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(1)) {
            // We're rotating
            cam.transform.RotateAround(transform.position, cam.transform.right, Input.GetAxis("Mouse Y"));
            cam.transform.RotateAround(transform.position, cam.transform.up, Input.GetAxis("Mouse X"));
        }
        if (floatingTime > 0f) {
            floatingTime -= Time.deltaTime;
            if (floatingTime < 0f) {
                floatingTime = 0f;
            }
            float completed = moveCurve.Evaluate(floatingTime / floatTime);
            transform.position = Vector3.Lerp(lastPos, currentStar.position, 1.0f - completed);
            cam.transform.LookAt(currentStar);
            Quaternion future = cam.transform.rotation;
            cam.transform.rotation = Quaternion.Lerp(future, lastRotation, completed);
            if (floatingTime == 0f) {
                cam.transform.LookAt(currentStar);
            }
        }
    }
}
