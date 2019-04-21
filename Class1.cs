using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using On.EntityStates.Mage.Weapon;
using RoR2;
using UnityEngine;

namespace MarsModSprint
{
    [BepInPlugin("com.marsupilami.marsmodsprint", "MarsModSprint", "1.0.7")]

    public class MarsModSprint : BaseUnityPlugin
    {
        private bool init = false;
        private bool mmActivated = false;
        private bool mmIsSprinting = false;
        private bool mmIsCasting = false;

        public static bool mmFlameActive = false;

        public static ConfigWrapper<bool> mmWrapAutoActivate;

        public void Awake()
        {
            RoR2Application.isModded = true;

            mmWrapAutoActivate = Config.Wrap("Settings", "AutoActivate", "Automatically activate Sprint mod at runtime?", true);

            Flamethrower.OnEnter += delegate (Flamethrower.orig_OnEnter orig, BaseState self)
            {
                MarsModSprint.mmFlameActive = true;
                orig.Invoke(self);
            };
            Flamethrower.OnExit += delegate (Flamethrower.orig_OnExit orig, BaseState self)
            {
                MarsModSprint.mmFlameActive = false;
                orig.Invoke(self);
            };
        }

        public void Update()
        {
            if (!Run.instance || Run.instance.time < 1f)
            {
                init = false;
                mmActivated = false;
                return;
            }

            if (!init)
            {
                Chat.AddMessage("<color=blue>Mars Mod Sprint Loaded</color>");
                init = true;

                if (mmWrapAutoActivate.Value == true)
                {
                    mmActivated = true;
                    Chat.AddMessage("<color=red>MM Sprint: Activated</color>");
                }
            }

            mmIsSprinting = NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButton("Sprint");

            if (CharacterBody.readOnlyInstancesList[0].baseNameToken == "TOOLBOT_BODY_NAME")
            {
                mmIsCasting = false;
                if (NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButton("PrimarySkill") || NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButton("SecondarySkill"))
                {
                    mmIsCasting = true;
                }
            }

            if (CharacterBody.readOnlyInstancesList[0].baseNameToken == "MAGE_BODY_NAME")
            {
                if (NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButtonDown("UtilitySkill"))
                {
                    mmIsCasting = true;
                }
                else if (NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButtonUp("UtilitySkill"))
                {
                    mmIsCasting = false;
                }
            }

            if (Input.GetKey(KeyCode.LeftAlt) && NetworkUser.readOnlyLocalPlayersList[0].inputPlayer.GetButtonUp("Sprint"))
            {
                mmActivated = !mmActivated;
                if (mmActivated)
                {
                    Chat.AddMessage("<color=red>MM Sprint: Activated</color>");
                }
                else
                {
                    Chat.AddMessage("<color=red>MM Sprint: Deactivated</color>");
                }
            }
        }

        public void FixedUpdate()
        {
            if (!Run.instance || Run.instance.time < 1f || !mmActivated)
            {
                return;
            }

            if (mmFlameActive || mmIsCasting || (mmActivated && mmIsSprinting))
            {
                return;
            }

            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                if (playerCharacterMasterController != null)
                {
                    if (playerCharacterMasterController.isActiveAndEnabled)
                    {
                        playerCharacterMasterController.master.GetBodyObject().GetComponent<InputBankTest>().sprint.PushState(mmActivated);
                    }
                }
            }
        }
    }
}
