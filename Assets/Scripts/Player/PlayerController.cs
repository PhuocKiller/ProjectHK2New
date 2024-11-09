using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEditorInternal.VersionControl.ListControl;


public class PlayerController : NetworkBehaviour, ICanTakeDamage
{
    public PlayerStat playerStat;
    public StatusCanvas statusCanvas;
    CharacterInput characterInput;
    Vector2 moveInput;
    Vector3 moveDirection;
    CharacterController characterControllerPrototype;
    Animator animator;
    float speed;
    private int targetX, targetY, beforeTarget;
    private float previousSpeedX, currentSpeedX, previousSpeedY, currentSpeedY;
    public bool isGround;
    [Networked]
    bool isJumping { get; set; }
    [Networked]
    bool isBasicAttackAttack { get; set; }
    [Networked]
    float jumpHeight { get; set; }
    Vector3 velocity;


    // 0 là normal
    // 1 là jump
    // 2 là injured
    // 3 là die
    // 4 là active attack
    // 5 là đang cast skill
    [Networked(OnChanged = nameof(listenState))]
    [SerializeField]
    public int state { get; set; }


    [SerializeField]
    public GameObject basicAttackObject;
    [SerializeField]
    public Transform basicAttackTransform, transformCamera;
    [SerializeField]
    TextMeshProUGUI textHealth;
    [SerializeField] Player_Types playerType;
    public SkillButton[] skillButtons;


    private void Awake()
    {
        characterInput = new CharacterInput();
        characterControllerPrototype = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
    }
    public override void Spawned()
    {
        base.Spawned();
        
        if (Object.InputAuthority.PlayerId == Runner.LocalPlayer.PlayerId)
        {
            Singleton<CameraController>.Instance.SetFollowCharacter(transformCamera, transform);
            Singleton<PlayerManager>.Instance.SetRunner(Runner);
            playerStat.UpgradeLevel();
        }
        

    }

    public void Start()
    {
        
    }
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (state == 4)
        {
            return;
        }
        if (state != 5)
        {

        }

        CalculateMove();
        CalculateJump();
        
