using UnityEngine;

public class Enemy_AnimationTrigger : MonoBehaviour {
  private Enemy enemy => GetComponentInParent<Enemy>();

  private void AnimationTrigger() {
    enemy.AnimationFinishTrigger();
  }

  private void AttackTrigger() {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

    foreach (var hit in colliders) {
      if (hit.GetComponent<Player>()) {
        PlayerStats _target = hit.GetComponent<PlayerStats>();
        enemy.stats.DoDamage(_target);
      }
    }
  }

  private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
  private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
