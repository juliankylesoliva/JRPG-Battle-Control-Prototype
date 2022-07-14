using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CTRL_Movement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 6f;

    void Update()
    {
        DoMove();
    }

    private void DoMove()
    {
        Vector3 moveDirection = GetInputDirectionVector();
        Vector3 nextPosition = GetNextPosition();
        
        if (moveDirection.magnitude > 0f)
        {
            this.transform.forward = moveDirection;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextPosition, out hit, 1f, NavMesh.AllAreas))
        {
            this.transform.position = hit.position;
        }
    }

    private Vector3 GetNextPosition()
    {
        return (this.transform.position + (GetInputDirectionVector() * Time.deltaTime * moveSpeed));
    }

    private Vector3 GetInputDirectionVector()
    {
        Vector3 result = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) { result += Vector3.forward; }
        if (Input.GetKey(KeyCode.DownArrow)) { result -= Vector3.forward; }
        if (Input.GetKey(KeyCode.RightArrow)) { result += Vector3.right; }
        if (Input.GetKey(KeyCode.LeftArrow)) { result -= Vector3.right; }
        return result.normalized;
    }
}
