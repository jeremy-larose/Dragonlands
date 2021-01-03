using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("moving");
    private static readonly int MoveX = Animator.StringToHash("moveX");
    private static readonly int MoveZ = Animator.StringToHash("moveZ");
    private static readonly int Attacking = Animator.StringToHash("attacking");
    [SerializeField] private State _state;
    [SerializeField] private Behavior _behavior;

    [SerializeField] private GameObject aggroIndicator;
    public float aggroRadius;
    public float attackRange;
    public float moveSpeed;
    public float attackSpeed;
    public AttackDefinition attackAbility;
    private Animator _animator;
    private Camera _camera;
    private MeshRenderer _meshRenderer;
    private Character _myCharacter;
    private NavMeshAgent _navMesh;
    private Player _player;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;
    private float timeOfLastAttack;

    private void Awake()
    {
        _navMesh = GetComponent<NavMeshAgent>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
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
            _navMesh.isStopped = attackOnCooldown;
            float distanceFromPlayer = Vector3.Distance(transform.position, _player.transform.position);
            bool attackInRange = distanceFromPlayer < attackAbility.range;

            if (!attackOnCooldown && attackInRange)
            {
                timeOfLastAttack = Time.time;
                StartCoroutine(Attack());
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

    public void Hit()
    {
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized *
            Random.Range(4f, 4f);
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

                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance) _roamPosition = GetRoamingPosition();

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

                if (Vector3.Distance(transform.position, _navMesh.destination) < attackRange)
                {
                    Debug.Log($"[NPCController: {name} attacking Player!");
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

                if (Vector3.Distance(transform.position, _navMesh.destination) < attackRange)
                {
                    _state = State.ChasingPlayer;
                    _navMesh.destination = _player.transform.position;
                    aggroIndicator.gameObject.SetActive(true);
                }

                break;
        }
    }

    private IEnumerator Attack()
    {
        Vector3 attackDir = transform.position - _player.transform.position;
        attackDir.Normalize();
        _animator.SetFloat(MoveX, attackDir.x);
        _animator.SetFloat(MoveZ, attackDir.z);
        _animator.SetBool(Attacking, true);
        aggroIndicator.gameObject.SetActive(false);
        var attack = attackAbility.CreateAttack(_myCharacter, _player);
        _player.TakeDamage(attack.Damage);

        yield return new WaitForSeconds(attackSpeed);
        _animator.SetBool(Attacking, false);
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