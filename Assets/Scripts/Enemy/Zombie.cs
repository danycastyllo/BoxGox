using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameLord.Pool;

public class Zombie : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;
    public AudioClip audioClipDeath;

    public Transform topLeft, downRight;

    private float movementCooldown;

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
    }

    void Start()
    {
        
    }

    // Obtinee la direccion del playaer y se dirige a el para atacarlo
    protected override Vector3 GetHeading() {
        if (player == null) {
            SetPlayer();
        }
        if (player != null)
        {
            if (movementCooldown > 0) // comprueba que el cooldown despues de recibir un golpe haya terminado
            {
                movementCooldown--;
            }
            else
            {
                Vector3 heading = player.transform.position - transform.position;
                orientation = heading;
                return heading.normalized;
            }
        }

        return new Vector3(0, 0, 0);
    }

    protected override void Attack() {
        // handled by collision
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            Character target = collision.gameObject.GetComponent<Character>();
            if (target == this)
            {
                return;
            }
            target.HandleHit(25, 500f); // golpe al target que es el player
            rigidBody.AddForce(-orientation.normalized * 200f); // retrocede al zombie para qu este pueda atacar de nuevo

        }
    }

    private void SetPlayer() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if(go.Length > 0)
            player = go[0];
    }

    public override void HandleHit(int Damage, float Recoil)
    {
        movementCooldown = 200f; // despues de recibir un golpe se activa el cooldown para que pueda volver a moverse
        base.HandleHit(Damage, Recoil);
    }

    protected override void Destroy() // reinicia algunas variables y desactiva el enemigo mandandolo a la lista del pool para usarse nuevamente
    {
        health = 100f;
        state = State.Alive;
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        ReturnToPool();
        EnemyManager.ZombieDied();
    }

    protected override void SetHealthAnimator()
    {

    }
}
