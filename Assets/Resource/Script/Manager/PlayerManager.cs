using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Player의 데이터를 저장하는 부분으로 Player와 관련된 데이터, 메소드는 해당 Manager에 작성 바람
/// States 역시 Player에 종속되는 부분으로 판정함
/// </summary>
public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private int hp;
    public int Hp { get => hp; }
    [SerializeField] private int mana;
    public int Mana { get => mana; set => mana = value; }
    public int row, col;  // 0: 왼쪽 위, 1: 가운데, 2: 오른쪽 아래

    public BaseState state;
    public Queue<BaseState> StatesQueue;
    public List<Card> PlayerCard;

    void Start()
    {
        PlayerCard = new List<Card>();

        //List<States> statesList = new List<States>(){States.Color};
        List<BaseState> statesList = new List<BaseState>() { new NormalState() };

        //PlayerCard.Add(new ColorCard("Alpha", "Alpha is Greek A", 1,
        //    statesList,  ColorCardEffect.Color1, false, 1, ColorTargetPosition.Center));
        //foreach (Card card in PlayerManager.Instance.PlayerCard)
        //{
        //    ColorState state1 = ((ColorCard)card).ColorState();
        //}
        
        StatesQueue = new Queue<BaseState>();
        state = new NormalState();
        //PlayerCard = CardData.Instance._load("PlayerCard.json");
        state.Enter();
        //CardData.Instance.saveData(PlayerCard, "PlayerCard.json");
        
        CardManager.Instance.SetUp();

    }

    void Update()
    {
         if(Input.GetMouseButton(0))
             state.MouseEvent();
    }

    void FixedUpdate()
    {
        state.Update();
    }

    public void ChangeStates(BaseState newState)
    {
        state.Exit();
        //state전환 과정 이 부분은 세부 State 구현이 나와야 가능할 것으로 예상됨
        state.Enter();
    }
}
