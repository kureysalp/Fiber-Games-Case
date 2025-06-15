using System.Threading.Tasks;
using FiberCase.Gameplay;
using FiberCase.Grid_System;
using UnityEngine;

namespace FiberCase.Game_State
{
    public class CoinStackManagerCoinSortingState : CoinStackManagerState
    {
        private Node[] _neighbourNodes;
            
        public CoinStackManagerCoinSortingState(CoinStackManager coinStackManager, CoinStackManagerStateMachine coinStackManagerStateMachine) : base(coinStackManager, coinStackManagerStateMachine)
        {
        }
        

        public override void EnterState()
        {
            base.EnterState();
            var nodeCoinHolderIsOn = CoinStackManager.GridManager.GetNodeFromWorldPosition(CoinStackManager.CurrentCoinHolder.transform.position);
            _neighbourNodes = CoinStackManager.GridManager.GetNeighbours(nodeCoinHolderIsOn);
            
            _ = UpdateStateAsync();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override async Task UpdateStateAsync()
        {
            await base.UpdateStateAsync();
            
            var anyTransferHappened = false;
            do
            {
                anyTransferHappened = false;

                for (int i = 0; i < _neighbourNodes.Length; i++)
                {
                    if (CoinStackManager.CurrentCoinHolder.CoinStack.Count == 0) break;

                    var neighbourNode = _neighbourNodes[i];
                    if (!neighbourNode.IsOccupied) continue;

                    var coinHolderOnNode = neighbourNode.CoinHolder;
                    var coinOnCurrent = CoinStackManager.CurrentCoinHolder.CoinStack.Peek();
                    var coinOnNeighbour = coinHolderOnNode.CoinStack.Peek();

                    if (coinOnCurrent.Value == coinOnNeighbour.Value)
                    {
                        await CoinStackManager.CurrentCoinHolder.DepositCoinsToHolder(coinHolderOnNode, coinOnCurrent.Value);
                        anyTransferHappened = true;
                        break;
                    }
                }

            } while (anyTransferHappened && CoinStackManager.CurrentCoinHolder.CoinStack.Count > 0);

            CoinStackManagerStateMachine.ChangeState(CoinStackManager.CoinSpawningState);
        }
    }
}