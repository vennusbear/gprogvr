using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private Transform player;
    public Vector3 currentTarget;
    float distanceTarget;

    public Transform pointParent;
    private Transform[] points;
    public Transform escapePoint;
    private int destPoint = 0;
    private int prevPoint;
    private float linkCD;

    ThiefCarScript carScript;

    Coroutine currentCoroutine; //coroutine that is currently running.

    public Transform eyeTarget;
    public Transform spawnPos;

    #region //AI 
    public enum BehaviourState { Paused, Idle, Patrol, Spy, Run, Escape, Die}
    [Header("AI Settings")]
    public BehaviourState currentBehaviour; //Used to check what state the AI is currently doing.
    NavMeshAgent agent;
    public float FOV = 110f;
    SphereCollider col;
    [SerializeField] List<GameObject> stolenObject = new List<GameObject>();
    #endregion

    Footsteps soundScript;

    #region //Animator
    public Animator thiefAnim;
    #endregion

    #region //Head
    public HeadLookController headScript;
    #endregion

    #region //GameManager
    GameController gameManager;
    #endregion

    #region //alarm
    AlarmLight alarmController;
    #endregion

    private void Awake()
    {
        AssignDestinationPoints();
        gameManager = FindObjectOfType<GameController>();
        carScript = FindObjectOfType<ThiefCarScript>();
        alarmController = FindObjectOfType<AlarmLight>();
        soundScript = thiefAnim.gameObject.GetComponent<Footsteps>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        headScript = GetComponent<HeadLookController>();
        agent = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        agent.autoTraverseOffMeshLink = false;
    }

    private void Start()
    {
        currentBehaviour = BehaviourState.Paused;
        thiefAnim.gameObject.SetActive(false);
    }

    public IEnumerator StartAI()
    {
        GetComponent<Collider>().enabled = true;
        prevPoint = 0;
        destPoint = 0;
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        StartCoroutine(carScript.LerpCar(carScript.middlePos));
        while (carScript.transform.position.x != carScript.middlePos)
        {
            yield return null;
        }

        thiefAnim.gameObject.SetActive(true);
        currentCoroutine = StartCoroutine(PatrolState());
        while (true)
        {
            if (agent.isOnOffMeshLink && linkCD >= 1.5f)
            {              
                yield return StartCoroutine(Parabola(agent, 1.5f, 1.2f));
                agent.CompleteOffMeshLink();
                linkCD = 0;
            }
            linkCD += Time.deltaTime;
            yield return null;
        }
    }

    void AssignDestinationPoints()
    {
        points = new Transform[pointParent.childCount];

        for( int i = 0; i < points.Length; i ++)
        {
            points[i] = pointParent.GetChild(i);
        }
    }

    void Update()
    {
        switch (currentBehaviour)
        {
            case BehaviourState.Idle:
                headScript.target = currentTarget;
                break;

            case BehaviourState.Run:
                headScript.target = currentTarget;
                break;

            case BehaviourState.Spy:
                headScript.target = eyeTarget.position;
                break;

            default:
                headScript.target = eyeTarget.position;
                break;
        }
    }

    IEnumerator PatrolState()
    {
        currentBehaviour = BehaviourState.Patrol;
        agent.autoBraking = false;
        agent.stoppingDistance = 0;
        agent.speed = 2;
        agent.angularSpeed = 200;
        GotoNextPoint();
        thiefAnim.SetFloat("Speed_f", agent.speed);
        while (currentBehaviour == BehaviourState.Patrol)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.1f)
            {
                if (points[prevPoint].CompareTag("WindowPoint"))
                {
                    currentCoroutine = StartCoroutine(SpyState());
                }

                else
                {
                    GotoNextPoint();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Set the agent to go to the currently selected destination.
        agent.destination = points[destPoint].position;
        prevPoint = destPoint;
        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    IEnumerator SpyState()
    {
        currentBehaviour = BehaviourState.Spy;
        agent.speed = 0;
        agent.angularSpeed = 0;
        thiefAnim.SetFloat("Speed_f", 0);
        float normalizedTime = 0.0f;
        Vector3 currentAngle = agent.transform.eulerAngles;
        while (normalizedTime < 1.0f)
        {
            float angle = Mathf.LerpAngle(currentAngle.y, currentAngle.y - 90, normalizedTime);
            transform.eulerAngles = new Vector3(currentAngle.x, angle, currentAngle.z);
            normalizedTime += Time.deltaTime / 0.5f;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(3);
        currentCoroutine = StartCoroutine(PatrolState());
    }

    IEnumerator RunState(GameObject target)
    {
        currentBehaviour = BehaviourState.Run;
        agent.autoBraking = true;
        agent.stoppingDistance = 1f;
        agent.speed = 3.5f;
        agent.angularSpeed = 500;
        thiefAnim.SetFloat("Speed_f", agent.speed);
        alarmController.alarmOn = true;
        while (currentBehaviour == BehaviourState.Run)
        {
            currentTarget = target.transform.position;
            agent.SetDestination(currentTarget);
            distanceTarget = agent.remainingDistance;
            if (distanceTarget <= agent.stoppingDistance)
            {
                currentCoroutine = StartCoroutine(IdleState(target));
            }

            yield return null;
        }
    }

    IEnumerator IdleState(GameObject target)
    {
        currentBehaviour = BehaviourState.Idle;
        thiefAnim.SetFloat("Speed_f", 0);
        currentTarget = player.position;
        distanceTarget = agent.remainingDistance;
        agent.SetDestination(currentTarget);
        float elapsedTime = 0;
        while (distanceTarget <= agent.stoppingDistance && currentBehaviour == BehaviourState.Idle)
        {
            currentTarget = target.transform.position;
            agent.SetDestination(currentTarget);
            distanceTarget = agent.remainingDistance;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1)
            {
                StartCoroutine(EscapeState(target));
                StopCoroutine(currentCoroutine);
            }

            yield return null;
        }

        currentCoroutine = StartCoroutine(RunState(target));
    }

    IEnumerator EscapeState(GameObject target)
    {
        currentBehaviour = BehaviourState.Escape;
        if (target != null)
        {
            Vector3 startPos = target.transform.position;
            target.GetComponent<Collider>().enabled = false;
            float normalizedTime = 0;
            thiefAnim.SetTrigger("Pick");
            while (normalizedTime < 1.0f)
            {
                target.transform.position = Vector3.Lerp(startPos, transform.position, normalizedTime);
                normalizedTime += Time.deltaTime / 0.5f;
                yield return null;
            }

            target.transform.position = spawnPos.position;
            target.transform.parent = null;
            target.transform.SetParent(transform);
            target.SetActive(false);
        }
        agent.SetDestination(escapePoint.position);
        distanceTarget = agent.remainingDistance;
        agent.speed = 3.5f;
        agent.angularSpeed = 500;
        agent.stoppingDistance = 0;
        thiefAnim.SetFloat("Speed_f", agent.speed);
        while (currentBehaviour == BehaviourState.Escape)
        {
            distanceTarget = agent.remainingDistance;
            if (agent.pathPending)
            {
                yield return null;
            }

            else if (distanceTarget <= agent.stoppingDistance) //when ai reach last point
            {
                alarmController.alarmOn = false;
                if (target != null)
                {
                    target.SetActive(true);
                    stolenObject.Add(target);
                    if (stolenObject.Count >= 3)
                    {
                        thiefAnim.gameObject.SetActive(false);
                        StartCoroutine(carScript.LerpCar(carScript.endPos));
                        foreach (GameObject obj in stolenObject)
                        {
                            stolenObject.Remove(obj);
                            Destroy(obj);
                        }
                        yield return new WaitForSeconds(30);
                        StartCoroutine(StartAI());
                    }
                }

                prevPoint = 0;
                destPoint = 0;
                GetComponent<Collider>().enabled = true;
                StartCoroutine(PatrolState());
            }

            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        Vector3 dir = (endPos - agent.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        thiefAnim.SetTrigger("Jump_b");
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f)
        {
            agent.transform.rotation = lookRotation;
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }

    IEnumerator TargetSpotted(Collider other)
    {
        yield return new WaitForSeconds(1);
        if (other.gameObject != null)
        {
            currentTarget = other.gameObject.transform.position;
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(RunState(other.gameObject));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentBehaviour == BehaviourState.Spy)
        {
            if (other.gameObject.GetComponent<Food>() != null)
            {
                if (other.gameObject.GetComponent<Food>().currentState == Food.FoodState.Cooked) //check if it's cooked
                {
                    Vector3 eyePos = transform.position + (transform.up * 1.5f);
                    Vector3 direction = other.transform.position - eyePos;
                    float angle = Vector3.Angle(direction, transform.forward);
                    if (angle < FOV * 0.5f) // check if infront
                    {
                        RaycastHit hit;
                        Debug.DrawRay(eyePos, direction.normalized * col.radius);
                        if (Physics.Raycast(eyePos, direction.normalized, out hit, col.radius)) //check if the enemy can see it
                        {
                            if (hit.collider.gameObject == other.gameObject)
                            {
                                StartCoroutine(TargetSpotted(other));
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude > 1f)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(DeathState());
        }
    }

    IEnumerator DeathState()
    {
        alarmController.alarmOn = false;
        soundScript.Death();
        currentBehaviour = BehaviourState.Die;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Collider>().enabled = false;
        thiefAnim.SetBool("Death_b", true);
        foreach(GameObject obj in stolenObject)
        {
            obj.transform.position = spawnPos.position;
            obj.transform.parent = null;
            obj.GetComponent<Collider>().enabled = true;
            //Rigidbody rb = obj.GetComponent<Rigidbody>();
            //print(rb);
            //rb.AddExplosionForce(3, spawnPos.position, 0.5f, 3f);
        }
        stolenObject.Clear();
        yield return new WaitForSeconds(10);
        thiefAnim.SetBool("Death_b", false);
        yield return null;
        yield return new WaitForSeconds(thiefAnim.GetCurrentAnimatorClipInfo(0).Length);
        GetComponent<NavMeshAgent>().enabled = true;
        StartCoroutine(EscapeState(null));
    }
}
