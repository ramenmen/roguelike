using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MovingObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    public AudioClip enemyAttackSound1;
    public AudioClip enemyAttackSound2;

    [SerializeField]
    private Text hpText;
    public int hp;

    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        hp = Random.Range((int)Mathf.Log(GameManager.instance.level,2f)*50, (int)Mathf.Log(GameManager.instance.level,2f)*100);
        UpdateHpText();
        base.Start();
    }

    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove <T> (xDir, yDir);

        skipMove = true;
    }

    void UpdateHpText()
    {
        hpText.text = hp.ToString();
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;
        if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else   
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player> (xDir, yDir);
    }

    protected override void OnCantMove<T> (T component)
    {
        Player hitPlayer = component as Player;

        animator.SetTrigger("EnemyAttack");
        SoundManager.instance.RandomizeSfx(enemyAttackSound1,enemyAttackSound2);
        LoseHp(hitPlayer.enemyDamage);
        hitPlayer.LoseFood(playerDamage);
    }

    public void LoseHp (int loss)
    {
        hp -= loss;
        UpdateHpText();
        if (hp <= 0)
        {
            GameManager.instance.RemoveEnemyFromList(this);
            Destroy(gameObject);
        }
    }
}
