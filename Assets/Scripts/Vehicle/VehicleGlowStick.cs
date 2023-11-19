using UnityEngine;

public class VehicleGlowStick : MonoBehaviour
{
  [SerializeField] private Light light;
  [SerializeField] private float maxIntensity;
  [SerializeField] private float percentDrainPerSecond;
  [SerializeField] private Renderer glowMaterial;
  [SerializeField] private Gradient materialColourGradient;

  private float chargePercent;
  private RaycastHit clickRayInfo = new();

  private void Start()
  {
    chargePercent = 0;
    SetGlowstickIntensity(0);
  }

  private void SetGlowstickIntensity(float intensityFraction)
  {
    light.intensity = maxIntensity * intensityFraction;
    glowMaterial.material.color = materialColourGradient.Evaluate(intensityFraction);
  }

  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      Physics.Raycast(
        Camera.main.transform.position,
        Camera.main.transform.forward,
        out clickRayInfo,
        5);
      if (clickRayInfo.collider != null)
      {
        // TODO can probably replace this object check with a layer or tag or something,
        // so that it doesn't matter what part of the object gets clicked.
        if (clickRayInfo.collider.gameObject == transform.GetChild(1).gameObject)
        {
          float fractionMissing = 1 - (chargePercent / 100);
          chargePercent += 20f * fractionMissing;
          chargePercent = Mathf.Clamp(chargePercent, 0, 100);
          GetComponent<Rigidbody>().AddTorque(Camera.main.transform.forward * 100);
          GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 100);
        }
      }
    }

    if (chargePercent > 0)
    {
      chargePercent -= percentDrainPerSecond * Time.deltaTime;
      SetGlowstickIntensity(chargePercent / 100f);
    }
  }
}
