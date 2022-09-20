using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Code
{
    public class CardDataGenerator
    {
        public async UniTask<Card.Params> GenerateCardParams()
        {
            var result = new Card.Params();

            int seed = Random.Range(int.MinValue, int.MaxValue);
            var texture = UnityWebRequestTexture.GetTexture($"https://picsum.photos/seed/{seed}/200/300");
            await texture.SendWebRequest();
            while (texture.result != UnityWebRequest.Result.Success)
            {
                await UniTask.Delay(1000);
                await texture.SendWebRequest();
            }

            result.icon = DownloadHandlerTexture.GetContent(texture);
            result.attack = Random.Range(1, 10);
            result.mana = Random.Range(1, 10);
            result.hp = Random.Range(1, 10);
            result.header = "SeedCard" + seed;

            return result;
        }
    }
}