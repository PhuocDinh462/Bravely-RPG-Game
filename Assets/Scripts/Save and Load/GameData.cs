using System.Collections.Generic;

[System.Serializable]
public class GameData
{
  public int currency;

  public SerializableDictionary<string, bool> skillTree;
  public SerializableDictionary<string, int> inventory;
  public List<string> equipmentId;

  public float lostCurrencyX;
  public float lostCurrencyY;
  public int lostCurrencyAmount;


  public SerializableDictionary<string, bool> checkpoints;
  public string closestCheckpointId;

  public GameData()
  {
    this.lostCurrencyX = 0;
    this.lostCurrencyY = 0;
    this.lostCurrencyAmount = 0;

    this.currency = 0;
    skillTree = new SerializableDictionary<string, bool>();
    inventory = new SerializableDictionary<string, int>();
    equipmentId = new List<string>();

    closestCheckpointId = string.Empty;
    checkpoints = new SerializableDictionary<string, bool>();
  }
}
