using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InstructionBookPage
{
  public string name;
  [SerializeField] private GameObject pageText;
  [SerializeField] private GameObject[] blacklightText;
  public bool unlocked;
  public bool read { get; private set; }
  [SerializeField] private UnityEvent onPageRead;

  public void Hide()
  {
    pageText.SetActive(false);
  }

  public void Show(bool blacklight)
  {
    pageText.SetActive(true);
    for (int i = 0; i < blacklightText.Length; i++)
      blacklightText[i].SetActive(blacklight);

    if (!read)
    {
      read = true;
      onPageRead?.Invoke();
    }
  }
}
