using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;

namespace AdvancedRPG.Gameplay.Boss
{
    public sealed class BossAttackState : IEnemyState
    {
        private readonly BossBrain boss;
        private readonly EnemyStateMachine machine;
        private float nextAttack;
        private int attackCounter;

        public BossAttackState(BossBrain boss, EnemyStateMachine machine)
        {
            this.boss = boss;
            this.machine = machine;
        }

        public void Enter()
        {
            boss.StopAgent();

            nextAttack = 0f;
        }

        public void Tick()
        {
            if (boss.Player == null)
            {
                machine.ChangeState(new BossIdleState(boss, machine));
                return;
            }

            if (boss.DistanceToPlayer > boss.AttackDistance * 1.4f)
            {
                machine.ChangeState(new BossAggroState(boss, machine));
                return;
            }

            LookAtPlayer();

            if (Time.time < nextAttack)
                return;

            attackCounter++;

            if (attackCounter % 3 == 0)
                machine.ChangeState(new BossStrongAttackState(boss, machine));
            else
                boss.Attack();

            nextAttack = Time.time + boss.AttackDelay;
        }

        public void Exit()
        {
            boss.ResumeAgent();
        }

        private void LookAtPlayer()
        {
            Vector3 direction = boss.Player.position - boss.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.001f)
                return;

            boss.transform.rotation = Quaternion.Slerp(
                boss.transform.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * 10f);
        }
    }
}
