using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public Bullet bullet;
    public bool isCocked;
    private bool isCocking;
    public float cockTime;

    public int BulletSpeed;

    public Transform RotationPoint;
    public Transform FirePoint;

	// Use this for initialization

    public void Fire()
    {
        if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("idle") && isCocked)
        {
            var _bullet = Instantiate(bullet, FirePoint.position, transform.rotation);
            var vel = transform.right * (BulletSpeed);
            _bullet.GetComponent<Rigidbody2D>().velocity = vel;
            GetComponentInParent<Animator>().SetTrigger("fire");
            isCocked = false;
        }
        else if (!GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("cock"))
        {
            StartCoroutine(Cock());
        }
    }

    IEnumerator Cock()
    {
        isCocking = true;
        GetComponentInParent<Animator>().SetTrigger("cock");
        yield return new WaitForSeconds(0f);
        isCocked = true;
        isCocking = false;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
