using Vintagestory.GameContent;

public class BlockWoodBucket : BlockBucket
{
    protected override string meshRefsCacheKey => "bucketMeshRefs" + this.Code;
}