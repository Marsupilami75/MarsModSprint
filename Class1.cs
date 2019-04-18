using BepInEx;
using RoR2;

namespace MarsModSprint
{
    [BepInPlugin("com.marsupilami.marsmodsprint", "MarsModSprint", "1.0.6")]

    public class MarsModSprint : BaseUnityPlugin
    {
        private bool mmIsSprinting;

        public void Awake()
        {
            mmIsSprinting = false;
            RoR2Application.isModded = true;
        }

        public void Update()
        {
            if (!Run.instance)
            {
                mmIsSprinting = false;
                return;
            }

            if (NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButtonUp("Sprint"))
            {
                mmIsSprinting = !mmIsSprinting;
            }
        }

        public void FixedUpdate()
        {
            if (!Run.instance || !mmIsSprinting)
            {
                return;
            }

            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                playerCharacterMasterController.master.GetBodyObject().GetComponent<InputBankTest>().sprint.PushState(mmIsSprinting);
            }
        }
    }
}
