using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Jumper : Agent
{
    [SerializeField] private float jumpAmount;
    [SerializeField] private KeyCode keyForJump;
    
    private Rigidbody rigid_Body;
    private Vector3 initialPosition;
    private bool isJumpReady = true;
    private int score = 0;
    public event Action OnReset;
    
    public override void Initialize()
    {
        rigid_Body = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if(isJumpReady)
            RequestDecision();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
            Jump();
    }

    public override void OnEpisodeBegin()
    {
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        
        if (Input.GetKey(keyForJump))
            actionsOut[0] = 1;
    }

    private void Jump()
    {
        if (isJumpReady)
        {
            rigid_Body.AddForce(new Vector3(0, jumpAmount, 0), ForceMode.VelocityChange);
            isJumpReady = false;
        }
    }
    
    private void Reset()
    {
        score = 0;
        isJumpReady = true;
        transform.position = initialPosition;
        rigid_Body.velocity = Vector3.zero;
        OnReset?.Invoke();
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Street"))
            isJumpReady = true;
        
        else if (coll.gameObject.CompareTag("Mover") || coll.gameObject.CompareTag("DoubleMover"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("score"))
        {
            AddReward(0.1f);
            score++;
            ScoreCollector.Instance.AddScore(score);
        }
    }
}
