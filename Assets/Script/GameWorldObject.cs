using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DPScript;
using System;

public class GameWorldObject : MonoBehaviour
{
    public Dictionary<string, scriptEntry> states = new Dictionary<string, scriptEntry>();
    public Dictionary<string, scriptEntry> subroutines = new Dictionary<string, scriptEntry>();
    public Dictionary<string, scriptEntry> commonSubroutines = new Dictionary<string, scriptEntry>();
    public Dictionary<string, collisionEntry> collisions = new Dictionary<string, collisionEntry>();
    public Dictionary<byte, uponEntry> uponStatements = new Dictionary<byte, uponEntry>();
    public Dictionary<string, StateEntry> stateCancels = new Dictionary<string, StateEntry>();

    public List<string> stateCancelIDs = new List<string>();

    public byte playerNum;
    public string idStr;
    public DPS_AudioManager audioManager;
    public DPS_EffectManager effectManager;
    Transform cameraMain;
    public Transform meshParent;

    [SerializeField]
    GameObject CollisionChild;
    [SerializeField]
    PlayerInput inputs;
    public SpriteRenderer spriteChild;

    public Dictionary<int, GameWorldObject> worldObjects = new Dictionary<int, GameWorldObject>();
    public GameWorldObject player, opponent, attacker, attacking;
    Object_Collision hitBoxThatHit, hurtBoxThatWasHit;

    public int curHealth = 1, maxHealth = 1;
    public int tick = 0, hitstopTick = 0, onStage = 0, scriptPos = 0;
    public ObjectDir dir = (ObjectDir)1;
    public ObjectDirType dirType = ObjectDirType.scale;
    public string curState = "", lastState = "", nextState = "", landingState = "";
    public bool landToState = false;
    public string curCollision = "", lastCollision = "", lerpCollision = "";
    public int locX = 0, locY = 0, locZ = 0; 
    public int xImpulse = 0, yImpulse = 0, zImpulse = 0;
    public int xImpulseAdd = 0, yImpulseAdd = 0, zImpulseAdd = 0;
    public bool isInHKD = false;
    public int HKDtimer = 0;
    public bool transferMomentum = false;
    public int distance = 0;
    public bool hasWallCollision = true;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public byte stateType = 0;
    public bool stateHasHit = false;
    public int comboCounter = 0, addComboHit = 1;
    public bool momentumPause = false;
    public bool rest = false, willRest = false, lerping = false, switchingState = false;
    public string returnString = "";
    public int returnInt = 0;
    public bool faceCamera = true;
    public bool playingAnim = false;
    public float animTime = 0;
    public bool invincible = false, hitboxesDisabled = false, noCollision = false, ignoreFreezes = false;
    public byte[] armourTypes = { 0, 0, 0, 0, 0, 0 };
    public bool friendlyFire = false; //doesnt work if they are on the same parent
    public bool isProjectile = false;
    public string objectStartState = "CmnStand";
    public bool isPlayer = true, isActive = true;
    public bool willHit = false, willBeHit = false, willClash = false;
    public uint superFreezeTime = 0;
    public int projectileLevel = 1;
    public Dictionary<int, int> globalVariables = new Dictionary<int, int>(), tempVariables = new Dictionary<int, int>();
    public Dictionary<uint, int> labelPositions = new Dictionary<uint, int>();

    public Dictionary<byte, collisionBox> boxes = new Dictionary<byte, collisionBox>();
    public Dictionary<byte, collisionChunk> chunks = new Dictionary<byte, collisionChunk>();

    public bool useArmature = false;
    public List<string> armatureList = new List<string>();
    public Dictionary<string, Animator> armatures = new Dictionary<string, Animator>();
    public Dictionary<string, SkinnedMeshRenderer> renderers = new Dictionary<string, SkinnedMeshRenderer>();
    public RuntimeAnimatorController cameraAnimator;
    public Animator spriteAnimator;

    public Dictionary<int, DPS_Stage> world = new Dictionary<int, DPS_Stage>();
    public List<Object_Collision> loadedCollisions = new List<Object_Collision>();

    public List<string> hitOrBlockCancels = new List<string>(), hitCancels = new List<string>(), blockCancels = new List<string>(), whiffCancels = new List<string>();
    public List<string> cancelableStates = new List<string>();

