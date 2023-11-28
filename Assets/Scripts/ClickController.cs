using UnityEngine;

/// <summary>
/// Handles the logic for mousing over objects, showing popup
/// text when you do, and clicking on them.
/// </summary>
public class ClickController : MonoBehaviour
{
  [SerializeField] private GameObject pointerUi;
  [SerializeField] private TMPro.TMP_Text interactionText;

  private bool active;
  private Transform camTransform;
  private bool rayHit;
  private RaycastHit hitInfo;

  private ClickTarget newTarget;
  private ClickTarget currentTarget;

  private void Start()
  {
    camTransform = Camera.main.transform;
    DeselectTarget();
    SetEnabled(true);
  }

  private void Update()
  {
    if (!active) return;

    rayHit = Physics.Raycast(
      origin: camTransform.position,
      direction: camTransform.forward,
      hitInfo: out hitInfo,
      maxDistance: 30);
    // TODO add layer mask.

    if (rayHit == false
      || hitInfo.collider == null
      || hitInfo.collider.GetComponent<ClickTarget>() == null)
    {
      if (currentTarget != null)
        DeselectTarget();
    }
    else
    {
      newTarget = hitInfo.collider.GetComponent<ClickTarget>();
      if (currentTarget != newTarget)
        SelectTarget(newTarget);
    }

    if (Input.GetMouseButtonUp(0) && currentTarget != null)
      currentTarget.OnClick();
  }

  public void SetEnabled(bool enabled)
  {
    active = enabled;
    interactionText.gameObject
      .SetActive(enabled && currentTarget != null);
    pointerUi.SetActive(enabled);
  }

  private void SelectTarget(ClickTarget target)
  {
    currentTarget = target;
    interactionText.gameObject.SetActive(true);
    interactionText.text = target.GetMessage();
  }

  private void DeselectTarget()
  {
    interactionText.gameObject.SetActive(false);
    currentTarget = null;
  }
}
