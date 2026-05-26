using AdvancedRPG.Gameplay.Combat;
using AdvancedRPG.Gameplay.Enemies.States;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancedRPG.Gameplay.Boss
{
    [RequireComponent(typeof(NavMeshAgent), typeof(CharacterHealthView))]
    public sealed class BossBrain : MonoBehaviour
    {
        private static readonly int IsRun = Animator.StringToHash("isRun");
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private static readonly int StrongAttackTrigger = Animator.StringToHash("strongAttack");
        private static readonly int IsEnraged = Animator.StringToHash("isEnraged");

        [Header("Target")]
        public Transform Player;

        [Header("Components")]
        public Animator Animator;
        public MeleeHitbox MeleeHitbox;
        public MagicProjectile StrongProjectilePrefab;
        public Transform ProjectilePoint;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private BossElementVisuals elementVisuals;

        [Header("Boss Settings")]
        public float AggroDistance = 15f;
        public float AttackDistance = 3f;
        public int Damage = 30;
        [SerializeField] private bool peacefulUntilHit = true;
        [SerializeField] private float strongAttackMultiplier = 1.7f;
        [SerializeField] private float enragedCooldownMultiplier = 0.5f;

        [Header("Loadouts")]
        [SerializeField] private BossLoadout[] possibleLoadouts;
        [SerializeField] private bool chooseRandomLoadoutOnStart = true;
        [SerializeField] private BossLoadout startLoadout;

        [Header("Compatibility")]
        public LayerMask PlayerMask;

        public NavMeshAgent Agent { get; private set; }
        public CharacterHealthView Health { get; private set; }
        public bool WasHit { get; private set; }
        public BossLoadout CurrentLoadout { get; private set; }
        public BossWeaponType CurrentWeaponType => CurrentLoadout == null ? BossWeaponType.Melee : CurrentLoadout.WeaponType;
        public BossElement CurrentElement => CurrentLoadout == null ? BossElement.Fire : CurrentLoadout.Element;
        public float AttackDelay => GetCurrentAttackDelay();
        public float DistanceToPlayer => Player == null ? float.MaxValue : Vector3.Distance(transform.position, Player.position);
        public bool CanAggro => !peacefulUntilHit || WasHit || DistanceToPlayer <= AggroDistance;

        private EnemyStateMachine machine;
        private float previousHealth;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();
            Health = GetComponent<CharacterHealthView>();

            if (Animator == null)
                Animator = GetComponent<Animator>();

            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            if (elementVisuals == null)
                elementVisuals = GetComponent<BossElementVisuals>();

            machine = new EnemyStateMachine();
        }

        private void OnEnable()
        {
            if (Health != null)
            {
                previousHealth = Health.Health.Current;
                Health.Health.Changed += OnHealthChanged;
                Health.Health.Died += OnDied;
                Health.Damaged += OnDamaged;
            }
        }

        private void Start()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player")?.transform;

            ChooseLoadout();
            machine.ChangeState(new BossIdleState(this, machine));
        }

        private void Update()
        {
            if (Player == null)
                Player = GameObject.FindGameObjectWithTag("Player")?.transform;

            machine.Tick();
            UpdateMoveAnimation();
        }

        private void OnDisable()
        {
            if (Health != null)
            {
                Health.Health.Changed -= OnHealthChanged;
                Health.Health.Died -= OnDied;
                Health.Damaged -= OnDamaged;
            }
        }

        public void Attack()
        {
            if (Animator != null)
                Animator.SetTrigger(AttackTrigger);

            PlaySound(CurrentLoadout == null ? null : CurrentLoadout.AttackSound);

            if (CurrentWeaponType == BossWeaponType.Ranged)
                SpawnProjectile(Damage, DamageType.Magical);
            else
                MeleeHitbox?.Activate(gameObject, Damage);
        }

        public void StrongAttack()
        {
            if (Animator != null)
                Animator.SetTrigger(StrongAttackTrigger);

            PlaySound(CurrentLoadout == null ? null : CurrentLoadout.StrongAttackSound);

            int strongDamage = Mathf.RoundToInt(Damage * strongAttackMultiplier);

            if (CurrentWeaponType == BossWeaponType.Ranged)
                SpawnProjectile(strongDamage, DamageType.Magical);
            else
                MeleeHitbox?.Activate(gameObject, strongDamage);
        }

        public void ForceAggro()
        {
            WasHit = true;
        }

        private void ChooseLoadout()
        {
            if (chooseRandomLoadoutOnStart && possibleLoadouts != null && possibleLoadouts.Length > 0)
                CurrentLoadout = possibleLoadouts[Random.Range(0, possibleLoadouts.Length)];
            else
                CurrentLoadout = startLoadout;

            if (CurrentLoadout == null)
                return;

            Damage = CurrentLoadout.Damage;
            AttackDistance = CurrentLoadout.AttackDistance;

            if (CurrentLoadout.ProjectilePrefab != null)
                StrongProjectilePrefab = CurrentLoadout.ProjectilePrefab;

            elementVisuals?.Apply(CurrentLoadout);
        }

        private float GetCurrentAttackDelay()
        {
            float baseDelay = CurrentLoadout == null ? 1.4f : Mathf.Max(0.1f, CurrentLoadout.AttackDelay);

            if (Health != null && Health.Health.Current / Health.Health.Max < 0.5f)
                return baseDelay * enragedCooldownMultiplier;

            return baseDelay;
        }

        private void SpawnProjectile(int projectileDamage, DamageType damageType)
        {
            if (StrongProjectilePrefab == null || ProjectilePoint == null)
            {
                Debug.LogWarning($"{name}: boss projectile settings are not assigned.");
                return;
            }

            Vector3 direction = ProjectilePoint.forward;
            if (Player != null)
                direction = (Player.position + Vector3.up - ProjectilePoint.position).normalized;

            Quaternion rotation = direction.sqrMagnitude > 0.001f ? Quaternion.LookRotation(direction) : ProjectilePoint.rotation;
            MagicProjectile projectile = Instantiate(StrongProjectilePrefab, ProjectilePoint.position, rotation);
            projectile.Init(gameObject, projectileDamage, damageType);
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
                audioSource.PlayOneShot(clip);
        }

        private void OnDamaged(CharacterHealthView view, DamageData damage)
        {
            WasHit = true;
        }

        private void OnHealthChanged(float current, float max)
        {
            if (current < previousHealth)
                WasHit = true;

            previousHealth = current;

            if (Animator != null)
                Animator.SetBool(IsEnraged, current / max < 0.5f);
        }

        private void OnDied()
        {
            if (Agent != null)
            {
                Agent.isStopped = true;
                Agent.enabled = false;
            }
        }

        private void UpdateMoveAnimation()
        {
            if (Animator == null || Agent == null || !Agent.enabled)
                return;

            Animator.SetBool(IsRun, Agent.velocity.sqrMagnitude > 0.05f);
        }
    }
}
