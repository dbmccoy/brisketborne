using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Man : MonoBehaviour {


    public Type type;
    public bool isAggro;

    private Rigidbody2D rb;
    private SpriteRenderer sp;

    private Transform hand;
    public Transform handL;
    public Transform handR;

    public float Speed;
    public float RollSpeed;
    public float Stamina;
    public float MaxStamina;
    private float lastStamina;
    private Text staminaMeter; //Player only move this

    private bool isRolling;

    private float xInput;
    private float yInput;

    public Vector2 MoveInput( )
    {
        return new Vector2(xInput, yInput).normalized;
    }

    public Vector2 MoveInput(float _x, float _y)
    {
        xInput = _x;
        yInput = _y;
        return new Vector2(_x, _y).normalized;
    }


    public Vector2 SetInput(Vector2 input)
    {
        xInput = input.x;
        yInput = input.y;
        return input;
    }

    private float speed()
    {
        return Speed / 32f;
    }

    public Weapon weapon;

    public void Attack()
    {
        weapon.Fire();
        //hand.GetComponent<Animator>().SetTrigger("fire");
    }

    public Transform Reticle;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        staminaMeter = GameObject.Find("Stamina").GetComponent<Text>(); //move this to Player
        hand = GetComponentInChildren<Hand>().transform;
        Stamina = MaxStamina;
	}

    private Vector2 mouse() {
        var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(p.x, p.y);
    }

    Vector3 target;

    private void Update()
    {
        target = transform.position + Vector3.left;

        if(type == Type.Player)
        {
            target = mouse();
        }

        var weapon = hand.GetComponentInChildren<Weapon>();
        Vector3 diff = target - (weapon.RotationPoint.position);
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (handL.position.x - target.x > 0f)
        {
            weapon.transform.localScale = new Vector3(1, -1, 1);
            hand.position = handR.position;
        }
        else
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
            hand.position = handL.position;
        }


        hand.rotation = Quaternion.Euler(0f, 0f, rot_z);

        Reticle.position = mouse();
        //hand.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(hand.position, mouse()) - 125f);
        if(type == Type.Player)
        {
            SetInput(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            if (Input.GetButtonDown("Jump")) Roll();

            if (Input.GetMouseButtonDown(0)) Attack();

            staminaMeter.text = "Stamina: " + (Stamina * 100f).ToString();
        }

    }

    public void Roll()
    {
        //Debug.Log("jump");
        //rb.velocity = MoveInput().normalized * RollSpeed;
        //isRolling = true;
        //Stamina = Mathf.Clamp(Stamina - .3f, 0f, 1f);
        //lastStamina = Mathf.Clamp(Stamina,.05f,MaxStamina/100f);
    }

    // Update is called once per frame
    void FixedUpdate () {

        if(type == Type.Player)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }

        rb.velocity = MoveInput() * speed();

        if (rb.velocity.x < 0)
        {
            sp.flipX = true;
        }
        else sp.flipX = false;

        //Stamina = Mathf.Clamp(Stamina + Time.fixedDeltaTime/5f,0,MaxStamina/100f);
	}

    public enum Type
    {
        Player,
        Neutral,
        Hostile
    }
}
