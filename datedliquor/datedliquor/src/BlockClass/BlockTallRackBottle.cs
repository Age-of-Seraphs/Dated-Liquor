using ACulinaryArtillery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace datedliquor.src.BlockClass
{
    public class BlockTallRackBottle : BlockBottle
    {
        public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
        {
                var api = byEntity.World.Api;
                if (api.World.BlockAccessor.GetBlock(blockSel.Position).Code.PathStartsWith("bottlerackcorner"))
                {
                    api.Logger.Event("Bottle Rack Slot Index: " + blockSel.SelectionBoxIndex.ToString());
                    //blockSel.SelectionBoxIndex
                    //beBottleRack.Inventory.
                    if (false) 
                    {
                        (api as ICoreClientAPI)?.TriggerIngameError(this, "racktoocrowded", Lang.Get("datedliquor:bottlerack-slot-toocrowded"));
                        handHandling = EnumHandHandling.NotHandled;
                    return;
                    }
            }
            base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
        }
    }
}
