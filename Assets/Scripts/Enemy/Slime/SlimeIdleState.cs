public class SlimeIdleState : SlimeGroundedState {
  public SlimeIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBooleanName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBooleanName, _enemy) {
  }

  public override void Enter() {
    base.Enter();

    stateTimer = enemy.idleTime;
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    if (stateTimer < 0) {
      stateMachine.ChangeState(enemy.moveState);
    }
  }
}
