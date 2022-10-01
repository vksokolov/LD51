using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameModeTextWrapper : MonoBehaviour
{
    private const string InAnimation = "In";
    
    [SerializeField] public TextMeshProUGUI _title;
    [SerializeField] public TextMeshProUGUI _description;
    [SerializeField] public Animator _animator;
    
    public void OnGameModeChanged(GameModeInfo info)
    {
        _title.text = info.Name;
        _description.text = info.Description;
        _animator.Play(InAnimation);
    }
}
