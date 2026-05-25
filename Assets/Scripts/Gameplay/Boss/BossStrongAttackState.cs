using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossStrongAttackState : IEnemyState
    {
        private readonly BossBrain boss; private readonly EnemyStateMachine machine; private float endTime;
        public BossStrongAttackState(BossBrain boss, EnemyStateMachine machine) { this.boss = boss; this.machine = machine; }
        public void Enter() { boss.Agent.isStopped = true; boss.StrongAttack(); endTime = Time.time + boss.AttackDelay; }
        public void Tick() { if (Time.time >= endTime) machine.ChangeState(new BossAttackState(boss, machine)); }
        public void Exit() { }
    }
}
