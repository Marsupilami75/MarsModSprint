using BepInEx;
using RoR2;

namespace MarsModSprint
{
    [BepInPlugin("com.marsupilami.marsmodsprint", "MarsModSprint", "1.0.2")]

    public class MarsModSprint : BaseUnityPlugin
    {
        private bool mmIsSprinting;

        public void Awake()
        {
            mmIsSprinting = false;
        }

        public void Update()
        {
            if (!Run.instance) { return; }

            var player = NetworkUser.readOnlyLocalPlayersList[0];

            if (player.inputPlayer.GetButtonUp("Sprint"))
            {
                mmIsSprinting = !mmIsSprinting;
            }
        }

        public void FixedUpdate()
        {
            if (!Run.instance) { return; }

            var player = NetworkUser.readOnlyLocalPlayersList[0];
            var bodyInputs = PlayerCharacterMasterController.instances[0].master.GetBodyObject().GetComponent<InputBankTest>();

            if (mmIsSprinting)
            {
                bodyInputs.sprint.PushState(!player.inputPlayer.GetButton("Sprint"));
            }
        }
    }
}
