using UnityEngine;

public class GhostPowerDrain : MonoBehaviour
{
  [SerializeField] Transform vehicle;

  private float previousPowerDrained;

  private void Update()
  {
    if (previousPowerDrained > 0)
      VehiclePower.ghostPowerInhibited -= previousPowerDrained;

    var offset = (transform.position - vehicle.position);
    if (offset.sqrMagnitude > 900)
      previousPowerDrained = 0;
    else
    {
      previousPowerDrained = (30 - offset.magnitude) * 2.5f;
      VehiclePower.ghostPowerInhibited += previousPowerDrained;
    }
  }
}
