using TMPro;
using UnityEngine;

public class Blackhole_Hotkey_Controller : MonoBehaviour {
  private SpriteRenderer sr;
  private KeyCode myHotKey;
  private TextMeshProUGUI myText;

  private Transform myEnemy;
  private Blackhole_Skill_Controller blackHole;

  public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, Blackhole_Skill_Controller _myBlackHole) {
    sr = GetComponent<SpriteRenderer>();
    myText = GetComponentInChildren<TextMeshProUGUI>();

    myEnemy = _myEnemy;
    blackHole = _myBlackHole;

    myText.text = _myNewHotKey.ToString();
    myHotKey = _myNewHotKey;
  }

  private void Update() {
    if (Input.GetKeyDown(myHotKey)) {
      blackHole.AddEnemyToList(myEnemy);

      myText.color = Color.clear;
      sr.color = Color.clear;
    }
  }
}
