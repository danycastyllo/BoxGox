using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public abstract class Character : PoolItem
{
    public float movSpeed; // velocidad de movimiento de los personajes
    public float MovementSpeed; //auxiliar para modficar la velocidad cuando va en diagonal
    public Animator animator;  

    protected Direction direction;
    protected State state;
    protected Vector3 orientation;
    public Rigidbody2D rigidBody;
    public float dyingTime = 0.5f;

    public float health = 100f;
    protected float accumTime;

    public enum Direction
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        UpRight = 4,
        UpLeft = 5,
        DownRight = 6,
        DownLeft = 7
    }
    
    //estados de un personaje
    public enum State 
    {
        Alive,
        Dying,
        Dead
    }

    void Update()
    {
        switch (state)
        {
            case State.Alive:
                Vector3 heading = GetHeading();
                SetDirection(heading);
                rigidBody.velocity = heading * movSpeed;
                //movSpeed = MovementSpeed;
                Attack();
                break;
            case State.Dying:
                Destroy();
                break;
            case State.Dead:
                break;

        }
    }

    private void SetDirection(Vector3 heading) {
        if (heading.sqrMagnitude == 0) {
            return;
        }

        if(Mathf.Abs(heading.x) > Mathf.Abs(heading.y))
        {
            direction = heading.x > 0? Direction.Right : Direction.Left;
        } else if(Mathf.Abs(heading.x) == Mathf.Abs(heading.y))
        {
            if(heading.y > 0)
            {
                direction = heading.x > 0 ? Direction.UpRight : Direction.UpLeft;
            }
            else
            {
                direction = heading.x > 0 ? Direction.DownRight : Direction.DownLeft;
            }
        } else
        {
            direction = heading.y > 0 ? Direction.Up : Direction.Down;
        }
        //if (gameObject.tag == "Player")
        //{
        //    print(direction);

        //}
        // cambia la imagen (sprite) del personaje en la direccion que se mueve
        animator.SetInteger("Direction", Convert.ToInt32(direction)); 
    }

    protected void Die() // cambia al estado "muriendo"
    {
        state = State.Dying;
        //animator.SetInteger("Dead", Convert.ToInt32(direction));
    }

    protected abstract Vector3 GetHeading();

    protected abstract void Attack();

    // funcion para producir el golpe de un ataque
    public virtual void HandleHit(int Damage, float Recoil) {
        health -= Damage;
        SetHealthAnimator();
        rigidBody.AddForce(-orientation.normalized * Recoil); // produce el retroceso del golpe
        if (health <= 0f) // comprueba que la vida del personaje sea menor o igual a 0
        {
            Die();
        }
    }

    protected abstract void SetHealthAnimator();

    protected abstract void Destroy();

}