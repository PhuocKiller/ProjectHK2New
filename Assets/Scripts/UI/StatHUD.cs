using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatHUD : MonoBehaviour
{
    public PlayerController player;
    [SerializeField] TextMeshProUGUI attackTMP, defTMP, attackSpeedTMP, magicAmpliTMP, magicResisTMP, moveSpeedTMP;
        
    void Start()
    {
        StartCoroutine(DelayCheckPlay());
    }

    // Update is called once per frame
    void Update()
    {
        if (player==null) return;
        attackTMP.text= player.playerStat.damage.ToString();
    }
    IEnumerator DelayCheckPlay()
    {
        yield return new WaitForSeconds(0.5f);
        Singleton<PlayerManager>.Instance.CheckPlayer(out int? state, out PlayerController player);
        Debug.Log(player);
        this.player = player;
    }
}
