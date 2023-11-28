using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Lets the attached object show text when hovered over,
/// and perform an action when clicked. The <see cref="ClickController"/>
/// handles the logic; this just describes an object to be
/// clicked.
/// </summary>
public interface ClickTarget
{
  public string GetMessage();
  public void OnClick();
}

public class StandaloneClickTarget : MonoBehaviour, ClickTarget
{
  [SerializeField] private string hoverMessage;
  [SerializeField] private UnityAction onClick;

  public string GetMessage() => hoverMessage;
  public void OnClick() => onClick?.Invoke();
}