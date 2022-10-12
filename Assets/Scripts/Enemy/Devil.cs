using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using FrameLord.Pool;

public class Devil : Character
{
    public const string Tag = "Enemy";
    public GameObject player;
    public Transform startPosition;
    public AudioClip audioClipDeath;

    public Transform topLeft, downRight;

    private float attackCooldown;
    private float movementCooldown;
    

    // Start is called before the first frame update
    void Awake()
    {
        state = State.Alive;
        direction = Direction.Down;
    }

    void Start()
    {
        transform.position = new Vector2(startPosition.position.x, startPosition.position.y);
    }

    protected override Vector3 GetHeading()
    {
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

        return new Vector3(0,0,0);
    }

    private void SetPlayer() {
        var go = GameObject.FindGameObjectsWithTag("Player");
        if(go.Length > 0)
            player = go[0];
    }

    protected override void Attack()
    {
        if (attackCooldown > 0) {
            attackCooldown--;
            return;
        }

        var projectile = PoolManager.Instance.GetPool("EnemyProjectile").GetItem();
        if (projectile != null)
        {
            Projectile proj = projectile.gameObject.GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            proj.Bearing = orientation;
            proj.Shooter = this;
            proj.Hit = false;
            proj.screenMarginLimitXLeft = topLeft.position.x;
            proj.screenMarginLimitXRight = downRight.position.x;
            proj.screenMarginLimitYTop = topLeft.position.y;
            proj.screenMarginLimitYDown = downRight.position.y;
            projectile.gameObject.SetActive(true);
            attackCooldown = 300f; // tiempo de cooldown de el proyectil de los diablos
        }
    }

    public override void HandleHit(int Damage, float Recoil)
    {
        attackCooldown = 200f;
        movementCooldown = 200f; // despues de recibir un golpe se activa el cooldown para que pueda volver a moverse
        base.HandleHit(Damage, Recoil);
    }

    protected override void Destroy() // reinicia algunas variables y desactiva el enemigo mandandolo a la lista del pool para usarse nuevamente
    {
        health = 100f;
        state = State.Alive;
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        ReturnToPool();
        EnemyManager.DevilDied();
    }

    protected override void SetHealthAnimator()
    {

    }
}
