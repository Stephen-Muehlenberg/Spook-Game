using UnityEngine;

public class VehicleGlowStick : MonoBehaviour
{
  [SerializeField] private Light lightComponent;
  [SerializeField] private float maxIntensity;
  [SerializeField] private float percentDrainPerSecond;
  [SerializeField] private Renderer glowMaterial;
  [SerializeField] private Gradient materialColourGradient;

  private float chargePercent;

  private void Start()
  {
    chargePercent = 0;
    SetGlowstickIntensity(0);
  }

  private void SetGlowstickIntensity(float intensityFraction)
  {
    lightComponent.intensity = maxIntensity * intensityFraction;
    glowMaterial.material.color = materialColourGradient.Evaluate(intensityFraction);
  }

  private void Update()
  {
    if (chargePercent > 0)
    {
      chargePercent -= percentDrainPerSecond * Time.deltaTime;
      SetGlowstickIntensity(chargePercent / 100f);
    }
  }

  public void OnClick()
  {
    float fractionMissing = 1 - (chargePercent / 100);
    chargePercent += 20f * fractionMissing;
    chargePercent = Mathf.Clamp(chargePercent, 0, 100);
    GetComponent<Rigidbody>().AddTorque(Camera.main.transform.forward * 100);
    GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 100);
  }
}
