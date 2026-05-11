using AdvancedRPG.Gameplay.Enemies.States;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossAggroState : IEnemyState
    {
        private readonly BossBrain boss; private readonly EnemyStateMachine machine;
        public BossAggroState(BossBrain boss, EnemyStateMachine machine) { this.boss = boss; this.machine = machine; }
        public void Enter() { boss.Agent.isStopped = false; }
        public void Tick()
        {
            if (boss.DistanceToPlayer <= boss.AttackDistance) machine.ChangeState(new BossAttackState(boss, machine));
            else boss.Agent.SetDestination(boss.Player.position);
        }
        public void Exit() { }
    }
}