    public Dictionary<byte, string> hitstunAnims = new Dictionary<byte, string>();

    [SerializeField]
    private InputElement currInput = new InputElement(5);
    [SerializeField]
    private List<InputElement> buffer = new List<InputElement>();
    public Dictionary<string, uint> comboUsesCount = new Dictionary<string, uint>();

    public List<Material> spriteMaterials = new List<Material>();

    //some various variables for misc stuff
    public int[] walkingSpeed = new int[] { 4000, -3000 }; //fwalk, bwalk
    public int[] dashSpeed = new int[] { 6000, 400, 8200 }; //inital speed, accel, max
    public int[] defualtAirActionsCount = new int[] { 1, 1, 1 }; //air jump, fdash, bdash
    public int[] airActionsCount = new int[] { 1, 1, 1 }; //air jump, fdash, bdash
    public int[] jumpSpeed = new int[] { 5000, -5000, 15000 }; //fspeed, bspeed, height
    public int defaultGravity = -900;
    public float weightMultiplier = 1;

    public bool inHitstun = false;
    public int hitstun = 0;

    public byte[] hitAnims = { 0, 0, 0 };
    public int damage = 0, pushBackX = 0, pushBackY = 0, pushBackZ = 0, friction = 0, hitGravity = 0, attackHitstun = 0, hitstop = 0;
    public uint[] untechTime = { 0, 0 };
    public uint hardKnockdown = 0;
    public float blockMultiplier = 1, chipMultiplier = 1;
    public bool launchOpponent = false;
    public byte counterType = 0;
    public byte[] hitTypes = { 0, 0, 0, 0, 0, 0 };

    public byte hitEff_type = 0;
    public string hitEff_str = "";
    public Vector3 hitEff_offset = Vector3.zero;
    public uint hitEff_time = 0;

    public string killCamAnim = "";
    public byte killCamAnimType = 0;
    public Vector3 killCamAnimOffset = Vector3.zero, killCamAnimRot = Vector3.zero;
    public int killCamAnimBlendInTime = 0, killCamAnimBlendOutTime = 0;

    public bool comboIsValid = true, isInCombo = false;

    public int isInIf = 0, ifFailed = 0;
    public int requestedLabel = -1;
    public bool isInUpon = false;
    public uponEntry uponCodeCreate;
    public bool canElse = false;
    public StateEntry entryAdd;


    [SerializeField]
    bool isInDebug = false;
    [SerializeField]
    bool debugNoLoad = false;
    [SerializeField]
    [Tooltip("Will log the data in the variable")]
    int logVar = -1;
    [SerializeField]
    [Tooltip("Will log if this upon currently exists")]
    int logHasUpon = -1;

    public bool initalized = false;
    public bool ignoreInputs = false;

    public bool generateColBox = false;
    public Vector3 standingColPos = Vector3.zero;
    public Vector3 standingColSize = Vector3.zero;
    public Vector3 crouchingColPos = Vector3.zero;
    public Vector3 crouchingColSize = Vector3.zero;
    public Vector3 airealColPos = Vector3.zero;
    public Vector3 airealColSize = Vector3.zero;

    public void recallAwake()
    {
        Awake();
    }

    private void Awake()
    {
        cameraMain = GameObject.Find("Main Camera").GetComponent<Transform>();
        //GameObject temp = transform.Find("Meshes").gameObject;

        for (int i = 0; i < armatureList.Count; i++)
        {
            armatures.Add(armatureList[i], meshParent.Find(armatureList[i]).GetComponent<Animator>());
            armatures[armatureList[i]].speed = 0;
            renderers.Add(armatureList[i], meshParent.Find(armatureList[i]).GetComponentInChildren<SkinnedMeshRenderer>());
        }

        if (debugNoLoad)
            return;

        if (isInDebug)
            Objects_Load.debugLoad("Char/" + idStr + "/" + idStr + "_load", this, idStr, true);
    }

