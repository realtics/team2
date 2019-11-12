using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float movePower = 1f;
    public float attackRange = 2f;

    private Animator   _anim;
    protected Rigidbody2D _rgdBody;

    private int        _movementFlag = 0; //0:Idle , 1:Left, 2:Right , 3:Up, 4:Down

    private GameObject  _traceTarget;
    private bool        _isTracing = false;

    // Start is called before the first frame update
    void Start()
    {
        _anim = gameObject.GetComponentInChildren<Animator>();
        _rgdBody = GetComponent<Rigidbody2D>();

        StartCoroutine("ChangeMoveMent");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
    
        //Trace or Random
        if (_isTracing)
        {
            Vector3 playerPos = _traceTarget.transform.position;

            Vector3 direction = playerPos - transform.position;
            direction.Normalize();


            transform.position += direction * movePower * Time.deltaTime;
            if(direction.x >0)
                transform.localScale = new Vector3(-1, 1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);

            if ( (Mathf.Abs(playerPos.x - transform.position.x) < attackRange) && (Mathf.Abs(playerPos.y - transform.position.y) < attackRange/2) )
            {
                _movementFlag = 0;
                _anim.SetBool("isAttacking", true);
                gameObject.GetComponent<MonsterAttack>().enabled = true;
            }
        }

        else 
        {
            //Movement Assign
            if (_movementFlag == 1)
            {
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(1, 1, 1);
            }

            else if (_movementFlag == 2)
            {
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(-1, 1, 1);
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
    }

    //Coroutine
    IEnumerator ChangeMoveMent()
    {
        //Random Change Movement
        _movementFlag = Random.Range(0, 3);

        //Mapping Animaotion
        if (_movementFlag == 0)
            _anim.SetBool("isMoving", false);
        else
            _anim.SetBool("isMoving", true);

        //wait
        yield return new WaitForSeconds(3f);

        //restart
        StartCoroutine("ChangeMoveMent");
    }

    //Trace Start
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _traceTarget = other.gameObject;
            movePower = 2.0f;

            StopCoroutine("ChangeMoveMent");
        }
    }

    //Trace Mainatain
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTracing = true;

            _anim.SetBool("isMoving", true);
        }
    }

    //Trace Over
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isTracing = false;
            movePower = 1.0f;
            StartCoroutine("ChangeMoveMent");
        }
    }
}
