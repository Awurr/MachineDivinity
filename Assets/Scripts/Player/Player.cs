using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager Manager;
    private Rigidbody2D PlayerBody;
    public PlayerProfile Profile;
    public Transform MovementAimer;
    public float ColorChangeRate;
    public int Health;

    [Header("Movement")]
    bool TransitionJump, Jumped;
    private float RemainingFlightTime;

    [Header("Weapons")]
    public Transform WeaponAimer;

    [Header("Visuals")]
    public Color BloodColor;
    public float BloodColorVariance;
    public GameObject BloodParticle;
    public SpriteRenderer HeadRenderer;
    public Sprite[] HeadSprites;
    public Animator BodyAnimator;
    public Transform[] ShoulderLocations;
    public LineRenderer[] ArmLines;
    

    void Start()
    {
        Profile.ResetStats();
        Profile.PlayerTransform = transform;
        Manager = FindObjectOfType<GameManager>();

        PlayerBody = GetComponent<Rigidbody2D>();
        ReplenishFlight();

        Manager.UpdateHealth();
    }

    void Update()
    {
        // Rotate weapon
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = 0;

        Vector3 ObjectPos = Camera.main.WorldToScreenPoint(transform.position);
        MousePos.x = MousePos.x - ObjectPos.x;
        MousePos.y = MousePos.y - ObjectPos.y;

        float Angle = Mathf.Atan2(MousePos.y, MousePos.x) * Mathf.Rad2Deg;
        Angle -= 90;
        WeaponAimer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angle));

        // set head sprite
        if (MousePos.y > transform.position.y + 100)
        {
            HeadRenderer.sprite = HeadSprites[2];
        }
        else if (MousePos.y < transform.position.y - 100)
        {
            HeadRenderer.sprite = HeadSprites[0];
        }
        else
        {
            HeadRenderer.sprite = HeadSprites[1];
        }

        // set body animation
        if (PlayerBody.velocity.y > 0.01f || PlayerBody.velocity.y < -0.01f)
        {
            BodyAnimator.Play("Fly");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (transform.localScale.x > 0)
            {
                BodyAnimator.Play("WalkBack");
            }
            else
            {
                BodyAnimator.Play("Walk");
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (transform.localScale.x < 0)
            {
                BodyAnimator.Play("Walk");
            }
            else
            {
                BodyAnimator.Play("WalkBack");
            }
        }
        else
        {
            BodyAnimator.Play("Stand");
        }

        // set arm locations
        ArmLines[0].SetPosition(0, ShoulderLocations[0].position);
        ArmLines[0].SetPosition(1, ArmLines[0].transform.position);
        ArmLines[1].SetPosition(0, ShoulderLocations[1].position);
        ArmLines[1].SetPosition(1, ArmLines[1].transform.position);

        // flip sprite (if neccessary)
        if (MousePos.x > transform.position.x)
        {
            BodyAnimator.gameObject.transform.localScale = new Vector3(1, 1, 1);
            HeadRenderer.gameObject.transform.localScale = new Vector3(1, 1, 1);
            WeaponAimer.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            BodyAnimator.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            HeadRenderer.gameObject.transform.localScale = new Vector3(-1, 1, 1);
            WeaponAimer.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        // temporary
        if (Health <= 0)
        {
            Damageable.CreateAndThrowParticle(BloodParticle, transform, 5, BloodColor, BloodColorVariance);
        }
    }

    void FixedUpdate()
    {
        float HorizontalMovement = Profile.GetStat(PlayerProfile.StatKey.HorizontalMovement);
        float VerticalMovement = Profile.GetStat(PlayerProfile.StatKey.VerticalMovement);

        // Horizontal Movement
        // ( checking if the velocity is greater/less than the movement speed 
        // makes it so that if the player is moving faster than their normal movement speed
        // they will lose control of movement until their speed reaches a normal level )
        if (PlayerBody.velocity.x < HorizontalMovement || PlayerBody.velocity.x > -HorizontalMovement)
        {
            // Right
            if (Input.GetKey(KeyCode.D))
            {
                PlayerBody.velocity = new Vector2(HorizontalMovement, PlayerBody.velocity.y);
            }
            // Left
            else if (Input.GetKey(KeyCode.A))
            {
                PlayerBody.velocity = new Vector2(-HorizontalMovement, PlayerBody.velocity.y);
            }
        }
        // Decelerate
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            PlayerBody.AddForce(new Vector2(0 - PlayerBody.velocity.x, 0) * HorizontalMovement);
        }

        // Vertical Movement 
        if (TransitionJump && !Jumped)
        {
            PlayerBody.AddForce(new Vector2(0, 400));
            Jumped = true;
        }
        else if (!TransitionJump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (RemainingFlightTime > 0)
                {
                    RemainingFlightTime -= Time.deltaTime;
                    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, VerticalMovement);
                }
                else
                {
                    PlayerBody.velocity = new Vector2(PlayerBody.velocity.x, -VerticalMovement / 2);
                }
            }
        }
    }

    public void TakeDamage(int Amount, GameObject Source)
    {
        Health -= Amount;
        Manager.UpdateHealth();
        Profile.ProcOnHit(Source);

        for (int i = 0; i < 10; i++)
        {
            Damageable.CreateAndThrowParticle(BloodParticle, transform, 5, BloodColor, BloodColorVariance);
        }

        if (Health <= 0)
        {
            FindObjectOfType<DeathAnimation>().StartAnimation();
        }
    }

    public void Heal(int Amount)
    {
        Health += Amount;
    }

    public void ReplenishFlight()
    {
        RemainingFlightTime = Profile.GetStat(PlayerProfile.StatKey.MaxFlightTime);
    }

    void CycleColor(SpriteRenderer Renderer)
    {
        float Hue, Sat, Val;
        Color.RGBToHSV(Renderer.color, out Hue, out Sat, out Val);
        Hue += ColorChangeRate * Time.deltaTime;
        Color NewColor = Color.HSVToRGB(Hue, Sat, Val);

        Renderer.color = NewColor;
    }

    void CycleColor(TrailRenderer Renderer)
    {
        float Hue, Sat, Val;
        Color.RGBToHSV(Renderer.startColor, out Hue, out Sat, out Val);
        Hue += ColorChangeRate * Time.deltaTime;
        Color NewColor = Color.HSVToRGB(Hue, Sat, Val);

        Renderer.startColor = NewColor;
        Renderer.endColor = NewColor;
    }

    public void StartBoing()
    {
        RemainingFlightTime = Profile.GetStat(PlayerProfile.StatKey.MaxFlightTime);
        StartCoroutine(Boing());
    }

    public IEnumerator MissileFlurry(GameObject Missile, int Missiles, float Delay, Transform Target)
    {
        for (int i = 0; i < Missiles; i++)
        {
            GameObject Instance = Instantiate(Missile, Profile.PlayerTransform.position, Profile.PlayerTransform.rotation);
            Instance.GetComponent<Missile>().Damage = 1; // temp
            Instance.GetComponent<Missile>().Target = Target;
            Instance.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            yield return new WaitForSeconds(Delay);
        }
    }

    IEnumerator Boing()
    {
        Jumped = false;
        TransitionJump = true;
        yield return new WaitForSeconds(0.5f);
        TransitionJump = false;
    }
}
