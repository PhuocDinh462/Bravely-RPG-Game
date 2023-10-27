using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
  [SerializeField] private GameObject hotKeyPrefab;
  [SerializeField] private List<KeyCode> keyCodeList;

  public float maxSize;
  public float growSpeed;
  public bool canGrow;

  private List<Transform> targets = new List<Transform>();

  private void Update()
  {
    if (canGrow)
    {
      transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.GetComponent<Enemy>())
    {
      collision.GetComponent<Enemy>().FreezeTime(true);

      CreateHotKey(collision);
    }
  }

  private void CreateHotKey(Collider2D collision)
  {
    if (keyCodeList.Count <= 0)
    {
      Debug.LogWarning("Not enough hot key in a key code list!");
      return;
    }

    GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), quaternion.identity);

    KeyCode choosenKey = keyCodeList[UnityEngine.Random.Range(0, keyCodeList.Count)];
    keyCodeList.Remove(choosenKey);

    Blackhole_Hotkey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_Hotkey_Controller>();

    newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
  }

  public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
