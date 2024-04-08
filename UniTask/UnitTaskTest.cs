using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameDevKit.Document
{
    public class UnitTaskTest:MonoBehaviour
    {
        public Button button;

        public async void TestAsyOperation()
        {
            // await SceneManager.LoadSceneAsync(1).WithCancellation().ContinueWith();
        }
        private async void Start()
        {
            UniTask.Create(async() =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(2));
                Debug.Log("Wait 2 seconds");
            }).Forget();

            var completionSource = new UniTaskCompletionSource<string>();

            completionSource.TrySetResult("Finish");
            completionSource.TrySetCanceled(CancellationToken.None);
            await completionSource.Task;

            Debug.Log(completionSource.Task);

            //await button.GetAsyncClickEventHandler().OnClickAsync();
            
            //每点击两次触发
            button.OnClickAsAsyncEnumerable().Where((x, i) =>i>0&&i % 2 == 0).Subscribe(_ =>
            {
                Debug.Log("Click 2 times");
            });
        }
        
        
    }
}