using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using FrameLord.Pool;

public class Player : Character
{
    public const string Tag = "Player";

    public Transform[] projectileStart;

    private Vector2 MovementInput;

    private SpriteRenderer spriteRenderer;
    public Sprite[] healthbarSprites;
    public GameObject healthbar;
    public LineRenderer lineRenderer;

    public AudioSource audioClipShoot;
    public AudioClip audioClipDeath;

    public Transform topLeft, downRight;

    private Transform rayStart;

    private Vector3 rayDirection;

    private KeyCode[] movementKeyCodes = new KeyCode[]
 {
         KeyCode.LeftArrow,
         KeyCode.RightArrow,
         KeyCode.DownArrow,
         KeyCode.UpArrow,
         KeyCode.D
 };

    void Awake()
    {
        state = State.Alive;
        direction = Direction.Right;
        // Start facing up 
        orientation = new Vector2(1, 0);
    }
    //private void Update()
    //{
    //    print(state);
    //    if(state == State.Alive)
    //    {
    //        Animate();
    //        Move();
    //        Attack();
    //        Vector3 heading = GetHeading();
    //    }
    //}

    void Start()
    {
        spriteRenderer = healthbar.GetComponent<SpriteRenderer>();
    }


    protected override Vector3 GetHeading()
    {
        //Move() mueve al personaje en la direcion que se presione las teclas
        //MovementSpeed = movSpeed;

        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");

        if (Horizontal == 0 && Vertical == 0)
        {
            return new Vector3(0, 0, 0);
        }
        //else if (Horizontal != 0 && Vertical != 0)
        //{
        //    movSpeed = movSpeed / 1.5f;
        //}

        

        Vector3 v = new Vector3(Horizontal, Vertical, 0);

        // ejecuta la animacion segun la direcion del personaje
        animator.SetFloat("MovementX", v.x);
        animator.SetFloat("MovementY", v.y);

        orientation = v;
        return v.normalized;
    }

    protected override void Attack() {
        if (!Input.GetKeyDown(KeyCode.Space))
        {
            return;
        }


        //dependiendo la direcion guarda los datos que se utilizaran para crear el raycast
        switch (direction)
        {
            case Direction.Left:
                rayStart = projectileStart[0];
                rayDirection = -projectileStart[0].right;
                break;
            case Direction.Right:
                rayStart = projectileStart[1];
                rayDirection = projectileStart[1].right;
                break;
            case Direction.Up:
                rayStart = projectileStart[2];
                rayDirection = projectileStart[2].up;
                break;
            case Direction.Down:
                rayStart = projectileStart[3];
                rayDirection = -projectileStart[3].up;
                break;
        }
        rayStart.GetComponent<SpriteRenderer>().enabled = true; // activa el sprite del fogaje al accionar el arma
        RaycastHit2D hitInfo = Physics2D.Raycast(rayStart.position, rayDirection); // crea y guarda la informacion del raycast

        if (hitInfo) // si el raycast golpea algo
        {
            if (string.Compare(hitInfo.transform.tag, "Enemy") == 0) // comprueba que el objeto impactado sea un enemigo
            {
                Character target = hitInfo.transform.GetComponent<Character>(); // guarda en la variable target el character impactado

                target.HandleHit(25, 500f); // golpe a target con cierta cantidad de daño

            }

            lineRenderer.SetPosition(0, rayStart.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else // sino golpea algo dibuja una linea en la direcion multiplicado por 100
        {
            lineRenderer.SetPosition(0, rayStart.position);
            lineRenderer.SetPosition(1, rayStart.position + rayDirection * 100);
        }

        lineRenderer.enabled = true;

        audioClipShoot.Play();

        StartCoroutine(Shoot());
    }
    IEnumerator Shoot() // luego de 0.02 segundos desaparece la linea que se dibuja con el raycast
    {
        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(0.05f);
        rayStart.GetComponent<SpriteRenderer>().enabled = false;
    }

    private bool AnyKeyPressed(KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKey(keyCodes[i]))
                return true;
        }
        return false;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        
        if (state == State.Alive)
        {
            
            if (string.Compare(col.gameObject.tag, "Enemy") == 0)
            {
                Die();
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

        public void OnTriggerStay2D(Collider2D col)
    {

    }

    public void OnTriggerExit2D(Collider2D col)
    {

    }

    public void Respawn()
    {

    }

    protected override void Destroy()
    {
        AudioSource.PlayClipAtPoint(audioClipDeath, transform.position);
        gameObject.SetActive(false);
        GameManager.Instance.NotifyPlayerDeath();
    }

    protected override void SetHealthAnimator()
    {
        switch (health)
        {
            case 100f:
                spriteRenderer.sprite = healthbarSprites[0];
                break;
            case 75f:
                spriteRenderer.sprite = healthbarSprites[1];
                break;
            case 50f:
                spriteRenderer.sprite = healthbarSprites[2];
                break;
            case 25f:
                spriteRenderer.sprite = healthbarSprites[3];
                break;
        }
    }

    public void RegenerateHealth()
    {
        health = Math.Min(health + 25, 100);
        SetHealthAnimator();
    }
}
