using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFilterSystem : MonoBehaviour
{
  [SerializeField] private List<VehicleFilter> filters;

  private int currentFilterIndex;
  private VehicleFilter currentFilter;

  private void Start()
  {
    currentFilterIndex = 0;
    currentFilter = filters[currentFilterIndex];
  }

  private void Update()
  {
    float noise = Mathf.PerlinNoise1D(Time.time * 0.4f);
    int level = noise > 0.98 ? 5
      : noise > 0.95 ? 4
      : noise > 0.9 ? 3
      : noise > 0.8 ? 2
      : noise > 0.4 ? 1
      : 0;
    currentFilter.SetLevel(level);
  }
}
