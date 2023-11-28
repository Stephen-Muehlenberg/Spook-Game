#if ENABLE_INPUT_SYSTEM && ENABLE_INPUT_SYSTEM_PACKAGE
#define USE_INPUT_SYSTEM
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Controls;
#endif

using UnityEngine;

/// <summary>
/// Handles first person movement.
/// </summary>
public class FirstPersonVehicleController : MonoBehaviour
{
  class CameraState
  {
    public float yaw;
    public float pitch;

    public void SetFromTransform(Transform t)
    {
      pitch = t.eulerAngles.x;
      yaw = t.eulerAngles.y;
    }

    public void LerpTowards(CameraState target, float rotationLerpPct)
    {
      yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
      pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
    }

    public void UpdateTransform(Transform body, Transform head)
    {
      body.localEulerAngles = new Vector3(0, yaw, 0);
      head.localEulerAngles = new Vector3(pitch, 0, 0);
    }
  }

  CameraState TargetCameraState = new CameraState();
  CameraState InterpolatingCameraState = new CameraState();

  [Header("Movement Settings")]
  [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
  public float boost = 3.5f;

  [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
  public float positionLerpTime = 0.2f;

  [Header("Rotation Settings")]
  [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
  public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

  public float mouseSensitivity = 1f;

  [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
  public float rotationLerpTime = 0.01f;

  [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
  public bool invertY = false;

  [Tooltip("Object rotated left and right.")]
  public Transform body;
  [Tooltip("Object rotated up and down.")]
  public Transform head;

  public void Start()
  {
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
#if UNITY_EDITOR
#else
    if (Input.GetKey(KeyCode.Escape))
    {
      Cursor.visible = !Cursor.visible;
      Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
#endif

    if (Input.GetKey(KeyCode.Q))
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));

    var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

    TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor * mouseSensitivity;
    TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor * mouseSensitivity;
    TargetCameraState.pitch = Mathf.Clamp(TargetCameraState.pitch, -66, 66);

    // Framerate-independent interpolation
    // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
    var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
    InterpolatingCameraState.LerpTowards(TargetCameraState, rotationLerpPct);

    InterpolatingCameraState.UpdateTransform(body, head);
  }

  public void SetRotation(Quaternion rotation)
  {
    InterpolatingCameraState.yaw = rotation.eulerAngles.y;
    InterpolatingCameraState.pitch = rotation.eulerAngles.x;
    TargetCameraState.yaw = InterpolatingCameraState.yaw;
    TargetCameraState.pitch = InterpolatingCameraState.pitch;
    InterpolatingCameraState.UpdateTransform(body, head);
  }
}
