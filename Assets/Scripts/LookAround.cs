﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LookAround : MonoBehaviour
{
    public float floatTime;
    public AnimationCurve moveCurve;
    private float floatingTime, distBetween;
    public Transform currentStar;
    private Vector3 lastPos;
    private Quaternion lastRotation, lookDirection;

    public float maxDist, minDist;
    private float dist;
    private Camera cam;
    private float fov;
    public AnimationCurve fovSlide;

    private MotionBlur mb = null;

    private void Start() {
        cam = FindObjectOfType<Camera>();
        cam.transform.localPosition = new Vector3(minDist, 0f);
        fov = cam.fieldOfView;
        dist = minDist;
        PostProcessVolume p = cam.GetComponent<PostProcessVolume>();
        p.profile.TryGetSettings(out mb);
    }

    public void GoToStar(GameObject star) {
        // We don't wanna go back to the same star!
        if (star.transform == currentStar)
            return;
        lastPos = transform.position;
        lastRotation = cam.transform.rotation;
        currentStar = star.transform;
        lookDirection = Quaternion.FromToRotation(cam.transform.forward, currentStar.position - lastPos);
        floatingTime = floatTime;
        distBetween = Vector3.Distance(lastPos, currentStar.position);
        mb.enabled.value = true;
    }

    // Update is called once per frame
    void Update() {
        dist -= Input.mouseScrollDelta.y;
        dist = Mathf.Clamp(dist, minDist, maxDist);
        cam.transform.localPosition = cam.transform.localPosition.normalized * dist;
        if (Input.GetMouseButtonDown(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
            cam.fieldOfView = fov * fovSlide.Evaluate(1.0f - completed);
            if (distBetween < 20f) {
                float dampen = Mathf.Max(0f, (distBetween - 10f) / 10f);
                cam.fieldOfView = Mathf.Lerp(fov, cam.fieldOfView, dampen);
            }
            if (floatingTime == 0f) {
                cam.transform.LookAt(currentStar);
                cam.fieldOfView = fov;
                mb.enabled.value = false;
            }
        } else if (Input.GetMouseButton(1)) {
            // We're rotating
            cam.transform.RotateAround(transform.position, cam.transform.right, Input.GetAxis("Mouse Y"));
            cam.transform.RotateAround(transform.position, cam.transform.up, Input.GetAxis("Mouse X"));
        }
    }
}
