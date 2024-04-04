using Vintagestory.GameContent;

/// <summary>
/// Bucket with fixed bug that broke textures
/// </summary>
public class BlockWoodBucket : BlockBucket
{
    protected override string meshRefsCacheKey => "bucketMeshRefs" + this.Code;
}