using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;

namespace DPScript
{
    public class DPS_ObjectCommand : MonoBehaviour
    {
        [SerializeField]
        GameWorldObject o;
        [SerializeField]
        GameWorldObject p;
        [SerializeField]
        GameWorldObject owner;
        DPS_EffectManager e;
        DPS_AudioManager a;


        public void Start()
        {
            o = gameObject.GetComponent<GameWorldObject>();
            owner = o;
            p = o.player;
            e = gameObject.GetComponent<DPS_EffectManager>();
            a = gameObject.GetComponent<DPS_AudioManager>();
        }

        #region switchCase

        public void objSwitchCase(scriptCommand com)
        {
            if (p == null)
                p = o.player;
            if (isInUpon && com.id != 31)
            {
                uponCode.commands.Add(com);
                return;
            }
            if (ifFailed && com.id != 17)
                return;

            switch(com.id)
            {
                case 1:
                    sprite(com.stringArgs[0], com.uintArgs[0]);
                    break;
                case 2:
                    lerp(com.stringArgs[0], com.uintArgs[0]);
                    break;
                case 3:
                    rest();
                    break;
                case 4:
                    label(com.uintArgs[0]);
                    break;
                case 5:
                    sendToLabel(com.uintArgs[0]);
                    break;
                case 6:
                    enterState(com.stringArgs[0]);
                    break;
                case 7:
                    createObject(com.stringArgs[0], com.intArgs[0], com.intArgs[1]);
                    break;
                case 8:
                    callSubroutine(com.stringArgs[0]);
                    break;
                case 10:
                    cmnSubroutine(com.stringArgs[0]);
                    break;
                case 12:
                    ifCom(com);
                    break;
                case 13:
                    if (canElse)
                        elseCom();
                    else
                        ifFailed = true;
                    break;
                case 14:
                    if (canElse)
                        elseIf(com);
                    else
                        ifFailed = true;
                    break;
                case 15:
                    ifNot(com);
                    break;
                case 16:
                    if (canElse)
                        elseIfNot(com);
                    else
                        ifFailed = true;
                    break;
                case 17:
                    endIf();
                    break;
                case 18:
                    o.returnInt = randomNum(com.intArgs[0], com.intArgs[1]);
                    break;
                case 19:
                    createVar(com.byteArgs[0], com.intArgs[0], com.intArgs[1]);
                    break;
                case 20:
                    editVar(com.byteArgs[0], com.intArgs[0], com.intArgs[1]);
                    break;
                case 23:
                    o.returnInt = Convert.ToInt32(compareNum(com));
                    break;
                case 24:
                    o.returnInt = Convert.ToInt32(o.input_CanInput(com.byteArgs[0], "", 1, 0));
                    break;
                case 30:
                    upon(com.byteArgs[0]);
                    break;
                case 31:
                    uponEnd();
                    break;
                case 32:
                    o.triggerUpon(com.byteArgs[0]);
                    break;
                case 33:
                    clearUpon(com.byteArgs[0]);
                    break;
                case 34:
                    if (com.byteArgs[0] == 0)
                        Battle_Manager.Instance.commonPlayer.spawnEffect(com.stringArgs[0],
                            new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]),
                            com.uintArgs[0], o);
                    else
                        e.spawnEffect(com.stringArgs[0], new Vector3(com.floatArgs[0], 
                            com.floatArgs[1], com.floatArgs[2]), com.uintArgs[0]);
                    break;
                case 40:
                    physicsXImpulse(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 41:
                    physicsYImpulse(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 42:
                    physicsZImpulse(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 43:
                    xImpulseModifier(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 44:
                    yImpulseModifier(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 45:
                    yImpulseModifier(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 46:
                    addPosX(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 47:
                    addPosY(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 48:
                    addPosZ(com.byteArgs[0], com.intArgs[0]);
                    break;
                case 49:
                    o.returnInt = getDistance(o.worldObjects[com.intArgs[0]]);
                    break;
                case 60:
                    o.returnInt = doMath(com);
                    break;
                case 100:
                    stateRegister(com.stringArgs[0]);
                    break;
                case 101:
                    stateConditions(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2],
                        com.byteArgs[3], com.byteArgs[4], com.byteArgs[5], com.byteArgs[6]);
                    break;
                case 102:
                    stateInput(com.byteArgs[0]);
                    break;
                case 103:
                    stateButton(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2], com.byteArgs[3], 
                        com.byteArgs[4], com.byteArgs[5], com.byteArgs[6], com.byteArgs[7]);
                    break;
                case 104:
                    stateConditionsSubroutine(com.byteArgs[0], com.stringArgs[0]);
                    break;
                case 105:
                    stateMeterCost(com.uintArgs[0]);
                    break;
                case 106:
                    stateRegisterEnd();
                    break;
                case 109:
                    setHitstunState(com.byteArgs[0], com.stringArgs[0]);
                    break;
                case 110:
                    addCancel(com.stringArgs[0]);
                    break;
                case 111:
                    addNeutralCancels();
                    break;
                case 112:
                    addNormalCancels();
                    break;
                case 113:
                    addSpecialCancels();
                    break;
                case 114:
                    addSuperCancels();
                    break;
                case 115:
                    hitCancel(com.stringArgs[0]);
                    break;
                case 116:
                    blockCancel(com.stringArgs[0]);
                    break;
                case 117:
                    hitOrBlockCancel(com.stringArgs[0]);
                    break;
                case 119:
                    removeCancel(com.stringArgs[0]);
                    break;
                case 120:
                    setStateType(com.byteArgs[0]);
                    break;
                case 121:
                    setNextState(com.stringArgs[0]);
                    break;
                case 122:
                    setLandingState(com.stringArgs[0]);
                    break;
                case 123:
                    transferMomentum(com.boolArgs[0]);
                    break;
                case 124:
                    pauseMomentum(com.boolArgs[0]);
                    break;
                case 125:
                    exitState();
                    break;
                case 126:
                    playAnimation(com.stringArgs[0], com.floatArgs[0]);
                    break;
                case 127:
                    Transform focus = null;
                    if (com.byteArgs[0] == 0)
                        focus = o.transform;
                    CameraManager.Instance.playCameraAnimation(o.cameraAnimator, (float)o.dir, com.stringArgs[0],
                        focus, com.uintArgs[0], com.uintArgs[1], new Vector3(com.floatArgs[0], com.floatArgs[1], com.floatArgs[2]),
                        new Vector3(com.floatArgs[3], com.floatArgs[4], com.floatArgs[5]));
                    break;
                case 129:
                    showMesh(com.stringArgs[0], com.boolArgs[0]);
                    break;
                case 130:
                    setAnimSpeed(com.floatArgs[0]);
                    break;
                case 150:
                    attackDamage(com);
                    break;
                case 151:
                    attackPushbackX(com);
                    break;
                case 152:
                    attackPushbackY(com);
                    break;
                case 153:
                    attackPushbackZ(com);
                    break;
                case 154:
                    attackLaunchOpponent(com.boolArgs[0]);
                    break;
                case 155:
                    attackHitFriction(com);
                    break;
                case 156:
                    attackHitGravity(com);
                    break;
                case 157:
                    attackHitAnim(com.byteArgs[0], com.byteArgs[1]);
                    break;
                case 158:
                    attackHitstun(com.intArgs[0]);
                    break;
                case 159:
                    attackHitstop(com.intArgs[0]);
                    break;
                case 160:
                    attackBlockMultiplier(com.floatArgs[0]);
                    break;
                case 161:
                    attackUntechTime(com.byteArgs[0], com.uintArgs[0]);
                    break;
                case 162:
                    attackHardKnockdown(com.uintArgs[0]);
                    break;
                case 163:
                    attackHitEffect(com.byteArgs[0], com.stringArgs[0], new Vector3(com.floatArgs[0],
                        com.floatArgs[1], com.floatArgs[2]), com.uintArgs[0]);
                    break;
                case 164:
                    attackCounterType(com.byteArgs[0]);
                    break;
                case 165:
                    attackChipDamageMultiplier(com.floatArgs[0]);
                    break;
                case 166:
                    attackUnblockableType(com.byteArgs[0], com.byteArgs[1], com.byteArgs[2],
                        com.byteArgs[3], com.byteArgs[4], com.byteArgs[5]);
                    break;
                case 169:
                    attackRefreshHit();
                    break;
            }
        }

        #endregion

        #region commands

        void sprite(string spr, uint tick)
        {
            if (o.willRest)
            {
                rest();
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

        void lerp(string spr, uint tick)
        {
            if (o.willRest)
            {
                rest();
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

        public void rest()
        {
            o.rest = true;
        }

        public void label(uint pos)
        {
            if(!o.labelPositions.ContainsKey(pos))
                o.labelPositions.Add(pos, o.scriptPos);
        }

        public void label(uint pos, int scriptPos)
        {
            if (!o.labelPositions.ContainsKey(pos))
                o.labelPositions.Add(pos, scriptPos);
        }

        void sendToLabel(uint pos)
        {
            if (!o.labelPositions.ContainsKey(pos))
            {
                bool labelFound = false;
                for(int i = o.scriptPos + 1; i < o.states[o.curState].commands.Count; i++)
                    if(o.states[o.curState].commands[i].id == 4 && o.states[o.curState].commands[i].uintArgs[0] == pos)
                    {
                        label(o.states[o.curState].commands[i].uintArgs[0], i);
                        labelFound = true;
                        break;
                    }
                if (!labelFound)
                {
                    Debug.LogError("Label " + pos + " not found, skipping");
                    return;
                }
            }

            o.willRest = false;
            o.scriptPos = o.labelPositions[pos];
        }

        public void enterState(string state)
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
                cmnSubroutine(state);
        }

        void createObject(string state, int offsetX, int offsetY)
        {
            GameObject sub = new GameObject(state +  "__" + o.idStr);
            GameWorldObject obj = sub.AddComponent<GameWorldObject>();
            DPS_AudioManager aud = sub.AddComponent<DPS_AudioManager>();
            aud = a;
            DPS_EffectManager eff = sub.AddComponent<DPS_EffectManager>();
            eff = e;
            sub.AddComponent<Rigidbody>();
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
            obj.player = p;
            obj.worldObjects.Add(2, o);
            obj.tempVariables = o.tempVariables;
            obj.globalVariables = o.globalVariables;
            obj.isPlayer = false;
            obj.objectStartState = state;

            GameObject mesh = new GameObject("Mesh");
            mesh.transform.parent = sub.transform;
            GameObject col = new GameObject("Collision");
            col.transform.parent = sub.transform;
            GameObject voices = new GameObject("Voices");
            voices.transform.parent = sub.transform;
            voices.AddComponent<AudioSource>();
            GameObject sounds = new GameObject("Sounds");
            sounds.transform.parent = sub.transform;
            GameObject spr = new GameObject("Sprites");
            SpriteRenderer sprRenderer = spr.AddComponent<SpriteRenderer>();
            sprRenderer = o.spriteAnimator.GetComponent<SpriteRenderer>();
            Animator sprAnim = spr.AddComponent<Animator>();
            sprAnim = o.spriteAnimator;
            GameObject eff2 = new GameObject("Effects");
            eff2.transform.parent = sub.transform;
        }

        public void callSubroutine(string sub)
        {
            if (o.subroutines.ContainsKey(sub))
                for (int i = 0; i < o.subroutines[sub].commands.Count; i++)
                    objSwitchCase(o.subroutines[sub].commands[i]);
        }

        void callSubroutineWithArgs(string sub, int arg1, int arg2, 
            int arg3, int arg4, string arg5)
        {
            
        }

        public void cmnSubroutine(string sub)
        {
            if (o.commonSubroutines.ContainsKey(sub))
                for (int i = 0; i < o.commonSubroutines[sub].commands.Count; i++)
                    objSwitchCase(o.commonSubroutines[sub].commands[i]);
        }

        public void createVar(byte table, int id, int data)
        {
            if (table == 0)
                o.globalVariables.Add(id, data);
            else
                o.tempVariables.Add(id, data);
        }

        public void editVar(byte table, int id, int data)
        {
            if (table == 0)
                o.globalVariables[id] = data;
            else
                o.tempVariables[id] = data;
        }

        public int randomNum(int min, int max)
        {
            int randomNum = UnityEngine.Random.Range(1, 100);
            if(randomNum >= min && randomNum <= max)
                return randomNum;
            return 0;
        }

        public bool compareNum(scriptCommand com)
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
                    objSwitchCase(com.commands[comNum]);
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
                    objSwitchCase(com.commands[comNum]);
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

        public void physicsXImpulse(byte type, int amount)
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

        public void physicsYImpulse(byte type, int amount)
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

        public void physicsZImpulse(byte type, int amount)
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

        public void xImpulseModifier(byte type, int amount)
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

        public void yImpulseModifier(byte type, int amount)
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

        public void zImpulseModifier(byte type, int amount)
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

        public void addPosX(byte type, int amount)
        {
            switch (type)
            {
                case 3:
                    o.locX += amount;
                    break;
                case 0:
                    o.locX += o.tempVariables[amount];
                    break;
                case 1:
                    o.locX += o.globalVariables[amount];
                    break;
            }
        }

        public void addPosY(byte type, int amount)
        {
            switch (type)
            {
                case 3:
                    o.locY += amount;
                    break;
                case 0:
                    o.locY += o.tempVariables[amount];
                    break;
                case 1:
                    o.locY += o.globalVariables[amount];
                    break;
            }
        }

        public void addPosZ(byte type, int amount)
        {
            switch (type)
            {
                case 3:
                    o.locZ += amount;
                    break;
                case 0:
                    o.locZ += o.tempVariables[amount];
                    break;
                case 1:
                    o.locZ += o.globalVariables[amount];
                    break;
            }
        }

        public int getDistance(GameWorldObject other)
        {
            return other.locX - o.locX;
        }

        public int doMath(scriptCommand com)
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
                    objSwitchCase(com.commands[comNum]);
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
                    objSwitchCase(com.commands[comNum]);
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

        public void addCancel(string state)
        {
            //Debug.Log("add cancel");
            if (o.stateCancelIDs.Contains(state) && !o.cancelableStates.Contains(state))
                o.cancelableStates.Add(state);
        }

        public void addNeutralCancels()
        {
            for(int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 3 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public void addNormalCancels()
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 0 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public void addSpecialCancels()
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 1 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public void addSuperCancels()
        {
            for (int i = 0; i < o.stateCancelIDs.Count; i++)
            {
                if (o.stateCancels[o.stateCancelIDs[i]].attackType == 2 && o.curState != o.stateCancelIDs[i]
                    && !o.cancelableStates.Contains(o.stateCancelIDs[i]) && o.stateCancels[o.stateCancelIDs[i]].useableIn != 1
                    && o.stateType == o.stateCancels[o.stateCancelIDs[i]].type)
                    o.cancelableStates.Add(o.stateCancelIDs[i]);
            }
        }

        public void hitCancel(string state)
        {
            o.hitCancels.Add(state);
        }

        public void blockCancel(string state)
        {
            o.blockCancels.Add(state);
        }

        public void hitOrBlockCancel(string state)
        {
            o.hitOrBlockCancels.Add(state);
        }

        public void removeCancel(string state)
        {
            o.cancelableStates.Remove(state);
        }

        public void setStateType(byte type)
        {
            o.stateType = type;
        }    

        public void setNextState(string state)
        {
            o.nextState = state;
        }

        public void setLandingState(string state)
        {
            o.landingState = state;
            o.landToState = true;
        }

        public void transferMomentum(bool b)
        {
            o.transferMomentum = b;
        }

        public void pauseMomentum(bool b)
        {
            o.momentumPause = b;
        }

        public void exitState()
        {
            enterState(o.nextState);
        }

        public void playAnimation(string state, float speed)
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

        public void showMesh(string mesh, bool b)
        {
            o.renderers[mesh].enabled = b;
            o.armatures[mesh].enabled = b;
            o.armatures[mesh].StopPlayback();
        }

        public void setAnimSpeed(float speed)
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

        #endregion

        #region ifCommands

        private bool ifFailed = false;
        private bool canElse = false;

        void ifCom(scriptCommand com)
        {
            bool callCom = false;
            if (com.byteArgs[0] == 2)
                callCom = true;

            int checkNum = 0;

            if(callCom)
            {
                objSwitchCase(com.commands[0]);
                checkNum = o.returnInt;
            }
            else
            {
                switch(com.byteArgs[0])
                {
                    case 0:
                        checkNum = o.globalVariables[com.intArgs[0]];
                        break;
                    case 1:
                        checkNum = o.tempVariables[com.intArgs[0]];
                        break;
                }
            }

            if (checkNum <= 0)
            {
                ifFailed = true;
                canElse = true;
            }
            else
            {
                ifFailed = false;
                canElse = false;
            }
        }

        void elseCom()
        {
            if (!canElse && ifFailed)
                return;

            canElse = false;
        }

        void elseIf(scriptCommand com)
        {
            if (!canElse && ifFailed)
                return;

            bool callCom = false;
            if (com.byteArgs[0] == 2)
                callCom = true;

            int checkNum = 0;

            if (callCom)
            {
                objSwitchCase(com.commands[0]);
                checkNum = o.returnInt;
            }
            else
            {
                switch (com.byteArgs[0])
                {
                    case 0:
                        checkNum = o.globalVariables[com.intArgs[0]];
                        break;
                    case 1:
                        checkNum = o.tempVariables[com.intArgs[0]];
                        break;
                }
            }

            if (checkNum <= 0)
            {
                ifFailed = true;
                canElse = true;
            }
            else
            {
                ifFailed = false;
                canElse = false;
            }
        }

        void ifNot(scriptCommand com)
        {
            bool callCom = false;
            if (com.byteArgs[0] == 2)
                callCom = true;

            int checkNum = 0;

            if (callCom)
            {
                //Debug.Log(com.commands[0].id);
                //Debug.Log(com.commands[0].intArgs.Count);
                //Debug.Log(com.commands.Count);
                objSwitchCase(com.commands[0]);
                checkNum = o.returnInt;
            }
            else
            {
                switch (com.byteArgs[0])
                {
                    case 0:
                        checkNum = o.globalVariables[com.intArgs[0]];
                        break;
                    case 1:
                        checkNum = o.tempVariables[com.intArgs[0]];
                        break;
                }
            }

            if (checkNum > 0)
            {
                ifFailed = true;
                canElse = true;
            }
            else
            {
                ifFailed = false;
                canElse = false;
            }    
        }

        void elseIfNot(scriptCommand com)
        {
            if (!canElse && ifFailed)
                return;

            bool callCom = false;
            if (com.byteArgs[0] == 2)
                callCom = true;

            int checkNum = 0;

            if (callCom)
            {
                objSwitchCase(com.commands[0]);
                checkNum = o.returnInt;
            }
            else
            {
                switch (com.byteArgs[0])
                {
                    case 0:
                        checkNum = o.globalVariables[com.intArgs[0]];
                        break;
                    case 1:
                        checkNum = o.tempVariables[com.intArgs[0]];
                        break;
                }
            }

            if (checkNum > 0)
            {
                ifFailed = true;
                canElse = true;
            }
            else
            {
                ifFailed = false;
                canElse = false;
            }
        }

        void endIf()
        {
            ifFailed = false;
        }

        #endregion

        #region stateRegister

        StateEntry entryAdd;

        public void stateRegister(string name)
        {
            entryAdd = new StateEntry(name);
        }

        public void stateConditions(byte stateType, byte usibility, byte lienant, byte attackType,
            byte holdBuffer, byte common, byte arg7)
        {
            entryAdd.type = stateType;
            entryAdd.useableIn = usibility;
            entryAdd.leniantInput = lienant;
            entryAdd.attackType = attackType;
            entryAdd.holdBuffer = holdBuffer;
            entryAdd.common = common;
        }

        public void stateInput(byte input)
        {
            //Debug.Log(input);
            entryAdd.input = input;
        }

        public void stateButton(byte a, byte b, byte c, byte d, byte e, byte f, byte g, byte h)
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
            entryAdd.button = button;
        }

        public void stateConditionsSubroutine(byte type, string subroutine)
        {
            entryAdd.useSubroutine = true;
            entryAdd.subroutineType = type;
            entryAdd.subroutine = subroutine;
        }

        public void stateMeterCost(uint cost)
        {
            entryAdd.meterCost = cost;
        }

        public void stateRegisterEnd()
        {
            o.stateCancels.Add(entryAdd.name, entryAdd);
            o.stateCancelIDs.Insert(0, entryAdd.name);
        }

        public void setHitstunState(byte animNum, string state)
        {
            o.hitstunAnims.Add(animNum, state);
        }

        #endregion

        #region upon


        private uponEntry uponCode;
        private bool isInUpon;

        private void upon(byte type)
        {
            uponCode = new uponEntry();
            uponCode.type = type;
            isInUpon = true;
        }

        private void uponEnd()
        {
            if (o.uponStatements.ContainsKey(uponCode.type))
                o.uponStatements.Remove(uponCode.type);
            o.uponStatements.Add(uponCode.type, uponCode);
            isInUpon = false;
        }

        public void clearUpon(byte type)
        {
            o.uponStatements.Remove(type);
        }

        #endregion

        #region attack stuffs

        public void attackDamage(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackPushbackX(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackPushbackY(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackPushbackZ(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackLaunchOpponent(bool b)
        {
            o.launchOpponent = b;
        }

        public void attackHitFriction(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackHitGravity(scriptCommand com)
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
                    objSwitchCase(com.commands[0]);
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

        public void attackHitAnim(byte type, byte anim)
        {
            if (type > o.hitAnims.Length)
                return;
            o.hitAnims[type] = anim;
        }

        public void attackHitstun(int amount)
        {
            o.attackHitstun = amount;
        }

        public void attackHitstop(int amount)
        {
            o.hitstop = amount;
        }

        public void attackBlockMultiplier(float amount)
        {
            o.blockMultiplier = amount;
        }

        public void attackUntechTime(byte type, uint amount)
        {
            if (type > 1)
                type = 1;
            o.untechTime[type] = amount;
        }

        public void attackHardKnockdown(uint time)
        {
            o.hardKnockdown = time;
        }

        public void attackHitEffect(byte type, string effect, Vector3 offset, uint time)
        {
            o.hitEff_type = type;
            o.hitEff_str = effect;
            o.hitEff_offset = offset;
            o.hitEff_time = time;
        }

        public void attackCounterType(byte type)
        {
            o.counterType = type;
        }

        public void attackChipDamageMultiplier(float amount)
        {
            o.chipMultiplier = amount;
        }

        public void attackUnblockableType(byte mid, byte crouch, byte air, 
            byte grab, byte projectile, byte arg6)
        {
            o.hitTypes[0] = mid;
            o.hitTypes[1] = crouch;
            o.hitTypes[2] = air;
            o.hitTypes[3] = grab;
            o.hitTypes[4] = projectile;
            o.hitTypes[5] = arg6;
        }

        public void attackRefreshHit()
        {
            o.hitboxesDisabled = false;
        }

        #endregion
    }
}