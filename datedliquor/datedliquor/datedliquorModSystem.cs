using datedliquor.src.BlockClass;
using datedliquor.src.oldshit;
using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace datedliquor
{
    public class datedliquorModSystem : ModSystem
    {
        Harmony harmonyInstance;
        public override void Start(ICoreAPI api)
        {
            var modid = Mod.Info.ModID;
            api.RegisterBlockClass(modid + ":TallRackBottle", typeof(BlockTallRackBottle));
            harmonyInstance = new Harmony(modid);
            //harmonyInstance.PatchAll();
        }
        public override void StartClientSide(ICoreClientAPI api)
        {

        }
        public override void StartServerSide(ICoreServerAPI api)
        {

        }
        public override void AssetsFinalize(ICoreAPI api)
        {

        }
        public override void Dispose()
        {
            base.Dispose();
            harmonyInstance.UnpatchAll(Mod.Info.ModID);
        }
    }
}
