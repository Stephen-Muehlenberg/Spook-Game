using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Standalone instance of a <see cref="ClickTarget"/>
/// that can be attached and arguments supplied in the
/// inspector.
/// </summary>
public class SimpleClickTarget : MonoBehaviour, ClickTarget
{
  [SerializeField] private string hoverMessage;
  [SerializeField] private UnityEvent onClick;

  public string GetMessage() => hoverMessage;
  public void OnClick() => onClick?.Invoke();
}