    private void Start()
    {
        //Application.targetFrameRate = 60;

        if (!isPlayer)
            return;

        for (int i = 0; i < Battle_Manager.Instance.stages.Count; i++)
            world.Add(Battle_Manager.Instance.stages[i].id, Battle_Manager.Instance.stages[i]);
        Battle_Manager.Instance.players.Add(this); 
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        //if (!ignoreFreezes && battleManager.superFreeze && superFreezeTime <= 0)
        //  return;
        //input_DebugBuffer();
#if UNITY_EDITOR
        if (logVar > -1 && globalVariables.ContainsKey(logVar))
            Debug.Log(globalVariables[logVar]);
        if (logHasUpon > -1)
            Debug.Log("Upon " + logHasUpon + " exists: " + uponStatements.ContainsKey((byte)logHasUpon));
#endif

        if(!initalized)
        {
            if(!isPlayer)
            {
                DPS_ObjectCommand.enterState(objectStartState, this);
                initalized = true;
                return;
            }
            player = this;
            for(int i = 0; i < Battle_Manager.Instance.players.Count; i++)
                if (Battle_Manager.Instance.players[i] != this) 
                {
                    opponent = Battle_Manager.Instance.players[i];
                    worldObjects.Add(3, opponent);
                    break;
                }
            if (opponent == null)
                return;
            addGlobalVariables();
            DPS_ObjectCommand.callSubroutine("init", this);
            DPS_ObjectCommand.cmnSubroutine("cmnInit", this);
            DPS_ObjectCommand.enterState(objectStartState, this);
            initalized = true;
            return;
        }

        if (debugNoLoad)
            return;

        if (opponent.inHitstun)
        {
            isInCombo = true;
            if (opponent.hitstun <= 0)
                comboIsValid = false;
        }
        else
            comboCounter = 0;

        if (isPlayer)
            inputUpdate();

        rotateToCamera();
        hitUpdate();

        if (hitstopTick > 0)
        {
            hitstopTick--;
            return;
        }

        positionUpdate();
        scaleUpdate();

        tick--;
        triggerUpon(3);

        if (hitstun > 0)
        {
            hitstun--;
            if (hitstun == 0)
            { xImpulse = 0; xImpulseAdd = 0; zImpulse = 0; zImpulseAdd = 0; }
            globalVariables[18] = hitstun;
        }

        if (untechTime[0] > 0 || untechTime[1] > 0)
        {
            if (stateType == 0 || stateType == 1)
                untechTime[1]--;
            else
                untechTime[0]--;

            globalVariables[19] = (int)untechTime[0];
            globalVariables[20] = (int)untechTime[1];
        }

        if (tick <= 0)
        {
            lerping = false;
            willRest = false;
            rest = false;
            tickUpdate();

            triggerUpon(4);

            colUpdate();

            if (lerping && !playingAnim)
                if (collisions.ContainsKey(lerpCollision))
                    lerpSprite();
        }

        if (playingAnim)
        {
            if (useArmature)
                for (int i = 0; i < armatureList.Count; i++)
                    if (renderers[armatureList[i]].enabled)
                    {
                        if (armatures[armatureList[i]].GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                            playingAnim = false;
                        break;
                    }
                    else
                if (spriteAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                        playingAnim = false;
        }
    }

    private void tickUpdate()
    {
        while (!rest)
        {
            switchingState = false;
            if (scriptPos + 1 > states[curState].commands.Count)
            { DPS_ObjectCommand.enterState(nextState, this); continue; }

            DPS_ObjectCommand.objSwitchCase(states[curState].commands[scriptPos], this);
            if (requestedLabel >= 0 && isInIf <= 0)
                DPS_ObjectCommand.sendToLabel((uint)requestedLabel, this);

            if (switchingState)
                continue;

            if (!rest || states[curState].commands[scriptPos].id == 3)
                scriptPos++;

            if (scriptPos + 1 > states[curState].commands.Count)
                break;
        }
    }

    private void colUpdate()
    {
        if (curCollision == lastCollision)
            return;

        if (!playingAnim)
            switchSprite();

        while (loadedCollisions.Count > 0)
            loadedCollisions[0].kill();

        if (collisions.ContainsKey(curCollision))
        {
            for (int i = 0; i < collisions[curCollision].boxCount; i++)
            {
                GameObject col = new GameObject();
                col.transform.position = transform.position;
                col.transform.rotation = transform.rotation;
                if (dir == ObjectDir.dir_Left)
                    col.transform.Rotate(0, 180, 0);
                col.transform.localScale = scale;
                col.transform.parent = CollisionChild.transform;
                Object_Collision temp = col.AddComponent<Object_Collision>();
                temp.init(collisions[curCollision].boxes[i], collisions[curCollision].sphere, player, this);
                loadedCollisions.Add(temp);
                Battle_Manager.Instance.collisions.Add(temp);
            }
            if (!collisionEntry.containsBoxType(collisions[curCollision], 0) && generateColBox)
            {
                GameObject col = new GameObject();
                col.transform.position = transform.position;
                col.transform.rotation = transform.rotation;
                if (dir == ObjectDir.dir_Left)
                    col.transform.Rotate(0, 180, 0);
                col.transform.localScale = scale;
                col.transform.parent = CollisionChild.transform;
                Object_Collision temp = col.AddComponent<Object_Collision>();
                switch (stateType)
                {
                    case 0:
                        temp.init(standingColPos, standingColSize, 0, 0, collisions[curCollision].sphere, player, this);
                        break;
                    case 1:
                        temp.init(crouchingColPos, crouchingColSize, 0, 0, collisions[curCollision].sphere, player, this);
                        break;
                    case 2:
                        temp.init(airealColPos, airealColSize, 0, 0, collisions[curCollision].sphere, player, this);
                        break;
                }
                loadedCollisions.Add(temp);
                Battle_Manager.Instance.collisions.Add(temp);
            }
        }
    }

    private void switchSprite()
    {
        if (!collisions.ContainsKey(curCollision))
            return;
        string state = collisions[curCollision].sprites[0].Remove(collisions[curCollision].sprites[0].LastIndexOf("_"));
        int frame = Int32.Parse(collisions[curCollision].sprites[0].Remove(0, collisions[curCollision].sprites[0].LastIndexOf("_") + 1));

        var stateId = Animator.StringToHash(state);
        if(useArmature)
            for (int i = 0; i < armatureList.Count; i++)
            {
                if (armatures[armatureList[i]].HasState(0, stateId) && renderers[armatureList[i]].enabled)
                {
                    armatures[armatureList[i]].Play(stateId, 0, (float)frame * (1f / 12f) / armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    armatures[armatureList[i]].speed = 0f;
                }
            }
        else if(spriteAnimator.HasState(0, stateId))
        {
            spriteAnimator.Play(stateId, 0, (float)frame * (1f/12f) / spriteAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            spriteAnimator.speed = 0f;
        }
    }

    private void lerpSprite()
    {
        string state = collisions[lerpCollision].sprites[0].Remove(collisions[lerpCollision].sprites[0].LastIndexOf("_"));
        int frame = Int32.Parse(collisions[lerpCollision].sprites[0].Remove(0, collisions[lerpCollision].sprites[0].LastIndexOf("_") + 1));

        var stateId = Animator.StringToHash(state);
        if(useArmature)
            for (int i = 0; i < armatureList.Count; i++)
            {
                if (armatures[armatureList[i]].HasState(0, stateId) && renderers[armatureList[i]].enabled)
                {
                    armatures[armatureList[i]].CrossFade(stateId, 1, 0, (float)frame * (1f / 12f) / armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    armatures[armatureList[i]].speed = 1;
                }
            }
        else if (spriteAnimator.HasState(0, stateId))
        {
            spriteAnimator.Play(stateId, 0, (float)frame * (1f / 12f) / spriteAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            spriteAnimator.speed = 1f;
        }
    }

    private void rotateToCamera()
    {
        if (faceCamera && useArmature)
        {
            meshParent.LookAt(cameraMain);
            meshParent.localEulerAngles = new Vector3(0, meshParent.localEulerAngles.y, 0) + rotation;
        }
        else
            meshParent.localEulerAngles = Vector3.zero;
        if(dirType == ObjectDirType.rotate)
            transform.localEulerAngles = rotation + new Vector3(0, 180, 0);
        else
            transform.localEulerAngles = rotation;
    }

    private void positionUpdate()
    {
        //if(!ignoreFreezes && battleManager.superFreeze && superFreezeTime <= 0)
        //return;
        transform.parent = world[onStage].transform;

        distance = getAbsoluteDistance(this, opponent);

        if (!momentumPause)
        {
            /*
            if (hasWallCollision)
            {
                if (distance + xImpulse > 60000 || Mathf.Abs(locX) + xImpulse > Battle_Manager.Instance.stateWidth)
                    locX += xImpulse * dir;
            }
            else
                locX += xImpulse * dir;
            */
            locX += xImpulse * (int)dir;
            locY += yImpulse; locZ += zImpulse;
            xImpulse += xImpulseAdd; yImpulse += yImpulseAdd; zImpulse += zImpulseAdd;
            if(hasWallCollision)
                if(Mathf.Abs(locX) > Battle_Manager.Instance.stateWidth)
                {
                    int side = (int)Mathf.Sign(locX);
                    locX = Battle_Manager.Instance.stateWidth * side;
                }
            /*
                else if(Mathf.Abs(distance) > CameraManager.Instance.maxNormalZoom)
                {
                    int side = (int)Mathf.Sign(locX);
                    locX = (locX - distance) * side;
                }
            */
        }

        if(hitstun > 0)
        {
            xImpulseAdd -= xImpulseAdd / 5; zImpulseAdd -= zImpulseAdd / 5;
            if((xImpulseAdd < 0 && xImpulse <= 0) || (xImpulseAdd > 0 && xImpulse >= 0))
            { xImpulse = 0; xImpulseAdd = 0; zImpulse = 0; zImpulseAdd = 0; }
        }

        if(locY <= 0)
        {
            locY = 0;
            yImpulse = 0;
            yImpulseAdd = 0;
            triggerUpon(2);
            if(landToState)
                DPS_ObjectCommand.enterState(landingState, this);
        }

        globalVariables[14] = locX;
        globalVariables[15] = locY;
        globalVariables[16] = locZ; 
        globalVariables[22] = xImpulse;
        globalVariables[23] = yImpulse;
        globalVariables[24] = zImpulse;

        transform.localPosition = new Vector3((float)locX / 100000, (float)locY / 100000, (float)locZ / 100000);
    }

    private void scaleUpdate()
    {
        if(dirType == ObjectDirType.scale)
            transform.localScale = new Vector3(scale.x * (int)dir, scale.y, scale.z);
        else
            transform.localScale = new Vector3(scale.x, scale.y, scale.z);
        CollisionChild.transform.localScale.Set(Mathf.Abs(scale.x), scale.y, scale.z);
    }

    public void triggerUpon(byte type)
    {
        bool willDestroy = false;
        byte? newUpon = null;
        if (uponStatements.ContainsKey(type))
            for (int i = 0; i < uponStatements[type].commands.Count; i++)
            {
                if (uponStatements[type].commands[i].id == (int)DPS_CommandEnum.ID_clearUpon && 
                    uponStatements[type].commands[i].byteArgs[0] == type && ifFailed <= 0)
                    willDestroy = true;
                else if (uponStatements[type].commands[i].id == (int)DPS_CommandEnum.ID_triggerUpon)
                    newUpon = uponStatements[type].commands[i].byteArgs[0];
                else
                    DPS_ObjectCommand.objSwitchCase(uponStatements[type].commands[i], this);
            }
        if (willDestroy)
            DPS_ObjectCommand.clearUpon(type, this);
        if (newUpon != null)
            triggerUpon((byte)newUpon);
    }

    public void boxesCollide(Object_Collision child, Object_Collision hit)
    {
        switch(hit.box.type)
        {
            case 1:
                if (child.box.type == 2)
                {
                    willHit = true;
                    attacking = hit.parent;
                }
                break;
            case 2:
                if (child.box.type == 1)
                {
                    willBeHit = true;
                    attacker = hit.parent;
                }
                else if (child.box.type == 2)
                    willClash = true;
                break;
        }
    }

    private void hitUpdate()
    {
        if(willHit)
        {
            triggerUpon(6);
            cancelableStates.AddRange(hitOrBlockCancels);
            hitstopTick = hitstop;

            if (attacking.armourTypes[attacking.stateType] <= hitTypes[attacking.stateType])
            {
                triggerUpon(5);
                comboCounter += addComboHit;
                cancelableStates.AddRange(hitCancels);
                if (hitEff_type == 0)
                    Battle_Manager.Instance.commonPlayer.spawnEffect(hitEff_str, hitEff_offset /*+= new
                        Vector3((hitBoxThatHit.posX - hurtBoxThatWasHit.posX) / 1000, (hitBoxThatHit.posY - hurtBoxThatWasHit.posY) / 250)*/,
                        hitEff_time, this);
                else
                    effectManager.spawnEffect(hitEff_str, hitEff_offset /*+= new Vector3((hitBoxThatHit.posX - 
                        hurtBoxThatWasHit.posX) / 1000, (hitBoxThatHit.posY - hurtBoxThatWasHit.posY) / 250)*/,
                        hitEff_time);
            }
            else
            {
                triggerUpon(7);
                cancelableStates.AddRange(blockCancels);
            }
            willHit = false;
            stateHasHit = true;
            hitboxesDisabled = true;
            comboIsValid = true;
        }

        if(willBeHit)
        {
            hitstopTick = attacker.hitstop;
            if (armourTypes[stateType] <= attacker.hitTypes[stateType])
            {
                DPS_ObjectCommand.enterState(hitstunAnims[attacker.hitAnims[stateType]], this);
                curHealth -= attacker.damage; 
                xImpulse = attacker.pushBackX;
                xImpulseAdd = attacker.friction;
                if (stateType == 2 || attacker.launchOpponent)
                {
                    yImpulse = attacker.pushBackY;
                    yImpulseAdd = attacker.hitGravity;
                }
                zImpulse = attacker.pushBackZ;
                if (zImpulse != 0)
                    zImpulseAdd = attacker.friction;
                hitstun = attacker.attackHitstun;
                globalVariables[18] = hitstun;
                rest = false;
                dir = (ObjectDir)(-(int)attacker.dir);
                tickUpdate();
                colUpdate();
            }
            willBeHit = false;
        }
    }

    public int getAbsoluteDistance(GameWorldObject p1, GameWorldObject p2)
    {
        return p2.locX - p1.locX;
    }

    #region inputs
    [Serializable]
    public class InputElement
    {
        public byte inputType;
        public string button;
        public float updateCount = 0;
        public float chargeTime = 0;
        public bool used = false;

        public InputElement(byte _inputType)
        {
            inputType = _inputType;
        }

        public bool same(InputElement other)
        {
            return inputType == other.inputType && button == other.button;
        }
    }

    private void inputUpdate()
    {
        inputs_AddToBuffer();

        //buffer update
        if (buffer.Count > 0)
        {
            while (buffer.Count > 15)
                buffer.RemoveAt(0);

            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i].updateCount += Time.deltaTime;

                if (buffer[i].updateCount >= 1f)
                    buffer.RemoveAt(i);
            }
        }

        if (ignoreInputs)
            return;

        for (int i = 0; i < cancelableStates.Count; i++)
        {
            if (!stateCancels.ContainsKey(cancelableStates[i]))
                continue;
            StateEntry entry = stateCancels[cancelableStates[i]];

            if (comboUsesCount.ContainsKey(cancelableStates[i]))
                if(entry.maxComboUse <= comboUsesCount[cancelableStates[i]])
                    continue;
            //Debug.Log("Swag");
            //Debug.Log(entry.input + ", " + entry.button);
            if (input_CanInput(entry.input, entry.button, entry.leniantInput,
                entry.holdBuffer))
            {
                if (entry.useSubroutine)
                    if (!stateCheckSubroutine(entry.subroutine, entry.subroutineType))
                        continue;

                if (!comboUsesCount.ContainsKey(cancelableStates[i]))
                    comboUsesCount.Add(cancelableStates[i], 1);
                else
                    comboUsesCount[cancelableStates[i]]++;
                //Debug.Log("boom");
                DPS_ObjectCommand.enterState(entry.name, this);
                break;
            }
        }
    }

    private bool stateCheckSubroutine(string subroutine, byte type)
    {
        DPS_ObjectCommand.createVar(1, 4, 0, this);
        if (type == 0)
            DPS_ObjectCommand.callSubroutine(subroutine, this);
        else
            DPS_ObjectCommand.cmnSubroutine(subroutine, this);
        return Convert.ToBoolean(tempVariables[4]);
    }

    private void inputs_AddToBuffer()
    {
        InputElement _input = new InputElement(5);
        if (buffer.Count > 0) _input.inputType = buffer[buffer.Count - 1].inputType;

        Vector2 directions = inputs.actions["Directions"].ReadValue<Vector2>();
        if (directions[1] <= -0.5 && directions[0] <= -0.5)
             _input.inputType = 1; 
        else if (directions[1] <= -0.5 && directions[0] >= 0.5)
            _input.inputType = 3;
        else if (directions[1] >= 0.5 && directions[0] <= -0.5)
            _input.inputType = 7;
        else if (directions[1] >= 0.5 && directions[0] >= 0.5)
            _input.inputType = 9;
        else if (directions[1] <= -0.5)
            _input.inputType = 2;
        else if (directions[1] >= 0.5)
            _input.inputType = 8;
        else if (directions[0] <= -0.5)
            _input.inputType = 4;
        else if (directions[0] >= 0.5)
            _input.inputType = 6;
        else
            _input.inputType = 5;
        string _but = "";


        if (inputs.actions["Action_A"].IsPressed() && !_but.Contains("A")) _but += "A";
        if (inputs.actions["Action_B"].IsPressed() && !_but.Contains("B")) _but += "B";
        if (inputs.actions["Action_C"].IsPressed() && !_but.Contains("C")) _but += "C";
        if (inputs.actions["Action_D"].IsPressed() && !_but.Contains("D")) _but += "D";
        if (inputs.actions["Action_E"].IsPressed() && !_but.Contains("E")) _but += "E";

        if (inputs.actions["Action_ABCD"].IsPressed()) _but = "ABCD";

        _input.button = _but;

        if (buffer.Count > 0)
        {
            if (buffer[buffer.Count - 1].same(_input))
            {
                buffer[buffer.Count - 1].updateCount = 0;
                buffer[buffer.Count - 1].chargeTime += Time.deltaTime;
                currInput = buffer[buffer.Count - 1];
                return;
            }
        }
        buffer.Add(_input);
        currInput = _input;
    }

    private void input_DebugBuffer()
    {
        string retrn = "";

        for (int i = 0; i < buffer.Count; i++)
        {
            retrn += buffer[i].inputType;
            retrn += ": ";
            retrn += buffer[i].button;
            retrn += ", ";
            retrn += buffer[i].updateCount;
            retrn += "\n";
        }

        Debug.Log(retrn);
    }

    public bool input_CanInput(short type, string _button, byte lieniant, 
        byte holdBuffer)
    {
        if (holdBuffer > currInput.chargeTime)
            return false;


        if (!string.IsNullOrEmpty(_button))
        {
            if (!currInput.button.Contains(_button))
                return false;
            else if (currInput.chargeTime > 3)
                return false;
        }

        if (type == 0)
            type = 5;

        byte[] _input;
        if (type < 10)
            _input = new byte[] { (byte)type };
        else 
            _input = InputSwitchCase.switchCase(type);

        if(dir == ObjectDir.dir_Left)
            //reverse inputs if turned around
            for(int i = 0; i < _input.Length; i++)
                switch(_input[i])
                {
                    case 1:
                        _input[i] = 3;
                        break;
                    case 3:
                        _input[i] = 1;
                        break;
                    case 4:
                        _input[i] = 6;
                        break;
                    case 6:
                        _input[i] = 4;
                        break;
                    case 7:
                        _input[i] = 9;
                        break;
                    case 9:
                        _input[i] = 7;
                        break;
                }

        if (_input.Length == 1)
        {
            if (lieniant == 0)
                switch (_input[0])
                {
                    case 5:
                        if (currInput.inputType == 5 || currInput.inputType == 4 || currInput.inputType == 6) return true;
                        else return false;
                    case 2:
                        if (currInput.inputType == 2 || currInput.inputType == 1 || currInput.inputType == 3) return true;
                        else return false;
                    case 8:
                        if (currInput.inputType == 8 || currInput.inputType == 7 || currInput.inputType == 9) return true;
                        else return false;
                    default:
                        if (currInput.inputType == _input[0]) return true;
                        else return false;
                }
            else if (currInput.inputType == _input[0]) return true;
        }

        if (currInput.inputType != _input[_input.Length - 1]) return false;

        int startBuf = 0;
        for (int i = buffer.Count - 1 - _input.Length; i > 0; i--)
            if (buffer[i].inputType == _input[0])
            {
                startBuf = i;
                break;
            }

        if (buffer.Count >= _input.Length && buffer.Count >= startBuf + _input.Length + 1)
            switch(_input.Length)
            {
                case 0:
                case 1:
                    return true;
                case 2:
                    if (buffer[startBuf].inputType == _input[0] && buffer[startBuf + 1].inputType == 5 &&
                        buffer[startBuf + 2].inputType == _input[1] && buffer[startBuf].updateCount <= .16) return true;
                    break;
                default:
                    bool multiTap = false;
                    for (int i = 0; i < _input.Length - 1; i++)
                    {
                        if (_input[i] != _input[i + 1]) { multiTap = false; break; }
                        multiTap = true;
                    }

                    int taps = 0;
                    if (multiTap)
                    {
                        for (int i = startBuf; i < buffer.Count; i++)
                        {
                            if (taps >= _input.Length) break;
                            if (buffer[i].inputType == _input[taps]) taps++;
                            else if (buffer[i].inputType != 5) break;
                        }
                    }
                    else
                    {
                        int misInput = _input.Length - 4;
                        for (int i = startBuf; i < buffer.Count; i++)
                        {
                            if (taps >= _input.Length) break;
                            if (buffer[i].inputType == _input[taps]) taps++;
                            else if (misInput > 0) misInput--;
                            else break;
                        }
                    }
                    if (taps >= _input.Length) return true;
                    break;
            }
        return false;
    }
    #endregion

    private void addGlobalVariables()
    {
        DPS_ObjectCommand.createVar(0, 0, maxHealth, this);
        DPS_ObjectCommand.createVar(0, 1, curHealth, this);
        DPS_ObjectCommand.createVar(0, 2, walkingSpeed[0], this);
        DPS_ObjectCommand.createVar(0, 3, walkingSpeed[1], this);
        DPS_ObjectCommand.createVar(0, 4, dashSpeed[0], this);
        DPS_ObjectCommand.createVar(0, 5, dashSpeed[1], this);
        DPS_ObjectCommand.createVar(0, 6, dashSpeed[2], this);
        DPS_ObjectCommand.createVar(0, 7, airActionsCount[0], this);
        DPS_ObjectCommand.createVar(0, 8, airActionsCount[1], this);
        DPS_ObjectCommand.createVar(0, 9, airActionsCount[2], this);
        DPS_ObjectCommand.createVar(0, 10, jumpSpeed[0], this);
        DPS_ObjectCommand.createVar(0, 11, jumpSpeed[1], this);
        DPS_ObjectCommand.createVar(0, 12, jumpSpeed[2], this);
        DPS_ObjectCommand.createVar(0, 13, defaultGravity, this);
        DPS_ObjectCommand.createVar(0, 14, locX, this);
        DPS_ObjectCommand.createVar(0, 15, locY, this);
        DPS_ObjectCommand.createVar(0, 16, locZ, this);
        DPS_ObjectCommand.createVar(0, 18, hitstun, this);
        DPS_ObjectCommand.createVar(0, 19, (int)untechTime[0], this);
        DPS_ObjectCommand.createVar(0, 20, (int)untechTime[1], this);
        DPS_ObjectCommand.createVar(0, 21, HKDtimer, this);
        DPS_ObjectCommand.createVar(0, 22, xImpulse, this);
        DPS_ObjectCommand.createVar(0, 23, yImpulse, this);
        DPS_ObjectCommand.createVar(0, 24, zImpulse, this);
    }

    private void kill()
    {
        foreach (Object_Collision col in loadedCollisions)
            col.kill();
        Destroy(gameObject);
        Destroy(meshParent);
        Destroy(CollisionChild);
        Destroy(audioManager.voiceParent.gameObject);
        Destroy(audioManager.soundsParent.gameObject);
        Destroy(spriteAnimator.gameObject);
        Destroy(effectManager.effectSpawner);
    }
}

[Serializable]
public enum ObjectDir
{
    dir_Right = 1,
    dir_Left = -1,
}

[Serializable]
public enum ObjectDirType
{
    scale,
    rotate,
}