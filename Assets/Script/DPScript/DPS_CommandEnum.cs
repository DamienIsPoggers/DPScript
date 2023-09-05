namespace DPScript
{
    public enum DPS_CommandEnum
    {
        entry = 0,
        ID_sprite = 1,
        ID_lerp = 2,
        ID_rest = 3,
        ID_label = 4,
        ID_sendToLabel = 5,
        ID_enterState = 6,
        ID_createObject = 7,
        ID_callSubroutine = 8,
        ID_callSubroutineWithArgs = 9,
        ID_cmnSubroutine = 10,
        ID_cmnSubroutineWithArgs = 11,
        ID_if = 12,
        ID_else = 13,
        ID_elseIf = 14,
        ID_ifNot = 15,
        ID_elseIfNot = 16,
        ID_endIf = 17,
        ID_randomNum = 18,
        ID_createVar = 19,
        ID_editVar = 20,
        ID_compareNum = 23,
        ID_checkInput = 24,
        ID_upon = 30,
        ID_uponEnd = 31,
        ID_triggerUpon = 32,
        ID_clearUpon = 33,
        ID_callEffect = 34,
        ID_physicsXImpulse = 40,
        ID_physicsYImpulse = 41,
        ID_physicsZImpulse = 42,
        ID_xImpulseModifier = 43,
        ID_yImpulseModifier = 44,
        ID_zImpulseModifier = 45,
        ID_addPosX = 46,
        ID_addPosY = 47,
        ID_addPosZ = 48,
        ID_getDistance = 49,
        ID_cameraMove = 50,
        ID_cameraMoveRelative = 51,
        ID_cameraFocusEnable = 52,
        ID_cameraFocusDelay = 53,
        ID_cameraFocusZoom = 54,
        ID_doMath = 60,
        ID_stateRegister = 100,
        ID_stateConditions = 101,
        ID_stateInput = 102,
        ID_stateButton = 103,
        ID_stateConditionsSubroutine = 104,
        ID_stateMeterCost = 105,
        ID_stateRegisterEnd = 106,
        ID_stateOverrideCommon = 107,
        ID_stateRemoveCommon = 108,
        ID_setHitstunState = 109,
        ID_addCancel = 110,
        ID_addNeutralCancels = 111,
        ID_addNormalCancels = 112,
        ID_addSpecialCancels = 113,
        ID_addSuperCancels = 114,
        ID_hitCancel = 115,
        ID_blockCancel = 116,
        ID_hitOrBlockCancel = 117,
        ID_whiffCancel = 118,
        ID_removeCancel = 119,
        ID_setStateType = 120,
        ID_setNextState = 121,
        ID_setLandingState = 122,
        ID_transferMomentum = 123,
        ID_pauseMomentum = 124,
        ID_exitState = 125,
        ID_playAnimation = 126,
        ID_playCameraAnimation = 127,
        ID_superFreeze = 128,
        ID_showMesh = 129,
        ID_setAnimSpeed = 130,
        ID_setSpriteIf = 131,
        ID_setSpriteIfNot = 132,
        ID_attackDamage = 150,
        ID_attackPushbackX = 151,
        ID_attackPushbackY = 152,
        ID_attackPushbackZ = 153,
        ID_attackLaunchOpponent = 154,
        ID_attackHitFriction = 155,
        ID_attackHitGravity = 156,
        ID_attackHitAnim = 157,
        ID_attackHitstun = 158,
        ID_attackHitstop = 159,
        ID_attackBlockMultiplier = 160,
        ID_attackUntechTime = 161,
        ID_attackHardKnockdown = 162,
        ID_attackHitEffect = 163,
        ID_attackCounterType = 164,
        ID_attackChipDamageMultiplier = 165,
        ID_attackUnblockableType = 166,
        ID_attackKillCameraAnimation = 167,
        ID_attackKillHitstop = 168,
        ID_attackRefreshHit = 169,
        ID_addToComboCounter = 190,
        ID_addToComboCounterOnHit = 191,
    }
}