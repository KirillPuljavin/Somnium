using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatSO : ScriptableObject
{
    [SerializeField]
    private float _health;
    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private float _maxHealth;
    public float MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private float _vision;
    public float Vision
    {
        get { return _vision; }
        set { _vision = value; }
    }

    [SerializeField]
    private float _damage;
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    [SerializeField]
    private float _dashDamage;
    public float DashDamage
    {
        get { return _dashDamage; }
        set { _dashDamage = value; }
    }

    [SerializeField]
    private float _dashCooldown;
    public float DashCooldown
    {
        get { return _dashCooldown; }
        set { _dashCooldown = value; }
    }

    [SerializeField]
    private float _attackRange;
    public float AttackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; }
    }

    [SerializeField]
    private float _weaponEvo;
    public float WeaponEvo
    {
        get { return _weaponEvo; }
        set { _weaponEvo = value; }
    }  



    // Bools for cards
    
    [SerializeField]
    private bool _card1;
    public bool Card1
    {
        get { return _card1; }
        set { _card1 = value; }
    }

    [SerializeField]
    private bool _card2;
    public bool Card2
    {
        get { return _card2; }
        set { _card2 = value; }
    }

    [SerializeField]
    private bool _card3;
    public bool Card3
    {
        get { return _card3; }
        set { _card3 = value; }
    }

    [SerializeField]
    private bool _card4;
    public bool Card4
    {
        get { return _card4; }
        set { _card4 = value; }
    }

    
}
