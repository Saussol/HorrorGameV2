using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using Unity.Netcode;

public enum MonsterState
{
	WANDER,
	CHASE,
	//SCARE
}

public class AIMovement : NetworkBehaviour
{
	[Title("AI Control")]
	[SerializeField] public NavMeshAgent agent;
	[SerializeField] private float range;

	[SerializeField] private float detectionRadius;

	[SerializeField] private float chaseDetectionRange;

	[SerializeField] private Transform centerPoint;

	[Title("Respawn logic")]

	[SerializeField] private Transform playerRespawnPoint;
	[SerializeField] private EndGameTrigger endGame;

	[Title("State Control")]
	[SerializeField] private MonsterState _monsterState;

	[SerializeField] private float minTime, maxTime;

	[SerializeField] private float wanderSpeed, chaseSpeed, scareSpeed;
	public NetworkVariable<float> currentSpeed = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

	float startSearchTimer;
	[SerializeField] float searchTimer;

	private List<CharacterController> players = new List<CharacterController>();
	private CharacterController chasingPlayer;

	bool scareEnd;
	bool chaseEnd;

	bool aiStarted;

	public void StartAI()
	{
		if (!IsOwner) return;

		agent = GetComponent<NavMeshAgent>();
		StartCoroutine(StateTimer());
		players = new List<CharacterController>(FindObjectsOfType<CharacterController>());
		startSearchTimer = searchTimer;
		aiStarted = true;
	}

	private void Update()
	{
		if (!IsOwner) return;
		if (!aiStarted) return;

		if (agent.velocity.magnitude > 0)
		{
			Vector3 direction = agent.velocity.normalized;
			direction.y = 0;

			if(direction != Vector3.zero)
			{
				Quaternion lookRotation = Quaternion.LookRotation(direction);
				float angle = Quaternion.Angle(transform.rotation, lookRotation);
				float rotationSpeedFactor = Mathf.Clamp(angle / 180, .1f, 1f);
				//transform.rotation = lookRotation;
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, (Time.deltaTime * rotationSpeedFactor) / 2);
			}
		}

		switch (_monsterState)
		{
			case MonsterState.WANDER:
				if (agent.remainingDistance <= agent.stoppingDistance)
				{
					Vector3 point;
					if (RandomPoint(centerPoint.position, range, out point))
					{
						if(agent.speed != wanderSpeed)
						{
							agent.speed = wanderSpeed;
							currentSpeed.Value = wanderSpeed;
						}

						Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
						agent.SetDestination(point);
					}
				}

				if (startSearchTimer <= 0)
				{
					Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);
					foreach (Collider hit in hitColliders)
					{
						if (hit.GetComponent<CharacterController>() && players.Contains(hit.GetComponent<CharacterController>()))
						{
							StopAllCoroutines();
							_monsterState = MonsterState.CHASE;
							chasingPlayer = hit.GetComponent<CharacterController>();
							agent.speed = chaseSpeed;
							currentSpeed.Value = chaseSpeed;
							StartCoroutine(StateTimer());
						}
					}
				}
				else
				{
					startSearchTimer -= Time.deltaTime;
				}

				break;
			case MonsterState.CHASE:
				if (Vector3.Distance(transform.position, chasingPlayer.transform.position) <= 3)
				{
					if (!chaseEnd)
					{
						//TO DO make some scary shit
						StopAllCoroutines();
						agent.SetDestination(transform.position);
						StartCoroutine(KillPlayer());

						chaseEnd = true;
					}
					break;
				}

				if (chasingPlayer != null)
					agent.SetDestination(chasingPlayer.transform.position);
				break;
			//case MonsterState.SCARE:

			//	if (Vector3.Distance(transform.position, chasingPlayer.transform.position) <= detectionRadius)
			//	{
			//		Debug.Log("near player");

			//		if (!scareEnd)
			//		{
			//			Debug.Log("change scare destination");
			//			scareEnd = true;
			//			//TO DO make some scary shit
			//			StopAllCoroutines();
			//			agent.SetDestination(transform.position + transform.forward * detectionRadius / 1.5f);
			//			StartCoroutine(SetScareDestination(chasingPlayer.transform.position));
			//		}

			//		break;
			//	}

			//	if (chasingPlayer != null)
			//		agent.SetDestination(chasingPlayer.transform.position);

