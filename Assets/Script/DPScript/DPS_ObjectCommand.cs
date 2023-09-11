using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DPScript
{
    public static class DPS_ObjectCommand
    {
        #region switchCase
        public static void objSwitchCase(scriptCommand com, GameWorldObject o)
        {
            if (o.isInUpon && com.id != (int)DPS_CommandEnum.ID_uponEnd)
            {
                o.uponCodeCreate.commands.Add(com);
                return;
            }

            if (o.ifFailed > 0)
                if (!isIfCommand(com.id))
                    return;

            if (o.canElse && (com.id != (int)DPS_CommandEnum.ID_else || com.id != (int)DPS_CommandEnum.ID_elseIf ||
                com.id != (int)DPS_CommandEnum.ID_elseIfNot))
                o.canElse = false;

            switch((DPS_CommandEnum)com.id)
            {
                case DPS_CommandEnum.ID_sprite:
                    sprite(com.stringArgs[0], com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_lerp:
                    lerp(com.stringArgs[0], com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_rest:
                    rest(o);
                    break;
                case DPS_CommandEnum.ID_label:
                    label(com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_sendToLabel:
                    sendToLabel(com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_enterState:
                    enterState(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_createObject:
                    createObject(com.stringArgs[0], com.intArgs[0], com.intArgs[1], o);
                    break;
                case DPS_CommandEnum.ID_callSubroutine:
                    callSubroutine(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_cmnSubroutine:
                    cmnSubroutine(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_if:
                    ifCom(com, o);
                    break;
                case DPS_CommandEnum.ID_else:
                    elseCom(o);
                    break;
                case DPS_CommandEnum.ID_elseIf:
                    elseIf(com, o);
                    break;
                case DPS_CommandEnum.ID_ifNot:
                    ifNot(com, o);
                    break;
                case DPS_CommandEnum.ID_elseIfNot:
                    elseIfNot(com, o);
                    break;
                case DPS_CommandEnum.ID_endIf:
                    endIf(o);
                    break;
                case DPS_CommandEnum.ID_randomNum:
                    o.returnInt = randomNum(com.intArgs[0], com.intArgs[1], o);
                    break;
                case DPS_CommandEnum.ID_createVar:
                    createVar(com.byteArgs[0], com.intArgs[0], com.intArgs[1], o);
                    break;
                case DPS_CommandEnum.ID_editVar:
                    editVar(com.byteArgs[0], com.intArgs[0], com.byteArgs[1], com, o);
                    break;
                case DPS_CommandEnum.ID_compareNum:
                    o.returnInt = Convert.ToInt32(compareNum(com, o));
                    break;
                case DPS_CommandEnum.ID_checkInput:
                    o.returnInt = Convert.ToInt32(o.input_CanInput(com.byteArgs[0], "", 1, 0));
                    break;
                case DPS_CommandEnum.ID_upon:
                    upon(com.byteArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_uponEnd:
                    uponEnd(o);
                    break;
                case DPS_CommandEnum.ID_triggerUpon:
                    o.triggerUpon(com.byteArgs[0]);
                    break;
                case DPS_CommandEnum.ID_clearUpon:
                    clearUpon(com.byteArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_callEffect:
                    if (com.byteArgs[0] == 0)
                        Battle_Manager.Instance.commonPlayer.spawnEffect(com.stringArgs[0],
                            new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]),
                            com.uintArgs[0], o);
                    else
                        o.effectManager.spawnEffect(com.stringArgs[0], new Vector3(com.floatArgs[0], 
                            com.floatArgs[1], com.floatArgs[2]), com.uintArgs[0]);
                    break;
                case DPS_CommandEnum.ID_physicsXImpulse:
                    physicsXImpulse(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_physicsYImpulse:
                    physicsYImpulse(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_physicsZImpulse:
                    physicsZImpulse(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_xImpulseModifier:
                    xImpulseModifier(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_yImpulseModifier:
                    yImpulseModifier(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_zImpulseModifier:
                    zImpulseModifier(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addPosX:
                    addPosX(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addPosY:
                    addPosY(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addPosZ:
                    addPosZ(com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_getDistance:
                    o.returnInt = getDistance(o.worldObjects[com.intArgs[0]], o);
                    break;
                case DPS_CommandEnum.ID_doMath:
                    o.returnInt = doMath(com, o);
                    break;
                case DPS_CommandEnum.ID_stateMaxComboUse:
                    stateMaxComboUse(com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_stateRegister:
                    stateRegister(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_stateConditions:
                    stateConditions(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2],
                        com.byteArgs[3], com.byteArgs[4], com.byteArgs[5], com.byteArgs[6], o);
                    break;
                case DPS_CommandEnum.ID_stateInput:
                    stateInput(com.byteArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_stateButton:
                    stateButton(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2], com.byteArgs[3], 
                        com.byteArgs[4], com.byteArgs[5], com.byteArgs[6], com.byteArgs[7], o);
                    break;
                case DPS_CommandEnum.ID_stateConditionsSubroutine:
                    stateConditionsSubroutine(com.byteArgs[0], com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_stateMeterCost:
                    stateMeterCost(com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_stateRegisterEnd:
                    stateRegisterEnd(o);
                    break;
                case DPS_CommandEnum.ID_setHitstunState:
                    setHitstunState(com.byteArgs[0], com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addCancel:
                    addCancel(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addNeutralCancels:
                    addNeutralCancels(o);
                    break;
                case DPS_CommandEnum.ID_addNormalCancels:
                    addNormalCancels(o);
                    break;
                case DPS_CommandEnum.ID_addSpecialCancels:
                    addSpecialCancels(o);
                    break;
                case DPS_CommandEnum.ID_addSuperCancels:
                    addSuperCancels(o);
                    break;
                case DPS_CommandEnum.ID_hitCancel:
                    hitCancel(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_blockCancel:
                    blockCancel(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_hitOrBlockCancel:
                    hitOrBlockCancel(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_removeCancel:
                    removeCancel(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setStateType:
                    setStateType(com.byteArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setNextState:
                    setNextState(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setLandingState:
                    setLandingState(com.stringArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_transferMomentum:
                    transferMomentum(com.boolArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_pauseMomentum:
                    pauseMomentum(com.boolArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_exitState:
                    exitState(o);
                    break;
                case DPS_CommandEnum.ID_playAnimation:
                    playAnimation(com.stringArgs[0], com.floatArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_playCameraAnimation:
                    Transform focus = null;
                    if (com.byteArgs[0] == 0)
                        focus = o.transform;
                    CameraManager.Instance.playCameraAnimation(o.cameraAnimator, (float)o.dir, com.stringArgs[0],
                        focus, com.uintArgs[0], com.uintArgs[1], new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]),
                        new Vector3(com.floatArgs[3], com.floatArgs[4], com.floatArgs[5]));
                    break;
                case DPS_CommandEnum.ID_showMesh:
                    showMesh(com.stringArgs[0], com.boolArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setAnimSpeed:
                    setAnimSpeed(com.floatArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setSpriteIf:
                    setSpriteIf(com.stringArgs[0], com.uintArgs[0], com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setSpriteIfNot:
                    setSpriteIfNot(com.stringArgs[0], com.uintArgs[0], com.byteArgs[0], com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setSpriteMaterial:
                    setSpriteMaterial(com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_setSpriteOffset:
                    setSpriteOffset(new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]), o);
                    break;
                case DPS_CommandEnum.ID_setMeshOffset:
                    setMeshOffset(new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]), o);
                    break;
                case DPS_CommandEnum.ID_attackDamage:
                    attackDamage(com, o);
                    break;
                case DPS_CommandEnum.ID_attackPushbackX:
                    attackPushbackX(com, o);
                    break;
                case DPS_CommandEnum.ID_attackPushbackY:
                    attackPushbackY(com, o);
                    break;
                case DPS_CommandEnum.ID_attackPushbackZ:
                    attackPushbackZ(com, o);
                    break;
                case DPS_CommandEnum.ID_attackLaunchOpponent:
                    attackLaunchOpponent(com.boolArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackHitFriction:
                    attackHitFriction(com, o);
                    break;
                case DPS_CommandEnum.ID_attackHitGravity:
                    attackHitGravity(com, o);
                    break;
                case DPS_CommandEnum.ID_attackHitAnim:
                    attackHitAnim(com.byteArgs[0], com.byteArgs[1], o);
                    break;
                case DPS_CommandEnum.ID_attackHitstun:
                    attackHitstun(com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackHitstop:
                    attackHitstop(com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackBlockMultiplier:
                    attackBlockMultiplier(com.floatArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackUntechTime:
                    attackUntechTime(com.byteArgs[0], com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackHardKnockdown:
                    attackHardKnockdown(com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackHitEffect:
                    attackHitEffect(com.byteArgs[0], com.stringArgs[0], new Vector3(com.floatArgs[0],
                        com.floatArgs[1], com.floatArgs[2]), com.uintArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackCounterType:
                    attackCounterType(com.byteArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackChipDamageMultiplier:
                    attackChipDamageMultiplier(com.floatArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_attackUnblockableType:
                    attackUnblockableType(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2],
                        com.byteArgs[3], com.byteArgs[4], com.byteArgs[5], o);
                    break;
                case DPS_CommandEnum.ID_attackRefreshHit:
                    attackRefreshHit(o);
                    break;
                case DPS_CommandEnum.ID_addToComboCounter:
                    addComboCounter(com.intArgs[0], o);
                    break;
                case DPS_CommandEnum.ID_addToComboCounterOnHit:
                    addComboCounterOnHit(com.intArgs[0], o);
                    break;


                case DPS_CommandEnum.ID_log_String:
                    Debug.Log(com.stringArgs[0]);
                    break;
            }
        }

        #endregion

        #region commands

        static void sprite(string spr, uint tick, GameWorldObject o)
        {
            if (o.willRest)
            {
                rest(o);
                if (o.lerping)
                    o.lerpCollision = spr;
            }
            else
            {
                o.lastCollision = o.curCollision;
                o.curCollision = spr;
                o.tick = (int)tick;
                o.willRest = true;
            }
        }

        static void lerp(string spr, uint tick, GameWorldObject o)
        {
            if (o.willRest)
            {
                rest(o);
                if (o.lerping)
                    o.lerpCollision = spr;
            }
            else
            {
                o.lerping = true;
                o.lastCollision = o.curCollision;
                o.curCollision = spr;
                o.tick = (int)tick;
                o.willRest = true;
            }
        }

        public static void rest(GameWorldObject o)
        {
            o.rest = true;
        }

        public static void label(uint pos, GameWorldObject o)
        {
            if(!o.labelPositions.ContainsKey(pos))
                o.labelPositions.Add(pos, o.scriptPos);
        }

        public static void label(uint pos, int scriptPos, GameWorldObject o)
        {
            if (!o.labelPositions.ContainsKey(pos))
                o.labelPositions.Add(pos, scriptPos);
        }

        public static void sendToLabel(uint pos, GameWorldObject o)
        {
            if (!o.labelPositions.ContainsKey(pos))
            {
                bool labelFound = false;
                for(int i = o.scriptPos + 1; i < o.states[o.curState].commands.Count; i++)
                    if(o.states[o.curState].commands[i].id == (int)DPS_CommandEnum.ID_label && o.states[o.curState].commands[i].uintArgs[0] == pos)
                    {
                        label(o.states[o.curState].commands[i].uintArgs[0], i, o);
                        labelFound = true;
                        break;
                    }
                if (!labelFound)
                {
                    Debug.Log("Label " + pos + " not found, skipping");
                    return;
                }
            }

            if(o.isInIf > 0)
            {
                o.requestedLabel = (int)pos;
                return;
            }

            o.willRest = false;
            o.scriptPos = o.labelPositions[pos];
            o.requestedLabel = -1;
        }

        public static void enterState(string state, GameWorldObject o)
        {
            o.triggerUpon(1);
            o.hitOrBlockCancels.Clear();
            o.hitCancels.Clear();
            o.blockCancels.Clear();
            o.cancelableStates.Clear();
            o.whiffCancels.Clear();
            o.cancelableStates.Clear();
            o.stateHasHit = false;
            o.hitboxesDisabled = false;
            o.invincible = false;
            o.scriptPos = 0;
            o.tick = 0;
            o.labelPositions.Clear();
            o.tempVariables.Clear();
            o.uponStatements.Clear();
            if(!o.transferMomentum)
            {
                o.xImpulse = 0;
                o.yImpulse = 0;
                o.zImpulse = 0;
                o.xImpulseAdd = 0;
                o.yImpulseAdd = 0;
                o.zImpulseAdd = 0;
            }
            o.transferMomentum = false;
            o.momentumPause = false;
            o.landToState = false;
            o.willRest = false;
            o.switchingState = true;
            o.launchOpponent = false;
            o.lastState = o.curState;
            o.curState = state;

            if (o.commonSubroutines.ContainsKey(state))
            {
                cmnSubroutine(state, o);
                o.comboUsesCount.Clear();
            }
        }

        static void createObject(string state, int offsetX, int offsetY, GameWorldObject o)
        {
            GameObject sub = (GameObject)GameObject.Instantiate(Resources.Load("Char/DefualtChar"));
            GameWorldObject obj = sub.GetComponent<GameWorldObject>();
            DPS_EffectManager eff = obj.effectManager;
            eff.effectList = o.effectManager.effectList;
            eff.effectNames = o.effectManager.effectNames;
            DPS_AudioManager aud = obj.audioManager;
            aud.soundList = o.audioManager.soundList;
            aud.soundNames = o.audioManager.soundNames;
            obj.locX = o.locX + offsetX;
            obj.locY = o.locY + offsetY;
            obj.locZ = o.locZ;
            obj.rotation = o.rotation;
            obj.scale = o.rotation;
            obj.states = o.states;
            obj.subroutines = o.subroutines;
            obj.commonSubroutines = o.commonSubroutines;
            obj.collisions = o.collisions;
            obj.dir = o.dir;
            obj.opponent = o.opponent;
            obj.player = o.player;
            obj.worldObjects.Add(2, o);
            obj.tempVariables = o.tempVariables;
            obj.globalVariables = o.globalVariables;
            obj.isPlayer = false;
            obj.objectStartState = state;
        }

        public static void callSubroutine(string sub, GameWorldObject o)
        {
            if (o.subroutines.ContainsKey(sub))
                for (int i = 0; i < o.subroutines[sub].commands.Count; i++)
                    objSwitchCase(o.subroutines[sub].commands[i], o);
        }

        static void callSubroutineWithArgs(string sub, int arg1, int arg2, 
            int arg3, int arg4, string arg5, GameWorldObject o)
        {
            
        }

        public static void cmnSubroutine(string sub, GameWorldObject o)
        {
            if (o.commonSubroutines.ContainsKey(sub))
                for (int i = 0; i < o.commonSubroutines[sub].commands.Count; i++)
                    objSwitchCase(o.commonSubroutines[sub].commands[i], o);
        }

        public static void createVar(byte table, int id, int data, GameWorldObject o)
        {
            if (table == 0)
                o.globalVariables.Add(id, data);
            else
                o.tempVariables.Add(id, data);
        }

        public static void editVar(byte table, int id, byte type, scriptCommand com, GameWorldObject o)
        {
            int data = 0;
            switch(type)
            {
                default:
                case 0:
                    data = o.globalVariables[com.intArgs[1]];
                    break;
                case 1:
                    data = o.tempVariables[com.intArgs[1]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    data = o.returnInt;
                    break;
                case 3:
                    data = o.opponent.globalVariables[com.intArgs[1]];
                    break;
                case 4:
                    data = com.intArgs[1];
                    break;
            }

            if (table == 0)
                o.globalVariables[id] = data;
            else
                o.tempVariables[id] = data;
        }

        public static int randomNum(int min, int max, GameWorldObject o)
        {
            int randomNum = UnityEngine.Random.Range(1, 100);
            if(randomNum >= min && randomNum <= max)
                return randomNum;
            return 0;
        }

        public static bool compareNum(scriptCommand com, GameWorldObject o)
        {
            byte comNum = 0;
            byte intNum = 0;
            int num1 = 0;
            switch(com.byteArgs[0])
            {
                case 0:
                    num1 = o.globalVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 1:
                    num1 = o.tempVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 2:
                    objSwitchCase(com.commands[comNum], o);
                    comNum++;
                    num1 = o.returnInt;
                    break;
                case 3:
                    num1 = o.opponent.globalVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 4:
                    num1 = com.intArgs[0];
                    intNum++;
                    break;
            }

            int num2 = 0;
            switch (com.byteArgs[1])
            {
                case 0:
                    num2 = o.globalVariables[com.intArgs[intNum]];
                    break;
                case 1:
                    num2 = o.tempVariables[com.intArgs[intNum]];
                    break;
                case 2:
                    objSwitchCase(com.commands[comNum], o);
                    num2 = o.returnInt;
                    break;
                case 3:
                    num2 = o.opponent.globalVariables[com.intArgs[intNum]];
                    break;
                case 4:
                    num2 = com.intArgs[intNum];
                    break;
            }

            switch(com.math)
            {
                default:
                case DPS_MathTypes.equals:
                    return num1 == num2;
                case DPS_MathTypes.less:
                    return num1 < num2;
                case DPS_MathTypes.greater:
                    return num1 > num2;
                case DPS_MathTypes.lessEqual:
                    return num1 <= num2;
                case DPS_MathTypes.greaterEqual:
                    return num1 >= num2;
            }
        }

        public static void physicsXImpulse(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.xImpulse = amount;
                    break;
                case 0:
                    o.xImpulse = o.tempVariables[amount];
                    break;
                case 1:
                    o.xImpulse = o.globalVariables[amount];
                    break;
            }
        }

        public static void physicsYImpulse(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.yImpulse = amount;
                    break;
                case 0:
                    o.yImpulse = o.tempVariables[amount];
                    break;
                case 1:
                    o.yImpulse = o.globalVariables[amount];
                    break;
            }
        }

        public static void physicsZImpulse(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.zImpulse = amount;
                    break;
                case 0:
                    o.zImpulse = o.tempVariables[amount];
                    break;
                case 1:
                    o.zImpulse = o.globalVariables[amount];
                    break;
            }
        }

        public static void xImpulseModifier(byte type, int amount, GameWorldObject o)
        {
            switch(type)
            {
                case 3:
                    o.xImpulseAdd = amount;
                    break;
                case 0:
                    o.xImpulseAdd = o.tempVariables[amount];
                    break;
                case 1:
                    o.xImpulseAdd = o.globalVariables[amount];
                    break;
            }
        }

        public static void yImpulseModifier(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.yImpulseAdd = amount;
                    break;
                case 0:
                    o.yImpulseAdd = o.tempVariables[amount];
                    break;
                case 1:
                    o.yImpulseAdd = o.globalVariables[amount];
                    break;
            }
        }

        public static void zImpulseModifier(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.zImpulseAdd = amount;
                    break;
                case 0:
                    o.zImpulseAdd = o.tempVariables[amount];
                    break;
                case 1:
                    o.zImpulseAdd = o.globalVariables[amount];
                    break;
            }
        }

        public static void addPosX(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.locX += amount * (int)o.dir;
                    break;
                case 0:
                    o.locX += o.tempVariables[amount] * (int)o.dir;
                    break;
                case 1:
                    o.locX += o.globalVariables[amount] * (int)o.dir;
                    break;
            }
        }

        public static void addPosY(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.locY += amount * (int)o.dir;
                    break;
                case 0:
                    o.locY += o.tempVariables[amount] * (int)o.dir;
                    break;
                case 1:
                    o.locY += o.globalVariables[amount] * (int)o.dir;
                    break;
            }
        }

        public static void addPosZ(byte type, int amount, GameWorldObject o)
        {
            switch (type)
            {
                case 3:
                    o.locZ += amount * (int)o.dir;
                    break;
                case 0:
                    o.locZ += o.tempVariables[amount] * (int)o.dir;
                    break;
                case 1:
                    o.locZ += o.globalVariables[amount] * (int)o.dir;
                    break;
            }
        }

        public static int getDistance(GameWorldObject other, GameWorldObject o)
        {
            return other.locX - o.locX;
        }

        public static int doMath(scriptCommand com, GameWorldObject o)
        {
            byte comNum = 0;
            byte intNum = 0;
            int num1 = 0;
            switch (com.byteArgs[0])
            {
                case 0:
                    num1 = o.globalVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 1:
                    num1 = o.tempVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 2:
                    objSwitchCase(com.commands[comNum], o);
                    comNum++;
                    num1 = o.returnInt;
                    break;
                case 3:
                    num1 = o.opponent.globalVariables[com.intArgs[0]];
                    intNum++;
                    break;
                case 4:
                    num1 = com.intArgs[0];
                    intNum++;
                    break;
            }

            int num2 = 0;
            switch (com.byteArgs[1])
            {
                case 0:
                    num2 = o.globalVariables[com.intArgs[intNum]];
                    break;
                case 1:
                    num2 = o.tempVariables[com.intArgs[intNum]];
                    break;
                case 2:
                    objSwitchCase(com.commands[comNum], o);
                    num2 = o.returnInt;
                    break;
                case 3:
                    num2 = o.opponent.globalVariables[com.intArgs[intNum]];
                    break;
                case 4:
                    num2 = com.intArgs[intNum];
                    break;
            }

            switch (com.math)
            {
                default:
                case DPS_MathTypes.add:
                    return num1 + num2;
                case DPS_MathTypes.sub:
                    return num1 - num2;
                case DPS_MathTypes.mul:
                    return num1 * num2;
                case DPS_MathTypes.div:
                    return num1 / num2;
                case DPS_MathTypes.remainder:
                    return num1 % num2;
            }
        }

        public static void addCancel(string state, GameWorldObject o)
        {
            //Debug.Log("add cancel");
            if (o.stateCancelIDs.Contains(state) && !o.cancelableStates.Contains(state))
                o.cancelableStates.Add(state);
        }

        public static void addNeutralCancels(GameWorldObject o)
        {
            for(int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 3 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public static void addNormalCancels(GameWorldObject o)
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 0 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public static void addSpecialCancels(GameWorldObject o)
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 1 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public static void addSuperCancels(GameWorldObject o)
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 2 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public static void hitCancel(string state, GameWorldObject o)
        {
            o.hitCancels.Add(state);
        }

        public static void blockCancel(string state, GameWorldObject o)
        {
            o.blockCancels.Add(state);
        }

        public static void hitOrBlockCancel(string state, GameWorldObject o)
        {
            o.hitOrBlockCancels.Add(state);
        }

        public static void removeCancel(string state, GameWorldObject o)
        {
            o.cancelableStates.Remove(state);
        }

        public static void setStateType(byte type, GameWorldObject o)
        {
            o.stateType = type;
        }    

        public static void setNextState(string state, GameWorldObject o)
        {
            o.nextState = state;
        }

        public static void setLandingState(string state, GameWorldObject o)
        {
            o.landingState = state;
            o.landToState = true;
        }

        public static void transferMomentum(bool b, GameWorldObject o)
        {
            o.transferMomentum = b;
        }

        public static void pauseMomentum(bool b, GameWorldObject o)
        {
            o.momentumPause = b;
        }

        public static void exitState(GameWorldObject o)
        {
            enterState(o.nextState, o);
        }

        public static void playAnimation(string state, float speed, GameWorldObject o)
        {
            o.playingAnim = true;
            if(o.useArmature)
                for(int i = 0; i < o.armatureList.Count; i++)
                {
                    if (!o.renderers[o.armatureList[i]].enabled)
                        continue;
                    o.armatures[o.armatureList[i]].Play(state, 0);
                    o.armatures[o.armatureList[i]].speed = speed;
                }
            else
            {
                o.spriteAnimator.Play(state, 0);
                o.spriteAnimator.speed = speed;
            }
        }

        public static void showMesh(string mesh, bool b, GameWorldObject o)
        {
            o.renderers[mesh].enabled = b;
            o.armatures[mesh].enabled = b;
            o.armatures[mesh].StopPlayback();
        }

        public static void setAnimSpeed(float speed, GameWorldObject o)
        {
            if(o.useArmature)
                for (int i = 0; i < o.armatureList.Count; i++)
                {
                    if (!o.renderers[o.armatureList[i]].enabled)
                        continue;
                    o.armatures[o.armatureList[i]].speed = speed;
                }
            else
                o.spriteAnimator.speed = speed;
        }

        public static void setSpriteIf(string spr, uint time, byte varType, int var, GameWorldObject o)
        {
            bool set;
            if (varType == 0)
                set = o.globalVariables[var] > 0;
            else
                set = o.tempVariables[var] > 0;
            
            if (set)
            {
                o.willRest = false;
                sprite(spr, time, o);
            }
        }

        public static void setSpriteIfNot(string spr, uint time, byte varType, int var, GameWorldObject o)
        {
            bool set;
            if (varType == 0)
                set = o.globalVariables[var] <= 0;
            else
                set = o.tempVariables[var] <= 0;

            if (set)
            {
                o.willRest = false;
                sprite(spr, time, o);
            }
        }

        public static void setSpriteMaterial(int id, GameWorldObject o)
        {
            o.spriteChild.material = o.spriteMaterials[id];
        }

        public static void setSpriteOffset(Vector3 off, GameWorldObject o)
        {
            o.spriteChild.transform.position = off;
        }

        public static void setMeshOffset(Vector3 off, GameWorldObject o)
        {
            o.meshParent.position = off;
        }

        public static void addComboCounter(int i, GameWorldObject o)
        {
            o.comboCounter += i;
        }

        public static void addComboCounterOnHit(int i, GameWorldObject o)
        {
            o.addComboHit = i;
        }

        #endregion

        #region ifCommands

        static void ifCom(scriptCommand com, GameWorldObject o)
        {
            o.isInIf++;

            if(o.ifFailed > 0)
            {
                o.ifFailed++;
                return;
            }

            int checkNum = 0;

            switch (com.byteArgs[0])
            {
                case 0:
                    checkNum = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    checkNum = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    checkNum = o.returnInt;
                    break;
            }

            bool check = checkNum > 0;

            if (!check)
            {
                o.ifFailed++;
                o.canElse = true;
            }
        }

        static void elseCom(GameWorldObject o)
        {
            o.isInIf++;
            if (!o.canElse || o.ifFailed > 0)
            {
                o.ifFailed++;
                return;
            }

            o.canElse = false;
        }

        static void elseIf(scriptCommand com, GameWorldObject o)
        {
            o.isInIf++;
            if (!o.canElse || o.ifFailed > 0)
            {
                o.ifFailed++;
                return;
            }
            o.canElse = false;

            int checkNum = 0;

            switch (com.byteArgs[0])
            {
                case 0:
                    checkNum = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    checkNum = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    checkNum = o.returnInt;
                break;
            }

            bool check = checkNum > 0;

            if (!check)
            {
                o.ifFailed++;
                o.canElse = true;
            }
        }

        static void ifNot(scriptCommand com, GameWorldObject o)
        {
            o.isInIf++;
            if(o.ifFailed > 0)
            {
                o.ifFailed++;
                return;
            }

            int checkNum = 0;

            switch (com.byteArgs[0])
            {
                case 0:
                    checkNum = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    checkNum = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    checkNum = o.returnInt;
                    break;
            }

            bool check = checkNum <= 0;

            if (!check)
            {
                o.ifFailed++;
                o.canElse = true;
            }
        }

        static void elseIfNot(scriptCommand com, GameWorldObject o)
        {
            o.isInIf++;
            if (!o.canElse || o.ifFailed > 0)
            {
                o.ifFailed++;
                return;
            }
            o.canElse = false;

            int checkNum = 0;

            switch (com.byteArgs[0])
            {
                case 0:
                    checkNum = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    checkNum = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    checkNum = o.returnInt;
                    break;
            }

            bool check = checkNum <= 0;

            if (!check)
            {
                o.ifFailed++;
                o.canElse = true;
            }
        }

        static void endIf(GameWorldObject o)
        {
            if(o.ifFailed > 0)
                o.ifFailed--;
            o.isInIf--;
        }

        #endregion

        #region stateRegister

        public static void stateMaxComboUse(uint count, GameWorldObject o)
        {
            o.entryAdd.maxComboUse = count;
        }

        public static void stateRegister(string name, GameWorldObject o)
        {
            o.entryAdd = new StateEntry(name);
        }

        public static void stateConditions(byte stateType, byte usibility, byte lienant, byte attackType,
            byte holdBuffer, byte common, byte arg7, GameWorldObject o)
        {
            o.entryAdd.type = stateType;
            o.entryAdd.useableIn = usibility;
            o.entryAdd.leniantInput = lienant;
            o.entryAdd.attackType = attackType;
            o.entryAdd.holdBuffer = holdBuffer;
            o.entryAdd.common = common;
        }

        public static void stateInput(byte input, GameWorldObject o)
        {
            //Debug.Log(input);
            o.entryAdd.input = input;
        }

        public static void stateButton(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h, GameWorldObject o)
        {
            string button = "";

            if (a > 0) button += "A";
            if (b > 0) button += "B";
            if (c > 0) button += "C";
            if (d > 0) button += "D";
            if (e > 0) button += "E";
            if (f > 0) button += "F";
            if (g > 0) button += "G";
            if (h > 0) button += "H";

            //Debug.Log(button);
            o.entryAdd.button = button;
        }

        public static void stateConditionsSubroutine(byte type, string subroutine, GameWorldObject o)
        {
            o.entryAdd.useSubroutine = true;
            o.entryAdd.subroutineType = type;
            o.entryAdd.subroutine = subroutine;
        }

        public static void stateMeterCost(uint cost, GameWorldObject o)
        {
            o.entryAdd.meterCost = cost;
        }

        public static void stateRegisterEnd(GameWorldObject o)
        {
            o.stateCancels.Add(o.entryAdd.name, o.entryAdd);
            o.stateCancelIDs.Insert(0, o.entryAdd.name);
        }

        public static void setHitstunState(byte animNum, string state, GameWorldObject o)
        {
            o.hitstunAnims.Add(animNum, state);
        }

        #endregion

        #region upon


        private static void upon(byte type, GameWorldObject o)
        {
            o.uponCodeCreate = new uponEntry();
            o.uponCodeCreate.type = type;
            o.isInUpon = true;
        }

        private static void uponEnd(GameWorldObject o)
        {
            if (o.uponStatements.ContainsKey(o.uponCodeCreate.type))
                o.uponStatements.Remove(o.uponCodeCreate.type);
            o.uponStatements.Add(o.uponCodeCreate.type, o.uponCodeCreate);
            o.isInUpon = false;
        }

        public static void clearUpon(byte type, GameWorldObject o)
        {
            o.uponStatements.Remove(type);
        }

        #endregion

        #region attack stuffs

        public static void attackDamage(scriptCommand com, GameWorldObject o)
        {
            switch(com.byteArgs[0])
            {
                case 0:
                    o.damage = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.damage = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.damage = o.returnInt;
                    break;
                case 3:
                    o.damage = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.damage = com.intArgs[0];
                    break;
            }
        }

        public static void attackPushbackX(scriptCommand com, GameWorldObject o)
        {
            switch (com.byteArgs[0])
            {
                case 0:
                    o.pushBackX = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.pushBackX = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.pushBackX = o.returnInt;
                    break;
                case 3:
                    o.pushBackX = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.pushBackX = com.intArgs[0];
                    break;
            }
        }

        public static void attackPushbackY(scriptCommand com, GameWorldObject o)
        {
            switch (com.byteArgs[0])
            {
                case 0:
                    o.pushBackY = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.pushBackY = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.pushBackY = o.returnInt;
                    break;
                case 3:
                    o.pushBackY = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.pushBackY = com.intArgs[0];
                    break;
            }
        }

        public static void attackPushbackZ(scriptCommand com, GameWorldObject o)
        {
            switch (com.byteArgs[0])
            {
                case 0:
                    o.pushBackZ = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.pushBackZ = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.pushBackZ = o.returnInt;
                    break;
                case 3:
                    o.pushBackZ = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.pushBackZ = com.intArgs[0];
                    break;
            }
        }

        public static void attackLaunchOpponent(bool b, GameWorldObject o)
        {
            o.launchOpponent = b;
        }

        public static void attackHitFriction(scriptCommand com, GameWorldObject o)
        {
            switch (com.byteArgs[0])
            {
                case 0:
                    o.friction = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.friction = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.friction = o.returnInt;
                    break;
                case 3:
                    o.friction = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.friction = com.intArgs[0];
                    break;
            }
        }

        public static void attackHitGravity(scriptCommand com, GameWorldObject o)
        {
            switch (com.byteArgs[0])
            {
                case 0:
                    o.hitGravity = o.globalVariables[com.intArgs[0]];
                    break;
                case 1:
                    o.hitGravity = o.tempVariables[com.intArgs[0]];
                    break;
                case 2:
                    objSwitchCase(com.commands[0], o);
                    o.hitGravity = o.returnInt;
                    break;
                case 3:
                    o.hitGravity = o.opponent.globalVariables[com.intArgs[0]];
                    break;
                case 4:
                    o.hitGravity = com.intArgs[0];
                    break;
            }
        }

        public static void attackHitAnim(byte type, byte anim, GameWorldObject o)
        {
            if (type > o.hitAnims.Length)
                return;
            o.hitAnims[type] = anim;
        }

        public static void attackHitstun(int amount, GameWorldObject o)
        {
            o.attackHitstun = amount;
        }

        public static void attackHitstop(int amount, GameWorldObject o)
        {
            o.hitstop = amount;
        }

        public static void attackBlockMultiplier(float amount, GameWorldObject o)
        {
            o.blockMultiplier = amount;
        }

        public static void attackUntechTime(byte type, uint amount, GameWorldObject o)
        {
            if (type > 1)
                type = 1;
            o.untechTime[type] = amount;
        }

        public static void attackHardKnockdown(uint time, GameWorldObject o)
        {
            o.hardKnockdown = time;
        }

        public static void attackHitEffect(byte type, string effect, Vector3 offset, uint time, GameWorldObject o)
        {
            o.hitEff_type = type;
            o.hitEff_str = effect;
            o.hitEff_offset = offset;
            o.hitEff_time = time;
        }

        public static void attackCounterType(byte type, GameWorldObject o)
        {
            o.counterType = type;
        }

        public static void attackChipDamageMultiplier(float amount, GameWorldObject o)
        {
            o.chipMultiplier = amount;
        }

        public static void attackUnblockableType(byte mid, byte crouch, byte air, 
            byte grab, byte projectile, byte arg6, GameWorldObject o)
        {
            o.hitTypes[0] = mid;
            o.hitTypes[1] = crouch;
            o.hitTypes[2] = air;
            o.hitTypes[3] = grab;
            o.hitTypes[4] = projectile;
            o.hitTypes[5] = arg6;
        }

        public static void attackRefreshHit(GameWorldObject o)
        {
            o.hitboxesDisabled = false;
        }

        #endregion

        static bool isIfCommand(int id)
        {
            switch((DPS_CommandEnum)id)
            {
                default:
                    return false;
                case DPS_CommandEnum.ID_if:
                case DPS_CommandEnum.ID_else:
                case DPS_CommandEnum.ID_elseIf:
                case DPS_CommandEnum.ID_ifNot:
                case DPS_CommandEnum.ID_elseIfNot:
                case DPS_CommandEnum.ID_endIf:
                    return true;
            }
        }
    }
}