using System.Collections.Generic;
using System.Threading;
using TestJeux.Business.Managers.API;
using TestJeux.Core.Aggregates;
using TestJeux.Core.Entities.Items;

namespace TestJeux.Business.Services
{
	public class ScriptService : IScriptService
    {
        private readonly GameAggregate _gameRoot;
        private readonly Dictionary<ItemModel, CancellationTokenSource> _cancellationTokens;

        public ScriptService(GameAggregate gameRoot)
        {
            _gameRoot = gameRoot;
            _cancellationTokens = new Dictionary<ItemModel, CancellationTokenSource>();
        }

        public void Add(int itemId)
        {
            var item = _gameRoot.GetItemFromCurrentLevel(itemId);
            if (!item.HasScript)
                return;
            var cancellationToken = new CancellationTokenSource();
            var task = item.GetTaskScript(item, cancellationToken.Token);
            _cancellationTokens[item] = cancellationToken;
            task.Start();
        }

        public void Subscribe()
        {
            // Nothing to do here
        }

        public void Reset()
        {
            foreach (var item in _cancellationTokens.Values)
                item.Cancel();
            _cancellationTokens.Clear();
        }
    }
}
