using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButtonDrawer : MonoBehaviour
{
    [SerializeField] Transform[] m_gridRoot;
    [SerializeField] SkillButton m_skillBtnPrefab;
    private Dictionary<SkillName, int> m_skillCollecteds;
    public void DrawSkillButton()
    {
        m_skillCollecteds = FindObjectOfType<SkillManager>().SkillCollecteds;
        if (m_skillCollecteds == null || m_skillCollecteds.Count <= 0) return;
        int index = -1;
        foreach (var skillCollected in m_skillCollecteds)
        {
            index++;
            Helper.ClearChilds(m_gridRoot[index]);
            var skillButtonClone = Instantiate(m_skillBtnPrefab);
            Helper.AssignToRoot(m_gridRoot[index], skillButtonClone.transform,
                Vector3.zero, index == 4 ? Vector3.one * 1f : (index == 0 ? 0.5f * Vector3.one : 0.7f * Vector3.one));
            skillButtonClone.Initialize(skillCollected.Key);
            skillButtonClone.skillButtonType = skillButtonClone.m_skillButtonTypes[index];

        }

    }
}
