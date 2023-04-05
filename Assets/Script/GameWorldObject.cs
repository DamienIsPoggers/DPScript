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
    public Dictionary<int, uponEntry> uponStatements = new Dictionary<int, uponEntry>();
    public Dictionary<string, StateEntry> stateCancels = new Dictionary<string, StateEntry>();

    public List<string> stateCancelIDs = new List<string>();

    public int idNum;
    public string idStr;

    DPS_ObjectCommand commands;
    Transform cameraMain;

    GameObject CollisionChild;

    Dictionary<int, GameWorldObject> worldObjects = new Dictionary<int, GameWorldObject>();
    public GameWorldObject player;

    public int curHealth = 1, maxHealth = 1;
    public int damage = 0, pushBackX = 0, pushbackZ = 0, airPushBackX = 0, airPushBackY = 0, airPushBackZ = 0, attackType = 0, counterType = 0;
    public int tick = 0, hitstopTick = 0, dir = 1, onStage = 0, scriptPos = 0;
    public string curState = "", lastState = "", nextState = "", landingState = "";
    public bool landToState = false;
    public string curCollision = "", lastCollision = "", lerpCollision = "";
    public int locX = 0, locY = 0, locZ = 0; 
    public int xImpulse = 0, yImpulse = 0, zImpulse = 0;
    public int xImpulseAdd = 0, yImpulseAdd = 0, zImpulseAdd = 0;
    public bool transferMomentum = false;
    public Vector3 rotation = Vector3.zero;
    public Vector3 scale = Vector3.one;
    public byte stateType = 0;
    public bool stateHasHit = false;
    public int comboCounter = 0;
    public bool momentumPause = false;
    public bool rest = false, willRest = false, lerping = false, switchingState = false;
    public string returnString = "";
    public int returnInt = 0;
    public bool faceCamera = true;
    public bool playingAnim = false;
    public float animTime = 0;
    public bool invincible = false, hitboxesDisabled = false, noCollision = false, ignoreFreezes = false, armor = false;
    public bool friendlyFire = false; //doesnt work if they are on the same parent
    public bool isProjectile = false;
    public bool isPlayer = true, isActive = true;
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

    public Dictionary<int, DPS_Stage> world = new Dictionary<int, DPS_Stage>();
    public List<Object_Collision> loadedCollisions = new List<Object_Collision>();

    public List<string> hitOrBlockCancels = new List<string>(), hitCancels = new List<string>(), blockCancels = new List<string>(), whiffCancels = new List<string>();
    public List<string> cancelableStates = new List<string>();

    private PlayerControls playerControls;

    private InputElement currInput = new InputElement(5);
    private List<InputElement> buffer = new List<InputElement>();

    //some various variables for misc stuff
    public int[] walkingSpeed = new int[] { 400, -300 }; //fwalk, bwalk
    public int[] dashSpeed = new int[] { 600, 40, 820 }; //inital speed, accel, max
    public int[] airActionsCount = new int[] { 1, 1, 1 }; //air jump, fdash, bdash
    public int[] jumpSpeed = new int[] { 500, -500, 1500 }; //fspeed, bspeed, height
    public int defaultGravity = -90;


    [SerializeField]
    bool isInDebug = false;
    [SerializeField]
    bool debugNoLoad = false;

    public bool initalized = false;

    private void Awake()
    {
        commands = gameObject.AddComponent<DPS_ObjectCommand>();
        cameraMain = GameObject.Find("Main Camera").GetComponent<Transform>();
        //GameObject temp = transform.Find("Meshes").gameObject;
        playerControls = new PlayerControls();

        for (int i = 0; i < armatureList.Count; i++)
        {
            armatures.Add(armatureList[i], transform.Find("Mesh").transform.Find(armatureList[i]).GetComponent<Animator>());
            armatures[armatureList[i]].speed = 0.005f;
            renderers.Add(armatureList[i], transform.Find("Mesh").transform.Find(armatureList[i]).GetComponentInChildren<SkinnedMeshRenderer>());
        }

        CollisionChild = transform.Find("Collision").gameObject;

        if (debugNoLoad)
            return;

        if (isInDebug)
        {
            Objects_Load tempLoad = new Objects_Load();
            tempLoad.mainLoad("Char/" + idStr + "/" + idStr + "_load", this, idStr, true);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        for (int i = 0; i < Battle_Manager.Instance.stages.Count; i++)
            world.Add(Battle_Manager.Instance.stages[i].id, Battle_Manager.Instance.stages[i]);
        Battle_Manager.Instance.players.Add(this);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        //if (!ignoreFreezes && battleManager.superFreeze && superFreezeTime <= 0)
        //  return;
        //input_DebugBuffer();
        if(!initalized)
        {
            commands.callSubroutine("init");
            commands.cmnSubroutine("cmnInit");
            commands.enterState("CmnStand");
            player = this;
            addGlobalVariables();
            initalized = true;
            return;
        }

        if (debugNoLoad)
            return;

        positionUpdate();
        rotateToCamera();
        scaleUpdate();

        inputUpdate();

        if (hitstopTick == 0 && !ignoreFreezes)
        {
            tick--;
            activateUpon(3);
        }
        else
            hitstopTick--;
        if(tick <= 0)
        {
            lerping = false;
            willRest = false;
            rest = false;

            /*
            tempSpriteNum++;
            if (tempSpriteNum > 11)
                tempSpriteNum = 0;
            curCollision = tempStateNam + tempSpriteNum;
            tick = 9;
            */

            while (!rest)
            {
                switchingState = false;
                if (scriptPos + 1 > states[curState].commands.Count)
                { commands.enterState(nextState); continue; }
                commands.objSwitchCase(states[curState].commands[scriptPos]);
                if (switchingState)
                    continue;
                if (!rest || states[curState].commands[scriptPos].id == 3)
                    scriptPos++;
                if (scriptPos + 1 > states[curState].commands.Count)
                    break;
            }

            activateUpon(4);


            if (curCollision != lastCollision)
            {
                if(!playingAnim)
                    switchSprite();

                if (collisions.ContainsKey(lastCollision))
                    for (int i = 0; i < collisions[lastCollision].boxCount; i++)
                        loadedCollisions[0].kill();

                if(collisions.ContainsKey(curCollision))
                for (int i = 0; i < collisions[curCollision].boxCount; i++)
                {
                    Object_Collision temp = CollisionChild.AddComponent<Object_Collision>();
                    temp.init(collisions[curCollision].boxes[i], collisions[curCollision].hasZ,
                        collisions[curCollision].sphere, this, this);
                    loadedCollisions.Add(temp);
                    Battle_Manager.Instance.collisions.Add(temp);
                    //Debug.Log(i);
                }

                
            }

            if (lerping && !playingAnim)
                if (collisions.ContainsKey(lerpCollision))
                    lerpSprite();
        }

        for (int i = 0; i < armatureList.Count; i++)
            if (renderers[armatureList[i]].enabled)
            {
                if (armatures[armatureList[i]].GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
                    playingAnim = false;
                break;
            }

        if (!isPlayer)
            return;
        if (!isActive)
            return;

        
    }

    private void switchSprite()
    {
        if(useArmature)
        {
            string state = collisions[curCollision].sprites[0].Remove(collisions[curCollision].sprites[0].LastIndexOf("_"));
            //Debug.Log(state);
            int frame = Int32.Parse(collisions[curCollision].sprites[0].Remove(0, collisions[curCollision].sprites[0].LastIndexOf("_") + 1));
            //Debug.Log(frame);
            var stateId = Animator.StringToHash(state);
            for (int i = 0; i < armatureList.Count; i++)
            {
                if (armatures[armatureList[i]].HasState(0, stateId) && renderers[armatureList[i]].enabled)
                {
                    armatures[armatureList[i]].Play(state, 0, (float)frame / 500 / (armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.length /
                        2 / armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.frameRate));
                    armatures[armatureList[i]].speed = 0f;
                }

            }
        }
    }

    private void lerpSprite()
    {
        if(useArmature)
        {
            string state = collisions[lerpCollision].sprites[0].Remove(collisions[lerpCollision].sprites[0].LastIndexOf("_"));
            int frame = Int32.Parse(collisions[lerpCollision].sprites[0].Remove(0, collisions[lerpCollision].sprites[0].LastIndexOf("_") + 1));

            var stateId = Animator.StringToHash(state);
            for (int i = 0; i < armatureList.Count; i++)
            {
                if (armatures[armatureList[i]].HasState(0, stateId) && renderers[armatureList[i]].enabled)
                {
                    armatures[armatureList[i]].CrossFade(state, 1, 0, (float)frame / 500 / (armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.length /
                        2 / armatures[armatureList[i]].GetCurrentAnimatorClipInfo(0)[0].clip.frameRate));
                    armatures[armatureList[i]].speed = 1;
                }
            }
        }
    }

    private void rotateToCamera()
    {
        if (faceCamera)
        { 
            transform.LookAt(cameraMain);
            transform.localEulerAngles = new Vector3(rotation.x, transform.localEulerAngles.y+rotation.y, rotation.z);
        }
        else
            transform.localEulerAngles = rotation;
    }

    private void positionUpdate()
    {
        //if(!ignoreFreezes && battleManager.superFreeze && superFreezeTime <= 0)
        //return;
        transform.parent = world[onStage].transform;
        
        if (!momentumPause)
        {
            locX += xImpulse * dir; locY += yImpulse * dir; locZ += zImpulse * dir;
            xImpulse += xImpulseAdd; yImpulse += yImpulseAdd; zImpulse += zImpulseAdd;
        }

        if(locY <= 0)
        {
            locY = 0;
            yImpulse = 0;
            yImpulseAdd = 0;
            activateUpon(2);
            if(landToState)
                commands.enterState(landingState);
        }

        transform.localPosition = new Vector3((float)locX / 10000, (float)locY / 10000, (float)locZ / 10000);
    }

    private void scaleUpdate()
    {
        transform.localScale = new Vector3(scale.x * dir, scale.y, scale.z);
    }

    public void activateUpon(int type)
    {
        if (uponStatements.ContainsKey(type))
            for (int i = 0; i < uponStatements[type].commands.Count; i++)
                commands.objSwitchCase(uponStatements[type].commands[i]);
    }

    public void boxesCollide(Object_Collision child, Object_Collision hit)
    {
        switch(child.box.type)
        {
            case 1:
                Debug.Log("Child is hurtbox");
                break;
            default:
                Debug.Log("Child is type " + child.box.type);
                break;
        }


    }

    public class InputElement
    {
        public byte inputType;
        public string button;
        public uint updateCount = 0;
        public uint chargeTime = 0;

        public InputElement(byte _inputType)
        {
            inputType = _inputType;
        }
    }

    private void inputUpdate()
    {
        inputs_AddToBuffer();

        //buffer update
        if (buffer.Count > 0)
        {
            while (buffer.Count > 15)
            {
                buffer.RemoveAt(0);
            }

            for (int i = 0; i < buffer.Count; i++)
            {
                buffer[i].updateCount++;

                if (buffer[i].updateCount == 60)
                {
                    buffer.RemoveAt(i);
                }
            }
        }

        for (int i = 0; i < cancelableStates.Count; i++)
        {
            if (!stateCancels.ContainsKey(cancelableStates[i]))
                continue;
            StateEntry entry = stateCancels[cancelableStates[i]];
            if (stateType != entry.type)
                continue;
            //Debug.Log("Swag");
            //Debug.Log(entry.input + ", " + entry.button);
            if (input_CanInput(entry.input, entry.button, entry.leniantInput,
                entry.holdBuffer))
            {
                //Debug.Log("boom");
                commands.enterState(entry.name);
                break;
            }
        }
    }

    private void inputs_AddToBuffer()
    {
        InputElement _input = new InputElement(5);
        if (buffer.Count > 0) _input.inputType = buffer[buffer.Count - 1].inputType;

        Vector2 directions = playerControls.Player.Directions.ReadValue<Vector2>();

        if (directions[1] <= -0.5 && directions[0] <= -0.5)
        { _input.inputType = 1; currInput.inputType = 1; }
        else if (directions[1] <= -0.5 && directions[0] >= 0.5)
        { _input.inputType = 3; currInput.inputType = 3; }
        else if (directions[1] >= 0.5 && directions[0] <= -0.5)
        { _input.inputType = 7; currInput.inputType = 7; }
        else if (directions[1] >= 0.5 && directions[0] >= 0.5)
        { _input.inputType = 9; currInput.inputType = 9; }
        else if (directions[1] <= -0.5)
        { _input.inputType = 2; currInput.inputType = 2; }
        else if (directions[1] >= 0.5)
        { _input.inputType = 8; currInput.inputType = 8; }
        else if (directions[0] <= -0.5)
        { _input.inputType = 4; currInput.inputType = 4; }
        else if (directions[0] >= 0.5)
        { _input.inputType = 6; currInput.inputType = 6; }
        else
        { _input.inputType = 5; currInput.inputType = 5; }

        string _but = "";


        if (playerControls.Player.Action_A.IsPressed() && !_but.Contains("A")) _but += "A";
        if (playerControls.Player.Action_B.IsPressed() && !_but.Contains("B")) _but += "B";
        if (playerControls.Player.Action_C.IsPressed() && !_but.Contains("C")) _but += "C";
        if (playerControls.Player.Action_D.IsPressed() && !_but.Contains("D")) _but += "D";
        if (playerControls.Player.Action_E.IsPressed() && !_but.Contains("E")) _but += "E";

        if (playerControls.Player.Action_ABCD.IsPressed()) _but = "ABCD";



        _input.button = _but;
        currInput.button = _but;

        if (buffer.Count > 0)
        {
            if (buffer[buffer.Count - 1].inputType == _input.inputType &&
                buffer[buffer.Count - 1].button == _input.button)
            {
                buffer[buffer.Count - 1].updateCount = 0;
                buffer[buffer.Count - 1].chargeTime++;
                return;
            }
        }
        buffer.Add(_input);
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
        }

        Debug.Log(retrn);
    }

    InputSwitchCase inputTypeSwitchCase = new InputSwitchCase();

    public bool input_CanInput(short type, string _button, byte lieniant, 
        byte holdBuffer)
    {
        if (holdBuffer > currInput.chargeTime)
            return false;


        if (!String.IsNullOrEmpty(_button))
            if (!currInput.button.Contains(_button)) return false;

        if (type == 0)
            type = 5;

        byte[] _input;
        if (type < 10)
            _input = new byte[] { (byte)type };
        else 
            _input = inputTypeSwitchCase.switchCase(type);

        if(dir <= -1)
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

        if (_input.Length != 0)
        {
            if (_input.Length == 1 && lieniant == 0)
            {
                if (_input[0] == 5)
                {
                    if (currInput.inputType == 5 || currInput.inputType == 4 || currInput.inputType == 6) return true;
                    else return false;
                }
                else if (_input[0] == 2)
                {
                    if (currInput.inputType == 2 || currInput.inputType == 1 || currInput.inputType == 3) return true;
                    else return false;
                }
                else if (_input[0] == 8)
                {
                    if (currInput.inputType == 8 || currInput.inputType == 7 || currInput.inputType == 9) return true;
                    else return false;
                }
            }
            else if (currInput.inputType != _input[_input.Length - 1]) return false;
        }

        int startBuf = 0;
        for (int i = 0; i < buffer.Count; i++)
        {
            if (buffer[i].inputType == _input[0])
            {
                startBuf = i;
                break;
            }
        }

        if (_input.Length == 0) return true;
        if (buffer.Count >= _input.Length)
        {
            if (_input.Length == 1) return true;
            else if (_input.Length == 2)
            {
                if (buffer[startBuf].inputType == _input[0] && buffer[startBuf + 1].inputType == 5 &&
                    buffer[startBuf + 2].inputType == _input[1]) return true;
            }
            else
            {
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
            }
        }
        return false;
    }

    private void addGlobalVariables()
    {
        commands.createVar(0, 0, maxHealth);
        commands.createVar(0, 1, curHealth);
        commands.createVar(0, 2, walkingSpeed[0]);
        commands.createVar(0, 3, walkingSpeed[1]);
        commands.createVar(0, 4, dashSpeed[0]);
        commands.createVar(0, 5, dashSpeed[1]);
        commands.createVar(0, 6, dashSpeed[2]);
        commands.createVar(0, 7, airActionsCount[0]);
        commands.createVar(0, 8, airActionsCount[1]);
        commands.createVar(0, 9, airActionsCount[2]);
        commands.createVar(0, 10, jumpSpeed[0]);
        commands.createVar(0, 11, jumpSpeed[1]);
        commands.createVar(0, 12, jumpSpeed[2]);
        commands.createVar(0, 13, defaultGravity);
        commands.createVar(0, 14, locX);
        commands.createVar(0, 15, locY);
        commands.createVar(0, 16, locZ);
    }
}
