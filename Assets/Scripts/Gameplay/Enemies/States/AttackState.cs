using UnityEngine;

namespace AdvancedRPG.Gameplay.Enemies.States
{
    public sealed class AttackState : IEnemyState
    {
        private readonly EnemyBrain enemy; private readonly EnemyStateMachine machine; private float nextAttack;
        public AttackState(EnemyBrain enemy, EnemyStateMachine machine) { this.enemy = enemy; this.machine = machine; }
        public void Enter() { enemy.Agent.isStopped = true; nextAttack = 0f; }
        public void Tick()
        {
            if (enemy.HasLowHp) { machine.ChangeState(new FleeState(enemy, machine)); return; }
            if (enemy.DistanceToPlayer > enemy.AttackDistance * 1.25f) { machine.ChangeState(new AggroState(enemy, machine)); return; }
            enemy.transform.LookAt(new Vector3(enemy.Player.position.x, enemy.transform.position.y, enemy.Player.position.z));
            if (Time.time >= nextAttack) { enemy.Attack(); nextAttack = Time.time + 1.5f; }
        }
        public void Exit() { enemy.Agent.isStopped = false; }
    }
}
