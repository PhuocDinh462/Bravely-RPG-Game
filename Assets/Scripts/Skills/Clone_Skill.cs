using System.Collections;
using UnityEngine;

public class Clone_Skill : Skill
{
  [Header("Clone info")]
  [SerializeField] private GameObject clonePrefab;
  [SerializeField] private float cloneDuration;
  [Space]
  [SerializeField] private bool canAttack;

  [Header("Clone can duplicate")]
  [SerializeField] private bool canDuplicateClone;
  [SerializeField] private float changeToDuplicate;

  [Header("Crystal instead of clone")]
  public bool crystalInsteadOfClone;

  public void CreateClone(Transform _clonePosition, Vector3 _offset)
  {
    if (crystalInsteadOfClone)
    {
      SkillManager.instance.crystal.CreateCrystal();
      return;
    }

    GameObject newClone = Instantiate(clonePrefab);

    newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack,
    _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, changeToDuplicate, player);
  }

  public void CreateCloneWithDelay(Transform _enemyTransform)
  {
    StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0)));
  }

  private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
  {
    yield return new WaitForSeconds(.4f);
    CreateClone(_transform, _offset);
  }
}
