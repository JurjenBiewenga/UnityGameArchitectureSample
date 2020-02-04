using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

public class Player : Entity
{
    public AbilityData HeadbuttData;
    public AbilityData BarkData;
    public AbilityData FluffData;
    
    public ContactFilter2D BarkContactFilter;
    public Collider2D BarkCollider;

    [SerializeField]
    private ParticleSystem barkParticle;
    [SerializeField]
    private ParticleSystem headbuttParticle;
    
    [SerializeField]
    private float _attackCooldown = .5f;

    private float _attackTimer;

    private Collider2D[] _barkResult = new Collider2D[20];

    protected override void Awake()
    {
        base.Awake();
        _attackTimer = _attackCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Entity foundEntity = CheckForEntity(HitDistance);
        if (foundEntity != null)
        {
            AttackEntity(foundEntity);
        }
        _attackTimer += Time.deltaTime;
        
        HeadbuttData.Update();
        BarkData.Update();
        FluffData.Update();

        if (!HeadbuttData.Active && headbuttParticle != null && headbuttParticle.gameObject.activeSelf) 
            headbuttParticle.gameObject.SetActive(false);
    }

    public override void DazeEntity(float dazeLength)
    {
        if(!HeadbuttData.Active)
            base.DazeEntity(dazeLength);
    }

    protected override Vector2 HandleMovement(Vector2 offset)
    {
        if (!Dazed)
        {
            if (!CheckForEntity(StopWalkingDistance) || HeadbuttData.Active)
            {
                if (!KnockedBack && Grounded)
                    offset += MovementDirection * Speed * SpeedModifier;
            }
        }
        return offset;
    }

    private void AttackEntity(Entity entity)
    {
        if (entity != null)
        {
            if (Faction.enemyFactions.Contains(entity.Faction))
            {
                if (_attackTimer >= _attackCooldown)
                {
                    entity.TakeDamage(Strength * DamageModifier, KnockbackStrength * KnockbackModifier, !HeadbuttData.Active, this);
                    _attackTimer = 0;
                }
            }
        }
    }

    public void Bark()
    {
        if (!BarkData.OnCooldown && !BarkData.Active)
        {
            if (!FluffData.Active && !HeadbuttData.Active)
            {
                if (barkParticle != null)
                {
                    barkParticle.gameObject.SetActive(true);
                }
                
                BarkData.Use(this);
                BarkCollider.enabled = true;
                int hitEnemies = Physics2D.OverlapCollider(BarkCollider, BarkContactFilter, _barkResult);
                BarkCollider.enabled = false;
                if (hitEnemies > 0)
                {
                    for (int i = 0; i < hitEnemies; i++)
                    {
                        Collider2D col = _barkResult[i];
                        if (col.gameObject != gameObject)
                        {
                            Entity e = col.GetComponent<Entity>();
                            if (Faction.enemyFactions.Contains(e.Faction))
                            {
                                e.DazeEntity(Mathf.Clamp(Strength/10f, 0, 2));
                            }
                        }
                    }
                }
            }
        }
    }

    public void Headbutt()
    {
        if (!HeadbuttData.OnCooldown && !HeadbuttData.Active)
        {
            if (!FluffData.Active)
            {
                if (headbuttParticle != null)
                {
                    headbuttParticle.gameObject.SetActive(true);
                }
                HeadbuttData.Use(this);
            }
        }
    }

    public void FluffUp()
    {
        if (!FluffData.OnCooldown && !FluffData.Active)
        {
            if(!HeadbuttData.Active)
                FluffData.Use(this);
        }
    }
}