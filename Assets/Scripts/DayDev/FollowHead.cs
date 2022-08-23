using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowHead : MonoBehaviour
{
    public float followTime;
    NavMeshAgent navAgent;
    public Transform targetObject;

    void Start(){
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        navAgent.destination = targetObject.position;
    }

}
