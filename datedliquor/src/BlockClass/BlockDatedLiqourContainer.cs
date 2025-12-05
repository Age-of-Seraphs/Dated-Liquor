using datedliquor.src.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using static HarmonyLib.Code;

namespace datedliquor.src.BlockClass
{
    internal class BlockDatedLiquorContainer : BlockLiquidContainerCorkable
    {
        public CorkedContainableProps CorkedProps = new CorkedContainableProps();



        public override void OnLoaded(ICoreAPI api)
        {
            base.OnLoaded(api);
            CorkedProps = Attributes?["liquidContainerProps"]?.AsObject(CorkedProps, Code.Domain) ?? CorkedProps;
        }
        public override void AddExtraHeldItemInfoPostMaterial(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world)
        {
            base.AddExtraHeldItemInfoPostMaterial(inSlot, dsc, world);
            if (IsTopOpened)
            {
                dsc.AppendLine("AddExtraHeldItemInfoPostMaterial Post Base Method");
            }
            
        }        
    }
}
