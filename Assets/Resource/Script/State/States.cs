using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// State에 대한 추상 클래스
/// </summary>
public abstract class BaseState
{
    /// <summary>
    /// 카드 사용시에 일어나야 하는 부분을 여기에 작성할 것
    /// </summary>
    public abstract void DoAction(States state);

    /// <summary>
    /// 처음 State로 진입시 호출되는 함수
    /// 각종 UI나 이펙트 Rendering 작업이 여기서 부탁호출 되도록 작성할 것
    /// </summary>
    public abstract void Enter();
    /// <summary>
    /// 해당 State에서 마우스를 클릭했을때 일어나는 상황을 작성할 것
    /// </summary>
    public abstract void MouseEvent();
    /// <summary>
    /// 해당 State에서 Update함수에 호출되야하는 요소를 작성할 것
    /// </summary>
    public abstract void Update();
    /// <summary>
    /// 다음 State로 전환시에 호출되는 함수
    /// Enter에서 Rendering한 요소들을 지울 것
    /// </summary>
    public abstract void Exit();
}

public class NormalState : BaseState
{
    private int DrawNum;
    private bool isNewPlayerTurn;
    
    public NormalState(int DrawNum = 5, bool isNewPlayerTurn = false)
    {
        this.DrawNum = DrawNum;
        this.isNewPlayerTurn = isNewPlayerTurn;
    }

    public override void DoAction(States state)
    {
        return;
    }

    public override void Enter()
    {
        if(isNewPlayerTurn)
            CardManager.Instance.DrawCard(DrawNum);
    }

    public override void MouseEvent()
    {
        return;
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        return;
    }
}
public class EnemyState : BaseState
{
    public override void DoAction(States state)
    {
        return;
    }

    public override void Enter()
    {
        EnemyManager.Instance.EnemyAttack();
    }

    public override void MouseEvent()
    {
        return;
    }

    public override void Update()
    {
        return;
    }

    public override void Exit()
    {
        return;
    }
}


public class MoveState : BaseState
{
    bool[,] movableSpace;
    public MoveState(MoveCard originalCard)
    {
        switch (originalCard.moveDirection)
        {
            case MoveDirection.UDLR:
                // 현재 위치로부터 상하좌우로 한 칸 이동
                {
                    this.movableSpace = new bool[3, 3];  // 모든 항이 false인 2D 배열
                    (int, int)[] coords =
                    {
                        (PlayerManager.Instance.Row - 1, PlayerManager.Instance.Col),  // 위
                        (PlayerManager.Instance.Row, PlayerManager.Instance.Col + 1),  // 오른쪽
                        (PlayerManager.Instance.Row + 1, PlayerManager.Instance.Col),  // 아래
                        (PlayerManager.Instance.Row, PlayerManager.Instance.Col - 1)   // 왼쪽
                    };
                    foreach ((int, int) coord in coords)
                    {
                        if (coord.Item1 >= 0 && coord.Item1 < 3 && coord.Item2 >= 0 && coord.Item2 < 3)
                        {
                            this.movableSpace[coord.Item1, coord.Item2] = true;
                        }
                    }
                    break;
                }

            case MoveDirection.Diagonal:
                // 현재 위치로부터 대각선으로 한 칸 이동
                {
                    this.movableSpace = new bool[3, 3];  // 모든 항이 false인 2D 배열
                    (int, int)[] coords =
                    {
                        (PlayerManager.Instance.Row - 1, PlayerManager.Instance.Col - 1),  // 왼쪽 위
                        (PlayerManager.Instance.Row - 1, PlayerManager.Instance.Col + 1),  // 오른쪽 위
                        (PlayerManager.Instance.Row + 1, PlayerManager.Instance.Col + 1),  // 오른쪽 아래
                        (PlayerManager.Instance.Row + 1, PlayerManager.Instance.Col - 1)   // 왼쪽 아래
                    };
                    foreach ((int, int) coord in coords)
                    {
                        if (coord.Item1 >= 0 && coord.Item1 < 3 && coord.Item2 >= 0 && coord.Item2 < 3)
                        {
                            this.movableSpace[coord.Item1, coord.Item2] = true;
                        }
                    }
                    break;
                }

            case MoveDirection.Colored:
                // 내가 색칠해 뒀던 칸으로 이동
                {
                    List<List<BoardColor>> boardColors = BoardManager.Instance.BoardColors;

                    for (int i = 0; i < boardColors.Count; i++)  // row
                    {
                        for (int j = 0; j < boardColors[0].Count; j++)  // col
                        {
                            if (boardColors[i][j] == BoardColor.Player)
                            {
                                this.movableSpace[i, j] = true;
                            }
                        }
                    }
                    break;
                }

            case MoveDirection.Dangerous:
                // 적이 이번 턴에 공격할 칸으로 이동
                {
                    //List<Enemy> enemyList = EnemyManager.EnemyList;
                    /*
                    List<Enemy> enemyList = new List<Enemy>();

                    foreach (Enemy enemy in enemyList)
                        foreach ((int, int) coord in enemy.WhereToAttack)
                            this.movableSpace[coord.Item1, coord.Item2] = true;
                    */
                    break;
                    
                }

            case MoveDirection.All:
                // 원하는 칸으로 이동
                this.movableSpace = new bool[,] {
                    { true, true, true },
                    { true, true, true },
                    { true, true, true }
                };
                break;

            default:
                return;
        }
    }

    public override void DoAction(States state)
    {
        
    }

    public override void Enter()
    {
        // 카메라 암전 등

    }

    public override void Exit()
    {
        // 이동 모션?
        // 카메라 다시 밝게
        //BoardManager.Instance.MovePlayer()
    }

    public override void MouseEvent()
    {
        // 이동 가능한 곳을 클릭할 시 진행.
    }

    public override void Update()
    {
        // UI 상으로 이동 가능한 곳은 O 표시.
    }

}

public class AttackState : BaseState
{
    private AttackCard Card;

    public AttackState(AttackCard card)
    {
        this.Card = card;
    }

//취소하면 normal state로 돌아감
    public override void DoAction(States state)
    {
        
    }
    public override void Enter()
    {
        //공격 가능한 대상의 테두리를 밝은 파란 테두리로 표시
        //카드 데이터의 공격 가능한 대상의 종류
        //몬스터 공격 가능한 경우(001)
        int targetType = Card.GetTargetType();
        bool isMonster = targetType % 10 != 0;
        bool isWall = (targetType / 10) % 10 != 0;
        bool isMinion = (targetType / 100) % 10 != 0;

        if(isMonster)
        {

        }
        if(isWall)
        {
            //플레이어 주변에 Wall이 있으면 하이라이트(Clickable)
            //플레이어 위치 주변 4칸에 Wall이 있는지 체크

        }
        if(isMinion)
        {
            //하수인 공격 가능한 경우 -> 플레이어 주변의 하수인(100)
        }
    }
    public override void Exit()
    {
    }

    public override void MouseEvent()
    {
        
    }
    public override void Update()
    {
        // UI 상으로 이동 가능한 곳은 O 표시.
    }
}


public class ColorState : BaseState
{
    
    private bool Selectable;
    private ColorTargetPosition Target;
    private int Cost;
    public ColorState(bool Selectable, ColorTargetPosition Target)
    {
        this.Selectable = Selectable;
        this.Target = Target;
    }
    public override void DoAction(States state)
    {
        
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void MouseEvent()
    {
        PlayerManager.Instance.ChangeStates(PlayerManager.Instance.StatesQueue.Dequeue());
    }

    public override void Update()
    {

    }
}

