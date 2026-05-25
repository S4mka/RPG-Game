using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossAttackState : IEnemyState
    {
        private readonly BossBrain boss; private readonly EnemyStateMachine machine; private float nextAttack; private int counter;
        public BossAttackState(BossBrain boss, EnemyStateMachine machine) { this.boss = boss; this.machine = machine; }
        public void Enter() { boss.Agent.isStopped = true; }
        public void Tick()
        {
            if (boss.DistanceToPlayer > boss.AttackDistance * 1.4f) { machine.ChangeState(new BossAggroState(boss, machine)); return; }
            boss.transform.LookAt(new Vector3(boss.Player.position.x, boss.transform.position.y, boss.Player.position.z));
            if (Time.time >= nextAttack)
            {
                counter++;
                if (counter % 3 == 0) machine.ChangeState(new BossStrongAttackState(boss, machine)); else boss.Attack();
                nextAttack = Time.time + boss.AttackDelay;
            }
        }
        public void Exit() { boss.Agent.isStopped = false; }
    }
}
