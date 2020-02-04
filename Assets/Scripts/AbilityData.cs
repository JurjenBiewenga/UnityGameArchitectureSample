using System.Collections;
using System.Collections.Generic;
using Architecture;
using UnityEngine;

[System.Serializable, CreateAssetMenu]
public class AbilityData : ScriptableObject
{
    public FloatReference CooldownLength;
    public FloatReference Length;

    public FloatReference SpeedModifier;
    public FloatReference ResistanceModifier;
    public FloatReference DamageModifier;
    public FloatReference KnockbackModifier;
    public FloatReference HealOnUsePercentage;

    private float _cooldownTimer;

    private float _timer;

    private Entity _affectedEntity;

    public bool Active
    {
        get { return _timer > 0 && _timer < Length.Value; }
    }

    public bool OnCooldown
    {
        get { return _cooldownTimer > 0 && _cooldownTimer < CooldownLength.Value; }
    }

    public float RemainingCooldown
    {
        get { return Mathf.Max(_cooldownTimer, 0); }
    }

    private void OnEnable()
    {
        _timer = 0;
        _cooldownTimer = 0;
    }

    public void Use(Entity entity)
    {
        if (!OnCooldown && !Active)
        {
            _cooldownTimer = CooldownLength.Value;
            _timer = Length.Value;
            if (ResistanceModifier > 0)
                entity.ResistanceModifier += ResistanceModifier;
            if (SpeedModifier > 0)
                entity.SpeedModifier += SpeedModifier;
            if (DamageModifier > 0)
                entity.DamageModifier += DamageModifier;
            if (KnockbackModifier > 0)
                entity.KnockbackModifier += KnockbackModifier;
            if (HealOnUsePercentage > 0)
                entity.Heal(entity.MaxHealth * HealOnUsePercentage);

            _affectedEntity = entity;
        }
    }

    public void Update()
    {
        if (_timer > 0 && _timer - Time.deltaTime < 0)
        {
            if (ResistanceModifier > 0)
                _affectedEntity.ResistanceModifier -= ResistanceModifier;
            if (SpeedModifier > 0)
                _affectedEntity.SpeedModifier -= SpeedModifier;
            if (DamageModifier > 0)
                _affectedEntity.DamageModifier -= DamageModifier;
            if (KnockbackModifier > 0)
                _affectedEntity.KnockbackModifier -= KnockbackModifier;
            _affectedEntity = null;
            _cooldownTimer = CooldownLength.Value;
        }
        _timer -= Time.deltaTime;
        _cooldownTimer -= Time.deltaTime;
    }
}