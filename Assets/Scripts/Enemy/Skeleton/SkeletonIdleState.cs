public class SkeletonIdleState : SkeletonGroundedState
{
  public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBooleanName, _enemy)
  {
  }

  public override void Enter()
  {
    base.Enter();

    stateTimer = enemy.idleTime;
  }

  public override void Exit()
  {
    base.Exit();

    AudioManager.instance.PlaySFX(24, enemy.transform);
  }

  public override void Update()
  {
    base.Update();

    if (stateTimer < 0)
    {
      stateMachine.ChangeState(enemy.moveState);
    }
  }
}
