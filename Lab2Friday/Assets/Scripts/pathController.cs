using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathController : MonoBehaviour
{
    [SerializeField]
    public pathManager pathManager;

    List<wayPoint> thePath;
    wayPoint target;

    public float MoveSpeed;
    public float RotateSpeed;
    public Animator animator;
    bool isSprinting;

    void Start()
    {
        isSprinting = false;
        animator.SetBool("Sprinting",isSprinting);

        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            target = thePath[0];
        }
    }


    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }
    void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            //you can modify here for overshooting condition
            return;
        }
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {         
            isSprinting = !isSprinting;
            Debug.Log(isSprinting);
            animator.SetBool("Sprinting", isSprinting);
        }
        if (isSprinting)
        {
            rotateTowardsTarget();
            moveForward();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        target = pathManager.GetNextTarget();
    }
}
