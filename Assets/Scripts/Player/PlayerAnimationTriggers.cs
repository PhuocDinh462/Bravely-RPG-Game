using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour {
  private Player player => GetComponentInParent<Player>();

  private void AnimationTrigger() {
    player.AnimationTrigger();
  }

  private void AttackTrigger() {
    AudioManager.instance.PlaySFX(2, null);

    Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

    foreach (var hit in colliders) {
      if (hit.GetComponent<Enemy>()) {
        EnemyStats _target = hit.GetComponent<EnemyStats>();

        if (_target)
          player.stats.DoDamage(_target);

        ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData)
          weaponData.Effect(_target.transform);
      }
    }
  }

  private void ThrowSword() {
    SkillManager.instance.sword.CreateSwords();
  }
}
