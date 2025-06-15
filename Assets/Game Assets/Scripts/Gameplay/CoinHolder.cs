using System.Collections.Generic;
using System.Threading.Tasks;
using FiberCase.Event;
using FiberCase.Grid_System;
using FiberCase.Scriptable_Objects;
using FiberCase.Systems;
using UnityEngine;

namespace FiberCase.Gameplay
{
    public class CoinHolder : Poolable
    {
        public Stack<Coin> CoinStack { get;} = new();

        private const float CoinHeight = 0.12f;

        private Node _node;

        public void CreateStack(CoinStack coinStack, CoinColorCoding  coinColorCoding)
        {
            var coinStackPairCount = coinStack.CoinStackPairs.Length;
            for (int i = coinStackPairCount - 1; i >= 0; i--)
            {
                var coinStackPair = coinStack.CoinStackPairs[i];
                for (int j = 0; j < coinStackPair.CoinCount ; j++)
                {
                    var newCoin = Poolable.Get<Coin>();
                    newCoin.SetValue(coinStackPair.CoinValue);
                    newCoin.SetColor(coinColorCoding.Colors[coinStackPair.CoinValue - 1]);
                    
                    CoinStack.Push(newCoin);
                    newCoin.transform.SetParent(transform);
                    newCoin.transform.localPosition = Vector3.zero + CoinHeight * CoinStack.Count * Vector3.up;
                }
            }
        }

        public void MovePosition(Vector3 position, float moveSpeed)
        {
            var newPosition = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime);
            transform.position = newPosition;
        }

        public async Task DepositCoinsToHolder(CoinHolder coinHolder, int coinValue)
        {
            while (CoinStack.Count > 0 && CoinStack.Peek().Value == coinValue)
            {
                var coinToDeposit = CoinStack.Pop();
                
                var coinEndPosition = coinHolder.CoinStack.Peek().transform.position + Vector3.up * CoinHeight;
                await MoveCoin(coinToDeposit, coinEndPosition);
                coinHolder.CoinStack.Push(coinToDeposit);
                coinToDeposit.transform.SetParent(coinHolder.transform);
            }

            coinHolder.DepositReceived();
            
            if(CoinStack.Count == 0)
                RemoveCoinHolder();
        }

        private void DepositReceived()
        {
            var coinOnTop = CoinStack.Peek();
            var sameCoinCount = 0;

            foreach (var coin in CoinStack)
            {
                if (coinOnTop.Value == coin.Value)
                    sameCoinCount++;
                else break;
            }

            if (sameCoinCount >= 10) //TODO: Make this variable
            {
                for (int i = 0; i < sameCoinCount; i++)
                {
                    var poppedCoin = CoinStack.Pop();
                    poppedCoin.transform.SetParent(null);
                    poppedCoin.ReturnToPool();
                }
                
                EventBus.Raise(new CoinsPoppedEvent
                {
                    CoinAmount = sameCoinCount,
                    CoinValue = coinOnTop.Value
                } );
                
                if(CoinStack.Count == 0)
                    RemoveCoinHolder();
            }
        }

        private void RemoveCoinHolder()
        {
            _node.RemoveCoinHolder();
            _node = null;
            ReturnToPool();
        }

        public void SetNode(Node node)
        {
            _node = node;
        }
        
        private static async Task MoveCoin(Coin coinToDeposit, Vector3 coinEndPosition)
        {
            var coinStartPosition = coinToDeposit.transform.position;
                
            var elapsedTime = 0f;
            var moveDuration = .1f; //TODO: Move this to editor.

            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                var nextPosition = Vector3.Lerp(coinStartPosition, coinEndPosition, elapsedTime / moveDuration);
                coinToDeposit.transform.position = nextPosition;
                await Task.Yield();
            }
        }
    }
}