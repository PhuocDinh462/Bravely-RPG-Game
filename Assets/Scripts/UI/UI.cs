using System.Collections;
using UnityEngine;

public class UI : MonoBehaviour
{
  [Header("End screen")]
  [SerializeField] private UI_FadeScreen fadeScreen;
  [SerializeField] private GameObject endText;
  [SerializeField] private GameObject restartButton;
  [Space]

  [SerializeField] private GameObject characterUI;
  [SerializeField] private GameObject skillTreeUI;
  [SerializeField] private GameObject craftUI;
  [SerializeField] private GameObject optionUI;
  [SerializeField] private GameObject inGameUI;

  public UI_SkillToolTip skillToolTip;
  public UI_ItemTooltip itemTooltip;
  public UI_StatToolTip statToolTip;
  public UI_CraftWindow craftWindow;

  private void Awake()
  {
    SwitchTo(skillTreeUI); // We need this to assign event on skill tree before we assign events on skill scripts
  }

  void Start()
  {
    SwitchTo(inGameUI);

    itemTooltip.gameObject.SetActive(false);
    statToolTip.gameObject.SetActive(false);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.C))
      SwitchWithKeyTo(characterUI);

    if (Input.GetKeyDown(KeyCode.B))
      SwitchWithKeyTo(craftUI);

    if (Input.GetKeyDown(KeyCode.K))
      SwitchWithKeyTo(skillTreeUI);

    if (Input.GetKeyDown(KeyCode.O))
      SwitchWithKeyTo(optionUI);
  }

  public void SwitchTo(GameObject _menu)
  {

    for (int i = 0; i < transform.childCount; i++)
    {
      bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // We need this to keep fade screen game object active

      if (!fadeScreen)
        transform.GetChild(i).gameObject.SetActive(false);
    }

    if (_menu)
      _menu.SetActive(true);
  }

  public void SwitchWithKeyTo(GameObject _menu)
  {
    if (_menu && _menu.activeSelf)
    {
      _menu.SetActive(false);
      CheckForInGameUI();
      return;
    }

    SwitchTo(_menu);
  }

  private void CheckForInGameUI()
  {
    for (int i = 0; i < transform.childCount; i++)
      if (transform.GetChild(i).gameObject.activeSelf) return;

    SwitchTo(inGameUI);
  }

  public void SwitchOnEndScreen()
  {
    fadeScreen.FadeOut();
    StartCoroutine(EndScreenCoroutine());
  }

  IEnumerator EndScreenCoroutine()
  {
    yield return new WaitForSeconds(1);
    endText.SetActive(true);
    yield return new WaitForSeconds(1.5f);
    restartButton.SetActive(true);
  }

  public void RestartGameButton() => GameManager.instance.RestartScene();
}
