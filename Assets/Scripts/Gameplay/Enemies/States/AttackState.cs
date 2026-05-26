using UnityEngine;
using AdvancedRPG.Gameplay.Combat;

namespace AdvancedRPG.Gameplay.Enemies.States
{
    public class AttackState : IEnemyState
    {
        private readonly EnemyBrain context;

        public AttackState(EnemyBrain context)
        {
            this.context = context;
        }

        public void Enter()
        {
            context.StopAgent();

            context.PlayAttackAnimation();
        }

        public void Tick()
        {
            if (context.Target == null)
            {
                context.ChangeState(new IdleState(context));
                return;
            }

            if (context.ShouldFlee())
            {
                context.ChangeState(new FleeState(context));
                return;
            }

            float distance = context.DistanceToTarget();
            if (distance > context.AttackDistance)
            {
                context.ChangeState(new AggroState(context));
                return;
            }

            LookAtTarget();

            if (!context.CanAttack())
                return;

            context.MarkAttackTime();
            context.PlayAttackAnimation();

            if (context.IsRanged)
                RangedAttack();
            else
                MeleeAttack();
        }

        public void Exit()
        {
            context.StopAttackAnimation();

            context.ResumeAgent();
        }

        private void LookAtTarget()
        {
            Vector3 direction = context.Target.position - context.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.001f)
                return;

            Quaternion rotation = Quaternion.LookRotation(direction);
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, rotation, Time.deltaTime * 10f);
        }

        private void MeleeAttack()
        {
            if (context.MeleeHitbox == null)
            {
                Debug.LogWarning($"{context.name}: MeleeHitbox is not assigned.");
                return;
            }

            context.MeleeHitbox.Activate(context.gameObject, context.Damage);
        }

        private void RangedAttack()
        {
            if (context.ProjectilePrefab == null)
            {
                Debug.LogWarning($"{context.name}: ProjectilePrefab is not assigned.");
                return;
            }

            if (context.ProjectileSpawnPoint == null)
            {
                Debug.LogWarning($"{context.name}: ProjectileSpawnPoint is not assigned.");
                return;
            }

            Vector3 direction = (context.Target.position + Vector3.up - context.ProjectileSpawnPoint.position).normalized;
            Quaternion rotation = direction.sqrMagnitude > 0.001f
                ? Quaternion.LookRotation(direction)
                : context.ProjectileSpawnPoint.rotation;

            MagicProjectile projectile = Object.Instantiate(
                context.ProjectilePrefab,
                context.ProjectileSpawnPoint.position,
                rotation);

            projectile.Init(context.gameObject, context.Damage, context.DamageType);
        }
    }
}
