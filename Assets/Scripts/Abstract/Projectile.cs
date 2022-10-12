using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using FrameLord.Pool;

public class Projectile : PoolItem
{
    public float movSpeed = 10f;
    public float screenMarginLimitXLeft;
    public float screenMarginLimitXRight;
    public float screenMarginLimitYTop;
    public float screenMarginLimitYDown;
    private Vector2 bearing;
    private Character shooter;
    public float damage;
    private bool hit = false;

    public float MovSpeed { get => movSpeed; set => movSpeed = value; }

    public bool Hit { get => hit; set => hit = value; }

    public float Damage { get => damage; set => damage = value; }
    public Vector2 Bearing { get => bearing; set => bearing = value; }
    public Character Shooter { get => shooter; set => shooter = value; }

    void Awake()
    {

    }

    void Update()
    {
        var x = transform.position.x;
        var y = transform.position.y;   
        x += bearing.x * movSpeed * Time.deltaTime;
        y += bearing.y * movSpeed * Time.deltaTime;

        if (x > screenMarginLimitXLeft && x < screenMarginLimitXRight && y > screenMarginLimitYDown && y < screenMarginLimitYTop)
            transform.position = new Vector3(x, y, transform.position.z);
        else
            Destroy();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(!hit && (string.Compare(col.gameObject.tag, "Player") == 0 || string.Compare(col.gameObject.tag, "Enemy") == 0)) 
        {
            Character target = col.gameObject.GetComponent<Character>();
            if(target == shooter) {
                return;
            }
            hit = true;
            target.HandleHit(25, 500f);
            
        }
        Destroy();
    }

    private void Destroy()
    {
        ReturnToPool();
    }
}