            statusCanvas.healthBarPlayer.UpdateBar(playerStat.currentHealth, playerStat.b_maxHealth);
        
       
        transform.GetChild(0).GetChild(0).forward =Camera.main.transform.forward;    //xoay statuscanvas để mọi player nhìn rõ
    }

    private void CalculateJump()
    {
        if (HasStateAuthority)
        {
            if (isJumping)
            {
                isGround = false;
                isJumping = false;
                velocity += new Vector3(0, 50f, 0);
            }
            if (isGround)
            {
                velocity.y = 0;
                characterControllerPrototype.Move(velocity * Runner.DeltaTime);
            }
            else
            {
                velocity += new Vector3(0, -100f * Runner.DeltaTime, 0);

                characterControllerPrototype.Move(velocity * Runner.DeltaTime);
            }
        }
    }

    public void Jump(GameObject VFXEffect)
    {
        isJumping = true;

        AnimatorRPC("Jump");
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void AnimatorRPC(string name)
    {
        animator.SetTrigger(name);
    }
    public virtual void Skill_1(GameObject VFXEffect, float levelDamage, float manaCost, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false,
        float timeTrigger = 0f, float TimeEffect = 0f)
    {
        AnimatorRPC("Skill_1");
        playerStat.currentMana -= manaCost;
    }

    public virtual void Skill_2(GameObject VFXEffect, float levelDamage, float manaCost, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        AnimatorRPC("Skill_2");
        playerStat.currentMana -= manaCost;
    }
    public virtual void Ultimate(GameObject VFXEffect, float levelDamage, float manaCost, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        AnimatorRPC("Ultimate");
        playerStat.currentMana -= manaCost;
    }

    public virtual void NormalAttack(GameObject VFXEffect, float levelDamage, bool isPhysicDamage,
        bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false, float timeTrigger = 0f, float TimeEffect = 0f)
    {
        AnimatorRPC("Attack");
    }

    void Update()
    {
        if (state == 3 || state == 4)
        {
            return;
        }

        moveInput = state != 5 ? characterInput.Character.Move.ReadValue<Vector2>() : Vector2.zero;


        if (isGround)
        {
            velocity = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.Space) && Object.HasInputAuthority)
        {

        }

    }
    private void OnEnable()
    {
        characterInput.Enable();
    }
    private void OnDisable()
    {
        characterInput.Disable();
    }
    void CalculateMove()
    {
        if (HasStateAuthority)
        {

            moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            CalculateAnimSpeed("MoveX", moveInput.x, true);
            CalculateAnimSpeed("MoveY", moveInput.y, false);
            speed = 2f + Vector2.Dot(moveInput, Vector2.up);
            Quaternion angleCamera = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);

            if (moveDirection.magnitude > 0)
            {
                if (!isJumping)
                {
                    //  animator.SetFloat("Speed", 1f);
                }

                // Quaternion lookRotation = Quaternion.LookRotation(angleCamera * moveDirection);

                characterControllerPrototype.Move(angleCamera * moveDirection * speed * 3 * Runner.DeltaTime);
            }
            Quaternion look = state == 5 ? Quaternion.LookRotation(GetComponent<SkillDirection>().directionNormalize) : angleCamera;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, look, 720 * Runner.DeltaTime);


        }
    }




    protected static void listenState(Changed<PlayerController> changed)
    {

    }
    public void SwithCharacterState(int newstate)
    {
        //Khi ket thuc trang thai cu thi toi lam gi do...
        switch (state)
        {
            case 0: { break; }
            case 1: { break; }
            case 2: { break; }
            case 3: { break; }
        }
        //Bat dau trang thai moi thi toi lam gi do...
        switch (newstate)
        {
            case 0: { break; }
            case 1: { break; }
            case 2: { break; }
            case 3:
                {
                    animator.SetTrigger("Die");
                    break;
                }
        }
        state = newstate;
    }
    public int GetCurrentState()
    {
        return state;
    }

    private void CalculateAnimSpeed(string animationName, float speed, bool isMoveX)
    {
        if (isMoveX)
        {
            currentSpeedX = speed;
        }
        else
        {
            currentSpeedY = speed;
        }

        if (isMoveX && previousSpeedX != currentSpeedX)
        {
            StartCoroutine(CaculateSmoothAnimation(animationName, true, speed));
        }
        if (!isMoveX && previousSpeedY != currentSpeedY)
        {
            StartCoroutine(CaculateSmoothAnimation(animationName, false, speed));
        }

        if (isMoveX)
        {
            previousSpeedX = speed;
        }
        else
        {
            previousSpeedY = speed;
        }
    }

    IEnumerator CaculateSmoothAnimation(string animationName, bool isMoveX, float? Speedtarget = null)
    {
        float time = 0;
        float start = animator.GetFloat(animationName);
        float x = Speedtarget == null ? 2 : 5;
        float targetTime = 1 / x;
        while (time <= targetTime)
        {
            //doi lai 1 khung hinh
            yield return null;
            if (Speedtarget != null
                && Speedtarget != (isMoveX ? currentSpeedX : currentSpeedY))
            {
                time = targetTime;
                break;
            }
            float valueRandomSmooth = Mathf.Lerp(start, Speedtarget == null ?
                (isMoveX ? targetX : targetY) : Speedtarget.Value, x * time);
            animator.SetFloat(animationName, valueRandomSmooth);
            time += Runner.DeltaTime;
        }
    }

    public void CheckCamera(PlayerRef player, bool isFollow)
    {
        if (player == Runner.LocalPlayer)
        {
            if (isFollow)
            {
                Singleton<CameraController>.Instance.SetFollowCharacter(transformCamera, transform);
            }
            else
            {
                Singleton<CameraController>.Instance.RemoveFollowCharacter();
            }
        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Ground") && !isGround)
        {
            isGround = true;
        }
    }

    public void ApplyDamage(float damage,bool isPhysicDamage, PlayerRef player, Action callback = null)
    {
        CalculateHealthRPC(damage, isPhysicDamage, player);
        callback?.Invoke();
    }
    public void ApplyEffect(PlayerRef player,bool isMakeStun = false, bool isMakeSlow = false, bool isMakeSilen = false,
        float TimeEffect = 0, Action callback = null)
    {
        CalculateEffectRPC(player);
        callback?.Invoke();
    }
    [Rpc(RpcSources.All, RpcTargets.All)] public void CalculateHealthRPC(float damage, bool isPhysicDamage, PlayerRef player)
    {
        if (playerStat.currentHealth == 0) return;
        if (playerStat.currentHealth > damage)
        {
            animator.SetTrigger("Injured");
            playerStat.currentHealth -= (int)damage;
        }
        else
        {
            playerStat.currentHealth = 0;
            SwithCharacterState(3);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void CalculateEffectRPC(PlayerRef player, bool isMakeStun = false,
        bool isMakeSlow = false, bool isMakeSilen = false, float TimeEffect = 0)
    {

    }
   
    public void CalculateStat(/*int level, float multipleHealth, float multipleMana, float multipleDamage, int multipleDefend,
        float multipleMagicResistance, float multipleCriticalChance, float multipleCriticalDamage, int multipleMoveSpeed, int multipleAttackSpeed*/)
    {
        playerStat.UpgradeLevel();
    }
    public Player_Types GetPlayerTypes()
    {
        return playerType;
    }
    private void OnTriggerEnter(Collider other)
    {
        InventoryItemBase item = other.GetComponent<InventoryItemBase>();
        if (item != null)
        {
            Singleton<Inventory>.Instance.AddItem(item);
            item.OnPickUp();
        }
    }

    
}

