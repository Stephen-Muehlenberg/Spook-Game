using System;
using System.Linq;
using UnityEngine;

public class VehicleInstructionBook : MonoBehaviour
{
  [SerializeField] private GameObject newPagePopup;
  [SerializeField] private FirstPersonVehicleController camController;
  [SerializeField] private VehicleDriveSystem driveSystem;
  [SerializeField] private VehicleLights lightSystem;
  [SerializeField] private ClickController clickController;
  [SerializeField] private GameObject instructionBookModelCloseUp;
  [SerializeField] private InstructionBookPage[] pages;

  private bool instructionsOpen;
  private bool blacklightOn;
  private int currentPage;
  /// <summary>Prevent the book being accidentally closed on the same
  /// frame it was opened.</summary>
  private bool delayAfterOpening;
  private bool delayAfterClosing;

  private void Awake()
  {
    for (int i = 0; i < pages.Length; i++)
      pages[i].Hide();
    CloseInstructions();
    OnCabinLightChanged(true);
    newPagePopup.SetActive(false);
  }

  private void Start()
  {
    lightSystem.cabinLightsChanged.AddListener(OnCabinLightChanged);
  }

  private void Update()
  {
    if (delayAfterOpening)
    {
      delayAfterOpening = false;
      return;
    }
    if (delayAfterClosing)
    {
      delayAfterClosing = false;
      return;
    }

    if (!instructionsOpen)
      return;

    if (Input.GetMouseButtonUp(0))
      CloseInstructions();
    else if (Input.GetKeyUp(KeyCode.A))
      OpenPreviousPage();
    else if (Input.GetKeyUp(KeyCode.D))
      OpenNextPage();
  }

  public void OpenInstructions()
  {
    if (delayAfterClosing) return;

    instructionsOpen = true;
    camController.enabled = false;
    driveSystem.SetSpeedLevel(0);
    instructionBookModelCloseUp.SetActive(true);
    SetPage(currentPage);
    clickController.SetEnabled(false);

    delayAfterOpening = true;
  }

  private void CloseInstructions()
  {
    instructionsOpen = false;
    camController.enabled = true;
    instructionBookModelCloseUp.SetActive(false);
    clickController.SetEnabled(true);
    delayAfterClosing = true;
  }

  private void SetPage(int page)
  {
    if (!pages[page].unlocked)
      Debug.LogWarning($"Attempting to show page {page} but it is not yet unlocked!");

    pages[currentPage].Hide();
    currentPage = page;
    pages[currentPage].Show(blacklight: blacklightOn);

    if (pages.All(it => it.read || !it.unlocked))
      newPagePopup.SetActive(false);
  }

  private void OpenNextPage()
  {
    // Open the next unlocked page.
    for (int i = currentPage + 1; i < pages.Length; i++)
      if (pages[i].unlocked)
      {
        SetPage(i);
        return;
      }
  }

  private void OpenPreviousPage()
  {
    // Open the most recent previous unlocked page.
    for (int i = currentPage - 1; i >= 0; i--)
      if (pages[i].unlocked)
      {
        SetPage(i);
        return;
      }
  }

  private void OnCabinLightChanged(bool on)
  {
    blacklightOn = !on;
    if (instructionsOpen)
      pages[currentPage].Show(blacklight: blacklightOn);
  }

  public void UnlockPage(int pageNumber)
  {
    if (pageNumber < 0 || pageNumber >= pages.Length)
      throw new Exception("Tried to unlock page " + pageNumber + " but it was out of range (0 - " + pages.Length + ").");

    if (pages[pageNumber].unlocked)
      return;

    pages[pageNumber].unlocked = true;
    newPagePopup.SetActive(true);
  }

  public void UnlockPage(string pageName)
  {
    var page = pages.First(it => it.name == pageName);
    if (page.unlocked) return;

    page.unlocked = true;
    newPagePopup.SetActive(true);
  }
}
