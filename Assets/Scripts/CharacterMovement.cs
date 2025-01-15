using System.Collections;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private float _delay = 0.4f;
    [SerializeField] private Transform[] _queuePositions;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private GameObject _reward;
    [SerializeField] private float _queueProcessTime = 5f;
    [SerializeField] private float _rewardDisplayTime = 5f;
    [SerializeField] private int _numberInQueue = 0;

    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _inQueue = true;

    private void OnEnable()
    {
        EventManager.OnMoveQueue += MoveQueueHandler;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        //_agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);
        MoveQueueHandler();

    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
        //Debug.Log(gameObject.name + _agent.remainingDistance);


        /*        if (_numberInQueue == 0)
                {
                    StartCoroutine(ProcessQueueRoutine());
                }

                if (_agent.remainingDistance < _agent.stoppingDistance)
                {
                    StartCoroutine(NewMoveRoutine());
                }
        */
    }

    private IEnumerator NewMoveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);
            _agent.SetDestination(_waypoints[Random.Range(0, _waypoints.Length)].position);

            while (_agent.remainingDistance > _agent.stoppingDistance)
            {
                yield return null;
            }
        }
    }

    private IEnumerator ProcessQueueRoutine()
    {
        float currentTime = 0f;
        float progress = 0f;

        _progressBar.gameObject.SetActive(true);

        while (progress < 1)
        {
            currentTime += Time.deltaTime;
            progress = currentTime / _queueProcessTime;
            _progressBar.value = progress;
            yield return null;
        }

        _progressBar.gameObject.SetActive(false);
        _reward.SetActive(true);
        StartCoroutine(NewMoveRoutine());
        yield return new WaitForSeconds(_rewardDisplayTime);
        _reward.SetActive(false);

        EventManager.MoveQueue();
    }

    private void MoveQueueHandler()
    {
        if (_inQueue)
        {
            _numberInQueue--;
            Debug.Log(gameObject.name + _numberInQueue);
            _agent.SetDestination(_queuePositions[_numberInQueue].position);

            if (_numberInQueue == 0)
            {
                _inQueue = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(gameObject.name + "trigger");
        StartCoroutine(ProcessQueueRoutine());
    }
}
