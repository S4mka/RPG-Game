using AdvancedRPG.Gameplay.Enemies.States;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossIdleState : IEnemyState
    {
        private readonly BossBrain boss;
        private readonly EnemyStateMachine machine;

        public BossIdleState(BossBrain boss, EnemyStateMachine machine)
        {
            this.boss = boss;
            this.machine = machine;
        }

        public void Enter()
        {
            if (boss.Agent != null)
                boss.Agent.isStopped = true;
        }

        public void Tick()
        {
            if (boss.Player == null)
                return;

            if (boss.CanAggro)
                machine.ChangeState(new BossAggroState(boss, machine));
        }

        public void Exit() { }
    }
}
