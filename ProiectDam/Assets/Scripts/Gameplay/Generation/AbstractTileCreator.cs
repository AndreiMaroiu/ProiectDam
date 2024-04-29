using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Generation
{
    [CreateAssetMenu(fileName = "NewAbstractTileCreator", menuName = "Scriptables/AbstractTileCreator")]
    public class AbstractTileCreator : ScriptableObject
    {
        
    }

    public sealed class AssetReferenceTileCreator : AssetReferenceT<AbstractTileCreator>
    {
        public AssetReferenceTileCreator(string guid) : base(guid)
        {
        }
    }
}
