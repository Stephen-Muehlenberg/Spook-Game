using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleInstructionBook : MonoBehaviour
{
  [SerializeField] private FirstPersonVehicleController camController;
  [SerializeField] private VehicleDriveSystem driveSystem;
  [SerializeField] private VehicleLights lightSystem;
  [SerializeField] private GameObject instructionBookModelCloseUp;
  [SerializeField] private GameObject[] pageText;
  [SerializeField] private GameObject[] blacklightText;

  private bool instructionsOpen;
  private int currentPage;
  private RaycastHit clickRayInfo = new();

  private void Awake()
  {
    CloseInstructions();
    OnCabinLightChanged(true);
  }

  private void Start()
  {
    lightSystem.lightsChanged.AddListener(OnCabinLightChanged);
  }

  private void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      if (instructionsOpen)
      {
        CloseInstructions();
      }
      else
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
          // TODO centralise the click system, so we don't have a dozen different raycasts each frame.
          if (clickRayInfo.collider.gameObject == transform.GetChild(0).gameObject)
          {
            OpenInstructions();
          }
        }
      }
    }

    else if (instructionsOpen)
    {
      if (Input.GetKeyUp(KeyCode.A))
        OpenPreviousPage();
      else if (Input.GetKeyUp(KeyCode.D))
        OpenNextPage();
    }
  }

  private void OpenInstructions()
  {
    instructionsOpen = true;
    camController.enabled = false;
    driveSystem.SetSpeedLevel(0);
    instructionBookModelCloseUp.SetActive(true);
    SetPage(currentPage);
  }

  private void CloseInstructions()
  {
    instructionsOpen = false;
    camController.enabled = true;
    instructionBookModelCloseUp.SetActive(false);
  }

  private void SetPage(int page)
  {
    currentPage = page;
    for (int i = 0; i < pageText.Length; i++)
      pageText[i].SetActive(page == i);
  }

  private void OpenNextPage()
  {
    if (currentPage + 1 < pageText.Length) 
      SetPage(currentPage + 1);
  }

  private void OpenPreviousPage()
  {
    if (currentPage > 0) 
      SetPage(currentPage - 1);
  }

  private void OnCabinLightChanged(bool on)
  {
    for (int i = 0; i < blacklightText.Length; i++)
      blacklightText[i].SetActive(!on);
  }
}