			//	break;
		}
	}

	Vector3 center;
	IEnumerator SetScareDestination(Vector3 playerPos)
	{
		while(agent.remainingDistance > agent.stoppingDistance)
		{
			yield return null;
		}

		agent.speed = chaseSpeed;
		currentSpeed.Value = chaseSpeed;
		center = (centerPoint.position - playerPos).normalized * (range + 10);
		Vector3 point;
		while (!RandomPoint(center, range, out point))
		{
			yield return null;
		}

		agent.SetDestination(point);

		_monsterState = MonsterState.WANDER;
		chasingPlayer = null;
		startSearchTimer = searchTimer;
		StartCoroutine(StateTimer());
		scareEnd = false;
	}

	private IEnumerator KillPlayer()
	{
		Debug.Log("KILL PLAYER");
		players.Remove(chasingPlayer);

		StartScreamerClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { chasingPlayer.GetComponent<NetworkObject>().OwnerClientId } } });

		//TO DO change to killing animation instead of just waiting
		yield return new WaitForSeconds(2f);

		if (players.Count <= 0)
		{
			//chasingPlayer.GetComponent<CharacterMovement>().RatTransformation(playerRespawnPoint.position, true);
			Debug.Log(chasingPlayer.GetComponent<NetworkObject>().OwnerClientId);
			KillPlayerClientRpc(true , new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { chasingPlayer.GetComponent<NetworkObject>().OwnerClientId } } });
			EndGameClientRpc();
		}
		else
		{
			Debug.Log(chasingPlayer.GetComponent<NetworkObject>().OwnerClientId);
			KillPlayerClientRpc(false, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { chasingPlayer.GetComponent<NetworkObject>().OwnerClientId } } });
		}

		_monsterState = MonsterState.WANDER;
		chasingPlayer = null;
		startSearchTimer = searchTimer;
		agent.speed = wanderSpeed;
		currentSpeed.Value = wanderSpeed;
		StartCoroutine(StateTimer());
		chaseEnd = false;
	}

	[ClientRpc]
	private void StartScreamerClientRpc(ClientRpcParams clientRpcParams)
	{
		Debug.Log("Start screamer");
		FindObjectOfType<CharacterMovement>().StartScreamer();
	}

	[ClientRpc]
	private void KillPlayerClientRpc(bool endGame, ClientRpcParams clientRpcParams)
	{
		Debug.Log("I'm dead");
		FindObjectOfType<CharacterMovement>().RatTransformation(playerRespawnPoint.position, endGame);
	}

	[ClientRpc]
	private void EndGameClientRpc()
	{
		endGame.LooseGame();
	}

	private bool RandomPoint(Vector3 center, float range, out Vector3 result)
	{
		Vector3 randomPoint = center + Random.insideUnitSphere * range;
		NavMeshHit hit;
		if(NavMesh.SamplePosition(randomPoint, out hit, 1, NavMesh.AllAreas))
		{
			result = hit.position;
			return true;
		}

		result = Vector3.zero;
		return false;
	}

	private IEnumerator StateTimer()
	{
		float timer = Random.Range(minTime, maxTime);
		Debug.Log($"start timer: {timer}");

		yield return new WaitForSeconds(timer);
		//select new random state that isn't the current state
		_monsterState = SetRandomState();
		switch (_monsterState)
		{
			case MonsterState.WANDER:
				chasingPlayer = null;
				startSearchTimer = searchTimer;
				agent.speed = wanderSpeed;
				currentSpeed.Value = wanderSpeed;
				break;
			case MonsterState.CHASE:
				CharacterController closestPlayer = null;
				float closestDistance = Mathf.Infinity;

				foreach (CharacterController player in players)
				{
					if (Vector3.Distance(transform.position, player.transform.position) < closestDistance)
					{
						closestDistance = Vector3.Distance(transform.position, player.transform.position);
						closestPlayer = player;
					}
				}
				chasingPlayer = closestPlayer;
				agent.speed = chaseSpeed;
				currentSpeed.Value = chaseSpeed;
				break;
			//case MonsterState.SCARE:
			//	chasingPlayer = players[Random.Range(0, players.Count)];
			//	currentSpeed.Value = scareSpeed;
			//	break;
		}
		Debug.Log($"new state: {_monsterState}");

		StartCoroutine(StateTimer());
	}

	private MonsterState SetRandomState()
	{
		System.Array states = System.Enum.GetValues(typeof(MonsterState));

		//if (_monsterState == MonsterState.SCARE)
		//	return MonsterState.WANDER;

		MonsterState state = (MonsterState)states.GetValue(Random.Range(0, states.Length));
		while(state == _monsterState)
		{
			state = (MonsterState)states.GetValue(Random.Range(0, states.Length));
		}

		return state;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, detectionRadius);
		Gizmos.DrawWireSphere(center, range);
		if(agent != null)
			Gizmos.DrawSphere(agent.destination, 1);
	}
}
