using BepInEx;
using RoR2;

namespace MarsModSprint
{
    [BepInPlugin("com.marsupilami.marsmodsprint", "MarsModSprint", "1.0.5")]

    public class MarsModSprint : BaseUnityPlugin
    {
        private bool mmIsSprinting;

        public void Awake()
        {
            mmIsSprinting = false;
        }

        public void Update()
        {
            if (!Run.instance) return;

            if (NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButtonUp("Sprint"))
            {
                mmIsSprinting = !mmIsSprinting;
            }
        }

        public void FixedUpdate()
        {
            if (!Run.instance) return;

            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                playerCharacterMasterController.master.GetBodyObject().GetComponent<InputBankTest>().sprint.PushState(mmIsSprinting);
            }
        }
    }
}
