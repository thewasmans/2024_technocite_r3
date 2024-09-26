using System;
using R3;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    private int MaxHealt;
    public event Action<int> OnPlayerDamage;

    public ReactiveProperty<int> CurrentHp {get; private set;}
    public ReactiveProperty<bool> IsDead {get; private set;}

    void Start()
    {
        CurrentHp.Value = MaxHealt;
        CurrentHp.Subscribe(hp => IsDead.Value = hp <= 0).AddTo(this);
    }

    public void TakeDamage(int damage)
    {
        OnPlayerDamage?.Invoke(damage);
        CurrentHp.Value -= damage;
    }
}