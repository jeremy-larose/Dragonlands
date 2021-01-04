using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    private static readonly int Attack = Animator.StringToHash("Attack");
    [SerializeField] private State _state;
    [SerializeField] private Behavior _behavior;

    [SerializeField] private GameObject aggroIndicator;
    public float aggroRadius;
    public float moveSpeed;
    public float attackSpeed;
    public AttackDefinition attackAbility;
    [SerializeField] private int roamingRadius;
    public AudioClip aggroSound;
    [SerializeField] private GameObject attackTarget;
    [SerializeField] private int activityLevel;

    public Transform spellHotSpot;
    private Animator _animator;
    private Camera _camera;
    private Character _myCharacter;
    private NavMeshAgent _navMesh;
    private Player _player;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;
    private float timeOfLastAttack;

    private void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main;
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        _myCharacter = GetComponent<Character>();
        timeOfLastAttack = float.MinValue;
    }

// Start is called before the first frame update
    private void Start()
    {
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        if (_behavior == Behavior.Wander)
        {
            HandleNPCWander();
        }

        if (_state == State.Attacking)
        {
            float timeSinceLastAttack = Time.time - timeOfLastAttack;
            bool attackOnCooldown = timeSinceLastAttack < attackAbility.cooldown;
            float distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);
            bool attackInRange = distanceFromPlayer < attackAbility.range;

            _navMesh.isStopped = attackOnCooldown;

            if (!attackOnCooldown && attackInRange)
            {
                timeOfLastAttack = Time.time;
                Hit();
            }
        }
    }

    private void FixedUpdate()
    {
        AnimateAndMoveCharacter();
    }

    private void LateUpdate()
    {
        transform.LookAt(_camera.transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aggroRadius);
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized *
            Random.Range(-roamingRadius, roamingRadius);
    }

    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) < aggroRadius)
        {
            aggroIndicator.gameObject.SetActive(true);
            _state = State.ChasingPlayer;
        }
    }

    private void AnimateAndMoveCharacter()
    {
        if (_navMesh.velocity != Vector3.zero)
        {
            _animator.SetBool(Moving, true);
            Vector3 velocity;
            (velocity = _navMesh.velocity).Normalize();

            // Have to pass flipped value here because of LookAt camera in LateUpdate
            _animator.SetFloat(MoveX, -velocity.x);
            _animator.SetFloat(MoveZ, velocity.z);
        }
        else
        {
            _animator.SetBool(Moving, false);
        }
    }

    private void HandleNPCWander()
    {
        switch (_state)
        {
            case State.Roaming:
                _navMesh.destination = _roamPosition;
                var reachedPositionDistance = 4f;

                // Reached the target position radius
                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance)
                    StartCoroutine(FindNextDestination());


                //_roamPosition = GetRoamingPosition();

                FindTarget();
                break;

            case State.ChasingPlayer:
                _navMesh.destination = _player.transform.position;

                if (Vector3.Distance(transform.position, _navMesh.destination) > aggroRadius)
                {
                    aggroIndicator.gameObject.SetActive(false);
                    _roamPosition = _startingPosition;
                    _state = State.Roaming;
                }

                var weapon = GetCurrentWeapon();

                if (Vector3.Distance(transform.position, _navMesh.destination) < weapon.attackDefinition.range)
                {
                    attackTarget = _player.gameObject;
                    _state = State.Attacking;

/*                    if (!GetComponent<Character>().Traits.ContainsKey(Character.CharacterFlags.Pacifist))
                    {
                        Debug.Log( "NPCController: Would have attacked, but is set to pacifist!");
                    }
                    else
                    {
                        StartCoroutine(Attack());
                    } */
                }

                break;

            case State.Attacking:
                if (Vector3.Distance(transform.position, _navMesh.destination) > aggroRadius)
                {
                    aggroIndicator.gameObject.SetActive(false);
                    AudioManager.instance.PlayMusic(GameAssets.i.overworldMusic);
                    _player.isInCombat = false;
                    _roamPosition = _startingPosition;
                    _navMesh.destination = _roamPosition;
                    _state = State.Roaming;
                }

                StartCoroutine(PursueAndAttackTarget());
                //StartCoroutine(PerformAttack());
                break;
        }
    }

    private IEnumerator FindNextDestination()
    {
        _navMesh.isStopped = true;
        var actions = -100 / (1 - activityLevel);
        yield return new WaitForSeconds(actions);
        _navMesh.isStopped = false;
        _roamPosition = GetRoamingPosition();
    }

    private IEnumerator PerformAttack()
    {
        Vector3 attackDir = transform.position - _player.transform.position;
        attackDir.Normalize();
        _animator.SetFloat(MoveX, attackDir.x);
        _animator.SetFloat(MoveZ, attackDir.z);
        _animator.SetBool(Attacking, true);
        aggroIndicator.gameObject.SetActive(false);
        var attack = attackAbility.CreateAttack(_myCharacter, _player);
        //_player.TakeDamage(attack.Damage);

        yield return new WaitForSeconds(attackSpeed);
        _animator.SetBool(Attacking, false);
    }

    private IEnumerator PursueAndAttackTarget()
    {
        _navMesh.isStopped = false;
        var weapon = GetCurrentWeapon();
        while (Vector3.Distance(transform.position, attackTarget.transform.position) > weapon.attackDefinition.range)
        {
            //Debug.Log("Player is out of range of enemy!");
            _navMesh.destination = attackTarget.transform.position;
            yield return null;
        }

        _navMesh.isStopped = true;
        Vector3 attackDir = transform.position - _player.transform.position;
        attackDir.Normalize();
        _animator.SetFloat(MoveX, attackDir.x);
        _animator.SetFloat(MoveZ, attackDir.z);
        _animator.SetBool(Attacking, true);
        aggroIndicator.gameObject.SetActive(false);
        if (_player.isInCombat == false)
        {
            AudioManager.instance.PlayVoice(aggroSound);
            AudioManager.instance.PlayMusic(GameAssets.i.battleMusic);
            _player.isInCombat = true;
        }

        yield return new WaitForSeconds(attackSpeed);
        _animator.SetBool(Attacking, false);
        //_animator.SetTrigger(Attack);
    }

    private Weapon GetCurrentWeapon()
    {
        var weapon = GetComponent<Character>().weapon.GetComponent<Weapon>();
        return weapon;
    }

    public void Hit()
    {
        switch (attackAbility)
        {
            case WeaponAttack weaponAttack:
                weaponAttack.ExecuteAttack(gameObject, _player.gameObject);
                break;
            case Spell spellAttack:
                spellAttack.Cast(gameObject, spellHotSpot.position, _player.GetPosition(), LayerMask.NameToLayer("EnemySpells"));
                break;
        }
    }

    public void SetDestination(Vector3 destination)
    {
        StopAllCoroutines();
        _navMesh.isStopped = false;
        _navMesh.destination = destination;
    }

    public void AttackTarget(GameObject target)
    {
        var weapon = GetCurrentWeapon();
        if (weapon != null)
        {
            StopAllCoroutines();

            _navMesh.isStopped = false;
            attackTarget = target;
            StartCoroutine(PursueAndAttackTarget());
        }
    }

    private enum State
    {
        Roaming,
        ChasingPlayer,
        Attacking
    }

    private enum Behavior
    {
        Idle,
        Wander,
        Patrolling
    }
}