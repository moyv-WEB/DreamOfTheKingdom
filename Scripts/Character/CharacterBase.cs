using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    // ��ɫ���������ֵ
    public int maxHp;
    // ��ǰ����ֵ�ı���
    public IntVariable hp;
    // �������ı���
    public IntVariable defense;
    // ����Ч�������Ļغ���
    public IntVariable buffRound;

    // ��ǰ����ֵ�����ԣ����ڻ�ȡ����������ֵ
    public int CurrentHP { get => hp.currentValue; set => hp.SetValue(value); }
    // �������ֵ�����ԣ����ڻ�ȡ�������ֵ
    public int MaxHP { get => hp.maxValue; }
    // ����������
    protected Animator animator;
    // ��ɫ�Ƿ�����
    public bool isDead;

    // ����Ч����Ԥ����
    public GameObject buff;
    // ����Ч����Ԥ����
    public GameObject debuff;

    // ����ħ��ֵ������ֵ
    public float baseMagic = 1f;
    public float baseStrength = 1f;
    // ����Ч���ļӳ�ϵ��
    public float strengthEffect = 0.5f;
    // ��ɫ�����¼�
    [Header("�㲥")]
    public ObjectEventSO characterDeadEvent;

    // ��Awakeʱ��ʼ������������
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // ��Startʱ��ʼ������ֵ�ͷ�����
    protected virtual void Start()
    {
        hp.maxValue = maxHp;
        CurrentHP = MaxHP;
        buffRound.currentValue = buffRound.maxValue;
        RestDefense();
    }

    // ÿ֡���¶���״̬
    protected virtual void Update()
    {
        animator.SetBool("isDead", isDead);
    }

    // �ܵ��˺�ʱ���ô˷���
    public virtual void TakeDamage(int damage)
    {
       
        // ����ʵ���ܵ����˺���������0
        var currentDamage = (damage - defense.currentValue) >= 0 ? (damage - defense.currentValue) : 0;
        // ���·�����������˺����ڷ����������������0
        var currentDefense = (damage - defense.currentValue) >= 0 ? 0 : (defense.currentValue - damage);
        defense.SetValue(currentDefense);

        // �����ǰ����ֵ�����ܵ����˺������������ֵ
        if (CurrentHP > currentDamage)
        {
            CurrentHP -= currentDamage;
            animator.SetTrigger("hit");
        }
        else
        {
            // �������ֵΪ0�����£����ɫ����
            CurrentHP = 0;
            isDead = true;
            // ������ɫ�����¼�
            Debug.Log("������ɫ�����¼�");
            characterDeadEvent.RaisEvent(this, this);
        }
    }

    // ���·�����
    public void UpdateDefense(int amount)
    {
        defense.SetValue(defense.currentValue + amount);
    }

    // ���÷�����Ϊ0
    public void RestDefense()
    {
        defense.SetValue(0);
    }

    // ��������ֵ
    public void HealHealth(int amount)
    {
        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        buff.SetActive(true);
    }

    // ��������Ч������������ͼ���
    public void SetupStrength(int round, bool isPositive)
    {
        if (isPositive)
        {
            // ��������ֵ
            float newStrength = baseStrength + strengthEffect;
            baseStrength = Mathf.Min(newStrength, 1.5f);
            //����ħ��ֵ
            float newMagic = baseMagic + strengthEffect;
            baseStrength = Mathf.Min(newMagic, 1.5f);
            buff.SetActive(true);
        }
        else
        {
            // ��������ֵ           
            baseStrength = 1 - strengthEffect;
            //����ħ��ֵ
            baseMagic = 1 - strengthEffect;
            debuff.SetActive(true);
        }
        // ��������Ч���Ļغ���
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

    // ��������Ч���ĳ����غ�
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