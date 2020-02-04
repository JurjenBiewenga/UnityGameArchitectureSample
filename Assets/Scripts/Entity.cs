using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Entity : MonoBehaviour
{
    public FloatReference Speed;
    public FloatReference Health;
    public FloatReference MaxHealth;
    public FloatReference Armor;
    public FloatReference Strength;
    public FloatReference KnockbackStrength;
    public FloatReference GroundedDistance;
    public FloatReference HitDistance;
    public FloatReference StopWalkingDistance;
    public FloatReference KnockbackResistance;
    public FloatReference DazeResistance;
    public LayerMaskReference EntityMask;
    public FalloffCurveReference DazeFalloff;
    public FalloffCurveReference KnockbackFalloff;
    public FalloffCurveReference ArmorFalloff;

    public EntityGameEvent OnEntityDied;

    public bool CanBeKnockedBack;
    public bool CanBeDazed;
    public bool Grounded { get; private set; }
    public bool KnockedBack { get; private set; }
    public bool Dazed { get; private set; }

    public Vector2 MovementDirection;
    public Faction Faction;

    public float SpeedModifier = 1;
    public float ResistanceModifier = 1;
    public float DamageModifier = 1;
    public float KnockbackModifier = 1;
    protected Rigidbody2D Rigidbody;
    protected Collider2D Collider2D;

    private Vector2 _knockbackForce;
    private Vector2 _startKnockbackForce;
    private float _knockbackTimer;
    private float _dazeTimer;
    private float _knockbackDazeDuration;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
    }

    protected virtual void FixedUpdate()
    {
        Vector2 offset = Vector2.zero;

        HandleDazed();

        offset = HandleMovement(offset);

        offset = HandleKnockbackMovement(offset);

        GroundCheck(Vector2.down);

        if (!Grounded)
            offset += Physics2D.gravity;

        Rigidbody.MovePosition(Rigidbody.position + offset * Time.deltaTime);
    }

    protected virtual void HandleDazed()
    {
        _dazeTimer -= Time.deltaTime * Mathf.Clamp(.5f + (DazeFalloff.Value.Eval(DazeResistance) * .05f) * ResistanceModifier, 0f, 1.5f);
        if (_dazeTimer < 0)
        {
            Dazed = false;
        }
    }

    protected virtual Vector2 HandleKnockbackMovement(Vector2 offset)
    {
        _knockbackTimer += Time.deltaTime * Mathf.Clamp(.5f + (KnockbackFalloff.Value.Eval(KnockbackResistance) * .05f) * ResistanceModifier, 0f, 1.5f);
        if (_knockbackTimer > 1 || (_knockbackTimer > .2f && Grounded))
        {
            if (KnockedBack)
            {
                DazeEntity(_knockbackDazeDuration);
                KnockedBack = false;
            }
        }
        if (KnockedBack && _knockbackTimer < .2f)
        {
            offset += _knockbackForce;
            _knockbackForce.y = Mathf.Lerp(_knockbackForce.y, -_startKnockbackForce.y, _knockbackTimer);
        }
        return offset;
    }

    protected virtual Vector2 HandleMovement(Vector2 offset)
    {
        if (!Dazed)
        {
            if (!CheckForEntity(StopWalkingDistance))
            {
                if (!KnockedBack && Grounded)
                    offset += MovementDirection * Speed * SpeedModifier;
            }
        }
        return offset;
    }

    protected virtual void GroundCheck(Vector2 direction)
    {
        RaycastHit2D[] hits = new RaycastHit2D[1];
        int hitCount = Rigidbody.Cast(direction.normalized, hits, GroundedDistance);
        Grounded = hitCount > 0 && hits[0].collider != null;
    }

    protected virtual Entity CheckForEntity(float distance)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MovementDirection, distance, EntityMask.Value);
        if (hit.collider != null)
        {
            Entity entity = hit.collider.gameObject.GetComponent<Entity>();
            return entity;
        }

        return null;
    }

    public virtual void DazeEntity(float dazeLength)
    {
        if (CanBeDazed)
        {
            Dazed = true;
            _dazeTimer = dazeLength;
        }
    }

    public virtual bool TakeDamage(float damage, float knockbackStrength, bool canCounterAttack, Entity attacker)
    {
        Health.Value = Mathf.Max(Health - CalculateMitigatedDamage(damage), 0);
        if (Health.Value <= 0)
        {
            Die();
            return true;
        }

        if (canCounterAttack && !Dazed)
        {
            attacker.TakeDamage(Strength * DamageModifier, this.KnockbackStrength * KnockbackModifier, false, this);
        }

        KnockbackEntity(knockbackStrength);

        return false;
    }

    public virtual float CalculateMitigatedDamage(float damage)
    {
        float percentage = Mathf.Clamp(100 - (ArmorFalloff.Value.Eval(Armor) * ResistanceModifier), 10, 150) / 100f;
        return damage * percentage;
    }

    protected virtual void KnockbackEntity(float knockbackStrength)
    {
        if (CanBeKnockedBack)
        {
            _startKnockbackForce = CalculateKnockbackDirection(knockbackStrength);
            _knockbackForce = _startKnockbackForce;
            _knockbackTimer = 0;
            KnockedBack = true;
            _knockbackDazeDuration = knockbackStrength / 70f;
        }
    }

    public Vector2 CalculateKnockbackDirection(float knockbackStrength)
    {
        return -MovementDirection.normalized * knockbackStrength + Vector2.up * 30;
    }

    public void Heal(float amount)
    {
        Health.Value = Mathf.Clamp(Health.Value + amount, 0, MaxHealth.Value);
    }

    public void Die()
    {
        if (OnEntityDied != null)
            OnEntityDied.Invoke(this);
        GameObject.Destroy(gameObject);
    }
}