using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    // 角色的最大生命值
    public int maxHp;
    // 当前生命值的变量
    public IntVariable hp;
    // 防御力的变量
    public IntVariable defense;
    // 增益效果持续的回合数
    public IntVariable buffRound;

    // 当前生命值的属性，用于获取和设置生命值
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    // 最大生命值的属性，用于获取最大生命值
    public int MaxHP { get => hp.maxValue; }
    // 动画控制器
    protected Animator animator;
    // 角色是否死亡
    public bool isDead;

    // 增益效果的预制体
    public GameObject buff;
    // 减益效果的预制体
    public GameObject debuff;

    // 基础魔法值和力量值
    public float baseMagic = 1f;
    public float baseStrength = 1f;
    // 力量效果的加成系数
    public float strengthEffect = 0.5f;
    // 角色死亡事件
    [Header("广播")]
    public ObjectEventSO characterDeadEvent;

    // 在Awake时初始化动画控制器
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // 在Start时初始化生命值和防御力
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;
        RestDefense();
    }

    // 每帧更新动画状态
    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    // 受到伤害时调用此方法
    public virtual void TakeDamage(int damage)
    {
       
        // 计算实际受到的伤害，不低于0
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        // 更新防御力，如果伤害大于防御力，则防御力归0
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);

        // 如果当前生命值大于受到的伤害，则减少生命值
        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            animator.SetTrigger("hit");
        }
        else
        {
            // 如果生命值为0或以下，则角色死亡
            CurrentHP = 0;
            isDead = true;
            // 触发角色死亡事件
            Debug.Log("触发角色死亡事件");
            characterDeadEvent.RaisEvent(this, this);
        }
    }

    // 更新防御力
    public void UpdateDefense(int amount)
    {
        defense.SetValue(defense.currentValue + amount);
    }

    // 重置防御力为0
    public void RestDefense()
    {
        defense.SetValue(0);
    }

    // 治疗生命值
    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    // 设置力量效果，包括增益和减益
    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            // 增加力量值
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            //增加魔法值
            float newMagic = baseMagic + strengthEffect;
            baseStrength = Mathf.Min(newMagic, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            // 减少力量值           
            baseStrength = 1 - strengthEffect;
            //降低魔法值
            baseMagic = 1 - strengthEffect;
            debuff.SetActive(true);
        }
        // 更新增益效果的回合数
        var currentRound = buffRound.currentValue + round;
        if (baseStrength == 1)
        {
            buffRound.SetValue(0);
        }
        else
        {
            buffRound.SetValue(currentRound);
        }
    }

    // 更新力量效果的持续回合
    public void UpdateStrengthRound()
    {
        buffRound.SetValue(buffRound.currentValue - 1);
        if (buffRound.currentValue <= 0)
        {
            buffRound.SetValue(0);
            baseStrength = 1;
        }
    }
}