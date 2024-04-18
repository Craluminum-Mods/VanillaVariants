using Vintagestory.GameContent;

namespace VanillaVariants;

public class BlockWoodBucket : BlockBucket
{
    protected override string meshRefsCacheKey => "bucketMeshRefs" + this.Code;
}