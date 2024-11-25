using System;
using System.Collections;
using R3;
using R3.Triggers;
using Unity.VisualScripting;
using UnityEngine;


public class Player : MonoBehaviour
{
    private int MaxHealt;
    public event Action<int> OnPlayerDamage;

    public ReactiveProperty<int> CurrentHp {get; private set;} = new ReactiveProperty<int>();
    public ReactiveProperty<bool> IsDead {get; private set;} = new ReactiveProperty<bool>();
    public ReactiveProperty<float> PlayerScore {get; private set;} = new ReactiveProperty<float>();
    public GameObject VFXPrefab;

    void Start()
    {
        CurrentHp.Value = MaxHealt;
        CurrentHp.Subscribe(hp => IsDead.Value = hp <= 0).AddTo(this);

        var camera = Camera.main;
        
        Observable.EveryUpdate().Subscribe(hp =>{
            var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -camera.transform.position.z);
            mousePosition = camera.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }).AddTo(this);

        this.OnTriggerEnterAsObservable()
        .Where(collider => collider.gameObject.CompareTag("Point"))
        .Select(collision => new DataStruct()
        {
            Position = collision.transform.position,
            Value = 1,
        })
        .Subscribe(data => {
            Instantiate(VFXPrefab, data.Position, Quaternion.identity);
            Debug.Log("Instantiate");
            PlayerScore.Value += 10;
        });
    }

    public void TakeDamage(int damage)
    {
        OnPlayerDamage?.Invoke(damage);
        CurrentHp.Value -= damage;
    }
}


 struct DataStruct : IEnumerable
{
    public Vector3 Position;
    public int Value;

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}