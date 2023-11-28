using UnityEngine;

public class VehicleInstructionBook : MonoBehaviour
{
  [SerializeField] private bool startGameLookingAtBook;
  [SerializeField] private FirstPersonVehicleController camController;
  [SerializeField] private VehicleDriveSystem driveSystem;
  [SerializeField] private VehicleLights lightSystem;
  [SerializeField] private ClickController clickController;
  [SerializeField] private GameObject instructionBookModelCloseUp;
  [SerializeField] private GameObject[] pageText;
  [SerializeField] private GameObject[] blacklightText;

  private bool instructionsOpen;
  private int currentPage;
  /// <summary>Prevent the book being accidentally closed on the same
  /// frame it was opened.</summary>
  private bool delayAfterOpening;

  private void Awake()
  {
    if (startGameLookingAtBook)
    {
      OpenInstructions();
      camController.SetRotation(Quaternion.Euler(20, -90, 0));
    }
    else
      CloseInstructions();
    OnCabinLightChanged(true);
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
