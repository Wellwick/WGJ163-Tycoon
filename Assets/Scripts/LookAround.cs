using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class LookAround : MonoBehaviour
{
    public float floatTime;
    public AnimationCurve moveCurve;
    private float floatingTime, distBetween;
    public Transform currentStar;
    private Vector3 lastPos;
    private Quaternion lastRotation;

    public float maxDist, minDist;
    public float dist;
    private Camera cam;
    private Transform camParent;
    private float fov;
    public AnimationCurve fovSlide;

    private MotionBlur mb = null;

    public GameObject pathPrefab;
    private Path aimPath = null;

    public SystemInfo systemInfo;
    public float focusTime;
    private float currentFocusTime;
    private bool focusSystem;
    private Vector3 focusPos, unfocusPos;

    private void Start() {
        foreach (Transform t in transform) {
            if (t.name == "CamParent") {
                camParent = t;
            }
        }
        cam = FindObjectOfType<Camera>();
        fov = cam.fieldOfView;
        dist = minDist;
        SetDist();
        PostProcessVolume p = cam.GetComponent<PostProcessVolume>();
        p.profile.TryGetSettings(out mb);
        transform.position = new Vector3(0f, 0f, -FindObjectOfType<Universe>().radius);
        focusSystem = false;
        currentFocusTime = 0f;
        focusPos = new Vector3(-2f, 0, -5f);
        camParent.GetComponent<BoxCollider>().enabled = false;
    }

    public void Aim(Star star) {
        // Don't aim at yourself, that would be silly
        if (star == currentStar.GetComponent<Star>() || floatingTime > 0f) {
            return;
        }
        // We may need to setup an aim, but we also might not!
        if (aimPath == null) {
            aimPath = GameObject.Instantiate(pathPrefab).GetComponent<Path>();
        }
        if (!aimPath.IsDestination(star)) {
            aimPath.SetSize(0.7f);
            aimPath.SetupPath(currentStar.GetComponent<Star>(), star, TradeItem.NONE);
        }
    }

    public void GoToStar(GameObject star) {
        // We don't wanna go back to the same star!
        if (star.transform == currentStar || focusSystem) {
            SwapFocus();
            return;
        }
        lastPos = transform.position;
        lastRotation = camParent.transform.rotation;
        currentStar = star.transform;
        floatingTime = floatTime;
        distBetween = Vector3.Distance(lastPos, currentStar.position);
        mb.enabled.value = true;
        if (aimPath != null) {
            Destroy(aimPath.gameObject);
            aimPath = null;
        }
    }

    private void SwapFocus() {
        focusSystem = !focusSystem;
        currentFocusTime = focusTime;
        if (focusSystem) {
            transform.position = currentStar.position;
            floatingTime = 0f;
            unfocusPos = cam.transform.localPosition;
            systemInfo.Show(currentStar.GetComponent<Star>(), focusTime);
            camParent.GetComponent<BoxCollider>().enabled = true;
        } else {
            systemInfo.Hide(focusTime*0.5f);
            camParent.GetComponent<BoxCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!focusSystem) {
            UnfocusedUpdate();
        } else {
            FocusedUpdate();
        }
    }

    private void UnfocusedUpdate() {
        if (currentFocusTime > 0f) {
            currentFocusTime -= Time.deltaTime;
            currentFocusTime = Mathf.Clamp(currentFocusTime, 0f, focusTime);
            float completion = moveCurve.Evaluate(currentFocusTime / focusTime);
            cam.transform.localPosition = Vector3.Lerp(unfocusPos, focusPos, completion);
        } else {
            dist -= Input.mouseScrollDelta.y;
            dist = Mathf.Clamp(dist, minDist, maxDist);
            SetDist();
        }
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
            camParent.transform.LookAt(currentStar);
            Quaternion future = camParent.transform.rotation;
            camParent.transform.rotation = Quaternion.Lerp(future, lastRotation, completed);
            cam.fieldOfView = fov * fovSlide.Evaluate(1.0f - completed);
            if (distBetween < 20f) {
                float dampen = Mathf.Max(0f, (distBetween - 10f) / 10f);
                cam.fieldOfView = Mathf.Lerp(fov, cam.fieldOfView, dampen);
            }
            if (floatingTime == 0f) {
                camParent.transform.LookAt(currentStar);
                cam.fieldOfView = fov;
                mb.enabled.value = false;
            }
        } else if (Input.GetMouseButton(1)) {
            // We're rotating
            camParent.transform.RotateAround(transform.position, camParent.transform.right, Input.GetAxis("Mouse Y"));
            camParent.transform.RotateAround(transform.position, camParent.transform.up, Input.GetAxis("Mouse X"));
        }
    }

    private void FocusedUpdate() {
        if (currentFocusTime > 0f) {
            currentFocusTime -= Time.deltaTime;
            currentFocusTime = Mathf.Clamp(currentFocusTime, 0f, focusTime);
            float completion = moveCurve.Evaluate(currentFocusTime / focusTime);
            cam.transform.localPosition = Vector3.Lerp(focusPos, unfocusPos, completion);
        }
        if (Input.GetMouseButtonDown(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        if (Input.GetMouseButtonUp(1)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetMouseButton(1)) {
            // We're rotating
            camParent.transform.RotateAround(transform.position, camParent.transform.right, Input.GetAxis("Mouse Y"));
            camParent.transform.RotateAround(transform.position, camParent.transform.up, Input.GetAxis("Mouse X"));
        }
    }

    private void SetDist() {
        cam.transform.localPosition = new Vector3(0f,0f, -dist);
    }
}
