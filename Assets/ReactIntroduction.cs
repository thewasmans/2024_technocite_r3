using System;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ReactIntroduction : MonoBehaviour
{
    public Button ActionButton0;
    public Button ActionButton1;
    public Player Player;
    public Text ScoreUI;

    void Start()
    {
        ActionButton0.onClick.AsObservable().Subscribe(_ =>{
            Debug.Log("Clickable normal synthax");
            Player.TakeDamage(10);
        }).AddTo(this);
        
        ActionButton1.OnClickAsObservable().Subscribe(_ => {
            Debug.Log("Clickable short synthax");
            Player.TakeDamage(50);
        }).AddTo(this);

        Observable.FromEvent<int>(
            handler => Player.OnPlayerDamage += handler,
            handler => Player.OnPlayerDamage -= handler
            )
            .Where(damage => damage > 10)
            .Subscribe(damage => Debug.Log("Player takes" + damage)).AddTo(this);

        Player.PlayerScore
        .Subscribe(score => ScoreUI.text = "Player Score " + score)
        .AddTo(this);
    }

    void Update()
    {

    }
}
